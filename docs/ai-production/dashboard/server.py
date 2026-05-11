#!/usr/bin/env python3
"""Local live dashboard relay server.

Serves the static dashboard and exposes a small JSON/SSE API for local PM
message queueing. This is intentionally stdlib-only so the dashboard launcher
works on a clean macOS Python install.
"""

from __future__ import annotations

import argparse
import json
import mimetypes
import os
import posixpath
import sys
import time
import uuid
from datetime import datetime, timezone
from http import HTTPStatus
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer
from pathlib import Path
from typing import Any
from urllib.parse import unquote, urlparse


ROOT_DIR = Path(__file__).resolve().parent
DATA_DIR = ROOT_DIR / "data"
DASHBOARD_FILE = DATA_DIR / "dashboard.json"
OUTBOX_FILE = DATA_DIR / "outbox.json"
EVENTS_FILE = DATA_DIR / "events-live.json"
MAX_BODY_BYTES = 64 * 1024
MAX_MESSAGE_CHARS = 4000
SERVER_NAME = "red-ball-dashboard-relay"


def iso_now() -> str:
    return datetime.now(timezone.utc).astimezone().isoformat(timespec="seconds")


def display_time(value: str) -> str:
    parsed = datetime.fromisoformat(value)
    return parsed.strftime("%Y-%m-%d %H:%M")


def read_json(path: Path, fallback: Any) -> Any:
    try:
        with path.open("r", encoding="utf-8") as handle:
            return json.load(handle)
    except FileNotFoundError:
        return fallback
    except json.JSONDecodeError as exc:
        raise ValueError(f"{path} is not valid JSON: {exc}") from exc


def atomic_write_json(path: Path, payload: Any) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    tmp_path = path.with_suffix(path.suffix + f".{os.getpid()}.{uuid.uuid4().hex}.tmp")
    with tmp_path.open("w", encoding="utf-8") as handle:
        json.dump(payload, handle, indent=2, ensure_ascii=True)
        handle.write("\n")
    os.replace(tmp_path, path)


def append_list_file(path: Path, item: dict[str, Any]) -> list[dict[str, Any]]:
    current = read_json(path, [])
    if not isinstance(current, list):
        raise ValueError(f"{path} must contain a JSON array")
    current.append(item)
    atomic_write_json(path, current)
    return current


def load_state() -> dict[str, Any]:
    dashboard = read_json(DASHBOARD_FILE, {})
    outbox = read_json(OUTBOX_FILE, [])
    live_events = read_json(EVENTS_FILE, [])

    if not isinstance(dashboard, dict):
        raise ValueError(f"{DASHBOARD_FILE} must contain a JSON object")
    if not isinstance(outbox, list):
        raise ValueError(f"{OUTBOX_FILE} must contain a JSON array")
    if not isinstance(live_events, list):
        raise ValueError(f"{EVENTS_FILE} must contain a JSON array")

    return {
        "relay": {
            "name": SERVER_NAME,
            "mode": "local-pm-dispatch-queue",
            "realSubagentDispatch": False,
            "updatedAt": iso_now(),
        },
        "dashboard": dashboard,
        "outbox": outbox,
        "liveEvents": live_events,
    }


def validate_message(payload: Any) -> dict[str, str]:
    if not isinstance(payload, dict):
        raise ValueError("JSON body must be an object")

    agent_id = str(payload.get("agentId", "")).strip()
    body = str(payload.get("body", "")).strip()
    author = str(payload.get("author", "PM")).strip() or "PM"

    if not agent_id:
        raise ValueError("agentId is required")
    if not body:
        raise ValueError("body is required")
    if len(body) > MAX_MESSAGE_CHARS:
        raise ValueError(f"body must be {MAX_MESSAGE_CHARS} characters or fewer")

    return {"agentId": agent_id, "body": body, "author": author}


def make_message(payload: dict[str, str]) -> tuple[dict[str, Any], dict[str, Any]]:
    now = iso_now()
    message_id = f"msg-{datetime.now(timezone.utc).strftime('%Y%m%d%H%M%S')}-{uuid.uuid4().hex[:8]}"
    event_id = f"evt-{message_id}"
    queued = {
        "id": message_id,
        "agentId": payload["agentId"],
        "body": payload["body"],
        "author": payload["author"],
        "createdAt": now,
        "status": "PM dispatch pending",
        "dispatchMode": "local-pm-dispatch-queue",
        "realSubagentDispatch": False,
    }
    event = {
        "id": event_id,
        "time": now,
        "displayTime": display_time(now),
        "speaker": payload["author"],
        "agentId": payload["agentId"],
        "kind": "pm-dispatch-queued",
        "message": payload["body"],
        "status": "PM dispatch pending",
        "dispatchMode": "local-pm-dispatch-queue",
    }
    return queued, event


class DashboardHandler(BaseHTTPRequestHandler):
    server_version = "RedBallDashboardRelay/1.0"

    def handle(self) -> None:
        try:
            super().handle()
        except ConnectionResetError:
            self.close_connection = True

    def log_message(self, fmt: str, *args: Any) -> None:
        sys.stderr.write("%s - - [%s] %s\n" % (self.address_string(), self.log_date_time_string(), fmt % args))

    def end_headers(self) -> None:
        self.send_header("Cache-Control", "no-store")
        self.send_header("X-Content-Type-Options", "nosniff")
        super().end_headers()

    def send_json(self, status: int, payload: Any) -> None:
        body = json.dumps(payload, indent=2, ensure_ascii=True).encode("utf-8")
        self.send_response(status)
        self.send_header("Content-Type", "application/json; charset=utf-8")
        self.send_header("Content-Length", str(len(body)))
        self.end_headers()
        self.wfile.write(body)

    def send_error_json(self, status: int, message: str) -> None:
        self.send_json(status, {"ok": False, "error": message})

    def do_OPTIONS(self) -> None:
        self.send_response(HTTPStatus.NO_CONTENT)
        self.send_header("Allow", "GET, POST, OPTIONS")
        self.send_header("Access-Control-Allow-Origin", "*")
        self.send_header("Access-Control-Allow-Methods", "GET, POST, OPTIONS")
        self.send_header("Access-Control-Allow-Headers", "Content-Type")
        self.end_headers()

    def do_GET(self) -> None:
        path = urlparse(self.path).path
        try:
            if path == "/api/health":
                self.send_json(HTTPStatus.OK, {"ok": True, "name": SERVER_NAME, "root": str(ROOT_DIR)})
            elif path == "/api/state":
                self.send_json(HTTPStatus.OK, load_state())
            elif path == "/api/events":
                self.send_events()
            else:
                self.send_static(path)
        except ValueError as exc:
            self.send_error_json(HTTPStatus.INTERNAL_SERVER_ERROR, str(exc))

    def do_POST(self) -> None:
        path = urlparse(self.path).path
        if path != "/api/messages":
            self.send_error_json(HTTPStatus.NOT_FOUND, "Unknown endpoint")
            return

        length = int(self.headers.get("Content-Length", "0") or "0")
        if length <= 0:
            self.send_error_json(HTTPStatus.BAD_REQUEST, "JSON body is required")
            return
        if length > MAX_BODY_BYTES:
            self.send_error_json(HTTPStatus.REQUEST_ENTITY_TOO_LARGE, "Request body is too large")
            return

        try:
            raw_body = self.rfile.read(length)
            payload = json.loads(raw_body.decode("utf-8"))
            message_payload = validate_message(payload)
            queued, event = make_message(message_payload)
            append_list_file(OUTBOX_FILE, queued)
            append_list_file(EVENTS_FILE, event)
        except json.JSONDecodeError:
            self.send_error_json(HTTPStatus.BAD_REQUEST, "Request body must be valid JSON")
            return
        except ValueError as exc:
            self.send_error_json(HTTPStatus.BAD_REQUEST, str(exc))
            return

        self.send_json(HTTPStatus.CREATED, {"ok": True, "message": queued, "event": event})

    def send_events(self) -> None:
        self.close_connection = True
        self.send_response(HTTPStatus.OK)
        self.send_header("Content-Type", "text/event-stream; charset=utf-8")
        self.send_header("Connection", "keep-alive")
        self.end_headers()

        last_signature = ""
        while True:
            try:
                events = read_json(EVENTS_FILE, [])
                outbox = read_json(OUTBOX_FILE, [])
                if not isinstance(events, list):
                    raise ValueError(f"{EVENTS_FILE} must contain a JSON array")
                if not isinstance(outbox, list):
                    raise ValueError(f"{OUTBOX_FILE} must contain a JSON array")

                latest_id = events[-1]["id"] if events and isinstance(events[-1], dict) else ""
                signature = f"{len(events)}:{len(outbox)}:{latest_id}"
                if signature != last_signature:
                    payload = {"events": events, "outbox": outbox}
                    frame = (
                        f"id: {latest_id}\n"
                        "event: state\n"
                        f"data: {json.dumps(payload, ensure_ascii=True)}\n\n"
                    ).encode("utf-8")
                    self.wfile.write(frame)
                    self.wfile.flush()
                    last_signature = signature
                else:
                    self.wfile.write(b": heartbeat\n\n")
                    self.wfile.flush()

                time.sleep(1)
            except (BrokenPipeError, ConnectionResetError):
                return

    def send_static(self, request_path: str) -> None:
        if request_path in {"", "/"}:
            request_path = "/index.html"

        clean_path = posixpath.normpath(unquote(request_path)).lstrip("/")
        target = (ROOT_DIR / clean_path).resolve()
        if not str(target).startswith(str(ROOT_DIR)) or target.is_dir():
            self.send_error_json(HTTPStatus.NOT_FOUND, "Not found")
            return
        if not target.exists():
            self.send_error_json(HTTPStatus.NOT_FOUND, "Not found")
            return

        content_type = mimetypes.guess_type(str(target))[0] or "application/octet-stream"
        body = target.read_bytes()
        self.send_response(HTTPStatus.OK)
        self.send_header("Content-Type", content_type)
        self.send_header("Content-Length", str(len(body)))
        self.end_headers()
        self.wfile.write(body)


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description="Serve the Red Ball live dashboard relay.")
    parser.add_argument("--host", default="127.0.0.1")
    parser.add_argument("--port", type=int, default=int(os.environ.get("RED_BALL_DASHBOARD_PORT", "8765")))
    return parser.parse_args()


def main() -> int:
    args = parse_args()
    DATA_DIR.mkdir(parents=True, exist_ok=True)
    for path in (OUTBOX_FILE, EVENTS_FILE):
        if not path.exists():
            atomic_write_json(path, [])

    server = ThreadingHTTPServer((args.host, args.port), DashboardHandler)
    print(f"{SERVER_NAME} serving {ROOT_DIR} at http://{args.host}:{args.port}/index.html", flush=True)
    try:
        server.serve_forever()
    except KeyboardInterrupt:
        print("\nStopping dashboard relay.", flush=True)
    finally:
        server.server_close()
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
