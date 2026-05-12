# Runtime Generated Asset Pack Report

Date: 2026-05-12
Task: `TASK-0033-runtime-generated-asset-pack`
Owner: Runtime Asset Producer / Unity Integrator

## Summary

TASK-0033 produced and integrated a runtime-visible generated asset pack. The new art is not limited to docs or concept boards: it is loaded from Unity `Resources` and used by the main menu, level select, Level 14 lift platforms, Level 15 crumbling tiles, warning signs, and checkpoints.

## Asset List

| Asset | Path | Size | SHA-256 | Runtime use |
| --- | --- | ---: | --- | --- |
| Clear mastery badge | `Assets/Resources/Generated/Sprint02/mastery_badge_clear.png` | 160x160 | `1025e02e20b7a70485aa9e50896185e65bc0ebb917a799f39c6acbe8a5b63e7d` | Main menu badge strip; level select badge icon 1. |
| All Coins mastery badge | `Assets/Resources/Generated/Sprint02/mastery_badge_all_coins.png` | 160x160 | `a624b5a676ec0a684fe6ff99c59dd19cbb62246c99e19f9b2a23d3a2eb530579` | Main menu badge strip; level select badge icon 2. |
| Clean Run mastery badge | `Assets/Resources/Generated/Sprint02/mastery_badge_clean_run.png` | 160x160 | `b16ffcba42045f7c1013616891f4f7cabaaaa8a286a803ab4f25bc374728f841` | Main menu badge strip; level select badge icon 3. |
| Lift platform polish | `Assets/Resources/Generated/Sprint02/lift_platform_polish.png` | 320x112 | `d2466eae14fa8a9b20016e2a8048d9322e4b6a7363f3fe36c891dd6348312225` | Overlay on moving platforms, including Level 14 vertical lifts. |
| Crumbling tile polish | `Assets/Resources/Generated/Sprint02/crumbling_tile_polish.png` | 128x96 | `1bfdaca1cd55e0bff980374919b6fb5892f59e8437dfd7cfc77fe588e909f969` | Sprite for each Level 15 crumbling tile segment. |
| Warning spark | `Assets/Resources/Generated/Sprint02/warning_spark.png` | 128x128 | `34dd62c90c1eacfcaa34d01300189544e1324d4f0b4c7e803c3c3356cd50318a` | Overlay on runtime warning signs. |
| Checkpoint spark | `Assets/Resources/Generated/Sprint02/checkpoint_spark.png` | 128x128 | `bfd0a9674551015474a966173804df2a91e42f0c6d6406b04492d6860be13983` | Overlay on runtime checkpoints. |
| Dust sparkle | `Assets/Resources/Generated/Sprint02/dust_sparkle.png` | 128x128 | `502a8d891ede47b2e2c1a2c3fd9fdf51f3aee4d2a6be7c3b5652ef7102e10fab` | Added to Level 15 crumbling tile visuals. |

## Provenance

All eight PNGs are original handmade/programmatic raster assets generated locally with Python 3 and Pillow 11.1.0 on 2026-05-12. No third-party image sources, web downloads, AI image model outputs, or external source files were used. Usage rights are project-owned/original for this repository.

## Runtime Integration

- `RedBallRuntime.LoadSprites()` registers the Sprint02 sprites from `Resources/Generated/Sprint02`.
- `AddGeneratedSprite()` uses `Resources.Load<Texture2D>()`; if a generated PNG is missing, it assigns an existing UI or RedBall fallback sprite.
- Main menu: `CreateMasteryBadgeStrip()` renders clear/all-coins/clean-run badge PNGs.
- Level select: every level button receives three badge images; earned badges render white, missing badges are dimmed, and locked levels hide the icons.
- Level 14: `AddMovingPlatform()` adds a generated lift overlay, so both vertical lifts in Level 14 visibly use the new platform art.
- Level 15: `AddCrumblingTile()` renders generated cracked tile sprites and dust sparkles on crumble platforms.
- Warning/checkpoint polish: `AddWarningSign()` and `AddCheckpoint()` add generated overlay sprites.

## Verification

- Unity import/compile gate: PASS.
- EditMode smoke: PASS, 6/6.
- PlayMode smoke: PASS, 5/5.
- Test result files:
  - `TestResults/task0033-editmode-results.xml`
  - `TestResults/task0033-playmode-results.xml`
- Logs:
  - `Logs/task0033-import-compile.log`
  - `Logs/task0033-editmode.log`
  - `Logs/task0033-playmode.log`

## Technical Debt

Technical debt added: none. Existing `TD-0006` still covers manual/mobile route and viewport evidence beyond this runtime asset smoke pass.
