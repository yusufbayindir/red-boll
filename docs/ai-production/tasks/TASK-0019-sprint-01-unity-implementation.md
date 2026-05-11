# TASK-0019 - Sprint 01 Unity Implementation

## Status

QA PARTIAL - Compile Pass / Play-mode Pending

## Owner

Gameplay Integration Owner, UI/Badge Owner, Mechanic Component Owner, QA Owner, Unity Sprint 01 Implementation Architect.

## PM Intent

Implement the Sprint 01 playable slice without PM product-code changes: mastery badges, one Safe Route vs Skill Route pilot, Level 14 vertical lift, and Level 15 crumbling tile. Keep the implementation narrow, testable, and honest for future marketing/capture work.

## Scope

In:

- Per-level `Clear`, `All Coins`, and `Clean Run` badge tracking.
- Local badge persistence using separate save keys from existing unlock/completion saves.
- Completion feedback and compact level-select badge state.
- Clean-run attempt tracking that fails on damage, enemy contact, hazard damage, and fall/death.
- Level 14 vertical lift prototype using current moving-platform capability.
- Level 15 crumbling tile teaching section using a standalone component.
- Safe Route vs Skill Route pilot with optional coin route.
- QA compile, play-mode smoke, persistence, UI readability, and regression gates.

Out:

- PM-authored product code.
- Full Trickline contract framework.
- Ghost replay, daily challenge, timers, ranked medals, or run modifiers.
- Real telemetry/analytics if no shared wrapper exists.
- New monetization, ads, energy changes, or retry gates.
- Broad art import or full menu redesign.
- Large runtime modularization beyond the narrow files needed for this slice.

## Developer Write Scope

`Assets/Scripts/RedBallRuntime.cs` is single-owner for Sprint 01 because it currently contains level selection, save/progression, completion flow, UI construction, object helpers, and level builders. Only the Gameplay Integration Owner should edit it during this task.

Allowed shared ownership:

- `Assets/Scripts/RedBallUi.cs`: UI/Badge Owner may add helper methods after badge model shape is known.
- New `Assets/Scripts/RedBallCrumblingTile.cs`: Mechanic Component Owner may add standalone crumble behavior.
- New `.meta` file for any new script Unity creates or the developer creates consistently with existing script metas.
- This task MD and `docs/ai-production/reports/sprint-01-implementation-plan.md`.

Avoid edits unless a defect requires them:

- `Assets/Scripts/RedBallPlayer.cs`.
- `Assets/Scripts/RedBallCheckpoint.cs`.
- Project settings, resources, imported assets, package files, or generated Unity folders.

## Acceptance Criteria

- Existing Levels 1-13 still load and play after implementation.
- Level count, level-select UI, HUD level count, and completion end condition correctly include Levels 14 and 15.
- Completing any implemented level records `Clear`.
- Completing with all coins records `All Coins`.
- Completing without any damage/fall/death records `Clean Run`.
- Badge state persists after returning to menu and relaunching the app.
- Replay can improve missing badges on an already cleared level without relocking progression.
- Completion feedback clearly shows earned and missing badges.
- Level select shows compact badge progress without making touch targets unreadable.
- Level 14 contains a safe vertical lift tutorial and one optional skill/coin route.
- Level 15 contains readable crumbling tiles with safe first exposure and reset on level reload/restart.
- No forced ad, wait timer, energy cost, or currency cost is added to retry/mastery play.
- QA evidence is recorded before closure.

## Implementation Sequence

1. Reserve `Assets/Scripts/RedBallRuntime.cs` for the Gameplay Integration Owner.
2. Add badge model/save keys and per-attempt clean-run tracking while keeping current 13-level content unchanged.
3. Add minimal completion feedback and confirm old unlock/completion flow remains intact.
4. Add compact level-select badge state through `RedBallUi.cs` helpers or a narrow UI pass.
5. Update LevelCount/config from 13 to 15 and add Level 14 vertical lift using `AddMovingPlatform` with vertical endpoints.
6. Add `RedBallCrumblingTile.cs`, then wire `AddCrumblingTile` and Level 15.
7. Run compile gate.
8. Run play-mode gate and fix only task-scoped regressions.
9. Update this task's Activity Log, Changed Files, Verification, QA Notes, and Technical Debt line.

## QA Gate

Compile gate:

- Use Unity 6000.4.5f1 batch import/compile.
- Compile log must show successful script compilation/build exit.
- Compile log search must have no matches for `error CS`, `Scripts have compiler errors`, `Compilation failed`, `Build failed`, `Tundra build failed`, `Exception:`, or `Fatal error`.

Play-mode gate:

- Fresh save starts with Level 1 unlocked and no badges.
- Existing save with completed levels still loads old progress.
- Level 1 basic route, Level 3 bounce/moving platform, Level 5 enemy patrol, and Level 13 checkpoint/warning signs still work.
- Badge matrix passes: clear only, all coins with damage, clean run without all coins, full mastery.
- Clean Run fails on hazard damage, enemy side contact, and fall below `KillY`.
- Restart/new attempt resets clean-run state.
- All Coins awards only if `coinsCollectedInLevel == coinsInLevel` at completion.
- Badge UI and saved state match after menu return and app relaunch.
- Level 14 first lift is safe, speed is at or below 1.4 for first exposure, checkpoint works, optional route is not required to clear.
- Level 15 first crumble tile is readable and safe; crumble-over-pit arrives only after teaching; restart/reload restores tiles.
- Mobile landscape readability: HUD, level-select badges, completion feedback, joystick, and jump button do not overlap.

## Risk Check

Shared systems touched:

- `RedBallRuntime.cs` level count, load routing, completion flow, PlayerPrefs saves, coin tracking, damage/fall state, UI construction, level-select refresh, and level builders.

Regression risks:

- Old completion/unlock saves could be misread if badge keys are not separate.
- Level-select layout may not fit 15 levels plus badges.
- Clean-run tracking can be inaccurate if only hazards are counted and fall/enemy paths are missed.
- Crumbling tiles can create soft-locks or fail to reset after restart.
- Vertical moving platforms can jitter or be too fast for touch controls.

Rollback idea:

- Keep badge save keys separate and UI additive so badge display can be hidden without breaking normal completion.
- Keep Level 14/15 routed through `LevelCount` and `LoadLevel`; rollback can return LevelCount to 13 and remove new cases.
- Keep crumbling tile behavior in one component so it can be disabled or replaced with normal platforms if QA fails.

## Technical Debt References

- TD-0004: Unity source tracking/release scope remains a Git/Release blocker.
- TD-0005: Asset provenance remains open; use existing assets/placeholders for Sprint 01.
- TD-0006: Play-mode QA is mandatory before closing this implementation task.

Candidate debt to register only if accepted during implementation:

- Progression/badge state remains embedded in `RedBallGame`.
- Mastery telemetry is skipped because no analytics facade exists.
- Crumbling tiles reset only on level reload/restart, not checkpoint respawn.
- Levels 14-15 are authored as additional methods in `RedBallRuntime.cs` instead of a data-driven level system.

## Activity Log

- 2026-05-12 - QA Runner reran Unity 6000.4.5f1 compile/import gate successfully and completed static blocker review; no confirmed blocker found, but no play-mode/load-path automation exists for Level 1/14/15, so verdict remains QA PARTIAL.
- 2026-05-12 - Re-ran Unity 6000.4.5f1 batchmode import/compile after final Level 14/15 spacing edits; required compile error-pattern scan returned no matches.
- 2026-05-12 - Tightened Level 14/15 platform spacing after implementation sanity check so new clear routes are less likely to exceed normal jump reach before QA tuning.
- 2026-05-12 - Unity 6000.4.5f1 batchmode import/compile completed successfully; required compile error-pattern scan returned no matches. Play-mode QA remains pending.
- 2026-05-12 - Registered accepted Sprint 01 technical debt for crumbling-tile checkpoint reset behavior and missing mastery telemetry facade.
- 2026-05-12 - Added Level 14 vertical-lift route with optional coin skill path, Level 15 crumbling-tile teaching route, and standalone `RedBallCrumblingTile` behavior that resets through level reload/restart.
- 2026-05-12 - Added compact text-based mastery feedback to completion messaging and level-select labels, and updated menu/level-select copy for 15 levels.
- 2026-05-12 - Added separate PlayerPrefs badge masks and per-attempt clean-run invalidation through the existing damage/restart flow; legacy completion mask remains intact for unlock/continue behavior.
- 2026-05-12 - Gameplay Integration Owner started implementation, reserved runtime/UI scope, and began codebase read-through before product-code edits.
- 2026-05-12 - Implementation Architect created docs-only plan and developer queue task after reading Sprint 01 docs and current Unity runtime files.

## Changed Files

- `Assets/Scripts/RedBallRuntime.cs`
- `Assets/Scripts/RedBallUi.cs`
- `Assets/Scripts/RedBallCrumblingTile.cs`
- `Assets/Scripts/RedBallCrumblingTile.cs.meta`
- `docs/ai-production/reports/sprint-01-qa-run-2026-05-12.md`
- `docs/ai-production/technical-debt/TECHNICAL_DEBT_REGISTER.md`
- `docs/ai-production/tasks/TASK-0019-sprint-01-unity-implementation.md`
- `docs/ai-production/STATUS_BOARD.md`

## Verification

- QA Runner compile/import rerun: Unity 6000.4.5f1 batchmode from project root, log `/tmp/red_ball_qa/sprint01-compile-20260512.log`, exit code 0.
- QA Runner compile log search had no matches for `error CS`, `Scripts have compiler errors`, `Compilation failed`, `Build failed`, `Tundra build failed`, `Exception:`, or `Fatal error`.
- QA Runner attempted EditMode and PlayMode Test Runner commands, but no result XML was produced and no Sprint 01 load-path smoke tests exist. Level 1/14/15 load paths remain play-mode residual risk.
- Unity 6000.4.5f1 batchmode import/compile run from project root.
- Compile evidence: `Csc Library/Bee/artifacts/.../Assembly-CSharp.dll`, `Tundra build success`, and `Exiting batchmode successfully now!`.
- Compile log search had no matches for `error CS`, `Scripts have compiler errors`, `Compilation failed`, `Build failed`, `Tundra build failed`, `Exception:`, or `Fatal error`.
- Play-mode verification not run in this implementation pass; QA must complete TASK-0021 coverage before closure.

## QA Notes

2026-05-12 QA Runner verdict: QA PARTIAL. Compile/import passed and static review did not confirm a blocker in badge persistence/unlock flow, `LevelCount` 15 routing, Level 14/15 builders, Clean Run invalidation, All Coins gating, or crumbling tile restart/reload rebuild behavior. Full PASS is blocked by missing play-mode/manual evidence for Level 1/14/15 load paths, actual route completion, badge persistence after relaunch, crumbling checkpoint interactions, and mobile landscape readability. Detailed run report: `docs/ai-production/reports/sprint-01-qa-run-2026-05-12.md`.

QA must not close this task on compile evidence alone; play-mode evidence is required under TD-0006.

Priority scenarios:

- Fresh save: Level 1 unlocked, Levels 14-15 locked until progression, no badges shown as earned.
- Existing save: old completed levels still unlock/continue correctly, and legacy completion appears as clear badge state.
- Badge matrix: clear only, all coins with damage, clean run without all coins, and full mastery.
- Clean Run failures: triangle hazard, enemy side contact, fall below `KillY`, and manual restart/new attempt reset.
- Level 14: first vertical lift safe at 1.25 speed, checkpoint after first lift, optional high coin route not required for clear.
- Level 15: first crumbling tile is over safe ground, later pit crumble is readable, restart/reload restores all crumbling tiles.
- UI: level-select `GCT` compact badge text and completion badge summary remain readable in mobile landscape.

## Repo Controller Notes

Implementation changed only the planned runtime/UI/component files plus task/status/debt docs. Future staging should avoid Unity generated folders and unrelated modified docs.

2026-05-12 Git / Release Executor note: Sprint 01 release staging includes Unity source/project-open files (`Assets/`, `Packages/`, `ProjectSettings/`), EditMode smoke tests, dashboard/docs handoff files, repo launcher, and `TestResults/editmode-smoke-results.xml` as durable QA evidence. Generated Unity folders, logs, raw asset holding folders, `.DS_Store`, root `package.json`, and the parent launcher outside the repo are excluded.

## PM Closure

Implementation complete; QA PARTIAL. Compile/import and static QA are green, but play-mode evidence is still required before Sprint 01 PASS.

## Technical Debt Added

TD-0009, TD-0010
