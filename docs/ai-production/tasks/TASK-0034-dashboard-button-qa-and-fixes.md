# TASK-0034 - Dashboard Button QA And Fixes

## Status

Fix Complete - Awaiting PM Review

## Owner

Dashboard Button Fix Developer

## PM Intent

Verify dashboard buttons and links from the running local web dashboard without changing production code, then hand off a concrete bug list for a fix agent.

## Scope

In:

- Start or attach to `docs/ai-production/dashboard/server.py`.
- Browser automation for dashboard button/action behavior.
- Agent transcript selection.
- Codex handoff button.
- Selected-agent composer and `/api/messages` outbox/event behavior.
- Red Ball menu and Build Workspace action.
- Filter/status buttons, section navigation, task/report/artifact links.
- Refresh/polling/SSE state behavior.
- Console/network error capture.
- QA report and task tracking docs.

Out:

- Code changes.
- Fixing the broken dashboard links.
- Changing server routing or UI behavior.
- Leaving test queue data in `outbox.json` or `events-live.json`.

## QA Result

QA found one P1 broken navigation class and two lower-priority UX/behavior risks.

Report:

- `docs/ai-production/reports/dashboard-button-qa-2026-05-12.md`

## Bug List

| Priority | Area | Status | Summary |
| --- | --- | --- | --- |
| P1 | Task/report/artifact links | Broken | Local dashboard doc links return 404 because the server root is `docs/ai-production/dashboard` while links resolve to `/tasks`, `/reports`, and root markdown paths. |
| P2 | Agent card selection | Functional risk | Agent card clicks open transcripts but also POST `/api/open-codex-agent` and launch Codex as a side effect. |
| P3 | Build Workspace feedback | UX risk | `/api/open-workspace` succeeds, but the success text is hidden when the Red Ball menu closes. |

## PASS Coverage

- Local relay health endpoint returned `ok: true`.
- Dashboard loaded with live relay/SSE status.
- Agent card transcript selection works when clicking a non-selected agent.
- `Open Codex Chat` button posts to `/api/open-codex-agent` and returns fallback copy for unavailable direct agent deep links.
- Selected-agent composer posts to `/api/messages`, writes outbox, writes live event, and updates the transcript without a reload.
- Red Ball menu opens.
- Build Workspace posts to `/api/open-workspace` and opens `Unity-iPhone.xcworkspace`.
- Filter/status buttons update visible cards and counts.
- In-page section navigation updates hash and targets existing sections.
- SSE/polling remains live after queue updates.

## QA Data Cleanup

- Temporary `/api/messages` test message was removed.
- `docs/ai-production/dashboard/data/outbox.json` restored to `[]`.
- `docs/ai-production/dashboard/data/events-live.json` restored to `[]`.

## Fix Handoff

Recommended fix order:

1. Fix link serving or URL generation for docs outside `dashboard/`.
2. Remove or gate automatic Codex handoff from agent-card selection if transcript-only click behavior is desired.
3. Improve Build Workspace success feedback visibility.

## Fix Implementation Result

- Added a safe `/docs/<path>` static-serving path on the local dashboard server with path normalization and root-boundary checks.
- Normalized dashboard document links to `/docs/...` at load/render time so task, report, artifact, and root markdown links open from the local dashboard server.
- Restricted agent-card selection clicks to transcript selection only; Codex launch remains on the explicit `Open Codex Chat` button.
- Added persistent inline workspace status in the top bar so Build Workspace success remains visible after the Red Ball menu closes.
- Validation run required by task:
  - `node --check docs/ai-production/dashboard/dashboard.js`
  - `python3 -m py_compile docs/ai-production/dashboard/server.py`

Report:

- `docs/ai-production/reports/dashboard-button-fix-report.md`

## Technical Debt Added

Technical debt added: none.
