# TASK-0017 - Gameplay Differentiation Loop

## Status

Open

## Owner

Gameplay Developer, UI Developer, Level Designer, QA, Gameplay Differentiation / Retention Designer

## PM Intent

Ship the first ethical retention slice that makes the game feel less like a plain Red Ball clone: optional mastery badges, one readable skill route, and completion feedback that invites fair replay.

## Product Direction

Sprint 01 should implement the **Mastery Badge Loop** first:

- `Clear`
- `All Coins`
- `Clean Run`

It should also include one pilot level adjustment that demonstrates **Safe Route vs Skill Route**. Full Trickline contracts, daily challenge, ghost runs, and run modifiers are explicitly later work unless the team finishes the core slice early without destabilizing the build.

## Scope

In:

- Track per-level completion for `Clear`, `All Coins`, and `Clean Run`.
- Show earned/missing badges on completion screen.
- Show compact badge state on level select for the affected levels.
- Add or tune one pilot level so it has a safe main route and one optional skill route.
- Make coins in the pilot level communicate the optional route.
- Ensure retry remains fast and does not require ads, waits, or currency.
- Add lightweight telemetry events or log hooks for clear, retry, badge earned, coin completion, and clean-run failure reason if telemetry already exists.
- Document any technical debt found by the developer in the final handoff.

Out:

- New monetization systems.
- Forced ad continues or retry gates.
- Energy, lives, wait timers, streak pressure, random rewards, or gacha.
- Full daily challenge system.
- Full ghost replay system.
- Broad Trickline contract framework.
- Power upgrades that affect movement or level balance.
- Large-scale level pack expansion.

## Minimal Code Scope

- Add a small per-level badge state model if one does not already exist.
- Persist badge completion locally using the existing save approach.
- Compute `All Coins` from collected coin count versus level coin total.
- Compute `Clean Run` from no damage and no death during the attempt.
- Reset per-attempt clean-run state on retry/restart.
- Update completion UI to show three badge outcomes.
- Update level-select UI to show three badge indicators per level where data is available.

## Assets

- Reuse existing coin, hazard, checkpoint, and UI assets where possible.
- Use simple badge icons or existing icon set placeholders for `Clear`, `All Coins`, and `Clean Run`.
- No new character skins, store art, or marketing art required for Sprint 01.
- If the pilot skill route needs signposting, use existing signs/arrows or minimal placeholder markers.

## QA Notes

- Verify each badge can be earned independently.
- Verify `Clean Run` fails on damage and death.
- Verify `Clean Run` state resets correctly after retry.
- Verify `All Coins` only awards when every level coin is collected in the completed attempt.
- Verify badge persistence after returning to menu and relaunching the game.
- Verify level select and completion screen match the saved badge state.
- Verify assisted/accessibility settings, if present, do not block normal progression.
- Verify retry loop remains fast and has no forced ad, timer, or currency gate.

## Telemetry / Ads Impact

- Telemetry should help answer whether players replay voluntarily after seeing missing badges.
- Suggested events if telemetry exists: `level_start`, `level_retry`, `level_complete`, `badge_earned`, `clean_run_failed`, `all_coins_failed`.
- Do not add forced interstitial ads to retry or immediate post-failure flow in this task.
- If ads are already active, QA must confirm they do not interrupt rapid mastery retries.
- Monetization tuning should wait until replay behavior is measured.

## Risks

- Badge logic can become inconsistent if level state is scattered across scenes.
- `Clean Run` can feel unfair if damage/hazard collisions are unclear.
- `All Coins` can feel tedious if coins are hidden without route language.
- Level-select UI can become cluttered on small screens if badge indicators are too large.
- Telemetry work may expand scope if no existing analytics abstraction exists.

## Acceptance Criteria

- `Clear`, `All Coins`, and `Clean Run` badge states are tracked per level.
- Badge state persists across scene changes and app relaunch.
- Completion screen clearly shows earned and missing badges after a level ends.
- Level select clearly shows badge progress for implemented levels.
- One pilot level contains a safe route and one optional skill route with coin/reward signposting.
- Retry/restart remains fast and has no forced ad, wait timer, energy cost, or currency cost.
- QA verifies badge awarding, badge persistence, retry reset behavior, and UI state consistency.
- Telemetry impact is implemented if an existing telemetry path exists; otherwise developer final notes the missing telemetry path as technical debt.
- Developer final includes a `Technical debt added/none` note with any recommended debt follow-up.

## Suggested Technical Debt Follow-Up

- If badge state needs temporary scene-local wiring, create a follow-up to consolidate progression state behind a single profile/progression service.
- If telemetry has no shared wrapper, create a follow-up for a small analytics facade before adding more retention events.
- If level coin totals are hardcoded, create a follow-up to derive totals from level content at load time.

## Risk Check

- Shared systems touched: save/progression, level completion, coin collection, damage/death state, completion UI, level-select UI.
- Regression risks: existing level unlock flow, coin reset behavior, retry/restart behavior, mobile UI fit.
- Rollback idea: hide badge UI and ignore badge save fields while keeping normal level completion intact.

## Activity Log

- 2026-05-12 00:00 - Task created from Gameplay Differentiation / Retention report v1.

## Changed Files

- `docs/ai-production/reports/gameplay-differentiation-retention-v1.md`
- `docs/ai-production/tasks/TASK-0017-gameplay-differentiation-loop.md`

## Verification

- Documentation-only task. Runtime verification pending implementation.

## QA Notes

Pending implementation.

## Repo Controller Notes

No runtime code should be changed by this design task.

## PM Closure

Pending.

