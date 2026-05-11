# TASK-0010 - Checkpoints And Warning Signs

## Status

Implementation Complete - QA Pending

## Owner

PM, Developer Gameplay, Level Designer, QA

## PM Intent

Make longer/future levels fair before expanding past Level 13. Checkpoints reduce frustration; warning signs make new dangers readable.

## Scope

In:

- Checkpoint trigger support.
- Visual checkpoint using existing assets.
- Respawn at latest checkpoint after damage/fall.
- Warning sign decoration helper.
- Add checkpoints/warnings to at least one existing late level or a Level 14 prototype after PM approval.

Out:

- Full Levels 14-20 expansion.
- Crumble platforms.
- Switch gates.
- Changing heart economy, except where needed to avoid unfair checkpoint behavior.

## Acceptance Criteria

- Player spawn updates after touching checkpoint.
- Damage/fall respawns at checkpoint instead of level start.
- Checkpoint can be visually distinguished from goal.
- Warning sign helper can place `tile_exclamation` clearly.
- Existing levels without checkpoints still work.
- Related task MD is updated before and after each code step.
- QA verifies repeated respawns, restart behavior, and level completion after checkpoint.

## Risk Check

- Shared systems touched: `playerSpawn`, `DamagePlayer`, fall/respawn logic, level helpers.
- Regression risks: checkpoint mistaken for goal, restart starts from checkpoint when full level restart is expected, stale checkpoint after level reload.
- Rollback idea: checkpoint writes only to `playerSpawn`; `LoadLevel` resets spawn from level builder.

## Activity Log

- 2026-05-11 23:40 - Draft task created by PM from TASK-0005 report.
- 2026-05-12 00:20 - PM opened implementation. Intended touches: `Assets/Scripts/RedBallRuntime.cs`, new checkpoint trigger component file, and this task MD. Scope is checkpoint support, warning sign helper, and a small Level 13 integration only.
- 2026-05-12 00:23 - PM added checkpoint trigger support, warning sign helper, and Level 13 checkpoint/warning placement.
- 2026-05-12 00:25 - PM ran Unity 6000.4.5f1 batch import/compile successfully after checkpoint changes.

## Implementation Plan

- Add `ActivateCheckpoint(Vector2 spawn)` to `RedBallGame`.
- Add `AddCheckpoint(Vector2 position)` helper using existing yellow body and flag sprites.
- Add `AddWarningSign(Vector2 position)` helper using `tile_exclamation`.
- Add a separate `CheckpointTrigger` component file.
- Place one checkpoint and warning signs in Level 13 as the first proof.
- Compile in Unity batchmode.

## Changed Files

- `Assets/Scripts/RedBallRuntime.cs`
- `Assets/Scripts/RedBallCheckpoint.cs`
- `Assets/Scripts/RedBallCheckpoint.cs.meta`
- `docs/ai-production/tasks/TASK-0010-checkpoints-warning-signs.md`

## Verification

- Unity 6000.4.5f1 batch import/compile log `/tmp/redball-unity-checkpoint.log` shows `Tundra build success`, `AssetDatabase: script compilation time: 2.513699s`, and `Exiting batchmode successfully now!`.
- `rg -n "(error CS|Scripts have compiler errors|Compilation failed|Build failed|Tundra build failed|Exception:|Fatal error)" /tmp/redball-unity-checkpoint.log` returned no matches.
- Static source review confirmed Level 13 now has one checkpoint at `new Vector2(43f, 2.15f)` and warning signs before late hazards/timing beats.
- Full play-mode checkpoint QA not run yet.

## QA Notes

QA pending. Must verify Level 13 checkpoint pickup, damage/fall respawn at checkpoint, restart still reloads from level start, and completion after checkpoint still works.

## Repo Controller Notes

Changed files are limited to source/docs for this task: `Assets/Scripts/RedBallRuntime.cs`, `Assets/Scripts/RedBallCheckpoint.cs`, `Assets/Scripts/RedBallCheckpoint.cs.meta`, and this task MD.

## PM Closure

Pending.
