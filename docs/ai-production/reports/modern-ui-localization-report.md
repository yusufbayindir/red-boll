# Modern UI Localization Report

Date: 2026-05-12
Task: TASK-0037
Owner: Modern UI + Localization Unity Developer

## Shipped MVP

- Modernized main menu with premium glass card, polished primary/secondary buttons, feature chips, mastery badge strip, language selector, and preserved Sprint01 key-art preview.
- Modernized level select with card-style level buttons, badge icons, featured Level 14/15 tags, localized title/subtitle/legend, back button, and language selector.
- Updated gameplay HUD with compact glass top panel, localized health/score/coin/level labels, cleaner touch-control plates, menu/restart icons, and in-game language selector.
- Added `RedBallLocalization` runtime helper with PlayerPrefs persistence at `RedBall.Language`.
- Default language maps from `Application.systemLanguage` when supported; otherwise falls back to English.
- Added Sprint04 gameplay polish across the existing 15 levels: parallax landscape band, route arrows, hazard floor glow, and goal halo.

## Languages

Runtime selector supports: `en`, `tr`, `es`, `fr`, `zh-Hans`, `hi`, `nl`, `de`, `pt-BR`, `ja`, `it`.

MVP coverage includes primary menu, buttons, level select labels, HUD labels, level state labels, level feature labels, level names, heart/status basics, and completion badge summary. Spanish, French, Dutch, German, Portuguese-Brazil, Italian, Simplified Chinese, Hindi, and Japanese now override the core UI/status keys that the smoke test protects from English fallback. Some deep/gameplay strings and linguistic QA remain debt.

## Font Strategy

Runtime uses `Font.CreateDynamicFontFromOSFont` against a fallback family list covering common Latin, CJK, Japanese, and Devanagari fonts: SF Pro, Arial Unicode MS, Noto Sans, Noto Sans CJK SC, Noto Sans Devanagari, PingFang SC, Hiragino Sans, Yu Gothic, Meiryo, Microsoft YaHei, Nirmala UI, Mangal, Segoe UI, Arial. If none resolve, it falls back to Unity built-in fonts.

## Assets

Sprint03 UI PNGs are original programmatic raster assets generated locally with Python 3 and Pillow. No external download or AI model output was used.

Sprint04 gameplay/decor PNGs are also original programmatic raster assets generated locally with Python 3 and Pillow:

- `Assets/Resources/Generated/Sprint04/parallax_hills.png`
- `Assets/Resources/Generated/Sprint04/route_arrow.png`
- `Assets/Resources/Generated/Sprint04/hazard_floor_glow.png`
- `Assets/Resources/Generated/Sprint04/goal_halo.png`

## Verification

- Branch: `feature/sprint04-modern-ui-localization-polish`.
- Unity 6000.4.5f1 import/compile: PASS on the current project, log `/tmp/red_ball_sprint04_compile.log`.
- EditMode: `TestResults/editmode-modern-ui-localization-sprint04.xml`, 22/22 PASS.
- PlayMode: `TestResults/playmode-modern-ui-localization-sprint04.xml`, 5/5 PASS.
- Static grep: critical old main UI hardcoded labels now route through `RedBallLocalization`; remaining Turkish strings are localization data, level root object names, or non-user-facing object labels.
- Runtime polish smoke: EditMode asserts all 15 existing levels expose Sprint04 parallax, route arrow, and goal halo objects.

## Remaining Risk

- Hindi and CJK rendering depends on OS font availability; fallback list is production-aware but not a bundled-font guarantee.
- RTL and full complex-script shaping are not implemented.
- Full string audit and manual small-screen overflow QA are deferred.

Technical debt added: TD-0014.
