# TASK-0036 - Combined Runtime Dashboard QA

## Status

Completed - PASS

## Owner

Combined QA Runner - Runtime Assets + Dashboard Fix

## PM Intent

Run a no-code-change QA pass over the Sprint02 runtime asset/polish work and the dashboard button fixes, then produce a single PASS/BLOCKED result with clean test data.

## Scope

In:

- Verify `Assets/Resources/Generated/Sprint02` asset presence.
- Verify Level 1-13 polish helper/static code and test coverage.
- Confirm Unity compile/import, EditMode, and PlayMode evidence exists or rerun if needed.
- Run dashboard syntax checks.
- Start local dashboard server and smoke `/api/health`, safe docs routing, traversal blocking, Codex handoff separation, and Build Workspace route/status behavior.
- Keep dashboard `outbox.json` and `events-live.json` clean.

Out:

- Product/runtime/dashboard code changes.
- PM-authored implementation changes.
- Leaving dashboard queue/event test data behind.

## QA Result

PASS.

Combined report:

- `docs/ai-production/reports/combined-runtime-dashboard-qa-2026-05-12.md`

## Evidence Summary

- Sprint02 asset folder exists with eight PNGs and `.meta` files.
- Level 1-13 polish code exists in `Assets/Scripts/RedBallRuntime.cs`.
- EditMode coverage includes Level 1-13 Sprint02 polish marker cases.
- Unity evidence present: TASK-0035 import/compile PASS, EditMode 19/19 PASS, PlayMode 5/5 PASS.
- Dashboard checks passed:
  - `node --check docs/ai-production/dashboard/dashboard.js`
  - `python3 -m py_compile docs/ai-production/dashboard/server.py`
  - `GET /api/health` 200
  - `GET /docs/tasks/TASK-0034-dashboard-button-qa-and-fixes.md` 200
  - `curl --path-as-is /docs/../../..` 404
  - `/api/open-codex-agent` dry-run 200
  - `/api/open-workspace` dry-run 200
- `outbox.json` and `events-live.json` hashes unchanged and files remain clean.

## Notes

Ports `8765` and `8766` were occupied during this run; the isolated dashboard server smoke used `127.0.0.1:8791`.

Live browser click automation was not rerun because no local Playwright/Puppeteer/browser MCP runner was available. Static JS review confirms agent-card selection no longer auto-calls Codex and `Open Codex Chat` remains the explicit handoff path.

## Technical Debt

Technical debt added: none.
