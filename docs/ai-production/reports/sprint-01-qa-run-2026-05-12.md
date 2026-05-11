# Sprint 01 QA Run - 2026-05-12

Owner: Sprint 01 Unity QA Runner  
Workspace: `/Users/yusufbayindir/Desktop/ai game/red_ball`  
Verdict: QA PARTIAL - compile/import PASS, play-mode load-path automation unavailable  
Implementation under test: Goodall Sprint 01 MVP handoff

## Scope Run

- Unity compile/import gate rerun.
- Unity Test Runner smoke attempt for EditMode and PlayMode.
- Static QA of `RedBallRuntime.cs`, `RedBallUi.cs`, and `RedBallCrumblingTile.cs` for Sprint 01 blocker criteria.
- No product/runtime code changes made.

## Commands

```bash
git status --short
cat ProjectSettings/ProjectVersion.txt
/Applications/Unity/Hub/Editor/6000.4.5f1/Unity.app/Contents/MacOS/Unity -batchmode -quit -projectPath /Users/yusufbayindir/Desktop/ai\ game/red_ball -logFile /tmp/red_ball_qa/sprint01-compile-20260512.log
rg -n "error CS|Scripts have compiler errors|Compilation failed|Build failed|Tundra build failed|Exception:|Fatal error" /tmp/red_ball_qa/sprint01-compile-20260512.log
/Applications/Unity/Hub/Editor/6000.4.5f1/Unity.app/Contents/MacOS/Unity -batchmode -projectPath /Users/yusufbayindir/Desktop/ai\ game/red_ball -runTests -testPlatform EditMode -testResults /tmp/red_ball_qa/editmode-results-20260512.xml -logFile /tmp/red_ball_qa/editmode-20260512.log -quit
/Applications/Unity/Hub/Editor/6000.4.5f1/Unity.app/Contents/MacOS/Unity -batchmode -projectPath /Users/yusufbayindir/Desktop/ai\ game/red_ball -runTests -testPlatform PlayMode -testResults /tmp/red_ball_qa/playmode-results-20260512.xml -logFile /tmp/red_ball_qa/playmode-20260512.log -quit
```

Note: An initial parallel PlayMode Test Runner attempt exited with code 1 because another Unity instance was already opening the same project. The sequential PlayMode command above exited 0.

## Compile Gate

Result: PASS.

- Unity version: `6000.4.5f1`, matching `ProjectSettings/ProjectVersion.txt`.
- Exit code: 0.
- Log: `/tmp/red_ball_qa/sprint01-compile-20260512.log`.
- Evidence in log: `Tundra build success`, `CompileScripts`, and `Exiting batchmode successfully now!`.
- Required error-pattern scan: no matches for `error CS`, `Scripts have compiler errors`, `Compilation failed`, `Build failed`, `Tundra build failed`, `Exception:`, or `Fatal error`.

## Automation Smoke

Result: BLOCKED for Level 1/14/15 load-path coverage.

- EditMode Test Runner command exited 0 but produced no `/tmp/red_ball_qa/editmode-results-20260512.xml`.
- PlayMode Test Runner command exited 0 but produced no `/tmp/red_ball_qa/playmode-results-20260512.xml`.
- Project has no Sprint 01 test assemblies or existing smoke methods for loading Level 1, Level 14, or Level 15 without adding code.
- Residual risk: Level 1/14/15 runtime loadability, route completion, persistence after relaunch, and mobile layout remain unverified in Play mode.

## Static QA Findings

No confirmed blocker bug found by compile/static QA.

| Criterion | Result | Evidence |
| --- | --- | --- |
| Compile/import failure | PASS | Unity compile gate passed with clean error-pattern scan. |
| `LevelCount` includes 15 | PASS STATIC | `RedBallRuntime.cs:25`, HUD/level select loops route through `LevelCount`. |
| Level 14/15 load routing | PASS STATIC | `LoadLevel` routes index 13 to `BuildLevelFourteen` and default/index 14 to `BuildLevelFifteen` at `RedBallRuntime.cs:381-428`. |
| Save/progression separation | PASS STATIC | Badge keys are separate from legacy unlock/completion keys at `RedBallRuntime.cs:28-32`; legacy completion is OR'd into clear badge state at `RedBallRuntime.cs:439-443`. |
| Replay improvement without relock | PASS STATIC | Badge masks are OR'd on completion and unlock uses legacy completed/unlocked saves at `RedBallRuntime.cs:458-498`. |
| Clean Run invalidation | PASS STATIC | `DamagePlayer` invalidates clean run at `RedBallRuntime.cs:231-253`; hazards and enemy side contacts call `DamagePlayer` at `RedBallRuntime.cs:2365-2384` and `RedBallRuntime.cs:2458-2475`; fall below `KillY` calls `DamagePlayer` in `RedBallPlayer.cs:55-58`. |
| Restart/new attempt reset | PASS STATIC | `RestartLevel` reloads the level and `LoadLevel` resets attempt state at `RedBallRuntime.cs:308-314` and `RedBallRuntime.cs:369-377`. |
| All Coins only if coins exist | PASS STATIC | `SaveMasteryBadges` requires `coinsInLevel > 0` and collected count >= total at `RedBallRuntime.cs:469-483`. |
| Level 14 builder | PASS STATIC WITH RISK | Vertical lift speed is 1.25 for first lift and 1.35 for later lift at `RedBallRuntime.cs:1013` and `RedBallRuntime.cs:1018`; no physical playthrough evidence. |
| Level 15 crumbling tile reset | PASS STATIC WITH RISK | Reload/restart destroys and rebuilds level objects at `RedBallRuntime.cs:1105-1118`; checkpoint respawn does not reset collapsed tiles, already registered as TD-0009. |
| Mobile UI text overflow | NOT VERIFIED | Static layout uses 15 level buttons and compact `GCT` text, but no screenshot/viewport play-mode pass was available. |
| Forced retry/mastery monetization | PASS STATIC | No new ad, timer, currency, energy, or retry gate found in touched Sprint 01 flow; existing hearts remain. |

## Bugs

Confirmed bugs: none from compile/static QA.

Potential issues requiring PlayMode/manual QA before PASS:

- Level 14 and Level 15 safe-route completion cannot be proven from static inspection alone.
- Clean Run badge behavior after real hazard, enemy, and fall interactions needs play-mode verification.
- Badge persistence after stop/relaunch needs real PlayerPrefs play-mode verification.
- Mobile landscape HUD, level-select badges, completion copy, joystick, and jump button need screenshot or device/aspect verification.

## Residual Risks

- TASK-0021 cannot be closed as PASS because no play-mode or edit-mode automation exists for Level 1/14/15 load paths.
- Unity Test Runner commands exited successfully but produced no result XML, so they are not evidence of scenario coverage.
- TD-0006 remains open: compile-only verification can miss runtime behavior failures.
- TD-0009 remains relevant: crumbling tiles do not reset on checkpoint respawn; current Level 15 authoring appears to place the checkpoint before later crumbling sections, but runtime soft-lock risk is not play-tested.

## Technical Debt

Technical debt added: none.

Existing debt referenced: TD-0006, TD-0009.

## QA Verdict

QA PARTIAL. Sprint 01 compile/import is green and static QA did not find a confirmed blocker. Sprint 01 should not be marked full PASS until PlayMode/manual evidence covers Level 1/14/15 load paths, badge persistence, Clean Run failure paths, crumbling checkpoint/restart behavior, and mobile readability.
