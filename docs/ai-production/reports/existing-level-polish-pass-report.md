# Existing Level Polish Pass Report

Date: 2026-05-12
Task: `TASK-0035-existing-level-polish-pass`
Owner: Existing Level Art Polish Developer

## Summary

Level 1-13 now use the Sprint02 generated polish pack visibly, not only Level 14/15. The pass is decorative and readability-focused: coin/route sparkles, hazard warning signs, existing checkpoint sparkle, and existing lift overlays. No gameplay colliders, enemy patrol ranges, hazard triggers, checkpoint flow, unlocks, or mastery logic were changed.

## Runtime Changes

- Added `AddRouteSparkle(Vector2 position, float scale)` in `Assets/Scripts/RedBallRuntime.cs`.
- Added `dust_sparkle` route markers to Level 1-13.
- Added `warning_spark` warning-sign polish to Level 1-13 through existing `AddWarningSign()`.
- Existing `lift_platform_polish` remains visible on moving platforms in Level 3 and Level 6-13.
- Existing Level 13 checkpoint continues to show `checkpoint_spark`.
- Fallback behavior remains unchanged: `AddGeneratedSprite()` still falls back to existing RedBall/UI sprites if generated Sprint02 PNGs are missing.

## Level Coverage

| Level | Visible polish placement |
| --- | --- |
| 1 | Route sparkles at tutorial arc/end coin line; warning sign before the single triangle hazard. |
| 2 | Route sparkle at first arc; warning signs near both hazards. |
| 3 | Bounce/lift route sparkles; warning signs before hazards; lift polish on moving platform. |
| 4 | Step/arc sparkles; warning signs around thin-step hazards. |
| 5 | Patrol route and high-arc sparkles; warning signs before key patrol/hazard reads. |
| 6 | Bridge arc sparkles; warning signs before hazards; lift polish on both moving platforms. |
| 7 | Bounce ladder sparkles; warning signs before hazard reads; lift polish on moving platform. |
| 8 | Rhythm/lift route sparkles; warning signs before hazards; lift polish on moving platform. |
| 9 | High-line route sparkles; warning signs before low hazards; lift polish on moving platform. |
| 10 | Descent/lift route sparkles; warning signs before hazards; lift polish on moving platforms. |
| 11 | Patrol gate arc sparkles; warning signs near hazards; lift polish on moving platform. |
| 12 | Bounce/skill arc sparkles; warning signs near hazards; lift polish on moving platforms. |
| 13 | Final route, bounce, checkpoint, and high-line sparkles; checkpoint sparkle; warning signs; lift polish on moving platforms. |

## Verification

| Gate | Result | Evidence |
| --- | --- | --- |
| Diff whitespace check | PASS | `git diff --check` exited 0. |
| Unity import/compile | PASS | `Logs/task0035-import-compile.log`, exit 0; no compiler/build error patterns. |
| EditMode smoke | PASS 19/19 | `TestResults/task0035-editmode-results.xml`; includes 13 Level 1-13 polish marker cases. |
| PlayMode smoke | PASS 5/5 | `TestResults/task0035-playmode-results.xml`; Level 1/14/15 contracts, Level 14 lift, Level 15 crumble, goal badge persistence, badge roundtrip. |

Unity logs include licensing handshake/access-token warning lines seen in prior runs; batchmode still exited 0 and test XML passed.

## Files Changed By This Pass

- `Assets/Scripts/RedBallRuntime.cs`
- `Assets/Tests/EditMode/RedBallSmokeEditModeTests.cs`
- `docs/ai-production/tasks/TASK-0035-existing-level-polish-pass.md`
- `docs/ai-production/reports/existing-level-polish-pass-report.md`
- `docs/ai-production/assets-manifest.md`
- `docs/ai-production/STATUS_BOARD.md`

## Technical Debt

Technical debt added: none. Existing manual/mobile route evidence remains tracked under `TD-0006`.
