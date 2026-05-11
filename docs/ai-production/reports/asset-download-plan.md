# Red Ball Asset Download And Import Plan

Date: 2026-05-12  
Role: Asset Pipeline  
Status: Planning only. No new assets were downloaded or imported.

## Sources Read

- `docs/ai-production/reports/asset-research.md`
- `docs/ai-production/reports/TASK-0002-menu-ui-redesign-direction.md`
- `docs/ai-production/reports/TASK-0002-menu-ui-redesign-implementation-breakdown.md`
- `docs/ai-production/reports/level-design-expansion-plan.md`
- `docs/ai-production/reports/differentiation-challenger-red-ball-trickline.md`
- `docs/ai-production/reports/retention-mastery-badge-loop.md`
- `docs/ai-production/reports/marketing-launch-research.md`
- `docs/ai-production/tasks/TASK-0010-checkpoints-warning-signs.md`
- `docs/ai-production/tasks/TASK-0012-redball-ui-bridge-first-slice.md`
- Local source folders: `red-ball-assets/`, `kenney_game-icons-expansion/`, `Assets/Resources/RedBall/`, `Assets/Resources/UI/`, `Assets/Resources/Imported/`

## Pipeline Decision

The next asset work should be a small feedback/readability kit, not a broad art replacement.

The product direction is now "clean route / Trickline mastery", so the first imports should make progress, risk, checkpoint safety, and clean-line feedback readable. Downloading another full platformer tileset now would create style churn before the UI and mastery loop have enough shape.

## Download Now

These are license-safe, modest, and directly support the next UI/effects ideas. Download raw zips to `docs/ai-production/downloads/2026-05-12/<pack>/` first, with `source-url.txt`, original license file, and `SHA256SUMS.txt`. Import only selected files after PM approves the exact subset.

| Priority | Pack | Source | License | Why now | Import subset |
| --- | --- | --- | --- | --- | --- |
| 0 | Existing Kenney Shape Characters source | `red-ball-assets/`, source page `https://kenney-assets.itch.io/shape-characters` | CC0 1.0 Universal; local `License.txt` present | Already powers the red ball, terrain, clouds, trees, warning tile, checkpoint body, and goal marker. No new download needed. | Keep using `Assets/Resources/RedBall/`. Optional later import: more face/hand variants for mascot poses. |
| 0 | Existing Kenney Game Icons + Expansion source | `kenney_game-icons-expansion/`, sources `https://opengameart.org/content/game-icons` and `https://opengameart.org/content/game-icons-expansion` | CC0; local `license.txt` files present | Already covers current UI icons and has unused check, lock, medal, star, target, trophy, and start icons for level select/mastery. No new download needed. | Copy only selected 2x white icons into runtime Resources in a future UI task: `checkmark`, `locked`, `unlocked`, `medal1`, `medal2`, `star`, `target`, `trophy`, `buttonStart`. |
| 1 | Kenney Particle Pack | `https://www.kenney.nl/assets/particle-pack` | Creative Commons CC0; official page lists 80 files | Small enough for staging and directly supports checkpoint pickup, coin pickup, landing dust, clean-line sparkles, and screenshot appeal. | Start with `star_01` to `star_09`, `spark_05` to `spark_07`, `trace_01` to `trace_07`, `circle_01`, `circle_05`, `smoke_03`, and `dirt_01`. Verify file names after download before import. |
| 2 | Kenney UI Pack | `https://kenney-assets.itch.io/ui-pack` | CC0 1.0 Universal; page lists `kenney_ui-pack.zip` at 1.1 MB | Good for the Playable Hill menu, buttons, panels, chips, and store-screenshot captions if UGUI shapes are not enough. | Import only one color/style family for buttons/panels. Avoid dumping all 400+ sprites into runtime. |
| 3 | Kenney UI Audio or Interface Sounds | `https://kenney.nl/assets/ui-audio`, `https://kenney.nl/assets/interface-sounds` | Creative Commons CC0; UI Audio has 50 files, Interface Sounds has 100 files | Optional but useful once UI buttons and checkpoint feedback are visible. Current runtime uses procedural tones, so this can wait until an audio task unless PM wants quick tactile polish. | Choose 4 to 8 clips only: button confirm, back, error/no hearts, checkpoint ping, badge/clear stinger. |

## Download Later

These are useful, but should wait for a runtime/UI/audio task or a broader art-direction decision.

| Pack | Source | License | Why later |
| --- | --- | --- | --- |
| Kenney New Platformer Pack | `https://www.kenney.nl/assets/new-platformer-pack` | Creative Commons CC0; official page lists 440 files | Good fallback for future worlds, but it risks replacing the existing readable Red Ball style before the mastery identity is proven. |
| Kenney Mobile Controls | `https://www.kenney.nl/assets/mobile-controls` | Creative Commons CC0; official page lists 900 files | Current control icons already exist locally. Download only if touch control visual refresh becomes its own task. |
| Kenney Input Prompts | `https://www.kenney.nl/assets/input-prompts` | Creative Commons CC0; official page lists 1280 files | Useful for controller/keyboard prompts later, too large for the current mobile-first UI slice. |
| Kenney Impact Sounds | `https://kenney.nl/assets/impact-sounds` | Creative Commons CC0; official page lists 130 files | Useful for bounce, stomp, landing, and damage, but should wait for an audio-selection task so clips are auditioned in-game. |
| Kenney Music Jingles | `https://kenney.nl/assets/music-jingles` | Creative Commons CC0; official page lists 85 files | Useful for level clear and badge unlocks after completion/badge screens exist. |
| Fredoka / Nunito / Luckiest Guy | Google Fonts repository and specimen pages | Usually SIL Open Font License; verify the exact family directory before bundling | Good for UI and marketing captions, but Unity font integration should wait for a UI typography task. If distributed, include OFL/copyright files. |
| Ansimuz / Pixel Frog / Brackeys platformer packs | Existing research links | Reported CC0 or CC0-like by research | Good art references, but not first import. They risk pulling the game toward a generic pixel-platformer identity. |
| Game-icons.net / Freesound | Existing research links | Mixed CC-BY/public-domain or clip-by-clip | Avoid until credits/attribution UI and manifest automation are ready. Do not use CC-BY-NC. |

## Import Targets

Raw downloads:

- `docs/ai-production/downloads/2026-05-12/kenney-particle-pack/`
- `docs/ai-production/downloads/2026-05-12/kenney-ui-pack/`
- `docs/ai-production/downloads/2026-05-12/kenney-ui-audio/` or `docs/ai-production/downloads/2026-05-12/kenney-interface-sounds/`

Approved third-party source imports:

- `Assets/ThirdParty/Kenney/ParticlePack/`
- `Assets/ThirdParty/Kenney/UIPack/`
- `Assets/ThirdParty/Kenney/UIAudio/` or `Assets/ThirdParty/Kenney/InterfaceSounds/`

Runtime Resources import, only after a gameplay/UI implementation task expands ownership:

- `Assets/Resources/Imported/Kenney/Particles/`
- `Assets/Resources/Imported/Kenney/UI/`
- `Assets/Resources/Imported/Kenney/Audio/UI/`

The current runtime uses explicit `Resources.Load` paths, so source staging under `Assets/ThirdParty` is not enough by itself. A later implementation task should either add curated `Resources/Imported/Kenney/...` loads or move toward serialized asset references.

## UI Plan

Use existing local Game Icons first:

- Level select states: `locked`, `unlocked`, `checkmark`.
- Mastery/Trickline states: `medal1`, `medal2`, `star`, `target`, `trophy`.
- Primary action: `buttonStart`, existing `home`, `pause`, `return`.

Use Kenney UI Pack only where generated UGUI panels/buttons are not enough:

- Menu primary button frame.
- Small pill/chip backgrounds for hearts, progress, coins.
- Level node backing or node ring if the local shape/icon assets do not read well.

Do not import a whole UI theme. The menu direction should still feel like the existing Red Ball world: sky, grass, hill, ball mascot, and clean readable controls.

## Particles And Checkpoint Effects Plan

Checkpoint pickup should become a short, warm confirmation effect:

- One expanding yellow/white ring from `circle_01` or `circle_05`.
- 6 to 10 small `star_*` sprites emitted upward/outward.
- Optional `spark_05`/`spark_07` flicker near the flag.
- Optional soft `smoke_03` or `dirt_01` at ground contact if the checkpoint sits on terrain.

Coin pickup and level clear can share the same particle pool with different colors and count. Avoid portal/magic effects for the checkpoint; the level-design report specifically wants checkpoint language distinct from the green goal, not a teleport fantasy.

## Clean-Line / Trickline Visual Plan

Do not download a special line-art pack. The clean-line system should be mostly procedural so it scales to any level route:

- Use `LineRenderer` or a thin generated mesh for the "intended clean route".
- Use warm gold for the active clean line and teal/white for completed line feedback.
- Use imported `trace_*` or `spark_*` sprites as endpoint ticks, missed-line puffs, and perfect-line sparkle, not as the line itself.
- Use existing or local Kenney icons for contract badges: `target` for route contract, `star` for clean run, `medal1`/`medal2` for mastery tiers.
- Keep the route overlay subtle during gameplay and clearer on level select/completion screens.

This keeps the Trickline identity from looking like a generic particle-heavy arcade game.

## Marketing Screenshot Plan

Do not use stock art or external promotional backgrounds. Screenshots should be real gameplay captures after the UI and particle pass lands.

Minimum screenshot asset needs:

- Gameplay-first scenes from the marketing report: core jump, bounce pad route, moving platform timing, risky coins, enemy stomp, level select/progress, mobile controls.
- Optional caption font later: Fredoka for friendly readable captions; Luckiest Guy only for large store-card words if legibility holds.
- Optional UI Pack buttons/panels only if store captures need caption strips; captions should not hide gameplay.

Avoid screenshots for unimplemented features. In particular, do not show Clean Run badges, completion medals, daily contracts, bosses, skins, or hundreds of levels until those systems exist in-game.

## License Notes

- Kenney packs listed above are CC0 on current source pages or local license files. Attribution is not required, but keeping credit to Kenney in credits/release notes is still recommended.
- CC0 does not remove the need to track provenance. Keep source URL, download date, original zip, license file, and checksum for every download.
- Google Fonts assets require exact family-directory license review. If bundling font files, include the OFL/copyright metadata. Do not treat "available on Google Fonts" as a license file by itself.
- Do not use Freesound clips unless each clip is individually verified and NonCommercial is filtered out.
- Do not use Game-icons.net until attribution display and icon-level tracking exist.

## Next Implementation Tasks

1. PM approves TASK-0013 download subset.
2. Asset Pipeline downloads the raw zips into `docs/ai-production/downloads/2026-05-12/`, records checksums, and updates `docs/ai-production/assets-manifest.md`.
3. Asset Pipeline imports only the selected sprites/audio under `Assets/ThirdParty/Kenney/...`, preserving license files.
4. Developer UI/Polish opens a runtime task for UI icons/panels.
5. Developer Gameplay or Effects opens a runtime task for `RedBallEffects` and checkpoint/coin/trickline particles.
6. Marketing captures screenshots only after the runtime visuals are visible in real gameplay.
