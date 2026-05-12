# Dashboard Button QA Report

Date: 2026-05-12
Owner: Dashboard Web QA Runner
Task: `TASK-0034`

## Summary

Local dashboard QA was run against `docs/ai-production/dashboard/server.py` on `http://127.0.0.1:8767/index.html` because ports `8765` and `8766` were already occupied. Chrome/Playwright automation verified the main relay actions, selected-agent composer, filters, in-page navigation, workspace opener, and SSE/polling state updates.

Primary blocker: production doc/task/report/artifact links resolve to 404 from the local dashboard server.

## Test Commands

- `python3 docs/ai-production/dashboard/server.py --host 127.0.0.1 --port 8767`
- `curl -sS http://127.0.0.1:8767/api/health`
- `/tmp/red-ball-dashboard-qa` Playwright install/run using system Chrome at `/Applications/Google Chrome.app/Contents/MacOS/Google Chrome`
- Temporary test data backup/restore:
  - `docs/ai-production/dashboard/data/outbox.json`
  - `docs/ai-production/dashboard/data/events-live.json`

## Environment

- Dashboard URL: `http://127.0.0.1:8767/index.html`
- Health endpoint: PASS, returned `ok: true`
- Browser automation: Playwright with headless Google Chrome
- Screenshot artifact: `/tmp/red-ball-dashboard-qa/dashboard-qa.png`
- Test pollution cleanup: PASS, `outbox.json` and `events-live.json` restored to `[]`

## PASS List

| Area | Selector / Label | Result | Evidence |
| --- | --- | --- | --- |
| Initial load / relay | `#dashboardRefreshStatus` | PASS | UI showed `Live - relay SSE`; `/api/state` returned 200 and `/api/events` opened with 200. |
| Agent card transcript | `#agentGrid [data-agent-select]` / `Codex` card | PASS | Clicking a non-selected agent changed `#selectedAgentName` to `Codex` and populated `#agentTranscript`. |
| Open Codex Chat | `#openCodexChatButton` / `Open Codex Chat` | PASS | POST `/api/open-codex-agent` returned 200 with `directAgentDeepLinkAvailable: false`; UI displayed `Codex opened; direct agent deep link unavailable.` |
| Message selected agent | `#agentMessageForm button[type="submit"]` / `Send to queue` | PASS | POST `/api/messages` returned 201; message appeared in `/api/state.outbox`, selected transcript, and live events. |
| Red Ball menu | `#redBallMenu summary` / `Red Ball` | PASS | Details element opened. |
| Build Workspace | `#openWorkspaceButton` / `Build Workspace` | PASS | POST `/api/open-workspace` returned 200 and opened `Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace`. |
| Filter/status buttons | `[data-filter]` / All, Thinking, Talking, Done | PASS | Active class, `aria-pressed`, visible count, and hidden cards stayed consistent. |
| Section navigation | `.rail-nav a` | PASS | `#overview`, `#live-json`, `#agents`, `#tasks`, `#history`, `#decisions`, `#reports` updated `location.hash` and target sections existed. |
| Refresh/polling/SSE | `/api/state`, `/api/events` | PASS | Relay status stayed live, periodic state polling continued, queued message appeared without manual page reload. |

## Broken / Blocked Buttons And Links

### P1 - Dashboard document/task/report links return 404

- Selectors / labels:
  - `.brand` / `Red Ball AI Production Console`
  - `.source-links a` / `Agent record`, `Status board`, `Roster`
  - `.section-head a.text-link` / `TASK-0011`, `Status board`, `Source markdown`
  - `#taskGrid a` / task links such as `TASK-0001`, `TASK-0012`, `TASK-0027`
  - `#reports .resource-card` / report cards such as `Asset research`, `QA checklist`
  - `.doc-links a` / `Protocol`, `Agent dashboard markdown`, `Status board`, `TASK-0011`, `TASK-0012`
- Expected: links should open the referenced local markdown artifact from the dashboard server.
- Actual: all checked production doc/task/report links returned HTTP 404, for example:
  - `http://127.0.0.1:8767/AGENT_DASHBOARD.md`
  - `http://127.0.0.1:8767/tasks/TASK-0011-agent-dashboard-site.md`
  - `http://127.0.0.1:8767/reports/qa-checklist.md`
- Suspected cause: `server.py` serves static files from `docs/ai-production/dashboard` only. Browser resolution of `../tasks/...`, `../reports/...`, and `../STATUS_BOARD.md` escapes to server-root paths like `/tasks/...`, but `send_static()` maps them back under `dashboard/`, where those files do not exist.
- Priority: P1, user-facing navigation is broken across task/report/artifact actions.

## Functional Risks

### P2 - Agent card selection also opens Codex

- Selector / label: `#agentGrid [data-agent-select]` / any agent card.
- Expected: per QA scope, agent cards should open the agent transcript.
- Actual: transcript opens, but the click also sends POST `/api/open-codex-agent` and launches Codex as a side effect.
- Suspected cause: `makeAgentSelectable()` calls `openCodexAgent(agent, { passive: true })` on every card/list click. `passive` only affects button disabled state; it does not suppress the handoff request.
- Priority: P2, because normal transcript navigation can unexpectedly open an external app.

### P3 - Workspace success feedback is easy to miss

- Selector / label: `#openWorkspaceButton` / `Build Workspace`.
- Expected: user should see success/failure feedback after clicking.
- Actual: endpoint succeeds and the menu closes; automation saw the payload but visible status text was not observable after close.
- Suspected cause: `openBuildWorkspace()` writes `workspaceOpenStatus` then removes the `open` attribute from `#redBallMenu`, hiding the status area.
- Priority: P3, not a functional failure but weak feedback.

## Console / Network Errors

- API network failures: none.
- Request failures: none.
- HTTP 400+ responses: repeated 404s from the broken production links listed above, plus `/favicon.ico` 404 during page load.
- Console errors: Chrome logged `Failed to load resource: the server responded with a status of 404 (Not Found)` for the favicon and broken link fetches.

## Priority Order For Fix Agent

1. Fix local dashboard static routing or link generation so task/report/status/artifact markdown links return 200.
2. Decide whether agent-card transcript selection should stop opening Codex automatically; if yes, limit Codex handoff to the explicit `Open Codex Chat` button.
3. Keep `Build Workspace` endpoint behavior, but make the success/failure message visible after the menu closes or do not close the menu until feedback is read.

## Technical Debt

Technical debt added: none.
