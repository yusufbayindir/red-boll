# TASK-0002 UI/Menu Redesign Direction

Date: 2026-05-11
Role: UI/Menu Designer

## Sources Read

- `docs/ai-production/PROTOCOL.md`
- `docs/ai-production/roles/ui-menu-designer.md`
- `docs/ai-production/tasks/TASK-0002-menu-ui-redesign.md`
- `README.md`
- `Assets/Scripts/RedBallRuntime.cs`, especially `SetupUi`, UI helper methods, and UI refresh methods.

## Brutal Critique

### Main Menu

The current menu is functional but not screenshot-worthy. It is a flat blue overlay with centered text and two dark rectangular buttons. It says "RED BALL", "13 level", hearts, status, "Devam Et", and "Leveller", but it does not visually sell a platformer. There is no red ball character, no terrain, no motion implied, no world texture, and no clear reason for a player to feel they are entering a game rather than a test harness.

The hearts state is text-heavy: "Kalp 5/5 Dolu" or timer copy. This is mechanically correct but emotionally dead. Hearts should be visible as icons first, with timer text only when recovery matters.

The two menu actions have almost equal weight. First-session flow should make the next action obvious: play the next available level. Level select should be secondary.

### HUD

The HUD is a debug string: "Can 5/5 Skor 0 Coin 0/3". It is readable in code, not during play. A fast mobile player should see hearts and coins through icons and compact numbers, not parse Turkish labels across the top-left.

The HUD duplicates information across `scoreText` and `levelText`, both in the top band, while the top-right has small home/restart icons. This creates a crowded command strip instead of a calm play HUD.

The joystick and jump button are useful and sized generously, but they are visually disconnected from the rest of the UI. Their opacity makes them feel like stock touch controls dropped over the game. They need consistent tint, alpha, and pressed states.

The message text floats over the level with no container. It can disappear into bright backgrounds and has no hierarchy compared with score/level text.

### Level Select

The level select is the weakest screen. Thirteen rectangular buttons in a centered grid communicate "prototype" immediately. The words "Kilit" and "Gecti" inside each tile are heavy, repetitive, and not icon-friendly. A player should understand locked/completed/current from shape, color, and badges.

The current level highlight is effectively broken for level select. `RefreshLevelButtons` only marks `current` when `screenMode == Gameplay`, so when the player is actually on the level select screen, no current/continue target is highlighted.

The grid does not reveal progression as a journey. It has no path, no start-to-finish read, no sense that 13 levels are handcrafted. It hides the most valuable retention signal: "I am here, this is next, this much is done."

### Implementation Shape

All UI is generated inside `RedBallRuntime.cs`, with fixed 1920x1080 coordinates and repeated helper methods. That is acceptable for the prototype, but new polish should move toward a small UI layer rather than making the central runtime class even more responsible. This does not require a broad rewrite, but the next implementation should at least isolate style constants, screen construction, and state refresh.

## Recommended Direction: Playable Hill Menu

Use a full-screen, mobile-landscape menu that looks like the game world paused at the start of a run:

- Sky background with simple clouds and tree silhouettes from existing assets.
- Grass/tile ground strip along the lower third.
- Red ball mascot standing on a small hill or platform near the title.
- Large `RED BALL` title, but supported by world art instead of floating on a flat panel.
- A right-side action rail: large primary `Play` / `Devam Et`, secondary `Levels`, and a compact no-hearts state when blocked.
- Top-left heart pill with 5 heart icons. Show timer only when health is not full.
- Small progress pill, such as `7/13`, near the top or beside the primary action.

This direction gives the menu immediate identity, uses existing sprites, and stays feasible in Unity UGUI. It avoids needing paid assets, custom illustration, or a full scene camera menu.

## HUD Direction

Keep the play screen light. The player needs state, not a dashboard.

- Top-left: compact heart icons and coin icon count.
- Top-center: level chip, `Level 4` or `4/13`, with a flag icon if available.
- Top-right: icon buttons for pause/menu and restart, each at least 88px with safe-area padding.
- Bottom-left: joystick, visually matched to the menu palette, slightly smaller or lower-opacity than now.
- Bottom-right: jump button, same visual family as joystick.
- Center-top: message toast with a translucent dark backing, shown only for short success/failure messages.

Do not add visible tutorial copy or keyboard shortcuts to the mobile HUD.

## Level Select Direction

Replace the centered rectangle grid with a compact node path:

- 13 level nodes arranged as a 5-5-3 winding path across the screen.
- Completed nodes: green check badge.
- Current/next node: warm gold ring and subtle pulse.
- Available but not completed: solid readable node with large number.
- Locked nodes: dim node with lock badge, no "Kilit" word.
- Back button in the top-left safe area.
- Hearts pill and continue target visible in the top band.

This keeps all 13 levels visible in landscape without scrolling while making progress legible at a glance.

## Rejected Alternatives

- Keep the current blue panel and dark text buttons. It is fast, but it fails the role mission and will still look like placeholder UI.
- Build a full illustrated/animated landing page. Too much art dependency for this prototype and not aligned with mobile game first-session flow.
- Build a scrollable world map. Nice later, but unnecessary for 13 levels and higher implementation risk.
- Make the menu a shop/economy screen. Hearts matter, but monetization and paid asset decisions are explicitly out of scope.
- Add text explanations for controls/progression. The task asks for minimal clear text; icons and state should do the work.
- Do a broad architecture rewrite before UI polish. A small UI split is good, but the production protocol warns against wide refactors without a separate task.

## Component List

- `UiStyle`: colors, alpha values, font sizes, spacing, touch target sizes.
- `SafeAreaRoot`: landscape-safe anchored container with fixed margins.
- `WorldMenuBackground`: sky panel, clouds, trees, lower grass/tile strip.
- `RedBallMascot`: red ball body and face sprite, used on main menu.
- `TopStatusBar`: hearts, coins, progress.
- `HeartPill`: 5 heart icons plus optional recovery timer.
- `ProgressPill`: next/current level and total level count.
- `PrimaryButton`: large play/continue action with enabled, pressed, and disabled states.
- `SecondaryButton`: level select/back-style actions.
- `IconButton`: home/menu, restart, pause/back.
- `LevelPath`: non-interactive path line or dots behind nodes.
- `LevelNode`: number, locked badge, completed badge, current ring, interactable state.
- `HudCluster`: heart icons, coin count, level chip, top-right commands.
- `MessageToast`: short-lived centered status message with backing panel.
- `TouchControls`: joystick and jump button styling wrapper.

## Implementation Notes

- Use existing UGUI dependencies. `Packages/manifest.json` includes `com.unity.ugui`; do not assume TextMeshPro unless PM approves adding it.
- Keep `CanvasScaler` at 1920x1080 for now, but build with safe-area margins and anchored zones rather than only fixed center offsets.
- Use current sprites first: `UI/coin`, `UI/flag`, `UI/home`, `UI/return`, `UI/pause`, `UI/buttonA`, `UI/joystick`, `UI/joystickL_top`, plus `RedBall/red_body_circle`, faces, tiles, clouds, trees.
- Create any missing UI icons, such as lock/check/heart, as simple local sprites or lightweight Unity UI shapes. Do not buy assets for this task.
- Replace text states like "Kilit" and "Gecti" with badges. Keep numbers large.
- Fix current-level indication in level select by highlighting `GetContinueLevelIndex()` or the next incomplete unlocked level, not only `screenMode == Gameplay`.
- Keep touch targets at or above 88px. Recommended: primary CTA 420x110, secondary CTA 300x86, icon buttons 96x96, level nodes around 132-150px.
- Split UI construction/refresh into a small dedicated UI class when implementation begins. If speed wins, keep construction in `RedBallRuntime` for one pass but use style constants and clearly named builder methods so the later split is cheap.
- Verify on 16:9 landscape and notched mobile landscape. The top-left heart pill and top-right command buttons must not collide with safe areas.

