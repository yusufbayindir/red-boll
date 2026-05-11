# Unity Play/EditMode Smoke Harness Report

Date: 2026-05-12

## Summary

Added a Unity Test Framework EditMode smoke harness for Sprint 01 QA evidence. The harness verifies the runtime level count, badge mask helper behavior, and Level 1/14/15 level construction paths.

## Files Changed

- `Packages/manifest.json`
- `Packages/packages-lock.json`
- `Assets/Scripts/RedBallRuntime.cs`
- `Assets/Tests/EditMode/RedBall.Tests.EditMode.asmdef`
- `Assets/Tests/EditMode/RedBallSmokeEditModeTests.cs`
- `Assets/Tests/EditMode*.meta`
- `TestResults/editmode-smoke-results.xml`
- `Logs/editmode-smoke.log`
- `docs/ai-production/tasks/TASK-0022-unity-playmode-smoke-harness.md`
- `docs/ai-production/reports/unity-playmode-smoke-harness.md`
- `docs/ai-production/STATUS_BOARD.md`

## Test Coverage

- `RuntimeConstantsExposeFifteenLevels`
- `BadgeMaskHelperReportsTargetLevelBits`
- `UiBadgeSummarySurfaceFormatsThreeBadgeStates`
- `TargetLevelsLoadWithoutThrowing(0, "Level 01")`
- `TargetLevelsLoadWithoutThrowing(13, "Level 14")`
- `TargetLevelsLoadWithoutThrowing(14, "Level 15")`

## Command

```bash
/Applications/Unity/Hub/Editor/6000.4.5f1/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -projectPath "/Users/yusufbayindir/Desktop/ai game/red_ball" \
  -runTests \
  -testPlatform editmode \
  -testResults "/Users/yusufbayindir/Desktop/ai game/red_ball/TestResults/editmode-smoke-results.xml" \
  -logFile "/Users/yusufbayindir/Desktop/ai game/red_ball/Logs/editmode-smoke.log"
```

## Result

PASS. Unity exited with code `0`.

- XML: `TestResults/editmode-smoke-results.xml`
- Log: `Logs/editmode-smoke.log`
- XML summary: `testcasecount="6" result="Passed" total="6" passed="6" failed="0"`

Note: the first package-import run produced compile/import only, and the first actual test run exposed an EditMode setup issue where `Awake` had not initialized sprites before the smoke loader. The harness now invokes `Awake` in setup when Unity has not already done so.

## Remaining Coverage Gaps

- This is EditMode smoke coverage, not full PlayMode traversal.
- The tests assert level construction and player creation, but do not simulate mobile controls, physics completion, checkpoint recovery, or Level 15 crumble/checkpoint route safety.
- `TD-0006` remains open until play-mode smoke/manual traversal evidence exists for affected gameplay flows.

## Technical Debt

Technical debt added: none. Existing debt referenced: `TD-0006`.
