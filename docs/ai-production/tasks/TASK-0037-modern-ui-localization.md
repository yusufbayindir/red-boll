# TASK-0037 Modern UI Localization

Status: Implementation Complete - QA PASS / Ready for main merge
Owner: Modern UI + Localization Unity Developer
Date: 2026-05-12

## Scope

- Deliver a working MVP for modernized runtime UI and localization.
- Keep Sprint02 generated gameplay polish intact.
- Add runtime language selection with PlayerPrefs persistence.

## Acceptance

- Main menu and level select have a more modern card/button/chip treatment.
- Runtime language selector is available in main menu, level select, and HUD.
- Basic runtime languages: `tr`, `en`, `es`, `fr`, `zh-Hans`, `hi`, `nl`, `de`, `pt-BR`, `ja`; extra `it`.
- HUD labels and primary button labels are localized.
- Existing levels show visible runtime polish beyond UI: parallax band, route arrows, hazard glow, and goal halo.
- Unity compile/import, EditMode smoke, and PlayMode smoke pass.

## Result

Implemented in `Assets/Scripts/RedBallRuntime.cs`, `Assets/Scripts/RedBallUi.cs`, and new `Assets/Scripts/RedBallLocalization.cs`.

Programmatic UI assets added under `Assets/Resources/Generated/Sprint03/`:
`panel_glass`, `button_primary`, `button_secondary`, `language_chip`, `hud_panel`, `level_card`, and `feature_chip`.

Programmatic gameplay/decor assets added under `Assets/Resources/Generated/Sprint04/`:
`parallax_hills`, `route_arrow`, `hazard_floor_glow`, and `goal_halo`.

## QA

- Branch: `feature/sprint04-modern-ui-localization-polish`.
- Unity import/compile: PASS via Unity 6000.4.5f1 batchmode on the current project, log `/tmp/red_ball_sprint04_compile.log`.
- EditMode smoke: PASS, 22/22, `TestResults/editmode-modern-ui-localization-sprint04.xml`.
- PlayMode smoke: PASS, 5/5, `TestResults/playmode-modern-ui-localization-sprint04.xml`.
- EditMode now asserts required non-English core strings do not fall back to English and that every Level 1-15 has Sprint04 parallax, route arrow, and goal halo runtime objects.

## Technical Debt

Technical debt added: TD-0014 for deeper localization completeness, RTL/complex shaping validation, and full manual UI overflow QA.

## Activity Log

- 2026-05-12: Added Sprint03 UI skin set, runtime localization helper, language selector, localized menu/level/HUD labels, and localized badge/status messaging.
- 2026-05-12: Expanded localization overlays for Spanish, French, Dutch, German, Portuguese-Brazil, Italian, Simplified Chinese, Hindi, and Japanese so core UI does not silently fall back to English.
- 2026-05-12: Added Sprint04 gameplay polish assets and integrated parallax/decor/route/hazard/goal markers into all 15 existing levels without changing collision balance.
- 2026-05-12: QA passed on current project: compile/import PASS, EditMode 22/22 PASS, PlayMode 5/5 PASS.
