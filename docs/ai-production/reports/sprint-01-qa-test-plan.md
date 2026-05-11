# Sprint 01 QA Test Plan

Date: 2026-05-12  
Owner: QA Scenario Designer  
Source task: `TASK-0021-sprint-01-qa-playtest.md`  
Implementation dependency: `TASK-0019-sprint-01-unity-implementation.md`

## Purpose

Define the QA pass that runs as soon as Goodall finishes Sprint 01 Unity implementation. This is a docs-only QA scenario plan; it does not change Unity code or assets.

Sprint 01 QA must prove the player-visible slice works end to end: mastery badges, persistence, level-select display, Level 14 vertical lift, Level 15 crumbling tile, safe/skill route behavior, checkpoint/restart interactions, and mobile readability.

## QA Gate Summary

Block closure unless both gates pass:

- Compile gate: Unity 6000.4.5f1 batch import/compile succeeds with no compile/build/fatal errors.
- Play-mode gate: fresh-save, progressed-save, mastery matrix, Levels 14-15 mechanic checks, checkpoint/restart checks, and mobile readability checks pass or have PM-approved `Pass with risk`.

## Compile Gate

Setup:

- Use Goodall's completed implementation branch/worktree state.
- Use Unity 6000.4.5f1.
- Preserve the current working tree; do not revert Goodall's files.
- Prepare a fresh compile log artifact path under QA notes or a local ignored log folder.

Action:

- Run Unity in batch mode for project import/script compile.
- Search the compile log for: `error CS`, `Scripts have compiler errors`, `Compilation failed`, `Build failed`, `Tundra build failed`, `Exception:`, and `Fatal error`.
- Record Unity version, command used, exit code, log location, and changed-file scope observed.

Expected result:

- Unity exits successfully.
- Log has no matching compile/build/fatal error strings.
- Changed files remain within the Sprint 01 implementation/task/report scope unless separately approved.

Failure severity:

- Blocker. No play-mode QA starts until compile passes unless PM explicitly asks QA to inspect known compile failures.

## Play-Mode QA Matrix

| ID | Area | Setup | Action | Expected result | Failure severity |
| --- | --- | --- | --- | --- | --- |
| S01-QA-01 | Fresh save baseline | Clear local prototype saves or use a fresh profile. Start app in Editor Play mode. | Open main menu, continue to Level 1, open level select, return to menu. | Level 1 is unlocked, no mastery badges are shown as earned, locked/current/completed states are readable, navigation has no console errors. | Blocker if app cannot start or Level 1 cannot be entered; Major for unreadable state display. |
| S01-QA-02 | Existing save compatibility | Use a save with previous level unlock/completion data from before badge implementation. | Launch app, open level select, start a previously unlocked/completed level, return to menu. | Legacy completion/unlock data remains intact; new badge data defaults safely without corrupting old progress. | Blocker if progress is lost or levels relock; Major if badge defaults are misleading but progress remains playable. |
| S01-QA-03 | Clear badge | Fresh or reset badge state for a test level. | Complete a level without collecting all coins; damage is allowed. | `Clear` is awarded and next-level unlock behavior still works. Missing badges are shown as missing, not earned. | Blocker if completion/unlock breaks; Major if only badge display is wrong. |
| S01-QA-04 | All Coins badge with damage | Choose a level with coins and a safe damage opportunity. | Collect every coin, take damage or enemy/hazard contact, then finish. | `Clear` and `All Coins` are awarded. `Clean Run` is not awarded. | Blocker if all-coin state cannot be earned; Major if clean-run is incorrectly awarded. |
| S01-QA-05 | Clean Run without all coins | Choose a level with optional coins. | Complete without damage, enemy damage, hazard damage, or fall/death while intentionally skipping at least one coin. | `Clear` and `Clean Run` are awarded. `All Coins` is not awarded. | Major. Blocker if clean-run tracking prevents normal completion or corrupts saves. |
| S01-QA-06 | Full mastery | Choose a level with coins. | Complete with all coins and no damage/fall/death. | `Clear`, `All Coins`, and `Clean Run` are all awarded and shown in completion feedback and level select. | Major; Blocker if persistence/unlock also fails. |
| S01-QA-07 | Clean Run failure paths | Use hazards, enemy side contact, and fall below `KillY` across one or more attempts. | Trigger each failure path, then finish the level cleanly afterward except for the prior failure in that same attempt. | Any damage, enemy damage, hazard damage, or fall/death invalidates `Clean Run` for that attempt. | Major; Blocker if death/fall causes soft-lock or broken respawn. |
| S01-QA-08 | Replay badge improvement | Use a level already cleared with only `Clear`. | Replay to earn missing `All Coins`, then replay again if needed to earn `Clean Run`. | Previously earned badges remain, missing badges can be added, progression is not relocked. | Major. |
| S01-QA-09 | Persistence after menu return | Earn a known badge set. | Return to menu, reopen level select, restart the same level. | Badge state persists through menu navigation and matches completion feedback. | Major. |
| S01-QA-10 | Persistence after app relaunch | Earn a known badge set. | Stop Play mode, relaunch Play mode, open level select. | Badge state persists after relaunch and old unlock/completion state remains valid. | Blocker if save data is lost/corrupted; Major if display refresh is wrong. |
| S01-QA-11 | Level select display | Use mixed badge states across multiple levels, including Levels 14-15 if available. | Open level select at desktop and phone-landscape resolutions. | Each level shows compact badge progress without clipping text, reducing tap targets below intended size, or confusing locked/completed states. | Major for unreadable/misleading state; Minor for small polish issues. |
| S01-QA-12 | Completion feedback | Produce clear-only, all-coins-with-damage, clean-without-all-coins, and full-mastery outcomes. | Observe the completion feedback for each outcome. | Earned and missing badges are clear enough for a player to understand what happened and what remains. | Major if player cannot tell earned vs missing; Minor for copy/polish only. |
| S01-QA-13 | Regression smoke, old levels | Use unlocked access to Levels 1, 3, 5, and 13. | Play Level 1 basic route, Level 3 bounce/moving platform, Level 5 enemy patrol/stomp/damage, Level 13 checkpoint/warning signs. | Existing mechanics still work; no new badge or level-count work breaks old content. | Blocker if representative old level cannot be completed; Major for mechanic regression with workaround. |
| S01-QA-14 | Level 14 vertical lift introduction | Start Level 14 from fresh attempt. | Ride the first vertical lift, wait on it, jump off it, and intentionally miss once if safe. | First lift is over safe ground or recoverable terrain, speed is readable, and the player is not forced into a surprise death. | Blocker if Level 14 cannot be completed; Major if first exposure is unfair. |
| S01-QA-15 | Level 14 safe route | Start Level 14 with no mastery pressure. | Take the main route and skip optional/skill coins. | Goal is reachable through the safe route; optional skill route is not required for `Clear`. | Blocker if clear requires optional route; Major if route readability is poor. |
| S01-QA-16 | Level 14 skill route | Start Level 14 after safe route is understood. | Attempt optional lift/coin route, then finish. | Optional route is reachable, readable, and can support `All Coins` without making the level impossible after a miss. | Major; Minor if coin placement needs tuning but route remains fair. |
| S01-QA-17 | Level 14 checkpoint | Start Level 14 and activate checkpoint after first safe lift. | Take damage/fall after checkpoint, then restart level from pause/control. | Damage/fall respawns at latest checkpoint and spends hearts as existing rules require. Manual restart resets to level start unless PM approved checkpoint restart behavior. | Blocker for soft-lock or broken respawn; Major for wrong restart semantics. |
| S01-QA-18 | Level 15 crumble teaching | Start Level 15 fresh. | Step on first crumbling tile, wait, retreat/recover if possible, and observe visual timing. | First crumbling tile teaches safely before pit pressure; delay and disappearance are readable. | Blocker if first exposure causes unavoidable death; Major if timing/visual feedback is unclear. |
| S01-QA-19 | Level 15 crumble-over-pit | Reach the later pit/timing section. | Cross crumbling tiles normally, then intentionally fail one attempt. | Pit section is fair after teaching; failure respawns cleanly and does not leave an impossible world state. | Blocker for soft-lock/uncompletable state; Major for unfair timing. |
| S01-QA-20 | Crumbling tile reset | In Level 15, trigger crumble. | Use manual restart, level reload, and damage/fall respawn where applicable. | Restart/reload restores crumbling tiles. If checkpoint respawn does not restore them, QA must classify whether this is a bug or accepted debt. | Blocker if restart/reload cannot restore; Major if checkpoint respawn creates unfair soft-lock. |
| S01-QA-21 | Checkpoint and clean-run interaction | Use a level with checkpoint and a clean-run opportunity. | Activate checkpoint, take damage/fall, finish; then start a new attempt and finish cleanly. | Damage/fall after checkpoint invalidates `Clean Run` for that attempt. New attempt resets clean-run state. | Major; Blocker if attempt state leaks between levels or app sessions. |
| S01-QA-22 | Restart and badge attempt state | Start any badge-capable level. | Take damage, press restart, then complete with no damage and all coins. | Restart begins a fresh badge attempt; prior failed clean-run state does not carry over. Restart does not spend a heart by itself. | Major; Blocker if restart breaks economy or level load. |
| S01-QA-23 | Mobile landscape readability | Use phone-landscape resolution and a notched/safe-area simulation if available. | Check HUD, joystick, jump button, level chip/count, badge indicators, completion feedback, and Level 14-15 mechanic visibility. | UI elements do not overlap or obscure essential hazards/lifts/tiles. Text remains readable and touch controls remain usable. | Major for overlap that affects play; Minor for cosmetic spacing. |
| S01-QA-24 | No monetization pressure regression | Use retry/mastery flows with low hearts and normal hearts. | Restart, replay for badges, fail Level 14/15, return to menu, and re-enter levels. | No forced ad, wait timer, energy cost, currency cost, or new retry gate is introduced for mastery play. Existing heart behavior remains as designed. | Blocker if new monetization/retry gate blocks QA or player progression. |

## Blocker Criteria

QA must block Sprint 01 closure for any of these:

- Unity compile/import fails.
- App cannot reach main menu, level select, Level 1, Level 14, or Level 15.
- Existing Levels 1-13 representative smoke fails in a way caused by Sprint 01 implementation.
- Save/progression data is lost, corrupted, or relocks previously unlocked levels.
- Badge persistence is missing or records incorrect earned badges after relaunch.
- `Clean Run` is awarded after damage, enemy damage, hazard damage, fall, or death.
- Level 14 or Level 15 is impossible to complete through the intended safe route.
- Crumbling tiles or checkpoint interactions create an unavoidable soft-lock after restart/reload.
- Mobile controls or critical UI overlap so badly that the level cannot be played.
- Any forced ad, timer, currency, or energy cost is added to retry/mastery play without PM approval.

## Bug vs Technical Debt Policy

QA findings are bugs when they break accepted Sprint 01 behavior, block completion, corrupt saves, misaward badges, create soft-locks, or make mobile play unreadable. Bugs must be added as blockers to `TASK-0021` and returned to the owning implementation task before closure.

QA findings are technical debt only when PM explicitly accepts the shipped behavior as a deferred limitation. Accepted debt must be registered in `docs/ai-production/technical-debt/TECHNICAL_DEBT_REGISTER.md` before closure and referenced in the final QA report.

Candidate debt examples only if accepted:

- Crumbling tiles reset on restart/reload but not checkpoint respawn, and QA confirms no soft-lock.
- Badge/progression logic remains embedded in runtime instead of a profile service.
- Level 14-15 authoring remains method-based rather than data-driven.
- Mastery telemetry is skipped because no analytics facade exists.

## Evidence Required

- Compile log path, Unity version, command, exit code, and error-string search result.
- Play-mode notes for each matrix row with Pass, Fail, Blocked, or Pass with risk.
- Screenshots or short clips for level select badge display, completion feedback, Level 14 lift, Level 15 crumbling tile, and mobile readability.
- Save-state notes: fresh save, progressed save, and mixed badge state.
- Console summary after play-mode smoke, including exact errors if present.

## Technical Debt

No new debt is added by this QA planning document. Future QA-accepted limitations must be registered before Sprint 01 closure.

## Execution Addendum - 2026-05-12 QA Runner

Verdict: QA PARTIAL.

- Compile gate rerun passed on Unity 6000.4.5f1. Log: `/tmp/red_ball_qa/sprint01-compile-20260512.log`.
- Required compile error-pattern scan returned no matches.
- EditMode and PlayMode Test Runner commands were attempted without changing code, but no result XML was produced and no Sprint 01 Level 1/14/15 load-path smoke tests exist.
- Static QA found no confirmed blocker for badge persistence keys, `LevelCount` 15 routing, Level 14/15 builder presence, Clean Run invalidation wiring, All Coins gating, crumbling tile restart/reload rebuild behavior, or retry monetization regressions.
- Full matrix execution remains pending for play-mode/manual evidence, screenshots, route completion, badge persistence after relaunch, crumble/checkpoint interactions, and mobile landscape readability.
- Detailed run report: `docs/ai-production/reports/sprint-01-qa-run-2026-05-12.md`.
- Technical debt added: none.
