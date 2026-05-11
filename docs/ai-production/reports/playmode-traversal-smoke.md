# PlayMode Traversal Smoke Report

Date: 2026-05-12

## Summary

Added PlayMode smoke automation for Sprint 01 traversal risk. The tests load Level 1, Level 14, and Level 15 through the runtime builder, verify object contracts, step physics for the Level 14 lift and Level 15 crumble mechanic, and confirm badge persistence roundtrip through PlayerPrefs.

`TD-0006` is reduced but not closed.

## Files Changed

- `Assets/Tests/PlayMode/RedBall.Tests.PlayMode.asmdef`
- `Assets/Tests/PlayMode/RedBall.Tests.PlayMode.asmdef.meta`
- `Assets/Tests/PlayMode/RedBallTraversalSmokePlayModeTests.cs`
- `Assets/Tests/PlayMode/RedBallTraversalSmokePlayModeTests.cs.meta`
- `Assets/Tests/PlayMode.meta`
- `docs/ai-production/tasks/TASK-0026-playmode-traversal-smoke.md`
- `docs/ai-production/reports/playmode-traversal-smoke.md`
- `docs/ai-production/STATUS_BOARD.md`
- `TestResults/playmode-traversal-smoke-results.xml`
- `Logs/playmode-traversal-smoke.log`

## Test Coverage

- `TargetLevelsBuildExpectedTraversalObjects`
  - Level 1: player, goal, hazard.
  - Level 14: player, goal, hazards, checkpoint, two moving platforms.
  - Level 15: player, goal, hazards, checkpoint, three crumbling tiles.
- `Level14VerticalLiftMovesDuringPhysicsStep`
  - Steps PlayMode physics and verifies the first Level 14 lift moves vertically while the player remains above kill bounds.
- `Level15FirstCrumblingTileCollapsesAfterPlayerContact`
  - Places the runtime player onto the first crumbling tile and verifies the tile collider collapses after physics steps.
- `GoalTriggerCompletesLevelAndPersistsBadgeMasks`
  - Overlaps the player with the Level 14 goal trigger and verifies completion, Clear badge, and Clean Run badge masks persist.
- `BadgeMasksRoundTripThroughPlayerPrefsAndRuntimeLoad`
  - Writes Level 1/14/15 badge masks to PlayerPrefs, creates a fresh runtime instance, and verifies the masks are loaded.

## Command

```bash
/Applications/Unity/Hub/Editor/6000.4.5f1/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -projectPath "/Users/yusufbayindir/Desktop/ai game/red_ball" \
  -runTests \
  -testPlatform playmode \
  -testResults "/Users/yusufbayindir/Desktop/ai game/red_ball/TestResults/playmode-traversal-smoke-results.xml" \
  -logFile "/Users/yusufbayindir/Desktop/ai game/red_ball/Logs/playmode-traversal-smoke.log"
```

## Result

PASS. Unity exited with code `0`.

- XML: `TestResults/playmode-traversal-smoke-results.xml`
- Log: `Logs/playmode-traversal-smoke.log`
- XML summary: `testcasecount="5" result="Passed" total="5" passed="5" failed="0"`
- Log note: final run has no C# compiler warnings. Unity logs one licensing handshake warning and two transient AudioListener warnings, but the Test Runner completed successfully.

## Remaining Coverage Gaps

- No real keyboard/touch input route runner exists yet.
- Level 14 and Level 15 safe-route completion under player-like movement is not proven by this smoke.
- Level 14 optional skill route, Level 15 crumble-over-pit recovery, crumble/checkpoint no-soft-lock, mobile HUD readability, and three phone-landscape viewport checks remain open.
- `TD-0006` should stay open until manual/mobile QA or a stronger input-driven automation harness covers those flows.

## Technical Debt

Technical debt added: none. Existing debt referenced: `TD-0006`, `TD-0009`.
