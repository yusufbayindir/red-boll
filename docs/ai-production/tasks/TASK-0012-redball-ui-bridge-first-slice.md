# TASK-0012 - RedBall UI Bridge First Slice

## Status

Implementation Complete - QA Pending

## Owner

PM, Developer UI / Polish, QA, Git Repo Controller

## PM Intent

Start implementing the approved UI direction without exploding the codebase. The first slice should create a clean UI ownership boundary and fix the level-select continue-target highlight issue.

## Scope

In:

- Introduce a small `RedBallUi` bridge/controller or equivalent UI boundary.
- Keep `RedBallGame` as gameplay/state orchestration.
- Preserve existing UI behavior while moving toward the Playable Hill Menu direction.
- Fix level-select highlight semantics so the next continue target is visible while browsing level select.
- Keep `VirtualJoystick` and `HoldButton` behavior intact.
- Update this MD before and after every code step.

Out:

- Full visual redesign in one pass.
- New paid assets or package dependencies.
- Gameplay/physics changes.
- Save key migrations.
- Mastery badges.
- Ads.

## Acceptance Criteria

- UI code has a clearer boundary than before.
- Existing main menu, HUD, and level select still work.
- Level select visually distinguishes locked, completed, available, and continue-target levels.
- Continue/start, level select/back, home/menu, restart, joystick, and jump still function.
- Unity 6000.4.5f1 compile passes.
- Task MD records changed files and verification.

## Risk Check

- Shared systems touched: `RedBallRuntime.cs`, possible new UI script file, UI references, screen switching, level button refresh.
- Regression risks: broken callbacks, stale joystick input, missing UI references, level buttons no longer starting the right level, text/icon overlap.
- Rollback idea: keep first slice mostly as extracted/bounded helper code and a targeted highlight fix.

## Activity Log

- 2026-05-12 00:06 - Task created by PM after UI/Menu Designer and Developer UI/Polish handoffs.
- 2026-05-11 23:45 +0300 - Developer UI/Polish started implementation. Intended touches: this task MD, UI-related sections of `Assets/Scripts/RedBallRuntime.cs`, and new `Assets/Scripts/RedBallUi.cs` plus `.meta` if needed.
- 2026-05-11 23:45 +0300 - Risk check: extracting UI ownership can break callbacks, joystick/jump references, screen visibility, and level button state; first slice will keep gameplay/state APIs in `RedBallGame` and preserve existing UI behavior while moving construction/refresh behind a small bridge.
- 2026-05-12 00:13 - PM stopped the stalled UI worker before code edits. PM is taking over the first slice with a narrower implementation: add UI state helpers and fix continue-target highlight without a broad UI extraction.
- 2026-05-12 00:15 - PM added `RedBallUi` level-button state helper and changed `RefreshLevelButtons()` to highlight the level-select continue target as `Devam`.
- 2026-05-12 00:18 - PM ran Unity 6000.4.5f1 batch import/compile successfully after the UI helper change.

## Changed Files

- `Assets/Scripts/RedBallRuntime.cs`
- `Assets/Scripts/RedBallUi.cs`
- `Assets/Scripts/RedBallUi.cs.meta`
- `docs/ai-production/tasks/TASK-0012-redball-ui-bridge-first-slice.md`

## Verification

- Unity 6000.4.5f1 batch import/compile log `/tmp/redball-unity-ui.log` shows `Tundra build success`, `AssetDatabase: script compilation time: 2.535143s`, and `Exiting batchmode successfully now!`.
- `rg -n "(error CS|Scripts have compiler errors|Compilation failed|Build failed|Tundra build failed|Exception:|Fatal error)" /tmp/redball-unity-ui.log` returned no matches.
- Static source review confirmed `RefreshLevelButtons()` now uses `GetContinueLevelIndex()` while `screenMode == ScreenMode.LevelSelect` to mark the continue target as `Devam`.
- Full play-mode UI smoke not run yet.

## QA Notes

QA pending. Needs visual/play-mode check that level select shows locked, completed, available, and continue target states correctly, and that continue/level start callbacks still work.

## Repo Controller Notes

Changed files are limited to source/docs for this task: `Assets/Scripts/RedBallRuntime.cs`, `Assets/Scripts/RedBallUi.cs`, `Assets/Scripts/RedBallUi.cs.meta`, and this task MD.

## PM Closure

Pending.
