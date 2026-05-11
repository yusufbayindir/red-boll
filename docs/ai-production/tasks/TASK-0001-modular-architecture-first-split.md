# TASK-0001 - Modular Architecture Plan And First Split

## Status

QA Pending - Unity Compile Passed

## Owner

Developer Gameplay, PM, QA, Git Repo Controller

## PM Intent

Stop the project from growing as one giant runtime file. Create a practical modular path that keeps the game working while separating systems over time.

## Scope

In:

- Analyze `Assets/Scripts/RedBallRuntime.cs`.
- Propose a modular target structure.
- Identify the safest first split.
- If implementation is approved, split one low-risk slice.
- Update this MD before and after every code step.

Out:

- Full rewrite.
- Changing gameplay feel without a separate task.
- Adding new features during refactor.

## Acceptance Criteria

- A proposed file/module structure exists.
- The first split has low regression risk.
- Any code change preserves current gameplay behavior.
- Unity compile path is considered.
- QA has a concrete checklist.
- Git Repo Controller reviews changed files.

## Proposed Modular Structure

- `RedBallRuntime.cs`: bootstrap plus `RedBallGame` orchestration, screen flow, and temporary glue while splits continue.
- `RedBallLevels.cs`: level selection and data/build definitions for the 13 current levels.
- `RedBallLevelObjects.cs`: player spawn, platforms, pickups, hazards, enemies, goal, dressing, and object factory helpers.
- `RedBallUi.cs`: menu/HUD/level-select construction, UI refresh, screen root switching, touch reset.
- `RedBallEconomy.cs`: hearts, recharge timing, progress unlocks, PlayerPrefs save/load.
- `RedBallAudio.cs`: generated SFX clip setup and playback helpers.
- `RedBallAssets.cs`: sprite/resource loading, fallback texture, atlas/animation helpers.
- `RedBallRuntimeComponents.cs`: standalone MonoBehaviour components used by runtime-generated objects.

## First Split Proposal

Extract `RedBallPlayer` from `RedBallRuntime.cs` into `Assets/Scripts/RedBallPlayer.cs` while preserving its class name, movement values, and public methods. This is the safest first split because the player component is runtime-added with `AddComponent<RedBallPlayer>()`, has no serialized scene reference in this project, and only depends on `RedBallGame` through existing public methods/properties.

## Risk Check

- Shared systems touched: likely `RedBallGame`, UI setup, level builders, object helper methods.
- Regression risks: missing partial class modifier, Unity compile errors, broken resource keys, broken level spawn.
- Rollback idea: first split should be reversible by moving methods back.
- First split risk check: no gameplay values or method bodies should change; main compile risks are duplicate/missing `RedBallPlayer` declarations or a missing `UnityEngine` using directive in the new file.
- First split rollback idea: move `RedBallPlayer` back under `RedBallRuntime.cs`.

## Activity Log

- 2026-05-11 23:20 - Task created by PM.
- 2026-05-11 23:28 - Developer Gameplay analyzed `Assets/Scripts/RedBallRuntime.cs`; proposed target modular structure; intends to touch `Assets/Scripts/RedBallRuntime.cs` and create one `Assets/Scripts/RedBall*.cs` file for the first low-risk component split.
- 2026-05-11 23:28 - Developer Gameplay narrowed first code step to only extract `RedBallPlayer` into `Assets/Scripts/RedBallPlayer.cs` for the smallest safe compile step.
- 2026-05-11 23:29 - Developer Gameplay extracted `RedBallPlayer` from `Assets/Scripts/RedBallRuntime.cs` into `Assets/Scripts/RedBallPlayer.cs` with method bodies and movement values preserved.
- 2026-05-11 23:32 - Developer Gameplay verified one `RedBallPlayer` declaration, preserved runtime references, and passed a direct Unity/Roslyn script compile probe using Unity 6000.4.5f1 reference assemblies.
- 2026-05-11 23:50 - PM reviewed split output and confirmed one `RedBallPlayer` declaration plus unchanged runtime references. PM will run Unity batch import/compile next; expected additional source-control file is `Assets/Scripts/RedBallPlayer.cs.meta`.
- 2026-05-11 23:53 - PM attempted Unity 6000.4.5f1 batch import/compile. It was blocked because another Unity instance already has this project open.
- 2026-05-11 23:54 - PM added `Assets/Scripts/RedBallPlayer.cs.meta` manually using the same minimal script meta format already used by `RedBallRuntime.cs.meta`, with a unique GUID.
- 2026-05-11 23:56 - Yusuf closed the open Unity instance; PM reran Unity 6000.4.5f1 batch import/compile successfully.

## QA Checklist

- Open the project in Unity 6000.4.5f1 and confirm the console has no compile errors after asset import.
- Start the game from the main menu and load Level 1.
- Verify keyboard movement, joystick movement, keyboard jump, and jump button still work.
- Verify falling below `KillY`, hazard touch, bounce pad, coin pickup, enemy stomp/contact, respawn, and level completion still behave as before.
- Verify menu, restart, and level-select navigation still work after entering gameplay.

## Changed Files

- `Assets/Scripts/RedBallRuntime.cs` - removed the in-file `RedBallPlayer` declaration; existing references and `AddComponent<RedBallPlayer>()` remain unchanged.
- `Assets/Scripts/RedBallPlayer.cs` - new extracted player movement component with the original method bodies and values.
- `docs/ai-production/tasks/TASK-0001-modular-architecture-first-split.md` - added plan, risk check, activity log, changed files, verification, and QA checklist.
- `Assets/Scripts/RedBallPlayer.cs.meta` - new Unity meta file with stable GUID for source control.

## Verification

- `rg -n "class RedBallPlayer|AddComponent<RedBallPlayer>|GetComponent<RedBallPlayer>|RedBallPlayer" Assets/Scripts` confirms a single `RedBallPlayer` declaration and unchanged runtime references.
- Unity/Roslyn compile probe passed using Unity 6000.4.5f1 reference assemblies, compiling `Assets/Scripts/RedBallRuntime.cs` and `Assets/Scripts/RedBallPlayer.cs` to `/tmp/redball_compile_probe.dll`.
- Initial full Unity batch import/compile was blocked by an open Unity instance, then rerun after Yusuf closed it.
- PM local review confirmed `rg -n "RedBallPlayer|public sealed class RedBallPlayer|AddComponent<RedBallPlayer>" Assets/Scripts` shows one class declaration and existing references only.
- Unity 6000.4.5f1 batch import/compile log `/tmp/redball-unity-import.log` shows `Tundra build success`, `AssetDatabase: script compilation time: 0.630904s`, and `Exiting batchmode successfully now!`.
- `rg -n "(error CS|Scripts have compiler errors|Compilation failed|Build failed|Tundra build failed|Exception:|Fatal error)" /tmp/redball-unity-import.log` returned no matches.
- Full play-mode smoke QA is still not run; QA should run the checklist above before PM closure.

## QA Notes

Compile/import passed in Unity 6000.4.5f1 batchmode. Full closure still requires play-mode smoke because this was a behavior-preserving refactor of the player component.

## Repo Controller Notes

Changed files are limited to source/docs for this task: `Assets/Scripts/RedBallRuntime.cs`, `Assets/Scripts/RedBallPlayer.cs`, `Assets/Scripts/RedBallPlayer.cs.meta`, and this task MD. Generated Unity cache/build folders are not task output.

## PM Closure

Pending.
