# Sprint 01 Unity Implementation Plan

Date: 2026-05-12  
Owner: Unity Sprint 01 Implementation Architect  
Source tasks: `TASK-0016`, `TASK-0017`, `TASK-0018`, `TASK-0001`, `TASK-0009`, `TASK-0010`

## Purpose

Turn Sprint 01's PM-approved product direction into a Unity implementation queue without writing product code in this planning pass. The implementation target is a small, verifiable slice: Mastery Badge Loop, one Safe Route vs Skill Route pilot, Level 14 vertical lift, Level 15 crumbling tile, and the minimum config changes needed to expose them.

## Current Runtime Shape

`Assets/Scripts/RedBallRuntime.cs` is still the main integration file. It owns bootstrap, `RedBallGame`, level count, save keys, heart economy, level loading, thirteen level builders, object helper factories, UI construction, UI refresh, sprite loading, audio, camera setup, and several runtime components.

Already split:

- `Assets/Scripts/RedBallPlayer.cs` owns player movement, jump buffer/coyote time, fall kill check, bounce, and respawn.
- `Assets/Scripts/RedBallUi.cs` owns level button state/color/label helpers only.
- `Assets/Scripts/RedBallCheckpoint.cs` owns checkpoint trigger behavior.

High-contention areas in `RedBallRuntime.cs`:

- `LevelCount`, `LoadLevel`, `BuildLevelOne` through `BuildLevelThirteen`.
- `LoadProgress`, `MarkLevelCompleted`, `SaveProgress`, PlayerPrefs keys, and heart save methods.
- `coinsInLevel`, `coinsCollectedInLevel`, `CollectCoin`, `DamagePlayer`, `CompleteLevel`, `RestartLevel`.
- `SetupUi`, completion messaging, level-select button layout, `RefreshHud`, and `RefreshLevelButtons`.
- Object helpers such as `AddMovingPlatform`, `AddCheckpoint`, `AddWarningSign`, `AddCoin`, and future `AddCrumblingTile`.

## Shared File Risk

| File | Risk | Recommended write owner |
| --- | --- | --- |
| `Assets/Scripts/RedBallRuntime.cs` | Very high. All Sprint 01 features naturally touch this file, and simultaneous edits would conflict around level count, save state, completion flow, UI, and level builders. | Single Gameplay Integration Owner only. Other workers submit notes or new component files, not direct edits. |
| `Assets/Scripts/RedBallUi.cs` | Medium. Badge indicator helper methods can live here, but only if one UI worker owns the file during the UI pass. | UI/Badge Owner after runtime badge model is merged. |
| `Assets/Scripts/RedBallPlayer.cs` | Low-to-medium. Clean-run failure currently should be recorded from `RedBallGame.DamagePlayer`; no movement edits are required for Sprint 01. | Avoid edits unless QA finds mechanic-specific player collision bugs. |
| `Assets/Scripts/RedBallCheckpoint.cs` | Low. Crumbling tile reset/checkpoint behavior should not be added here unless checkpoint state later grows. | Leave unchanged for the MVP unless Level 15 QA exposes reset needs. |
| New component files | Low. Vertical lift can reuse `MovingPlatform`; crumbling tile should be isolated in a new component. | Mechanic Owner may add one narrow file such as `RedBallCrumblingTile.cs`. |

## Developer Write-Scope Split

### Worker A - Gameplay Integration Owner

Owns the only planned edits to `Assets/Scripts/RedBallRuntime.cs`.

Scope:

- Raise `LevelCount` from 13 to 15.
- Add `BuildLevelFourteen` and `BuildLevelFifteen`.
- Route `LoadLevel` to Levels 14 and 15.
- Add runtime attempt state for mastery: attempt started, clean-run still valid, damage/fall failure reason if cheap.
- Award badges from `CompleteLevel` using current `coinsInLevel` and `coinsCollectedInLevel`.
- Keep old completion/unlock keys intact and add separate badge save keys.
- Add helper calls for vertical lift and crumbling tiles without embedding mechanic behavior directly in the level builders.

Do not own:

- Visual polish beyond functional badge text/indicator placement.
- New asset imports.
- Real telemetry or analytics infrastructure.

### Worker B - UI/Badge Owner

Owns `Assets/Scripts/RedBallUi.cs` and small UI helper additions after Worker A's save/model shape is known.

Scope:

- Add compact badge label/state helpers if needed.
- Keep badge indicators readable on the existing 5-column level-select layout.
- Implement completion feedback using existing UGUI primitives and existing icons/placeholders.

Do not own:

- Changing heart economy.
- Reworking the entire menu.
- Broad level-select redesign.

### Worker C - Mechanic Component Owner

Owns new component files only.

Scope:

- Add `RedBallCrumblingTile` as a standalone component with delay, disappear duration, reset on level reload, and clear visual feedback.
- Use public initialization from runtime-generated objects, similar to `MovingPlatform.Initialize`.
- Do not edit `RedBallRuntime.cs` directly unless temporarily paired with Worker A.

Vertical lift note: do not create a new vertical lift component for Sprint 01 unless `MovingPlatform` fails QA. `AddMovingPlatform(start, end, length, speed)` already supports vertical endpoints because it lerps `Vector2`.

### Worker D - QA Owner

Owns evidence, not implementation.

Scope:

- Compile gate.
- Play-mode smoke for old Levels 1, 3, 5, 13 and new Levels 14, 15.
- Badge persistence matrix.
- Checkpoint/restart/fall behavior.
- Mobile readability check.

## Minimum Viable Code Slice

1. Mastery badge persistence/UI:
   - Add per-level `Clear`, `All Coins`, `Clean Run` badge state.
   - Use separate PlayerPrefs keys from existing unlock/completion saves.
   - Treat existing `completedLevelMask` as legacy clear progress only; do not corrupt it.
   - Track clean run per attempt by invalidating on `DamagePlayer`, including hazards, enemy contact, and fall below `KillY`.
   - Award `All Coins` when `coinsInLevel > 0` and `coinsCollectedInLevel == coinsInLevel`.
   - Show completion feedback in the current message/UI path first; richer completion screen can be a follow-up.
   - Show compact state on level select, scoped to levels with saved data.

2. Safe Route vs Skill Route pilot:
   - Use Level 14 or 15 as the pilot; Level 14 is lower risk because vertical lift can introduce a safe path plus optional coin route without new stateful hazards.
   - Coins should mark the optional skill route.
   - The main route must remain completion-safe without all optional coins.

3. Level 14 vertical lift:
   - Implement as `BuildLevelFourteen`.
   - Reuse `AddMovingPlatform` with vertical endpoints and speed at or below 1.4 for first exposure.
   - First lift starts over safe ground.
   - Add checkpoint after first safe lift, following the 25-35 world-unit guidance when layout length allows.

4. Level 15 crumbling tile:
   - Implement as `BuildLevelFifteen`.
   - Add standalone crumbling tile component in a new file.
   - First crumble tile is safe or over a shallow recovery path before any pit use.
   - Reset by level reload/restart is enough for MVP. Mid-life checkpoint reset after damage is a candidate debt only if QA requires it for fairness.

5. Config:
   - Update level count, level-select layout assumptions, subtitle text, HUD level count, and completion end condition from 13 to 15.
   - Keep number-key shortcuts unchanged for Levels 1-9 unless a separate debug shortcut task is opened.

## Implementation Order

1. Worker A reserves `RedBallRuntime.cs` and adds badge data model/save keys plus attempt tracking without changing level content.
2. Worker A adds minimal completion feedback and verifies existing 13 levels compile.
3. Worker B adds compact level-select badge indicators in `RedBallUi.cs` or narrowly in UI construction if helper-only is insufficient.
4. Worker A updates `LevelCount` and adds Level 14 using vertical `AddMovingPlatform`.
5. Worker C adds `RedBallCrumblingTile.cs`; Worker A wires `AddCrumblingTile` and Level 15.
6. QA runs compile and play-mode gates. Fixes return to the owning worker; no broad refactor during fix pass.

## QA Gate

Compile gate:

- Unity 6000.4.5f1 batch import/compile must pass.
- Search the compile log for `error CS`, `Scripts have compiler errors`, `Compilation failed`, `Build failed`, `Tundra build failed`, `Exception:`, and `Fatal error`.
- Confirm changed files are limited to planned scripts, script meta files, task/report docs, and optionally `STATUS_BOARD.md`.

Play-mode gate:

- Fresh save: main menu, continue to Level 1, level select opens, locked states still work.
- Regression levels: Level 1 basic movement/coin/goal, Level 3 bounce/moving platform, Level 5 enemy damage/stomp, Level 13 checkpoint/warning signs.
- Mastery matrix: clear only, all coins with damage, clean run without all coins, full mastery.
- Persistence: badges survive return to menu and app relaunch; old unlock/completion progress remains valid.
- Retry/reset: clean-run state resets on restart and new level start; restart does not spend a heart.
- Level 14: vertical lift first exposure is safe, checkpoint works, skill route is optional, goal reachable without optional route.
- Level 15: crumbling tile teaches safely, disappears after contact/readable delay, level reload restores tile, no unavoidable soft-lock.
- UI: HUD/level-select/completion feedback remains readable at 1920x1080 and phone landscape.

## Technical Debt References

Relevant existing register entries:

- TD-0004: Unity source files are not yet included in first release commit scope.
- TD-0005: Asset provenance remains open; Sprint 01 should avoid new unapproved asset imports for badge/mechanic UI.
- TD-0006: Play-mode QA remains pending and blocks implementation closure.

Candidate debt, do not register yet unless the shortcut is accepted during implementation:

- Badge progression remains inside `RedBallGame` instead of a dedicated progression/profile service.
- No analytics facade exists for mastery events; TASK-0017 telemetry suggestions may be skipped.
- Crumbling tile state does not restore on checkpoint respawn, only on level reload/restart.
- Level authoring remains method-based in `RedBallRuntime.cs` for Levels 14-15.

## Handoff Notes

- PM requested no product code from this role; this report is docs-only.
- Do not revert existing working tree changes. Several Sprint 01 docs and Unity files are already untracked/modified from prior work.
- Implementation task source of truth: `docs/ai-production/tasks/TASK-0019-sprint-01-unity-implementation.md`.

## Verification

Documentation-only planning pass. Runtime compile/play-mode verification is intentionally pending implementation.

