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
import plistlib
import posixpath
import subprocess
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
AI_PRODUCTION_ROOT = ROOT_DIR.parent
DATA_DIR = ROOT_DIR / "data"
DASHBOARD_FILE = DATA_DIR / "dashboard.json"
OUTBOX_FILE = DATA_DIR / "outbox.json"
EVENTS_FILE = DATA_DIR / "events-live.json"
MAX_BODY_BYTES = 64 * 1024
MAX_MESSAGE_CHARS = 4000
SERVER_NAME = "red-ball-dashboard-relay"
REPO_ROOT = ROOT_DIR.parents[2]
CODEX_APP_PATH = Path("/Applications/Codex.app")
DOCS_ROUTE_PREFIX = "/docs"
REPO_ROUTE_PREFIX = "/repo"


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


def is_local_client(address: str) -> bool:
    return address in {"127.0.0.1", "::1", "localhost"}


def is_within_root(target: Path, root: Path) -> bool:
    try:
        target.relative_to(root)
    except ValueError:
        return False
    return True


def resolve_static_target(root: Path, request_path: str) -> Path | None:
    clean_path = posixpath.normpath(unquote(request_path or "/")).lstrip("/")
    if clean_path in {"", "."}:
        return None

    target = (root / clean_path).resolve()
    if not is_within_root(target, root.resolve()) or target.is_dir() or not target.exists():
        return None
    return target


def workspace_search_roots() -> list[Path]:
    roots = [REPO_ROOT, *REPO_ROOT.parents]
    home = Path.home().resolve()
    return [root for root in roots if root == home or home in root.parents or root in home.parents]


def find_workspace() -> Path | None:
    repo_candidates: list[Path] = []
    try:
        repo_candidates.extend(path for path in REPO_ROOT.rglob("*.xcworkspace") if path.is_dir())
    except OSError:
        pass

    if repo_candidates:
        return sorted(repo_candidates, key=lambda path: (len(path.parts), str(path)))[0]

    candidates: list[Path] = []
    for root in workspace_search_roots():
        try:
            candidates.extend(path for path in root.glob("*.xcworkspace") if path.is_dir())
        except OSError:
            continue

    if not candidates:
        return None

    return sorted(candidates, key=lambda path: (len(path.parts), str(path)))[0]


def codex_url_schemes() -> list[str]:
    info_plist = CODEX_APP_PATH / "Contents" / "Info.plist"
    try:
        with info_plist.open("rb") as handle:
            info = plistlib.load(handle)
    except (FileNotFoundError, OSError, plistlib.InvalidFileException):
        return []

    schemes: list[str] = []
    for entry in info.get("CFBundleURLTypes", []):
        if not isinstance(entry, dict):
            continue
        for scheme in entry.get("CFBundleURLSchemes", []):
            if isinstance(scheme, str):
                schemes.append(scheme)
    return schemes


def read_optional_json_body(handler: BaseHTTPRequestHandler) -> dict[str, Any]:
    length = int(handler.headers.get("Content-Length", "0") or "0")
    if length > MAX_BODY_BYTES:
        raise ValueError("Request body is too large")
    if length <= 0:
        return {}

    try:
        payload = json.loads(handler.rfile.read(length).decode("utf-8"))
    except json.JSONDecodeError as exc:
        raise ValueError("Request body must be valid JSON") from exc

    if not isinstance(payload, dict):
        raise ValueError("JSON body must be an object")
    return payload


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
            elif path == DOCS_ROUTE_PREFIX or path.startswith(f"{DOCS_ROUTE_PREFIX}/"):
                self.send_static_from(AI_PRODUCTION_ROOT, path.removeprefix(DOCS_ROUTE_PREFIX))
            elif path == REPO_ROUTE_PREFIX or path.startswith(f"{REPO_ROUTE_PREFIX}/"):
                self.send_static_from(REPO_ROOT, path.removeprefix(REPO_ROUTE_PREFIX))
            else:
                self.send_static(path)
        except ValueError as exc:
            self.send_error_json(HTTPStatus.INTERNAL_SERVER_ERROR, str(exc))

    def do_POST(self) -> None:
        path = urlparse(self.path).path
        if path == "/api/open-workspace":
            self.open_workspace()
            return
        if path == "/api/open-codex-agent":
            self.open_codex_agent()
            return
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

    def open_codex_agent(self) -> None:
        if not is_local_client(self.client_address[0]):
            self.send_error_json(HTTPStatus.FORBIDDEN, "Codex handoff is only available from localhost")
            return

        try:
            payload = read_optional_json_body(self)
        except ValueError as exc:
            status = HTTPStatus.REQUEST_ENTITY_TOO_LARGE if "too large" in str(exc) else HTTPStatus.BAD_REQUEST
            self.send_error_json(status, str(exc))
            return

        agent_id = str(payload.get("agentId", "")).strip()
        codex_agent_id = str(payload.get("codexAgentId", agent_id)).strip()
        dry_run = bool(payload.get("dryRun", False))
        schemes = codex_url_schemes()
        command = ["open", "-a", "Codex"]

        result = {
            "ok": True,
            "status": "opened" if not dry_run else "dry-run",
            "agentId": agent_id,
            "codexAgentId": codex_agent_id,
            "codexApp": str(CODEX_APP_PATH),
            "codexUrlSchemes": schemes,
            "directAgentDeepLinkAvailable": False,
            "command": command,
            "message": "Codex opened; direct agent deep link unavailable.",
        }

        if dry_run:
            self.send_json(HTTPStatus.OK, result)
            return

        try:
            subprocess.run(command, check=True)
        except (OSError, subprocess.CalledProcessError) as exc:
            self.send_error_json(HTTPStatus.INTERNAL_SERVER_ERROR, f"Unable to open Codex: {exc}")
            return

        self.send_json(HTTPStatus.OK, result)

    def open_workspace(self) -> None:
        if not is_local_client(self.client_address[0]):
            self.send_error_json(HTTPStatus.FORBIDDEN, "Workspace open is only available from localhost")
            return

        length = int(self.headers.get("Content-Length", "0") or "0")
        if length > MAX_BODY_BYTES:
            self.send_error_json(HTTPStatus.REQUEST_ENTITY_TOO_LARGE, "Request body is too large")
            return
        payload: dict[str, Any] = {}
        if length:
            try:
                payload = json.loads(self.rfile.read(length).decode("utf-8"))
            except json.JSONDecodeError:
                self.send_error_json(HTTPStatus.BAD_REQUEST, "Request body must be valid JSON")
                return
            if not isinstance(payload, dict):
                self.send_error_json(HTTPStatus.BAD_REQUEST, "JSON body must be an object")
                return
            unsupported = set(payload) - {"action", "dryRun"}
            if unsupported or payload.get("action", "open-workspace") != "open-workspace":
                self.send_error_json(HTTPStatus.BAD_REQUEST, "Unsupported workspace action")
                return

        workspace = find_workspace()
        if workspace is None:
            self.send_json(
                HTTPStatus.NOT_FOUND,
                {
                    "ok": False,
                    "status": "needs-build",
                    "error": "No .xcworkspace found in or above the repository",
                    "repoRoot": str(REPO_ROOT),
                },
            )
            return

        dry_run = bool(payload.get("dryRun", False))
        if not dry_run:
            try:
                subprocess.run(["open", str(workspace)], check=True)
            except (OSError, subprocess.CalledProcessError) as exc:
                self.send_error_json(HTTPStatus.INTERNAL_SERVER_ERROR, f"Unable to open workspace: {exc}")
                return

        self.send_json(
            HTTPStatus.OK,
            {
                "ok": True,
                "status": "dry-run" if dry_run else "opened",
                "workspace": str(workspace),
                "workspaceName": workspace.name,
            },
        )

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

        self.send_static_from(ROOT_DIR, request_path)

    def send_static_from(self, root: Path, request_path: str) -> None:
        target = resolve_static_target(root, request_path)
        if target is None:
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
