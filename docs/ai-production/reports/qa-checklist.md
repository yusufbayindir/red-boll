# QA Checklist

Date: 2026-05-11  
Role: QA  
Source tasks: `TASK-0001`, `TASK-0002`, `TASK-0004`, `TASK-0005`

## Universal Task Closure Gate

No task closes until these are true:

- The task MD has current `Status`, specific `Activity Log`, accurate `Changed Files`, and meaningful `Verification`.
- Code tasks have Unity compile evidence from Unity 6000.4.5f1 or a documented blocker.
- Gameplay-facing code tasks have play-mode smoke evidence, not only code review.
- QA Notes are filled by QA after verification, not by the implementing developer.
- Known regressions are either fixed or explicitly accepted by PM as `Pass with risk`.
- Generated folders such as `Library`, `Temp`, `Logs`, and `Builds` are not included as task output unless PM approves.
- Any new feature has clear player-observable acceptance criteria before implementation begins.

## Architecture / Refactor Checklist

Applies to `TASK-0001` and future modularization work.

### Closure Criteria

- Proposed module ownership is documented and still matches the protocol target: orchestration, levels, level objects, UI, economy/hearts, audio, assets, runtime components.
- First split is narrow and low-risk. For the current task, `RedBallPlayer` extraction must preserve class name, public methods, movement constants, physics behavior, and references from `RedBallGame`.
- Unity compiles with no missing type, duplicate type, namespace, using, or partial-class errors.
- `RedBallGame` boot still works from `Assets/Scenes/RedBall.unity` Play mode.
- Current gameplay behavior is unchanged: movement, jump, coyote/jump buffer, bounce pads, hazards, enemy stomp/damage, coin collection, goal completion, respawn, score, level unlocks, and heart spend/regen.
- All existing 13 levels still load without null sprites, missing resources, broken camera bounds, or empty levels.
- No feature work is bundled into the refactor unless PM opens/approves a separate task.
- Runtime-generated components remain discoverable and stable for Unity compile and play mode.

### Regression Tests

- Start a clean session and confirm main menu appears.
- Start Level 1 from continue and confirm player spawns, camera follows, HUD appears, and movement/jump works.
- Collect a coin and confirm coin count and score update.
- Trigger hazard/fall damage and confirm one heart is spent, player respawns, and input still works.
- Complete Level 1 and confirm Level 2 unlocks and loads or becomes available as before.
- Load representative mechanics: Level 3 bounce/moving platform, Level 5 enemy patrol, Level 13 final route.
- Return to menu, open level select, restart level, and confirm no stale player/level objects remain.

### Developer Evidence Required

- Changed-file list with a short ownership note for each moved class/file.
- Unity console screenshot or copied log showing compile success.
- Before/after note confirming no intentional gameplay tuning changed.
- Play-mode notes covering at least Level 1, Level 3, one enemy level, and menu/level select navigation.
- Any compile or test blocker with exact error text.

## UI Redesign Checklist

Applies to `TASK-0002` design closure and later UI implementation tasks.

### Design Closure Criteria

- Critique is specific enough to implement.
- Proposed UI supports mobile landscape, including safe-area/notched layouts.
- Hearts and level progress are visible without heavy explanatory text.
- Main menu, HUD, and level select have implementation-ready component lists.
- Direction stays feasible in Unity UGUI without a paid asset pipeline or unapproved TextMeshPro dependency.

### Implementation Closure Criteria

- Main menu reads as a real game screen: world background, red ball mascot, primary continue/play action, secondary levels action, heart display, and progress display.
- HUD is compact and playable: hearts, coin count, level chip, home/menu, restart, message toast, joystick, and jump button.
- Level select uses all 13 visible nodes in a readable 5-5-3 path or PM-approved equivalent.
- Level states are visually distinct: locked, completed, current/next, and available. Locked/completed must not rely mainly on repeated text like `Kilit` and `Gecti`.
- Current/continue target is highlighted while actually viewing level select.
- Touch targets meet the intended minimum of 88px; icon buttons and controls are easy to tap.
- Text does not overlap, clip, or become unreadable at 16:9 landscape and notched mobile landscape.
- Message/status text has sufficient backing or contrast and does not obscure important gameplay.
- UI state updates after heart loss, heart regen, level completion, replay, restart, and returning to menu.
- Touch controls reset when leaving gameplay so stale joystick/jump input does not carry across screens.

### Regression Tests

- Fresh install/default save: main menu shows 5/5 hearts and Level 1 continue target.
- No-hearts state: continue/start is blocked cleanly, timer is visible, level select does not start levels.
- Level select: locked nodes are non-interactable, completed nodes show progress, next target is highlighted.
- HUD during play: heart/coin/level displays update after coin pickup, damage, restart, and level completion.
- Navigation: menu -> level select -> back -> continue -> gameplay -> home -> menu works repeatedly.
- Visual QA at 1920x1080, 16:9 phone landscape, and notched landscape safe-area simulation.

### Developer Evidence Required

- Screenshots or short clips of main menu, HUD, level select, no-hearts state, and level completion state.
- Resolution/safe-area matrix showing tested view sizes.
- Confirmation of whether UI code was split or remained in runtime, with rationale.
- List of any new sprites/icons and their source.
- Unity console compile result and any UI warnings.

## Mastery / Checkpoint Checklist

Applies to future implementation from `TASK-0004` and `TASK-0005`.

### Mastery Badge Closure Criteria

- Per-level badges are implemented for MVP scope: `Clear`, `All Coins`, and `Clean Run`.
- `Clear` still unlocks the next level and does not break existing progress saves.
- `All Coins` is awarded only when all coins in that level are collected before completion.
- `Clean Run` is awarded only when the level is completed without damage, fall death, hazard damage, or enemy damage.
- Replay can improve badges on already cleared levels without relocking progress.
- Level select shows badge progress per level in a readable, compact way.
- Completion UI shows newly earned badges and remaining unearned badges.
- Existing saves migrate safely with sensible defaults.
- Hearts are not made more punitive by the mastery loop.

### Checkpoint Closure Criteria

- Checkpoints exist before any longer post-13 levels are considered complete.
- Checkpoint visual language is distinct from the green goal, using the approved yellow/body/flag direction or PM-approved equivalent.
- Touching a checkpoint updates the current respawn point clearly and only once unless reactivated intentionally.
- Damage/fall after a checkpoint respawns at the latest checkpoint and still spends hearts according to current heart rules.
- Restarting a level resets to the level start unless PM explicitly approves checkpoint restart behavior.
- Completing a level, loading another level, returning to menu, or opening level select clears the previous level checkpoint state.
- Levels after 13 place checkpoints every 25-35 world units.
- Warning signs appear before new surprise danger/timing beats.
- Vertical lifts are fair: first lift is over safe ground and speed is at or below 1.4 when introduced.
- Crumble platforms and switch gates are not accepted until their first uses teach safely and have clear visual feedback.

### Regression Tests

- Earn each badge independently: clear only, all coins with damage, clean run without all coins, full mastery.
- Confirm badge saves persist after app restart.
- Confirm old level completion/unlock data still loads after adding badge save keys.
- Activate checkpoint, take damage, confirm respawn location and heart count.
- Take damage before checkpoint, confirm respawn remains level start.
- Restart after checkpoint and confirm expected start behavior.
- Run out of hearts after checkpoint and confirm return-to-menu/no-hearts flow still works.
- Complete Level 14 prototype with checkpoint and warning sign; verify no unreadable surprise hazards.

### Developer Evidence Required

- Badge save schema and migration note.
- Completion-screen screenshot/clip for each badge outcome category.
- Level-select screenshot showing badge progress on mixed-completion levels.
- Checkpoint behavior clip: activate, damage, respawn, restart, complete, reload.
- Level 14 layout notes showing checkpoint distance and warning sign placement.
- Statement confirming no timers/ghosts/daily routes were added unless separately approved.

## General Smoke Checklist

Run before any gameplay, UI, economy, or level task closes.

### Menu And Navigation

- App opens to main menu without console errors.
- Continue starts the correct next incomplete unlocked level.
- Level select opens and back returns to menu.
- Home/menu button exits gameplay cleanly.
- Restart reloads current level only during gameplay and does not spend a heart by itself.
- Keyboard shortcuts used by the prototype still work where expected: movement, jump, restart, number keys 1-9, enter/space continue from non-gameplay screens.

### Gameplay

- Player moves left/right with keyboard and virtual joystick.
- Jump works with keyboard and jump button.
- Coyote time and jump buffering still feel present after player/refactor changes.
- Camera follows player and stays inside level bounds.
- Coin pickup increments count/score and destroys the coin.
- Hazards and falling spend one heart and respawn player.
- Bounce pads apply upward bounce and preserve control.
- Moving platforms carry or support the player without obvious jitter.
- Enemy contact damages from side, and stomp defeats enemy/bounces player.
- Goal completes the level exactly once.

### Hearts And Saves

- New save starts with 5 hearts, Level 1 unlocked, no completed levels.
- Damage spends one heart and saves it.
- Zero hearts blocks level start and shows the next-heart timer.
- One heart regenerates after one hour of elapsed real time; full hearts show as full.
- Completing a level saves completion and unlock progress.
- Restarting the app preserves hearts, unlocks, and completions.

### Levels

- All 13 current levels can be started if unlocked or via QA-prepared save state.
- Each level has player spawn, visible terrain, reachable goal, coin count, and valid camera bounds.
- Representative mechanics are checked: Level 1 basic route, Level 3 bounce/moving platform, Level 5 enemy patrol, Level 7 vertical bounce, Level 10 height changes, Level 13 finale.
- No missing resource appears as magenta fallback in normal play.
- No level soft-locks the player without a clear fail/respawn path.

### Evidence Required

- Unity version used.
- Platform tested: Editor, target mobile build, or both.
- Save state used: fresh save, progressed save, no-hearts save, or custom QA state.
- Short written pass/fail notes for each smoke area.
- Screenshots/clips for visual/UI tasks and clips for new gameplay mechanics.
- Console log excerpt showing no errors during tested flow, or exact errors if present.
