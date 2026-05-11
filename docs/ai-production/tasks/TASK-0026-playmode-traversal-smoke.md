# TASK-0026 - PlayMode Traversal Smoke

## Status

Implementation complete - PlayMode evidence generated

## Owner

PlayMode Traversal Automation Developer

## Context

Sprint 01 already has EditMode smoke coverage for Level 1/14/15 construction, but `TD-0006` remains open because real traversal and mobile QA are still incomplete. This task reduces the gap with PlayMode automation that builds runtime levels, steps Unity physics, and verifies key object contracts.

## Scope

- Add a PlayMode Unity Test Framework assembly under `Assets/Tests/PlayMode`.
- Load/build Level 1, Level 14, and Level 15 through the existing runtime smoke API.
- Verify player spawn, goal, hazards, checkpoints, vertical lift, and crumbling tile presence.
- Add PlayMode physics smoke for Level 14 vertical lift motion.
- Add PlayMode physics smoke for Level 15 crumbling tile collapse after player contact.
- Add goal-trigger completion persistence smoke for Level 14.
- Add PlayerPrefs badge-mask roundtrip verification.

## Testability Surface

No new production runtime surface was added in this task. Tests reuse the existing `RedBallGame.LoadLevelForSmokeTest`, `PlayerTransform`, `KillY`, and badge PlayerPrefs keys introduced by the earlier smoke harness.

## Verification

Command:

```bash
/Applications/Unity/Hub/Editor/6000.4.5f1/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -projectPath "/Users/yusufbayindir/Desktop/ai game/red_ball" \
  -runTests \
  -testPlatform playmode \
  -testResults "/Users/yusufbayindir/Desktop/ai game/red_ball/TestResults/playmode-traversal-smoke-results.xml" \
  -logFile "/Users/yusufbayindir/Desktop/ai game/red_ball/Logs/playmode-traversal-smoke.log"
```

Result: PASS. Unity exited with code `0`.

Evidence:

- `TestResults/playmode-traversal-smoke-results.xml`
- `Logs/playmode-traversal-smoke.log`
- XML summary: `testcasecount="5" result="Passed" total="5" passed="5" failed="0"`

Log note: final run has no C# compiler warnings. Unity logs one licensing handshake warning and two transient AudioListener warnings, but the Test Runner completed successfully.

## Remaining Coverage Gap

- This is automated PlayMode smoke, not full player-like safe-route traversal.
- The tests do not inject keyboard/touch input through the real controls, so they do not prove Level 14/15 end-to-end safe route completion under normal movement.
- The tests do not cover phone-landscape mobile controls, HUD readability, Level 14 optional skill route, Level 15 crumble/checkpoint no-soft-lock, or three-viewport mobile evidence required by `TD-0006`.
- Manual/mobile QA from `TASK-0024` remains required before `TD-0006` can close.

## Technical Debt Added

None. Existing `TD-0006` remains open and reduced; `TD-0009` remains relevant for crumbling tile checkpoint reset behavior.
