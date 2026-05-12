# Red Ball Asset Manifest

Last updated: 2026-05-12  
Owner: Asset Pipeline  
Purpose: Track third-party asset provenance before assets are downloaded, imported, or shipped.

## Policy

- Every third-party asset must have a source URL, creator, license, download/import date, local path, and license/notice file.
- Prefer CC0 for the first production imports.
- Keep raw downloads under `docs/ai-production/downloads/` until PM approves a curated runtime import.
- Do not use `red-ball-assets/downloads/` for new raw downloads; `red-ball-assets/` is already the local Kenney Shape Characters source package.
- Keep source/notice files beside imported third-party assets under `Assets/ThirdParty/`.
- If runtime code needs `Resources.Load`, import only a curated copy under `Assets/Resources/Imported/Kenney/...` after the implementation task expands ownership.
- Do not import CC-BY assets until in-game/store credits and icon/clip-level attribution tracking exist.
- Do not use NonCommercial or personal-use assets.

## Imported / Already Present

| Asset | Creator | Source | License | Local source path | Runtime path | Status / notes |
| --- | --- | --- | --- | --- | --- | --- |
| Shape Characters | Kenney | `https://kenney-assets.itch.io/shape-characters` | CC0 1.0 Universal; local `License.txt` present | `red-ball-assets/` | `Assets/Resources/RedBall/` | Existing local source and curated runtime subset. Used for red ball body, alternate square bodies, faces, terrain, trees, clouds, coin tile, warning sign, checkpoint body, and shadow. No new download in TASK-0013. |
| Game Icons + Game Icons Expansion | Kenney | `https://opengameart.org/content/game-icons`, `https://opengameart.org/content/game-icons-expansion` | CC0; local `license.txt` files present | `kenney_game-icons-expansion/` | `Assets/Resources/UI/` | Existing local source and curated runtime subset. Current runtime uses joystick, button, home, pause, return, flag, and coin UI icons. Unused source icons can support lock/check/medal/star/target states later. No new download in TASK-0013. |
| Existing Imported Audio / Effects / Food / Mushroom / Props / Skeleton | Unknown from current docs | Unknown | Unknown | None found in docs | `Assets/Resources/Imported/` | Present before TASK-0013. Do not ship or expand usage until provenance is recovered or assets are replaced with manifest-tracked alternatives. |

## Approved Candidates For Next Download

No candidate below was downloaded in TASK-0013. These are approved-for-planning entries only until PM explicitly opens the download/import pass.

| Priority | Asset | Creator | Source | License | Planned raw path | Planned import path | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| 1 | Particle Pack | Kenney | `https://www.kenney.nl/assets/particle-pack` | Creative Commons CC0 | `docs/ai-production/downloads/2026-05-12/kenney-particle-pack/` | `Assets/ThirdParty/Kenney/ParticlePack/` | First new download candidate. Select only checkpoint, coin, dust, clean-line, and route feedback textures. Official page lists 80 files. |
| 2 | UI Pack | Kenney | `https://kenney-assets.itch.io/ui-pack` | CC0 1.0 Universal | `docs/ai-production/downloads/2026-05-12/kenney-ui-pack/` | `Assets/ThirdParty/Kenney/UIPack/` | Small pack; page lists `kenney_ui-pack.zip` at 1.1 MB. Import only one visual family for buttons/pills/panels if UGUI shapes are insufficient. |
| 3 | UI Audio | Kenney | `https://kenney.nl/assets/ui-audio` | Creative Commons CC0 | `docs/ai-production/downloads/2026-05-12/kenney-ui-audio/` | `Assets/ThirdParty/Kenney/UIAudio/` | Optional. Official page lists 50 files. Use only after a UI/audio task chooses exact clips. |
| 3 | Interface Sounds | Kenney | `https://kenney.nl/assets/interface-sounds` | Creative Commons CC0 | `docs/ai-production/downloads/2026-05-12/kenney-interface-sounds/` | `Assets/ThirdParty/Kenney/InterfaceSounds/` | Optional alternative to UI Audio. Official page lists 100 files. Do not import both unless audio task needs both. |

## AI-Generated Asset Planning

Prompt pack: `docs/ai-production/reports/creative-asset-prompt-pack-v1.md`  
Storyboard pack: `docs/ai-production/marketing/ad-creative-storyboards-v1.md`

One ChatGPT/imagegen concept board has been archived as generated-asset source material. TASK-0032 copied that exact board into Unity `Resources` only as a first-screen concept preview so the Sprint 01 update is visible in Play mode. It is not approved as final UI, store screenshot, app icon, paid media, or shipping key art. TD-0013 remains the manifest validator/export-gate debt; until that debt is closed, every generated creative asset requires manual manifest and provenance review before use.

### Archived AI-Generated Outputs

| Asset ID | Asset | Provider / tool | Generation date | Local path | Metadata | Intended use | Review status | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| `RB-AI-20260512-001` | Red Ball Creative Board V1 | ChatGPT/imagegen built-in | 2026-05-12 | Source archive: `docs/ai-production/generated-assets/2026-05-12/red-ball-creative-board-v1.png`; concept-only runtime copy: `Assets/Resources/Generated/Sprint01/mastery_update_concept.png` | PNG, 1536x1024, 2,084,800 bytes, SHA-256 `e4783cfe4e8ba41c92a0f2e569bab3b5cd0d4ef6f99ddf441aa2ededb79568ac` | Internal concept board for app icon/key art, mastery badge set, and crumbling-tile ad still direction; temporary main-menu preview in TASK-0032 | Concept-only Unity preview / provenance recorded; final art, license, QA, store, and marketing approval pending | Provenance: `docs/ai-production/generated-assets/2026-05-12/red-ball-creative-board-v1.provenance.md` and `.json`. Runtime copy is not final UI or public marketing art. Manual gate required while TD-0013 is open. |

### Runtime Programmatic Outputs

TASK-0033 created the following runtime asset pack locally with Python 3 and Pillow 11.1.0. These are original handmade/programmatic raster assets, not third-party downloads and not AI model outputs. No external source URL or license file is required; usage rights are project-owned/original for this repository. Unity import generated `.meta` files beside every PNG.

| Asset ID | Asset | Tool / creator | Generation date | Runtime path | Metadata | Runtime use | Review status | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| `RB-PROG-20260512-001` | Clear mastery badge | Codex Runtime Asset Producer using Pillow | 2026-05-12 | `Assets/Resources/Generated/Sprint02/mastery_badge_clear.png` | PNG, 160x160, SHA-256 `1025e02e20b7a70485aa9e50896185e65bc0ebb917a799f39c6acbe8a5b63e7d` | Main menu mastery strip and level select badge icon. | Runtime integrated; Unity import/compile PASS; EditMode 6/6 PASS; PlayMode 5/5 PASS | Fallback: existing UI `flag` sprite. |
| `RB-PROG-20260512-002` | All Coins mastery badge | Codex Runtime Asset Producer using Pillow | 2026-05-12 | `Assets/Resources/Generated/Sprint02/mastery_badge_all_coins.png` | PNG, 160x160, SHA-256 `a624b5a676ec0a684fe6ff99c59dd19cbb62246c99e19f9b2a23d3a2eb530579` | Main menu mastery strip and level select badge icon. | Runtime integrated; Unity import/compile PASS; EditMode 6/6 PASS; PlayMode 5/5 PASS | Fallback: existing UI `coin` sprite. |
| `RB-PROG-20260512-003` | Clean Run mastery badge | Codex Runtime Asset Producer using Pillow | 2026-05-12 | `Assets/Resources/Generated/Sprint02/mastery_badge_clean_run.png` | PNG, 160x160, SHA-256 `b16ffcba42045f7c1013616891f4f7cabaaaa8a286a803ab4f25bc374728f841` | Main menu mastery strip and level select badge icon. | Runtime integrated; Unity import/compile PASS; EditMode 6/6 PASS; PlayMode 5/5 PASS | Fallback: existing UI `buttonA` sprite. |
| `RB-PROG-20260512-004` | Lift platform polish | Codex Runtime Asset Producer using Pillow | 2026-05-12 | `Assets/Resources/Generated/Sprint02/lift_platform_polish.png` | PNG, 320x112, SHA-256 `d2466eae14fa8a9b20016e2a8048d9322e4b6a7363f3fe36c891dd6348312225` | Overlay on moving platforms, including Level 3, 6-13, and Level 14 vertical lift platforms. | Runtime integrated; Unity import/compile PASS; EditMode 6/6 PASS; PlayMode 5/5 PASS; TASK-0035 extended visible use to existing levels. | Fallback: existing RedBall `tile_half_center` sprite. |
| `RB-PROG-20260512-005` | Crumbling tile polish | Codex Runtime Asset Producer using Pillow | 2026-05-12 | `Assets/Resources/Generated/Sprint02/crumbling_tile_polish.png` | PNG, 128x96, SHA-256 `1bfdaca1cd55e0bff980374919b6fb5892f59e8437dfd7cfc77fe588e909f969` | Level 15 crumbling tile sprite. | Runtime integrated; Unity import/compile PASS; EditMode 6/6 PASS; PlayMode 5/5 PASS | Fallback: existing RedBall `tile_half` sprite. |
| `RB-PROG-20260512-006` | Warning spark | Codex Runtime Asset Producer using Pillow | 2026-05-12 | `Assets/Resources/Generated/Sprint02/warning_spark.png` | PNG, 128x128, SHA-256 `34dd62c90c1eacfcaa34d01300189544e1324d4f0b4c7e803c3c3356cd50318a` | Runtime warning sign overlay; TASK-0035 adds readable hazard warning signs across Level 1-13. | Runtime integrated; Unity import/compile PASS; EditMode 6/6 PASS; PlayMode 5/5 PASS; TASK-0035 extended visible use to existing levels. | Fallback: existing RedBall `tile_exclamation` sprite. |
| `RB-PROG-20260512-007` | Checkpoint spark | Codex Runtime Asset Producer using Pillow | 2026-05-12 | `Assets/Resources/Generated/Sprint02/checkpoint_spark.png` | PNG, 128x128, SHA-256 `bfd0a9674551015474a966173804df2a91e42f0c6d6406b04492d6860be13983` | Runtime checkpoint overlay. | Runtime integrated; Unity import/compile PASS; EditMode 6/6 PASS; PlayMode 5/5 PASS | Fallback: existing UI `flag` sprite. |
| `RB-PROG-20260512-008` | Dust sparkle | Codex Runtime Asset Producer using Pillow | 2026-05-12 | `Assets/Resources/Generated/Sprint02/dust_sparkle.png` | PNG, 128x128, SHA-256 `502a8d891ede47b2e2c1a2c3fd9fdf51f3aee4d2a6be7c3b5652ef7102e10fab` | Level 15 crumbling tile dust/sparkle overlay; TASK-0035 route/coin-path sparkle markers across Level 1-13. | Runtime integrated; Unity import/compile PASS; EditMode 6/6 PASS; PlayMode 5/5 PASS; TASK-0035 extended visible use to existing levels. | Fallback: existing RedBall `shadow` sprite. |

| Planned asset family | Prompt IDs | Tool | Current status | Provenance requirement |
| --- | --- | --- | --- | --- |
| App icon / key art | `A01` | ChatGPT imagegen recommended | Concept board archived; no final selected output | Record exact prompt, negative prompt, generation date, model/tool name, seed/settings if available, and selected output path. |
| Mastery badges | `A02` | ChatGPT imagegen recommended | Concept board archived; no runtime badge export | Record selected output and any manual edits before runtime or marketing use. |
| Gameplay polish concepts | `A03`-`A06` | ChatGPT imagegen or equivalent | Prompt only | Treat as concept art until a later implementation/import task approves scale, readability, and collision-safe simplification. |
| Ad stills | `A07`-`A12`, `S01`-`S06` | ChatGPT imagegen or equivalent | Concept board archived for crumbling-tile direction; no ad export approved | Mark as key art/concept unless replaced by real gameplay capture; avoid deceptive gameplay claims. |
| Video storyboard frames | `V01`-`V03` | ChatGPT imagegen/video tooling or editor storyboard | Prompt only | Store source prompts and confirm Sprint 01 feature accuracy before public use. |

## Sprint 01 Manual Download Prep

Detailed execution prep: `docs/ai-production/reports/asset-pipeline-sprint-01-execution.md`.

No Sprint 01 candidate below has been downloaded or imported. These entries tell Yusuf what to download manually if PM approves the subset.

| Sprint need | Preferred asset | Creator | Source | License | Size / scope verified 2026-05-12 | Planned raw path if approved | Planned import path if approved | Risk / note |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Vertical lift icon | Existing Game Icons + Game Icons Expansion | Kenney | `https://opengameart.org/content/game-icons-expansion` | CC0 | Expansion page lists a 2 MB zip and 64 new icons; local source already present. | None; already local. | Existing `Assets/Resources/UI/` curated subset, or future selected copy only. | Low. Prefer existing icons over adding a new source. |
| Crumbling tile sprite | Existing Shape Characters terrain first; Simplified Platformer Pack only as fallback | Kenney | `https://kenney-assets.itch.io/shape-characters`; fallback `https://www.kenney.nl/assets/simplified-platformer-pack` | CC0 1.0 Universal / Creative Commons CC0 | Shape Characters page lists 533 kB zip; fallback official page lists 90 assets. | Fallback only: `docs/ai-production/downloads/2026-05-12/kenney-simplified-platformer-pack/` | Fallback only: `Assets/ThirdParty/Kenney/SimplifiedPlatformerPack/` | Medium. Avoid broad tile style churn for a single cracked tile. |
| Mastery badge icons | Existing Game Icons + Game Icons Expansion | Kenney | `https://opengameart.org/content/game-icons-expansion` | CC0 | Local source already present. | None; already local. | Existing `Assets/Resources/UI/` curated subset, or future selected copy only. | Low. Use only three Sprint 01 states: Clear, All Coins, Clean Run. |
| Checkpoint/warning polish | Particle Pack plus existing Red Ball sign/checkpoint art | Kenney | `https://www.kenney.nl/assets/particle-pack` | Creative Commons CC0 | Official page lists 80 files, 512x512 tile size. | `docs/ai-production/downloads/2026-05-12/kenney-particle-pack/` | `Assets/ThirdParty/Kenney/ParticlePack/` | Low-medium. Import only ring/star/spark/dust subset later. |
| Button/checkpoint/badge SFX | UI Audio, or Interface Sounds as alternate | Kenney | `https://kenney.nl/assets/ui-audio`; `https://kenney.nl/assets/interface-sounds` | Creative Commons CC0 | Official pages list 50 files and 100 files respectively. | `docs/ai-production/downloads/2026-05-12/kenney-ui-audio/` or `kenney-interface-sounds/` | `Assets/ThirdParty/Kenney/UIAudio/` or `Assets/ThirdParty/Kenney/InterfaceSounds/` | Medium. Choose one pack and only 4-8 clips after audition. |
| Landing/crumble impact SFX | Impact Sounds | Kenney | `https://kenney.nl/assets/impact-sounds` | Creative Commons CC0 | Official page lists 130 files. | `docs/ai-production/downloads/2026-05-12/kenney-impact-sounds/` | `Assets/ThirdParty/Kenney/ImpactSounds/` | Medium. Defer unless crumbling tile implementation needs non-procedural impact audio. |
| Badge/level-clear stinger | Music Jingles | Kenney | `https://kenney.nl/assets/music-jingles` | Creative Commons CC0 | Official page lists 85 files. | `docs/ai-production/downloads/2026-05-12/kenney-music-jingles/` | `Assets/ThirdParty/Kenney/MusicJingles/` | Medium. Defer until completion screen/badge flow exists. |

## Deferred Candidates

| Asset | Creator | Source | License | Reason deferred |
| --- | --- | --- | --- | --- |
| New Platformer Pack | Kenney | `https://www.kenney.nl/assets/new-platformer-pack` | Creative Commons CC0 | 440 files and broad art-direction impact. Defer until PM decides whether to expand or replace world art. |
| Mobile Controls | Kenney | `https://www.kenney.nl/assets/mobile-controls` | Creative Commons CC0 | 900 files. Existing local controls are enough until touch UI gets a dedicated visual task. |
| Input Prompts | Kenney | `https://www.kenney.nl/assets/input-prompts` | Creative Commons CC0 | 1280 files. Useful later for broader device support, not needed for mobile-first polish. |
| Impact Sounds | Kenney | `https://kenney.nl/assets/impact-sounds` | Creative Commons CC0 | 130 files. Defer until audio selection and runtime sound hooks are planned. |
| Music Jingles | Kenney | `https://kenney.nl/assets/music-jingles` | Creative Commons CC0 | 85 files. Defer until completion/badge screens exist. |
| Fredoka / Nunito / Luckiest Guy | Google Fonts / family authors | `https://fonts.google.com/` and exact Google Fonts family repo directories | Usually SIL Open Font License; verify exact family directory | Defer until UI typography or marketing screenshot template task. Bundle OFL and copyright files if distributing font files. |
| Game-icons.net | Multiple icon authors | `https://game-icons.net/` | Mixed CC-BY / public-domain | Attribution tracking required at icon level. Avoid for now. |
| Freesound | Multiple audio authors | `https://freesound.org/` | Clip-by-clip CC0, CC-BY, CC-BY-NC | Clip-level verification required. Avoid NonCommercial assets entirely. |

## Open Provenance Questions

- `Assets/Resources/Imported/Audio/dark_woods.ogg` has no manifest source yet.
- `Assets/Resources/Imported/Effects/*.png` have no manifest source yet.
- `Assets/Resources/Imported/Food/*.png`, `Mushroom/*.png`, `Props/village_props.png`, and `Skeleton/*.png` have no manifest source yet.
- Before release, either recover original source/license entries for these files or replace them with manifest-tracked CC0 assets.
