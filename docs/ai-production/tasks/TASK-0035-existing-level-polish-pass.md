# TASK-0035 - Existing Level Polish Pass

## Status

Implementation Complete - Smoke PASS

## Owner

Existing Level Art Polish Developer

## PM Intent

Extend the Sprint02 generated runtime asset pack beyond Level 14/15 so the existing Level 1-13 content also shows visible polish, better hazard readability, and clearer coin/route guidance without changing gameplay balance.

## Scope

In:

- Inspect and update Level 1-13 build functions in `Assets/Scripts/RedBallRuntime.cs`.
- Use existing Sprint02 generated runtime assets where appropriate: `warning_spark`, `dust_sparkle`, `checkpoint_spark`, and existing moving-platform `lift_platform_polish`.
- Keep changes decorative: no new colliders, hazards, checkpoint flow, unlock logic, or mastery logic.
- Preserve generated asset fallback behavior.
- Update asset manifest, status board, task/report docs, and final technical debt line.
- Run Unity import/compile, EditMode smoke, and PlayMode smoke.

Out:

- Replacing base terrain art.
- Changing Level 14/15 mechanics.
- New third-party downloads.
- Monetization, UI redesign, or marketing capture.

## Implementation Notes

- Added decorative `AddRouteSparkle()` helper using `dust_sparkle`.
- Added route sparkles and warning signs to Level 1-13.
- Existing `AddWarningSign()` renders `warning_spark`, so new warning placements create visible generated hazard polish without gameplay components.
- Existing moving platforms already render `lift_platform_polish`; this is now visibly present in existing levels with movers: Level 3 and Level 6-13.
- Existing Level 13 checkpoint continues to render `checkpoint_spark`.
- Added EditMode smoke coverage that loads Level 1-13 and asserts each has at least one generated route sparkle and warning spark.

## Level Coverage

| Level | Visible Sprint02 polish |
| --- | --- |
| 1 | `dust_sparkle` route markers over tutorial coin path; `warning_spark` sign before first hazard. |
| 2 | `dust_sparkle` at first arc; `warning_spark` signs at both hazard reads. |
| 3 | `dust_sparkle` on bounce/lift route, `warning_spark` signs before hazards, `lift_platform_polish` on moving platform. |
| 4 | `dust_sparkle` around stepped coin arcs; `warning_spark` signs for thin-platform hazards. |
| 5 | `dust_sparkle` near patrol/coin route; `warning_spark` signs before patrol/hazard beats. |
| 6 | `dust_sparkle` on bridge coin arcs; `warning_spark` hazard signs; `lift_platform_polish` on moving platforms. |
| 7 | `dust_sparkle` on bounce ladder; `warning_spark` hazard signs; `lift_platform_polish` on moving platform. |
| 8 | `dust_sparkle` on rhythm/moving route; `warning_spark` hazard signs; `lift_platform_polish` on moving platform. |
| 9 | `dust_sparkle` high-line route markers; `warning_spark` hazard signs; `lift_platform_polish` on moving platform. |
| 10 | `dust_sparkle` on descent/lift route; `warning_spark` hazard signs; `lift_platform_polish` on moving platforms. |
| 11 | `dust_sparkle` on patrol gate arcs; `warning_spark` hazard signs; `lift_platform_polish` on moving platform. |
| 12 | `dust_sparkle` on bounce/skill arcs; `warning_spark` hazard signs; `lift_platform_polish` on moving platforms. |
| 13 | `dust_sparkle` on final run route and checkpoint area; existing `checkpoint_spark`; `warning_spark` signs; `lift_platform_polish` on moving platforms. |

## Acceptance

- Level 1-13 each have at least one visible Sprint02 generated polish asset.
- Level 1, 3, 5, 9, and 13 have clear, visible polish differences.
- No gameplay balance changes: colliders, hazards, checkpoints, enemy patrols, level unlocks, and mastery flow remain unchanged.
- Missing generated assets still fall back to existing sprites through the existing generated-sprite loader.
- Unity import/compile, EditMode smoke, and PlayMode smoke pass.

## Verification

- Unity import/compile: PASS
  - Log: `Logs/task0035-import-compile.log`
  - Exit code: 0
  - Compiler error scan: no `error CS`, compiler-error, script-compiler, or failed-build patterns. Unity licensing handshake warnings appeared but did not block batchmode.
- EditMode smoke: PASS 19/19
  - Results: `TestResults/task0035-editmode-results.xml`
  - Log: `Logs/task0035-editmode.log`
  - Added coverage: 13 parameterized cases assert Level 1-13 each expose `Generated Route Sparkle` and `Generated Warning Spark`.
- PlayMode smoke: PASS 5/5
  - Results: `TestResults/task0035-playmode-results.xml`
  - Log: `Logs/task0035-playmode.log`

## Technical Debt Added

Technical debt added: none.
