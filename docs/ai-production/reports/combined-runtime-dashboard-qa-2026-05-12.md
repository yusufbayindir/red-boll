# Combined Runtime + Dashboard QA Report

Date: 2026-05-12
Owner: Combined QA Runner - Runtime Assets + Dashboard Fix
Task: `TASK-0036-combined-runtime-dashboard-qa`

## Verdict

PASS.

Runtime Sprint02 assets, existing Level 1-13 polish evidence, Unity smoke evidence, and dashboard fix smoke all pass. Browser click automation was not rerun in this pass because no local Playwright/Puppeteer/browser MCP runner was available in the current tool surface; the agent-card/Codex behavior was verified by static JS event-path review plus explicit endpoint dry-run. This is acceptable for this scope because browser smoke was requested only if possible.

## Runtime Asset Pack

PASS: `Assets/Resources/Generated/Sprint02/` exists and contains eight PNGs plus Unity `.meta` files:

- `mastery_badge_clear.png`
- `mastery_badge_all_coins.png`
- `mastery_badge_clean_run.png`
- `lift_platform_polish.png`
- `crumbling_tile_polish.png`
- `warning_spark.png`
- `checkpoint_spark.png`
- `dust_sparkle.png`

Evidence:

- `docs/ai-production/reports/runtime-generated-asset-pack-report.md`
- `docs/ai-production/assets-manifest.md`
- `Assets/Scripts/RedBallRuntime.cs` registers Sprint02 generated sprites through `Generated/Sprint02/...` fallback-safe loading.

## Existing Level Polish

PASS: Level 1-13 polish helper/static code is present.

Evidence:

- `Assets/Scripts/RedBallRuntime.cs` has `AddRouteSparkle(...)` placements across Level 1-13.
- `Assets/Scripts/RedBallRuntime.cs` uses `dust_sparkle`, `warning_spark`, `checkpoint_spark`, and `lift_platform_polish`.
- `Assets/Tests/EditMode/RedBallSmokeEditModeTests.cs` includes `ExistingLevelExposesSprint02PolishMarkers(...)` coverage for Level 1-13.
- `docs/ai-production/reports/existing-level-polish-pass-report.md` records the decorative/readability-only Level 1-13 pass.

## Unity Evidence

PASS: Existing Unity import/compile and smoke evidence is present and current for the combined scope.

Evidence files:

- Import/compile: `Logs/task0035-import-compile.log`, batchmode exit 0.
- EditMode: `TestResults/task0035-editmode-results.xml`, 19/19 passed, start `2026-05-12 08:09:33Z`.
- PlayMode: `TestResults/task0035-playmode-results.xml`, 5/5 passed, start `2026-05-12 08:10:02Z`.
- Earlier asset-pack smoke: `TestResults/task0033-editmode-results.xml` 6/6 passed and `TestResults/task0033-playmode-results.xml` 5/5 passed.

No new Unity run was required because TASK-0035 already supersedes TASK-0033 for Level 1-13 polish plus retained Sprint02 runtime asset coverage.

## Dashboard Fix Smoke

PASS.

Commands run:

- `node --check docs/ai-production/dashboard/dashboard.js` -> PASS.
- `python3 -m py_compile docs/ai-production/dashboard/server.py` -> PASS.
- `python3 docs/ai-production/dashboard/server.py --host 127.0.0.1 --port 8791` -> PASS. Ports `8765` and `8766` were already in use, so this isolated run used `8791`.

HTTP checks:

| Check | Result |
| --- | --- |
| `GET /api/health` | 200, `ok: true` |
| `GET /docs/tasks/TASK-0034-dashboard-button-qa-and-fixes.md` | 200, markdown served |
| `curl --path-as-is /docs/../../..` | 404, traversal blocked |
| `POST /api/open-codex-agent` dry-run | 200, explicit Codex endpoint returns success payload |
| `POST /api/open-workspace` dry-run | 200, finds `Unity-iPhone.xcworkspace` |

Important note: a first curl without `--path-as-is` normalized `/docs/../../..` client-side and hit `/`, returning 200. That result was discarded; the valid traversal test is the `--path-as-is` 404.

## Agent Card / Codex Behavior

PASS by static route review and endpoint dry-run:

- `makeAgentSelectable(...)` only calls `setSelectedAgent(agent.id)` on card/list click.
- `openCodexChatButton` is the separate UI path that calls `openCodexAgent(agent)`.
- `/api/open-codex-agent` dry-run returned 200 without launching Codex.

## Build Workspace Inline Status

PASS by static route review and endpoint dry-run:

- `setWorkspaceStatus(...)` writes both `#workspaceOpenStatus` and persistent `#workspaceTopbarStatus`.
- `openBuildWorkspace()` uses `/api/open-workspace`.
- Dry-run returned the expected workspace: `Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace`.

## Test Data Cleanliness

PASS: `outbox.json` and `events-live.json` remained clean.

Before/after SHA-256:

- `docs/ai-production/dashboard/data/outbox.json`: `37517e5f3dc66819f61f5a7bb8ace1921282415f10551d2defa5c3eb0985b570`
- `docs/ai-production/dashboard/data/events-live.json`: `37517e5f3dc66819f61f5a7bb8ace1921282415f10551d2defa5c3eb0985b570`

Both files remained 3 bytes, consistent with `[]\n`.

## Residual Risk

No new blocker found. The only residual limitation in this combined pass is that live browser click automation was not available in the current runner; existing TASK-0034 browser QA plus this pass's static/HTTP verification cover the dashboard fix intent.

Technical debt added: none.
