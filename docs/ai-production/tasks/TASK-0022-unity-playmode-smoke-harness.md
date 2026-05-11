# TASK-0022 - Unity Play/EditMode Smoke Harness

## Status

Implementation complete - QA evidence generated

## Owner

Unity Test Harness Developer

## Context

Rawls QA returned PARTIAL for Sprint 01: compile/import passed, but there was no Unity Test Framework package, no test assembly, and no automated XML evidence for Level 1/14/15 smoke coverage.

## Scope

- Add Unity Test Framework to `Packages/manifest.json`.
- Add an EditMode test assembly under `Assets/Tests/EditMode`.
- Add minimal runtime testability surface without changing gameplay behavior:
  - public `RedBallGame.LevelCount`
  - public badge mask helper
  - public smoke-only level loader and level object count getter
- Generate Unity batchmode EditMode result XML/log evidence.

## Acceptance Criteria

- EditMode tests compile in Unity Test Runner.
- Static LevelCount is verified as 15.
- Badge mask helper verifies target bits for Level 1, 14, and 15.
- Level 1, 14, and 15 build/load paths execute without throwing and create a player/runtime objects.
- Result XML and log paths are recorded in the report.

## Evidence

- Report: `docs/ai-production/reports/unity-playmode-smoke-harness.md`
- Test XML: `TestResults/editmode-smoke-results.xml` (`6/6` passing)
- Test log: `Logs/editmode-smoke.log` (Unity exit code `0`)

## Technical Debt

Existing `TD-0006` is referenced. This harness reduces the compile-only gap but does not close full play-mode/manual traversal coverage by itself.
