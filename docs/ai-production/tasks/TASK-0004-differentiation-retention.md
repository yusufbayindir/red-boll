# TASK-0004 - Differentiation And Retention Design

## Status

Accepted

## Owner

Game Design / Retention, Game Design / Differentiation Challenger, PM

## PM Intent

Find a product hook that makes this more than another red ball platformer and creates ethical repeat play.

## Scope

In:

- Differentiation ideas.
- Core loop options.
- Progression/reward loop.
- "One more try" mechanics.
- What to avoid.

Out:

- Dark-pattern monetization.
- Huge meta systems before the core game is fun.
- Copying competitor identity.

## Acceptance Criteria

- 5 differentiation directions.
- 3 retention loop options.
- 1 recommended MVP loop.
- Clear reasons to reject weaker ideas.
- Risks and required implementation support.

## Activity Log

- 2026-05-11 23:20 - Task created by PM.
- 2026-05-11 23:29 - Differentiation Challenger returned `Red Ball: Trickline` proposal: route contracts, momentum medals, daily stunt route, ghost rival, stunt-machine identity.
- 2026-05-11 23:31 - Retention agent returned Mastery Badge Loop proposal: `Clear`, `All Coins`, `Clean Run`, level-select badge progress, completion results UI.

## Candidate Direction: Red Ball Trickline

- Core idea: levels become compact stunt-route puzzles, not just side-scrolling obstacle courses.
- Player goal: finish the level, then replay for route contracts and medals.
- Best early contracts: all coins, no damage, under par time, bounce chain, no-stop/momentum route.
- PM status: promising but not approved until retention and level design reports are compared.

## Candidate MVP: Mastery Badge Loop

- Core idea: keep the first implementation closer to existing systems.
- Per level badges: `Clear`, `All Coins`, `Clean Run`.
- Level select shows badge progress.
- Completion UI shows what improved and what remains.
- PM status: currently the safer MVP candidate than full Trickline contracts.

## Changed Files

- `docs/ai-production/reports/differentiation-challenger-red-ball-trickline.md`
- `docs/ai-production/reports/retention-mastery-badge-loop.md`

## Verification

- Design task; PM will evaluate against product thesis.

## QA Notes

Design task only. Feature QA will happen under `TASK-0009` or later mastery/Trickline implementation tasks.

## Repo Controller Notes

No runtime code changed in this design task.

## PM Closure

Accepted. PM direction is mastery-first: implement `Clear`, `All Coins`, and `Clean Run` before full Trickline contracts, ghosts, or daily challenges.
