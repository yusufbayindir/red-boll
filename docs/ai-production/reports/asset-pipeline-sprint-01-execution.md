# Asset Pipeline Sprint 01 Execution Prep

Date: 2026-05-12  
Owner: Asset Pipeline  
Status: Download prep only. No assets downloaded, imported, or moved.

## Scope Guard

This pass prepares a safe Sprint 01 asset shopping list for Yusuf to download manually. It does not touch Unity scenes, runtime code, prefabs, `.meta` files, or imported asset folders.

Because the user said "ben indiririm", the pipeline recommendation is:

- Do not download directly during this task.
- Do not use a repo-root `red-ball-assets/downloads/` staging folder for new downloads. That existing root source folder already represents the Kenney Shape Characters source package and should not become a mixed inbox.
- If/when PM opens a download pass, stage raw zips under `docs/ai-production/downloads/2026-05-12/<pack>/` with `source-url.txt`, original license/notice file, original zip, and `SHA256SUMS.txt`.
- Import nothing into `Assets/ThirdParty/` or `Assets/Resources/Imported/` until PM approves the exact subset and an implementation owner opens the runtime task.

## Inputs Read

- `docs/ai-production/reports/asset-download-plan.md`
- `docs/ai-production/assets-manifest.md`
- `docs/ai-production/tasks/TASK-0013-asset-pipeline-downloads.md`
- `docs/ai-production/tasks/TASK-0017-gameplay-differentiation-loop.md`
- `docs/ai-production/tasks/TASK-0018-level-pack-01-design.md`
- `docs/ai-production/reports/retention-mastery-badge-loop.md`
- `docs/ai-production/technical-debt/TECHNICAL_DEBT_REGISTER.md`

## License Sources Checked

Primary/official pages checked on 2026-05-12:

- Kenney Particle Pack: `https://www.kenney.nl/assets/particle-pack` - official page lists 80 files, 512x512 tile size, Creative Commons CC0.
- Kenney UI Pack: `https://kenney-assets.itch.io/ui-pack` - creator page lists 400+ sprites, `kenney_ui-pack.zip` at 1.1 MB, CC0 1.0 Universal.
- Kenney UI Audio: `https://kenney.nl/assets/ui-audio` - official page lists 50 files, Creative Commons CC0.
- Kenney Interface Sounds: `https://kenney.nl/assets/interface-sounds` - official page lists 100 files, Creative Commons CC0.
- Kenney Impact Sounds: `https://kenney.nl/assets/impact-sounds` - official page lists 130 files, Creative Commons CC0.
- Kenney Music Jingles: `https://kenney.nl/assets/music-jingles` - official page lists 85 files, Creative Commons CC0.
- Kenney Simplified Platformer Pack: `https://www.kenney.nl/assets/simplified-platformer-pack` - official page lists 90 assets, Creative Commons CC0.
- Kenney New Platformer Pack: `https://www.kenney.nl/assets/new-platformer-pack` - official page lists 440 files, Creative Commons CC0.
- Kenney Game Icons Expansion: `https://opengameart.org/content/game-icons-expansion` - page lists Kenney as author, CC0, 2 MB zip, optional credit.
- Kenney Shape Characters: `https://kenney-assets.itch.io/shape-characters` - creator page lists 100+ sprites, `kenney_shape-characters.zip` at 533 kB, CC0 1.0 Universal.

## Sprint 01 Asset List

| Need | Recommended source | Download link | License | Size / pack scope | Why needed | Proposed import target after approval | Risk |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Vertical lift icon | Existing Kenney Game Icons + Expansion | Already local; source `https://opengameart.org/content/game-icons-expansion` | CC0 | Source page lists 2 MB zip; local source already present | Use a simple up/down, arrow, or platform-state icon for Level 14 teaching, lift preview, or level-select mechanic chip. | Existing curated UI runtime path: `Assets/Resources/UI/`; future curated copy only if UI task needs a new named sprite. | Low. Existing local provenance is stronger than adding a new icon source. Verify exact icon filename before wiring. |
| Crumbling tile texture/sprite | Existing Kenney Shape Characters terrain first; fallback Kenney Simplified Platformer Pack | Existing source `https://kenney-assets.itch.io/shape-characters`; fallback `https://www.kenney.nl/assets/simplified-platformer-pack` | CC0 / CC0 1.0 Universal | Existing Shape Characters zip page lists 533 kB; fallback pack lists 90 assets | Level 15 needs readable "will crumble" state. Prefer recolor/overlay of current terrain style so the mechanic reads without a full tileset swap. | Prefer existing `Assets/Resources/RedBall/` style source. If fallback is approved later: `Assets/ThirdParty/Kenney/SimplifiedPlatformerPack/`, then curated runtime copy under `Assets/Resources/Imported/Kenney/Tiles/`. | Medium. Downloading a platformer pack for one tile can create style churn. Use only if existing terrain cannot produce a clear cracked/broken state. |
| Badge icons: Clear, All Coins, Clean Run | Existing Kenney Game Icons + Expansion | Already local; source `https://opengameart.org/content/game-icons-expansion` | CC0 | Source page lists 64 new icons, black/white, 1x/2x, spritesheets, vectors, and original pack | Sprint 01 mastery loop needs compact status icons on completion and level select. Use checkmark/trophy for Clear, coin for All Coins, star/target/medal for Clean Run. | Existing curated UI runtime path: `Assets/Resources/UI/`; future UI task can copy only selected 2x white icons if missing. | Low. Main risk is UI clutter on small screens, not license. Keep icon count to three badge states. |
| Checkpoint polish | Kenney Particle Pack plus existing checkpoint/warning art | `https://www.kenney.nl/assets/particle-pack` | Creative Commons CC0 | 80 files, 512x512 tile size | Checkpoint activation needs a small ring/star/spark confirmation so respawn safety is visible. | Raw stage after approval: `docs/ai-production/downloads/2026-05-12/kenney-particle-pack/`; source import target: `Assets/ThirdParty/Kenney/ParticlePack/`; curated runtime target: `Assets/Resources/Imported/Kenney/Particles/`. | Low-medium. Particle pack is safe and small, but runtime import should select only ring/star/spark/dust sprites. |
| Warning polish | Existing Shape Characters warning sign and Particle Pack optional sparks | Existing `https://kenney-assets.itch.io/shape-characters`; optional `https://www.kenney.nl/assets/particle-pack` | CC0 1.0 Universal / Creative Commons CC0 | Existing local source; optional particle pack 80 files | Hazards and crumbling tiles need clearer readable warning language, especially for Clean Run fairness. | Prefer existing `Assets/Resources/RedBall/`; optional curated particles under `Assets/Resources/Imported/Kenney/Particles/` after effects task. | Low if existing signs are used. Medium if overusing particles makes hazards look like rewards. |
| Button/checkpoint/badge SFX | Kenney UI Audio first; Interface Sounds as alternate | `https://kenney.nl/assets/ui-audio`; alternate `https://kenney.nl/assets/interface-sounds` | Creative Commons CC0 | UI Audio 50 files; Interface Sounds 100 files | Needs tactile feedback for badge earned, button confirm/back, checkpoint ping, error/missing badge. | Raw stage after approval: `docs/ai-production/downloads/2026-05-12/kenney-ui-audio/` or `kenney-interface-sounds/`; source import target: `Assets/ThirdParty/Kenney/UIAudio/` or `Assets/ThirdParty/Kenney/InterfaceSounds/`; curated runtime target: `Assets/Resources/Imported/Kenney/Audio/UI/`. | Medium. Current runtime uses procedural tones; importing too many clips creates bloat. Pick 4-8 clips only after audition. |
| Landing/crumble impact SFX | Kenney Impact Sounds, optional later | `https://kenney.nl/assets/impact-sounds` | Creative Commons CC0 | 130 files | Helpful for crumble break, landing dust, tile collapse, and bounce impact, but not required before the mechanic plays correctly. | Defer raw stage until audio task: `docs/ai-production/downloads/2026-05-12/kenney-impact-sounds/`; future import: `Assets/ThirdParty/Kenney/ImpactSounds/`. | Medium. Bigger audio pack and clip audition work. Defer unless crumbling tiles feel unclear without sound. |
| Badge/level-clear stinger | Kenney Music Jingles, optional later | `https://kenney.nl/assets/music-jingles` | Creative Commons CC0 | 85 files | Can make badge unlock or level clear feel rewarding once completion screen exists. | Defer raw stage until completion audio task: `docs/ai-production/downloads/2026-05-12/kenney-music-jingles/`; future import: `Assets/ThirdParty/Kenney/MusicJingles/`. | Medium. Jingles can overpower the small mastery loop. Use only one short clip if approved. |

## Download Order

1. Do not download anything until PM confirms the exact subset.
2. First approved manual download: Kenney Particle Pack, because checkpoint/warning polish and mastery feedback both use it.
3. Second approved manual download: Kenney UI Audio or Interface Sounds, not both, after the UI/audio owner chooses whether short clicks or broader interface tones fit better.
4. Defer Simplified Platformer Pack, Impact Sounds, and Music Jingles unless the vertical lift/crumbling tile implementation specifically requests them.

## Import Notes For Later Owners

- Existing local Kenney Shape Characters and Game Icons should be used before any new downloads.
- For the crumbling tile, the preferred visual is a cracked variant of the existing terrain, not a new world tile style.
- For mastery badges, use three stable icons only: Clear, All Coins, Clean Run. Do not introduce daily, timer, ghost, skin, or streak icons in Sprint 01.
- For checkpoint/warning polish, particles should clarify gameplay state. Reward-looking particles must not be placed on hazards unless the hazard state is unambiguous.
- For SFX, select by function first: button confirm, back/cancel, checkpoint ping, badge earned, crumble/impact, warning/error. Avoid broad audio imports.

## Provenance And Technical Debt

TD-0005 remains the active asset-provenance debt. Existing unknown files under `Assets/Resources/Imported/` must not be expanded, reused in marketing capture, or treated as approved until their source/license is recovered or they are replaced with manifest-tracked CC0 assets.

Technical debt added: none. TD-0005 already covers the current provenance risk.
