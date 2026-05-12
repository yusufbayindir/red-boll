# Dashboard Button Fix Report

Date: 2026-05-12

## Scope

Fixed the dashboard follow-up issues from `TASK-0034` in:

- `docs/ai-production/dashboard/server.py`
- `docs/ai-production/dashboard/dashboard.js`
- `docs/ai-production/dashboard/index.html`
- `docs/ai-production/dashboard/styles.css`

## Fix Summary

1. P1 docs links:
   - Added safe docs file serving through `/docs/<path>`.
   - Kept path resolution inside `docs/ai-production/` with normalized-path and root-boundary checks.
   - Normalized dashboard links so task/report/artifact/root markdown paths resolve through `/docs/...`.
2. P2 agent card click behavior:
   - Agent card and mini-agent selection now only updates the selected transcript panel.
   - Codex launch remains behind the explicit `Open Codex Chat` button.
3. P3 Build Workspace feedback:
   - Added a persistent inline topbar status element.
   - Build Workspace success/error text now remains visible after the Red Ball menu closes.

## Validation

- PASS: `node --check docs/ai-production/dashboard/dashboard.js`
- PASS: `python3 -m py_compile docs/ai-production/dashboard/server.py`

## Smoke

- PASS: `GET /api/health`
- PASS: docs link path resolves through `/docs/...`
- PASS: agent card click path no longer calls Codex handoff route in the frontend code path; selection stays local to transcript detail
- PASS: `POST /api/open-codex-agent` dry-run returns success payload
- PASS: Build Workspace dry-run/open flow returns visible inline status target in the top bar

## Test Data Cleanup

- `docs/ai-production/dashboard/data/outbox.json` left unchanged
- `docs/ai-production/dashboard/data/events-live.json` left unchanged

Technical debt added: none.
