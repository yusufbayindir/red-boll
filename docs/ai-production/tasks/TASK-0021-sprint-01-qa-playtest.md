# TASK-0021 - Sprint 01 QA Playtest

## Status

QA PARTIAL - Compile Pass / Play-mode Pending

## Owner

QA Scenario Designer / QA Lead.

## PM Intent

Prepare the Sprint 01 QA gate so QA can run immediately after Goodall finishes Unity implementation, without PM writing code and without reverting developer files.

## Scope

In:

- Compile gate definition for Sprint 01 Unity implementation.
- Play-mode QA matrix for mastery badges, persistence, level select, Level 14 vertical lift, Level 15 crumbling tile, safe/skill route, checkpoint/restart interactions, and mobile readability.
- Blocker criteria for Sprint 01 closure.
- Bug vs technical debt policy for QA findings.
- QA evidence requirements.

Out:

- Runtime code, Unity scene, prefab, asset, package, or project setting changes.
- Reverting Goodall's implementation files.
- PM-authored product code.
- Closing implementation before compile and play-mode evidence exists.

## Acceptance Criteria

- `docs/ai-production/reports/sprint-01-qa-test-plan.md` exists and covers all Sprint 01 QA areas requested by PM.
- Every test row includes setup, action, expected result, and failure severity.
- Compile/play-mode QA gate is explicit enough to run as soon as implementation completes.
- Blocker criteria are clear and tied to Sprint 01 acceptance.
- Technical debt policy states that bugs block tasks and accepted debt must be registered in the technical debt register.
- No code or Unity files are changed by this task.

## QA Run Trigger

Start this task when Goodall marks `TASK-0019` implementation complete or requests QA on a candidate build.

Before running:

- Do not revert Goodall's files.
- Capture current `git status --short` for evidence.
- Confirm Unity version is 6000.4.5f1 or record the exact version mismatch as a QA risk.
- Prepare fresh-save, progressed-save, and mixed-badge test states.

## QA Matrix

Source of truth: `docs/ai-production/reports/sprint-01-qa-test-plan.md`.

Required coverage:

- Fresh and existing save compatibility.
- Mastery badges: `Clear`, `All Coins`, `Clean Run`, full mastery, replay improvement, and invalidation paths.
- Persistence after menu return and app relaunch.
- Level select compact badge display.
- Completion feedback for earned and missing badges.
- Regression smoke for Levels 1, 3, 5, and 13.
- Level 14 vertical lift, safe route, skill route, and checkpoint.
- Level 15 crumbling tile teaching, pit timing, reset, and restart/reload behavior.
- Checkpoint, restart, clean-run, heart-spend, and no-cost retry interactions.
- Mobile landscape readability for HUD, controls, badge display, and completion feedback.

## Blocker Criteria

Block Sprint 01 closure if any blocker from the test plan occurs, especially:

- Compile/import failure.
- Main menu, level select, Level 1, Level 14, or Level 15 cannot be reached.
- Save/progression corruption.
- Incorrect badge persistence or incorrect `Clean Run` award after damage/fall/death.
- Uncompletable Level 14/15 safe route.
- Crumbling tile or checkpoint soft-lock after restart/reload.
- Mobile UI overlap that prevents play.
- Any unapproved retry/mastery monetization gate.

## Risk Check

- Shared systems touched by implementation: save keys, PlayerPrefs progression, completion flow, level count, level select UI, HUD/completion feedback, coin counting, damage/fall paths, checkpoints, moving platforms, Level 14-15 builders, and crumbling tile behavior.
- Regression risks: legacy saves can be misread; clean-run invalidation can miss fall/enemy paths; level select can become unreadable with badges; crumbling tiles can fail to reset; vertical lift timing can be unfair on touch controls.
- Rollback idea: if badges fail but progression works, hide badge display only with PM approval; if Level 14/15 fail, return implementation to Goodall and keep existing 13-level build as the last known playable baseline.

## Technical Debt Policy

QA bugs block this task and the owning implementation task. A finding is a bug when it violates accepted Sprint 01 behavior, breaks progression, corrupts saves, misawards badges, creates a soft-lock, or makes mobile play unreadable.

QA may classify a finding as technical debt only if PM accepts the limitation for this sprint. Accepted debt must be added to `docs/ai-production/technical-debt/TECHNICAL_DEBT_REGISTER.md` before closure and named in this task's final notes.

## Activity Log

- 2026-05-12 - QA Runner executed Sprint 01 QA pass: compile/import gate PASS, Unity Test Runner smoke attempted but produced no scenario result XML, static blocker review completed with no confirmed blocker; task remains QA PARTIAL pending play-mode/manual evidence.
- 2026-05-12 - Created QA playtest task and Sprint 01 QA test plan. Docs-only; no Unity files changed.

## Changed Files

- `docs/ai-production/reports/sprint-01-qa-run-2026-05-12.md`
- `docs/ai-production/reports/sprint-01-qa-test-plan.md`
- `docs/ai-production/tasks/TASK-0021-sprint-01-qa-playtest.md`
- `docs/ai-production/STATUS_BOARD.md`

## Verification

- QA Runner compile/import rerun passed with Unity 6000.4.5f1, exit code 0, log `/tmp/red_ball_qa/sprint01-compile-20260512.log`, and no required error-pattern matches.
- EditMode Test Runner command exited 0 but produced no `/tmp/red_ball_qa/editmode-results-20260512.xml`.
- PlayMode Test Runner command exited 0 but produced no `/tmp/red_ball_qa/playmode-results-20260512.xml`.
- Static QA checked the critical blocker criteria in `Assets/Scripts/RedBallRuntime.cs`, `Assets/Scripts/RedBallUi.cs`, `Assets/Scripts/RedBallCrumblingTile.cs`, and `Assets/Scripts/RedBallPlayer.cs`; no confirmed blocker found.
- Documentation-only planning pass.
- Read `TASK-0019`, `sprint-01-implementation-plan.md`, `qa-checklist.md`, `TASK-0016`, `TASK-0018`, `STATUS_BOARD.md`, and `TECHNICAL_DEBT_REGISTER.md`.
- Earlier planning pass had no Unity run; 2026-05-12 QA Runner has now completed compile/import and attempted Unity Test Runner smoke, with play-mode scenario coverage still pending.

## QA Notes

2026-05-12 QA Runner verdict: QA PARTIAL. Compile/import PASS; static review found no confirmed blocker. Full PASS remains blocked by missing play-mode/manual evidence for Level 1/14/15 load paths, badge persistence after app relaunch, Clean Run failure paths, Level 14/15 safe route completion, Level 15 crumble/checkpoint behavior, and mobile landscape readability. Detailed run report: `docs/ai-production/reports/sprint-01-qa-run-2026-05-12.md`.

## Repo Controller Notes

Docs-only QA planning. Do not stage or revert Goodall's Unity implementation files as part of this task.

## PM Closure

QA PARTIAL after implementation handoff. Do not close Sprint 01 as PASS until play-mode/manual evidence is recorded.

## Technical Debt Added

none
