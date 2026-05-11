# TASK-0009 - Mastery Badge MVP

## Status

Draft

## Owner

PM, Developer Gameplay, Developer UI/Polish, QA

## PM Intent

Turn replay value into the product hook: players should clear levels, then replay for visible mastery badges.

## Scope

In:

- Badge model for `Clear`, `All Coins`, and `Clean Run`.
- Per-level save data.
- Completion summary UI.
- Level-select badge indicators.
- Design compatibility with future Trickline contracts.

Out:

- Ghost racing.
- Daily challenges.
- Timed medals unless PM approves after playtesting.
- Cosmetic economy.
- Ads tied to badges.

## Acceptance Criteria

- Completing a level records `Clear`.
- Collecting all coins records `All Coins`.
- Finishing without damage records `Clean Run`.
- Level select shows badge state per level.
- Completion screen tells the player what they earned and what remains.
- Existing unlock flow still works.
- Related task MD is updated before and after each code step.
- QA verifies save/load and replay cases.

## Risk Check

- Shared systems touched: level completion, coin tracking, damage tracking, PlayerPrefs, UI.
- Regression risks: broken unlocks, corrupted old saves, level-select clutter, inaccurate clean-run tracking.
- Rollback idea: keep badge save keys separate from existing completion/unlock keys.

## Activity Log

- 2026-05-11 23:40 - Draft task created by PM from TASK-0004 reports.

## Changed Files

- None yet.

## Verification

- Not run yet.

## QA Notes

Pending.

## Repo Controller Notes

Pending.

## PM Closure

Pending.

