# Visible Game Update Report

Date: 2026-05-12  
Owner: Visible Game Change Developer  
Task: `TASK-0032`

## Summary

Implemented a visible Sprint 01 identity pass in the runtime UI. The main menu now opens with `SPRINT 01 MASTERY UPDATE`, a large `RED BALL MASTERY UPDATE` title, concept art, and a feature panel calling out 15 levels, badges, Level 14 lifts, and Level 15 crumbling tiles.

Level Select now has a Mastery banner, badge legend, highlighted Level 14/15 feature cards, and locked-state feature labels so players can see what is new before progression unlocks those levels.

## Player-Visible Changes

- First screen: dark refreshed menu, orange Mastery banner, generated concept preview image, `15 levels + badges + late-game preview`, and a Sprint 01 feature panel.
- Level Select: `Mastery Level Select` header, `Clear / All Coins / Clean Run` badge language, and visible `NEW LIFT` / `NEW CRUMBLE` previews for Level 14/15.
- Locked Level 14/15 buttons now show `LIFTS` / `CRUMBLE` hints instead of looking like ordinary locked levels.

## Asset Note

Runtime preview asset:

- `Assets/Resources/Generated/Sprint01/mastery_update_concept.png`
- Source/provenance archive: `docs/ai-production/generated-assets/2026-05-12/red-ball-creative-board-v1.png`
- SHA-256: `e4783cfe4e8ba41c92a0f2e569bab3b5cd0d4ef6f99ddf441aa2ededb79568ac`

Status: concept-only / not final UI / not store or paid-media approved. Manifest updated with the runtime copy and `TD-0013` reference.

## Verification

- Unity compile/import passed: `Logs/visible-game-update-compile.log`.
- Compile log error-pattern scan returned no matches.
- EditMode smoke passed: 6/6 in `TestResults/visible-game-update-editmode-results.xml`.
- PlayMode smoke passed: 5/5 in `TestResults/visible-game-update-playmode-results.xml`.

## Rebuild Note

This change is runtime code plus a new Unity resource. It will be visible after reopening/refreshing Unity Play mode or making a new build. An old installed/build binary will not show this update.

## Technical Debt

Technical debt added: none. Existing debt referenced: `TD-0013`.
