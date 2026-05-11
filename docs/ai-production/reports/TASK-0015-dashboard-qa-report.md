# TASK-0015 Dashboard QA Report

Role: Dashboard QA / Release Verifier  
Workspace: `/Users/yusufbayindir/Desktop/ai game/red_ball`  
Run time: 2026-05-12 01:13-01:20 Europe/Istanbul

## Verdict

**NEEDS FIX / QA BLOCKED**

The relay backend and launcher scripts pass syntax and direct API smoke tests. The dashboard UI also renders, polls `data/dashboard.json`, opens agent transcripts, and shows local pending-message feedback.

Blocking issue: the frontend message composer is not wired to the relay API. Browser QA showed that sending a message updates `localStorage` and the transcript UI, but does **not** call `/api/messages`; therefore `outbox.json` is not updated through the UI, `/api/state` does not see the browser-sent message, and relay-backed state/SSE behavior is not integrated into the dashboard runtime.

## Test Commands

```sh
python3 -m py_compile docs/ai-production/dashboard/server.py
node --check docs/ai-production/dashboard/dashboard.js
python3 -m json.tool docs/ai-production/dashboard/data/dashboard.json >/dev/null
python3 -m json.tool docs/ai-production/dashboard/data/dashboard.schema.json >/dev/null
python3 -m json.tool docs/ai-production/dashboard/data/outbox.json >/dev/null
python3 -m json.tool docs/ai-production/dashboard/data/events-live.json >/dev/null
npx --yes ajv-cli@5 validate -s docs/ai-production/dashboard/data/dashboard.schema.json -d docs/ai-production/dashboard/data/dashboard.json --spec=draft2020 --strict=false
zsh -n "/Users/yusufbayindir/Desktop/ai game/red_ball/open-ai-production-dashboard.command"
zsh -n "/Users/yusufbayindir/Desktop/ai game/open-ai-production-dashboard.command"
```

Additional smoke tests started `docs/ai-production/dashboard/server.py` on local test ports and used `curl` plus headless Chrome / Playwright Core with system Google Chrome.

## Pass / Fail

| Area | Result | Notes |
| --- | --- | --- |
| `server.py` syntax | PASS | `python3 -m py_compile` passed. |
| `dashboard.js` syntax | PASS | `node --check` passed. |
| JSON parse | PASS | `dashboard.json`, `dashboard.schema.json`, `outbox.json`, and `events-live.json` parse. |
| Schema validation | PASS | `ajv-cli` reported `dashboard.json valid`; date-time formats were ignored by the CLI without extra format plugin. |
| Relay API direct smoke | PASS | `GET /api/health`, `GET /api/state`, and direct `POST /api/messages` succeeded. Direct POST appended an outbox item and live event; `/api/state` saw both. Test data was restored afterward. |
| Static serving | PASS | `GET /index.html` returned the dashboard HTML with the expected title and `dashboard.js` script. |
| Launcher scripts | PASS | Both launcher files exist and pass `zsh -n`. Repo script starts `server.py`; parent script delegates to the repo script, so it is not the old static server. |
| Agent card transcript | PASS | Headless browser click on `pm-moderator` opened the selected-agent transcript. |
| Message pending UI | PASS | Composer showed `Queued for PM. PM dispatch pending.` and rendered the message in the transcript. |
| Frontend `/api/messages` integration | FAIL | Browser send made zero `/api/messages` calls and did not update relay `outbox.json`. |
| Frontend `/api/state` integration | FAIL | Browser made zero `/api/state` calls; state did not see UI-sent messages. |
| SSE `/api/events` integration | FAIL / GAP | Browser made zero `/api/events` calls. Current frontend uses 1s polling against `data/dashboard.json`, not relay state/SSE. |
| No manual refresh polling | PARTIAL PASS | Browser observed repeated `data/dashboard.json` requests; no relay-backed state polling was observed. |
| Visual QA | PASS with caveat | Desktop 1440x1000 and mobile 390x844 had no horizontal scroll or detected text overflow. Thinking indicators are visible. Codex-app-like conversation feel is acceptable, but relay disconnect blocks release. |

## Key Evidence

Direct relay smoke after `POST /api/messages`:

```json
{
  "ok": true,
  "agentId": "pm-moderator",
  "status": "PM dispatch pending",
  "eventKind": "pm-dispatch-queued"
}
```

Headless browser request counts after sending through the dashboard UI:

```json
{
  "dashboardJson": 5,
  "apiState": 0,
  "apiMessages": 0,
  "apiEvents": 0
}
```

Relay state before and after browser send:

```json
{
  "beforeOutboxCount": 0,
  "afterOutboxCount": 0,
  "apiOutboxChangedFromFrontend": false
}
```

## Findings

1. **Blocking: frontend composer bypasses relay outbox.**  
   `dashboard.js` stores outbound messages in `localStorage` under `redBallPendingOutboundMessages`. It never calls `/api/messages`, so messages sent from the UI are not durable in `outbox.json` and not visible through `/api/state`.

2. **Blocking: frontend does not consume relay state or SSE.**  
   The current runtime fetches `data/dashboard.json?t=...` every second. This proves static JSON polling works, but does not verify the relay-owned `/api/state` or `/api/events` path from the browser.

3. **Minor: server does not implement `HEAD` for static files.**  
   `GET /index.html` works, so this is not release-blocking for normal launcher/browser use.

## Remaining Gaps

- Wire dashboard send flow to `POST /api/messages` when served by the relay, with localStorage as fallback only if the API is unavailable.
- Merge relay `outbox` / `liveEvents` from `/api/state` into the selected-agent transcript and event list.
- Use `/api/events` SSE or documented `/api/state` polling for relay-backed live updates, then retest that an already-open browser updates without manual refresh after a relay-side message.
- Add a small automated smoke script for launcher + API + browser message flow so this regression is easy to catch.

## Changed Files

- `docs/ai-production/reports/TASK-0015-dashboard-qa-report.md`
- `docs/ai-production/tasks/TASK-0015-live-agent-dashboard-system.md`
