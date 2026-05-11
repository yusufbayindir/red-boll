# TASK-0002 UI Implementation Breakdown

Date: 2026-05-11 23:32 +03
Role: Developer UI / Polish
Status: Planning only. No runtime/code edits until PM chooses a UI direction.

## Sources Read

- `docs/ai-production/PROTOCOL.md`
- `docs/ai-production/roles/developer-ui-polish.md`
- `docs/ai-production/tasks/TASK-0002-menu-ui-redesign.md`
- `docs/ai-production/reports/TASK-0002-menu-ui-redesign-direction.md`
- `Assets/Scripts/RedBallRuntime.cs`, especially the UI fields, `SetupUi`, UI builders, refresh methods, screen switching, and sprite loading.
- `Packages/manifest.json`

## Current Runtime Read

- UI is currently built entirely in `RedBallGame` inside `Assets/Scripts/RedBallRuntime.cs`.
- UI state fields live near the top of the class: level button lists, joystick/jump button refs, screen roots, HUD/menu texts, and font refs.
- `SetupUi` creates the canvas, HUD, main menu, level select, joystick, jump button, text buttons, and level buttons in one method.
- Refresh logic is mixed with gameplay state through `RefreshHud`, `RefreshLevelButtons`, `RefreshAllUi`, `UpdateLifeTexts`, `ShowMessage`, and `ShowStatus`.
- Level select current-state highlighting is not usable on the level-select screen because `RefreshLevelButtons` only marks current when `screenMode == Gameplay`.
- The project has UGUI available through `com.unity.ugui`; there is no TextMeshPro dependency in the manifest.
- The workspace is not a local git repository, so implementation should not assume commits, branches, or git-based rollback.

## Safe File Boundaries

Safe after PM approval:

- `docs/ai-production/tasks/TASK-0002-menu-ui-redesign.md`
  - Update status/activity/risk before code edits.
  - Fill changed files and verification after implementation.
- `Assets/Scripts/RedBallRuntime.cs`
  - Keep edits limited to UI integration points: UI fields, `SetupUi`, UI refresh calls, screen switching calls, touch-control references, and sprite loading.
  - Do not change level definitions, physics constants, player damage rules, score rules, heart save keys, or build behavior as part of this task.
- New UI script file, recommended:
  - `Assets/Scripts/RedBallUi.cs`
  - Optional only if needed: `Assets/Scripts/RedBallUiStyle.cs` or `Assets/Scripts/RedBallUiState.cs`
- `Assets/Resources/UI/`
  - Existing sprites should be preferred.
  - Add only tiny missing UI sprites such as heart, lock, or check if PM approves source asset churn. Runtime-created UGUI shapes are acceptable for the first pass if they keep the slice smaller.

Out of bounds unless PM opens or expands a task:

- `Assets/Scripts/RedBallPlayer.cs`
- Level construction/gameplay methods in `RedBallRuntime.cs`
- Save key migrations
- Package additions, including TextMeshPro
- iOS build settings and `Assets/Editor/RedBallBuildWorkspace.cs`
- Paid assets or broad brand rename work

## Likely Code Split

Recommended split:

- Keep `RedBallGame` as the state/gameplay owner and orchestration layer.
- Add `RedBallUi` as a code-built UGUI view/controller attached under the existing game object or canvas.
- Add a small `RedBallUiState` value object with only display data:
  - current screen
  - health/max health/recharge text
  - score
  - coins collected/coins total
  - level index/level count
  - unlocked count
  - completed mask
  - continue target index
  - no-hearts status/message text
- Add a small callback bundle from UI to game:
  - start continue level
  - show level select
  - show main menu
  - restart level
  - try start level by index
- Leave `VirtualJoystick` and `HoldButton` in place. `RedBallGame.MoveInput` and `ConsumeJumpPressed` can still read through UI-exposed refs or small accessors.
- Start with one new file rather than many component files. The component list from the design handoff should become named private builder methods first; split into more files only if the implementation gets difficult to read.

## Implementation Slices

1. PM gate and task setup
   - PM selects the visual direction or variant.
   - Update the task MD with intended file touches and risk check.

2. Nonvisual UI contract
   - Add `RedBallUi` and state/callback plumbing.
   - Preserve the current UI behavior visually while routing refresh calls through the new UI layer.
   - Keep keyboard shortcuts unchanged: number keys, Return/Space continue, and R restart.

3. Style and primitives
   - Centralize colors, alpha, touch target sizes, safe-area margins, and font sizes.
   - Add helpers for panels, image buttons, text buttons, icon chips, status pills, and toast backing.
   - Keep the canvas scaler at 1920x1080 unless PM explicitly expands scope.

4. HUD pass
   - Replace the long debug HUD string with heart/coin/level clusters.
   - Add a message toast backing panel.
   - Restyle joystick and jump button to match the menu palette while preserving input behavior.
   - Keep top-right controls at or above 88 px and inside safe margins.

5. Main menu pass
   - Build the Playable Hill menu with existing sprites: sky, clouds, trees, grass/tile strip, red ball mascot, primary continue button, secondary levels button, heart pill, and progress pill.
   - Give the continue action clear visual priority.
   - Show no-hearts state without blocking level select access.

6. Level select pass
   - Replace rectangle grid with a 5-5-3 node path.
   - Use badge/state visuals instead of "Kilit" and "Gecti" text.
   - Highlight `GetContinueLevelIndex()` or the next incomplete unlocked node while on the level-select screen.
   - Keep all 13 levels visible in landscape without scrolling.

7. Polish and cleanup
   - Add pressed/disabled color states.
   - Add a very subtle current-node pulse only after static layout is verified.
   - Remove obsolete UI fields from `RedBallGame` after the UI class owns them.
   - Update task MD changed files and verification.

## QA Hooks

- Add stable GameObject names for QA inspection:
  - `UI/MainMenu/HeartPill`
  - `UI/MainMenu/PrimaryContinue`
  - `UI/HUD/TopStatus`
  - `UI/HUD/Toast`
  - `UI/LevelSelect/Node01` through `Node13`
- Add one internal UI validation method during implementation that logs missing required sprites/references once after build.
- Verify core interaction paths:
  - fresh launch opens main menu
  - continue starts the expected level
  - level select opens and back returns to menu
  - locked node cannot start
  - no-heart state blocks play and shows recovery text
  - restart/home buttons still work in gameplay
  - joystick and jump still drive gameplay
- Verify visual layouts at:
  - 1920x1080 landscape
  - 2436x1125 or similar notched landscape
  - a narrower 16:9 or 18:9 landscape game view
- Verification commands are Unity-dependent. If Unity is available locally, run an editor compile/batchmode smoke check before handoff and capture the log path in the task MD.

## Design Handoff Challenge

The Playable Hill direction is implementable and worth doing, but the component list should not become a large class explosion in the first code pass. For this project, one `RedBallUi` file with clear regions/private builders is the safer bridge away from the centralized runtime.

The proposed node size of 132-150 px may be tight with the top band and safe-area padding on notched landscape devices. I would start around 124-136 px and increase only if screenshots prove there is room.

Missing icons should be solved minimally. If a heart/check/lock icon is needed, use simple local sprites or UGUI shapes; do not add a package, icon font, or TextMeshPro for this task.

I agree with fixing the current-level highlight, but I would define it as "continue target" in menu/level select, not "currently loaded level", because no gameplay level is loaded while the player is browsing level select.
