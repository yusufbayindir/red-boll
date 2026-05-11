# TASK-0015 - Live Agent Dashboard System

## Status

QA Pass - Frontend Relay Integration Verified

## Owner

Realtime Dashboard Engineer

## PM Intent

Upgrade the dashboard from a static report page into a local Codex-like production cockpit showing active agents, statuses, event history, task state, and conversation summaries.

PM gives direction and acceptance criteria only. PM does not implement this dashboard directly.

## Scope

In:

- Clickable `.command` launcher.
- Local HTTP dashboard.
- One-second JSON polling refresh.
- Versioned live dashboard data contract.
- Stable agent status enum: `thinking`, `running`, `done`, `blocked`, `idle`.
- Sidebar active-agent mini panel.
- Animated thinking logo/indicator for agents in `thinking`.
- Click-to-select agent cards/mini cards.
- Open Codex Chat handoff for selected agents.
- Selected-agent conversation detail panel.
- Message composer with local pending outbound queue.
- Meeting notes / grooming queue / decision workflow board.
- Active agent cards.
- Conversation/event timeline.
- Task board.
- PM decision log.
- Transcript snippets.
- Report links and graphics/visual accents.
- Dashboard-visible grooming/process signals for active task status, recent meeting/debate notes, agent handoffs, and PM decisions.

Out:

- Full external database.
- Authentication.
- Remote cloud dashboard.
- Editing tasks from the UI.
- Real Codex subagent dispatch API integration.

## Acceptance Criteria

- User can double-click a script in `ai game` to open the dashboard.
- Dashboard opens through a local HTTP server, not direct `file://`, for live mode.
- Dashboard reads `data/dashboard.json`.
- Dashboard polls `data/dashboard.json` every one second with cache busting.
- User does not need to manually refresh to see agent replies/events.
- Sidebar mini status clearly shows active agents and animated thinking state.
- Clicking an agent mini card or generated agent card opens that agent's conversation/transcript in the main live panel.
- Clicking an agent card attempts the Codex chat handoff while preserving the selected-agent dashboard transcript.
- Selected-agent panel includes a one-click `Open Codex Chat` button.
- Selected-agent composer accepts a message and shows it locally as `PM dispatch pending`.
- Composer behavior is documented as local queue only until a real Codex subagent API exists.
- Meeting notes, task grooming queue, and decision flow are visible in the live UI.
- All active and completed agents are visible.
- Event/conversation history is visible.
- Task state and PM decision log are visible in dashboard UI.
- Agent state values are explicit enough for UI rendering.
- Polling clients can detect updates through `version`, `updatedAt`, and event ids.
- PM/moderator decisions, task state, transcript snippets, and artifact/report links are present in the data source.
- Dashboard chat/grooming process is linked to source-of-truth docs: task MDs, meeting MDs, decision logs, and `STATUS_BOARD.md`.
- Agent handoffs caused by active-agent limits can be represented as dashboard-visible events or status notes.
- PM can define dashboard process requirements without directly editing dashboard UI/runtime implementation.
- Links resolve.
- No build tooling required.

## Activity Log

- 2026-05-12 00:40 - Task opened by PM after user requested a Codex-app-like live UI.
- 2026-05-12 00:40 - Added `open-ai-production-dashboard.command` inside project and parent `ai game` folder.
- 2026-05-12 00:40 - Added `docs/ai-production/dashboard/data/dashboard.json` as first live data source.
- 2026-05-12 00:40 - Live Dashboard UX/Frontend worker Singer started.
- 2026-05-12 00:49 - Live dashboard frontend completed a Codex-style app shell; PM added missing `dashboard.js` and CSS support for JSON refresh/status filtering.
- 2026-05-12 00:52 - Agent State Recorder expanded `dashboard.json` with schema metadata, polling/version fields, stable status values, live agent indicators, timeline events, transcript snippets, PM decisions, task rows, and artifact links.
- 2026-05-12 00:52 - Added `dashboard.schema.json` and updated dashboard data README for UI/runtime consumers.
- 2026-05-12 00:58 - User clarified PM must not code directly; realtime dashboard engineer owns implementation.
- 2026-05-12 00:58 - Realtime dashboard engineer added stable sidebar live panel, topbar refresh meter, main live room markup, and JSON-driven task/decision/event targets.
- 2026-05-12 00:58 - Realtime dashboard engineer replaced 3.5s refresh with one-second cache-busted polling and wired sidebar agents, latest messages, agent grid, task board, PM decisions, and event streams to `data/dashboard.json`.
- 2026-05-12 00:58 - Realtime dashboard engineer added Codex-like live styling, active-agent mini cards, animated thinking logo, new-event glow, and responsive live panels.
- 2026-05-12 01:05 - User added priority requirement for Codex-app-like agent card click-through, selected-agent transcript, message composer, local/pending dispatch feedback, and workflow/grooming area.
- 2026-05-12 01:05 - Realtime dashboard engineer added selected-agent detail markup, transcript panel, message composer, and workflow board to the live dashboard.
- 2026-05-12 01:05 - Realtime dashboard engineer wired sidebar mini agents, live agent cards, and generated agent grid cards to select an agent and refresh the displayed transcript on each poll.
- 2026-05-12 01:05 - Realtime dashboard engineer added browser-local pending outbound message storage using `localStorage` key `redBallPendingOutboundMessages`; real agent dispatch remains a documented integration gap.
- 2026-05-12 01:05 - Dashboard data updated with per-agent conversation seed entries, meeting notes, grooming queue, and PM decision entry for local outbound queue behavior.
- 2026-05-12 01:08 - Realtime dashboard engineer validated JSON/schema parsing, JS syntax, launcher shell syntax, local HTTP serving, headless Chrome agent selection, local queued-message feedback, and no-refresh polling update behavior.
- 2026-05-12 - Product Ops linked dashboard chat/grooming requirements to the operating model: dashboard should expose task status, agent state, meeting/debate notes, handoffs, and PM decisions while source-of-truth records remain in docs.
- 2026-05-12 - Local Relay Backend Engineer added a stdlib Python relay server for static dashboard serving, `/api/state`, `/api/events`, `/api/messages`, and relay-owned `outbox.json` / `events-live.json` queue files.
- 2026-05-12 - Launcher now starts `docs/ai-production/dashboard/server.py`, reuses an existing relay when `/api/health` responds, and opens the localhost dashboard URL.
- 2026-05-12 - Frontend Relay Integration Fixer wired the dashboard frontend to prefer `/api/state`, submit composer messages through `POST /api/messages`, consume `/api/events` SSE updates when available, and keep the browser-local draft queue only as a static/fallback path.
- 2026-05-12 - Dashboard Workspace Menu Developer added a compact `Red Ball` topbar menu with a silent `Build Workspace` action, backed by `POST /api/open-workspace` on the local relay. The relay searches for an `.xcworkspace` in/above the repo and opens it through macOS `open` with an argument list, returning `needs-build` without alerts/toasts/notifications when none exists.
- 2026-05-12 01:50 - Dashboard Roster + Codex Chat Handoff Developer added Raman (`019e1933-977f-71d0-8a91-fe99f5d93691`) and the recent Goodall, Kierkegaard, Hubble, and Tesla completions to the live dashboard roster with task refs, transcript snippets, artifact refs, and Codex handoff metadata.
- 2026-05-12 01:50 - Local Codex.app inspection found `CFBundleURLSchemes = codex`, but no documented/discoverable agent-specific chat deep-link format. Dashboard handoff now uses `POST /api/open-codex-agent`; the relay opens Codex.app with `open -a Codex` and returns `Codex opened; direct agent deep link unavailable`.
- 2026-05-12 02:15 - Dashboard roster synced after Sprint 01 docs follow-up. Added James (`019e1942-4970-78c1-9e69-045cdce75024`), Arendt (`019e194b-43cd-7e33-b11f-603706ecc1a1`), Hume (`019e194b-751f-7342-8cbf-cc929c88d55a`), and Wegener (`019e1950-0889-79e1-8fff-5de3ca3e9fcf`) to `dashboard.json` live roster, historical roster, transcripts, events, tasks, and decisions. Manual roster/state sync remains existing debt under TD-0007 and TD-0008; no new debt entry added.

## Changed Files

- `open-ai-production-dashboard.command`
- `../open-ai-production-dashboard.command`
- `docs/ai-production/dashboard/data/dashboard.json`
- `docs/ai-production/dashboard/data/README.md`
- `docs/ai-production/dashboard/data/dashboard.schema.json`
- `docs/ai-production/dashboard/data/outbox.json`
- `docs/ai-production/dashboard/data/events-live.json`
- `docs/ai-production/dashboard/server.py`
- `docs/ai-production/dashboard/index.html`
- `docs/ai-production/dashboard/styles.css`
- `docs/ai-production/dashboard/dashboard.js`
- `docs/ai-production/technical-debt/TECHNICAL_DEBT_REGISTER.md`
- `docs/ai-production/tasks/TASK-0011-agent-dashboard-site.md`
- `docs/ai-production/OPERATING_MODEL.md`
- `docs/ai-production/STATUS_BOARD.md`
- `docs/ai-production/meetings/grooming-meeting-template.md`
- `docs/ai-production/templates/task-intake-template.md`
- `docs/ai-production/templates/decision-log-template.md`
- `docs/ai-production/templates/daily-overnight-production-run-template.md`
- This task file.

## Verification

- Launcher files are executable.
- `dashboard.json` parses as valid JSON.
- `dashboard.schema.json` parses as valid JSON.
- `dashboard.js` passes `node --check`.
- `open-ai-production-dashboard.command` and parent launcher pass `zsh -n`.
- Local HTTP server served `index.html`, `dashboard.js`, `styles.css`, and `data/dashboard.json`.
- Headless Chrome smoke confirmed sidebar mini agents render, thinking logos render, clicking Franklin opens Franklin's transcript, message composer queues a local `PM dispatch pending` message, and workflow notes render.
- No-refresh polling smoke temporarily appended a test Franklin event to `dashboard.json`; the already-open page showed it in both event history and selected-agent transcript within the polling window, then the data file was restored.
- Frontend relay fix verification passed `node --check docs/ai-production/dashboard/dashboard.js`.
- Workspace menu verification passed `node --check docs/ai-production/dashboard/dashboard.js`, `python3 -m py_compile docs/ai-production/dashboard/server.py`, and `POST /api/open-workspace` smoke with `open` stubbed to avoid launching Xcode. The relay found `Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace`; the no-workspace path returns `404 needs-build` without browser alerts, toasts, or OS notifications.
- Roster/handoff verification passed JSON parse, schema validation through `npx --yes ajv-cli@5 validate -s docs/ai-production/dashboard/data/dashboard.schema.json -d docs/ai-production/dashboard/data/dashboard.json --spec=draft2020 --strict=false`, `node --check docs/ai-production/dashboard/dashboard.js`, `python3 -m py_compile docs/ai-production/dashboard/server.py`, `GET /api/state` smoke confirming Raman is present, and `POST /api/open-codex-agent` dry-run smoke confirming `open -a Codex` fallback with `directAgentDeepLinkAvailable: false`.
- Sprint 01 docs follow-up roster sync verification passed `node` JSON parse, AJV schema validation, and a dashboard state assertion confirming James, Arendt, Hume, and Wegener are present in both `liveAgents` and `agents`, with transcript and conversation records.
- Local relay smoke confirmed `POST /api/messages` writes `outbox.json` / `events-live.json` and `GET /api/state` sees the queued message.
- Headless Chrome CDP smoke submitted the dashboard composer through the browser, saw `PM dispatch queued via relay`, and confirmed the relay-queued message rendered without manual refresh; test relay records were then removed.

## QA Notes

- 2026-05-12 01:20 QA verdict: **NEEDS FIX / QA BLOCKED**. Relay backend direct smoke passes, but the dashboard frontend does not call `/api/messages`, `/api/state`, or `/api/events`; browser-sent messages stay in `localStorage` and do not reach `outbox.json` or relay state. See `docs/ai-production/reports/TASK-0015-dashboard-qa-report.md`.
- 2026-05-12 01:20 frontend relay fix applied: dashboard messages now queue through `POST /api/messages` when opened from the local server, relay state is read through `/api/state`, and SSE `/api/events` updates are applied live. Prior QA BLOCKED finding is fixed in code and awaits re-QA.
- 2026-05-12 01:25 Re-QA verdict: **PASS**. Browser relay test opened the dashboard through the local server, selected `pm-moderator`, submitted a composer message, observed `PM dispatch queued via relay`, confirmed `POST /api/messages`, `/api/state`, and `/api/events` activity, verified `outbox.json` changed, and confirmed `/api/state` saw both outbox and live event without manual refresh. Test records were removed and `outbox.json` / `events-live.json` returned to empty arrays. See `docs/ai-production/reports/TASK-0015-dashboard-re-qa-report.md`.
- 2026-05-12 01:56 roster/Codex handoff QA verdict: **PASS**. Isolated relay smoke on port 8877 confirmed `/api/state` includes Raman plus Goodall/Kierkegaard/Hubble/Tesla, `/api/open-codex-agent` dry-run returns `directAgentDeepLinkAvailable: false`, and `/api/open-workspace` still returns the discovered Unity workspace with `open` stubbed. Browser fallback verification confirmed Raman is visible in the left roster, selecting Raman opens the internal transcript with `Open Codex Chat`, the fallback handoff updates in-panel status silently with no alerts/toasts/notifications, and the Red Ball > Build Workspace menu remains visible. See `docs/ai-production/reports/TASK-0015-roster-codex-handoff-qa.md`.
- Local relay syntax check: `python3 -m py_compile docs/ai-production/dashboard/server.py`.
- JSON parse check: `dashboard.json`, `outbox.json`, and `events-live.json`.
- Smoke coverage should include `GET /api/health`, `GET /api/state`, `GET /api/events`, and `POST /api/messages`.
- Current backend gap: dashboard-composed messages are queued as `PM dispatch pending`; PM still needs a real dispatcher from `outbox.json` to actual Codex subagents.

## Repo Controller Notes

Pending.

## PM Closure

Pending.
