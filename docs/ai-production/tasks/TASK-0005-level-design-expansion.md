# TASK-0005 - Level Design Expansion Pack

## Status

Accepted

## Owner

Level Designer, Game Design, PM

## PM Intent

Create a richer level plan that adds variety without losing readability.

## Scope

In:

- New obstacle/mechanic candidates.
- 20-level progression map.
- 5 detailed sample levels.
- Coin guidance rules.
- Difficulty curve.

Out:

- Immediate implementation before PM approval.
- Unreadable chaos levels.
- Precision-only levels that feel unfair on mobile.

## Acceptance Criteria

- Levels teach, test, and remix mechanics.
- Every new mechanic has a player-readable visual language.
- Mobile controls are respected.
- Proposed implementation list is ordered by value and risk.

## Activity Log

- 2026-05-11 23:20 - Task created by PM.
- 2026-05-11 23:37 - Level Designer returned expansion plan: checkpoints, warning signs, vertical lifts, crumble platforms, switch gates, 20-level map.

## PM Direction

- Approved as design direction, not yet approved for immediate implementation.
- Checkpoints are required before longer post-13 levels.
- Autoscroll/chase/crushers are rejected for now due mobile fairness and heart-system risk.
- First implementation slice should be checkpoint support + warning sign helper, then Level 14 prototype.

## Changed Files

- `docs/ai-production/reports/level-design-expansion-plan.md`

## Verification

- Design task; PM and QA will review for readability and testability.

## QA Notes

Design task only. Gameplay QA moved to checkpoint/level implementation tasks, starting with `TASK-0010`.

## Repo Controller Notes

No runtime code changed in this design task.

## PM Closure

Accepted. PM direction: checkpoints and warning signs before longer post-13 levels; autoscroll/chase/crushers rejected for now.
