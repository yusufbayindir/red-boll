# TASK-0032 - Visible Game Update

## Status

QA PASS - Runtime UI Update Visible After Rebuild / Unity Play Reopen

## Owner

Visible Game Change Developer

## PM Intent

Make the Sprint 01 work obvious in the first screen and first minute. The player should not open the game and feel it is the same old build.

## Scope

In:

- Main menu identity refresh for Sprint 01 Mastery Update.
- Level Select preview/hints for Level 14 lifts, Level 15 crumbling tiles, and mastery badges.
- Concept-only generated creative board copy under `Assets/Resources/Generated/Sprint01/`.
- Unity compile/import gate and existing smoke coverage.
- Asset manifest and worker report updates.

Out:

- Unlocking all levels or changing progression rules.
- Replacing gameplay mechanics, physics, level completion, or heart economy.
- Treating generated concept art as final shipping UI, store art, or paid creative.

## Activity Log

- 2026-05-12 - Copied the archived ChatGPT/imagegen creative board into `Assets/Resources/Generated/Sprint01/mastery_update_concept.png` as a concept-only menu preview.
- 2026-05-12 - Updated runtime main menu with `SPRINT 01 MASTERY UPDATE`, `RED BALL MASTERY UPDATE`, 15-level/badge copy, and a first-screen concept preview image.
- 2026-05-12 - Updated Level Select with a Mastery banner, badge legend, Level 14/15 feature glows, `NEW LIFT` / `NEW CRUMBLE` tags, and locked-state feature labels.
- 2026-05-12 - Ran Unity compile/import, EditMode smoke, and PlayMode smoke successfully.

## Changed Files

- `Assets/Scripts/RedBallRuntime.cs`
- `Assets/Scripts/RedBallUi.cs`
- `Assets/Resources/Generated.meta`
- `Assets/Resources/Generated/Sprint01.meta`
- `Assets/Resources/Generated/Sprint01/mastery_update_concept.png`
- `Assets/Resources/Generated/Sprint01/mastery_update_concept.png.meta`
- `docs/ai-production/assets-manifest.md`
- `docs/ai-production/tasks/TASK-0032-visible-game-update.md`
- `docs/ai-production/reports/visible-game-update-report.md`
- `Logs/visible-game-update-compile.log`
- `Logs/visible-game-update-editmode.log`
- `Logs/visible-game-update-playmode.log`
- `TestResults/visible-game-update-editmode-results.xml`
- `TestResults/visible-game-update-playmode-results.xml`

## Verification

- Compile/import: Unity 6000.4.5f1 batchmode, exit code `0`, log `Logs/visible-game-update-compile.log`.
- Compile log scan: no matches for `error CS`, `Scripts have compiler errors`, `Compilation failed`, `Build failed`, `Tundra build failed`, `Exception:`, or `Fatal error`.
- EditMode smoke: `TestResults/visible-game-update-editmode-results.xml`, `testcasecount="6" result="Passed" total="6" passed="6" failed="0"`.
- PlayMode smoke: `TestResults/visible-game-update-playmode-results.xml`, `testcasecount="5" result="Passed" total="5" passed="5" failed="0"`.

## QA Notes

- The visible update appears when the Unity project is reopened/refreshed and Play is run, or after a new app build is made. A previously built app binary will still show the old menu.
- The generated creative board is a concept-only runtime preview, not a final UI asset or marketing-approved key art.
- Progression remains gated by existing unlock/completion state. Level 14/15 can be previewed in Level Select while locked, but cannot be started until unlocked.

## QA Owner Review

- 2026-05-12 - QA PASS. Verified Cicero evidence from Unity compile/import log and EditMode/PlayMode smoke result XMLs; no rerun needed because logs/results were produced after the visible update import.
- Static UI QA PASS: `RedBallRuntime.cs` contains the Sprint 01 main menu copy, concept preview `Resources.Load<Texture2D>` path `Generated/Sprint01/mastery_update_concept`, Mastery Level Select copy, Level 14/15 tags, badge legend, and feature label refresh path.
- Asset path QA PASS: `Assets/Resources/Generated/Sprint01/mastery_update_concept.png` exists, is `1536 x 1024` PNG at about `2.0M`, and has Unity `.meta` import data.
- Player visibility note: players see the update only after reopening/refreshing the Unity project and pressing Play, or after rebuilding the app. Existing built binaries do not pick up these runtime/resource changes.
- Residual risk: QA did not perform a rendered manual Play screenshot pass, so this pass verifies compile/import, smoke behavior, resource presence, and static UI wiring rather than exact visual composition on every target aspect ratio.

## Technical Debt Added

Technical debt added: none. Existing debt referenced: `TD-0013` for generated-asset intake/export gating.
