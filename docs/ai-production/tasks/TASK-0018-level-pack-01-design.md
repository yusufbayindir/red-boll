# TASK-0018 - Level Pack 01 Design

## Status

Design Complete - PM Review Pending

## Owner

Level Design Lead

## PM Intent

Respond to the feedback that current levels are too plain by designing a richer Level Pack 01 and choosing a disciplined first implementation slice. PM will not write code for this task; this is a design and task-definition handoff.

## Scope

In:

- 8-12 Level Pack 01 section concepts.
- New hazard/obstacle candidates and readability rules.
- Skill focus, checkpoint guidance, collectible placement, and difficulty target per section.
- Sprint 01 recommendation for the first 2-3 implementable level features.
- QA playtest checklist.
- Technical debt note added to acceptance.

Out:

- Runtime code changes.
- Unity scene, prefab, asset, or script edits.
- Final tuning values for physics components.
- Monetization, ads, or store copy.

## Acceptance Criteria

- Level Pack 01 contains 8-12 section ideas with theme, new hazard/obstacle, skill focus, checkpoint point, collectible placement, and difficulty target.
- Recommended systems include moving platforms, crumbling tiles, fans/wind, one-way gates, keys/locks, timed hazards, bumpers, low gravity zones, and secret coins.
- Sprint 01 identifies exactly 2-3 level features to implement first and explains why.
- Deferred features are explicitly listed for later sprints.
- QA checklist covers fairness, camera, checkpoints, completion time, and mobile readability.
- Technical debt note is present: crumbling tile reset/checkpoint behavior, secret coin reward semantics, and future stateful route systems need implementation decisions before coding.
- No code or Unity asset files are changed.

## Level Pack 01 Summary

| Section | Name | Feature | Skill focus | Checkpoint plan | Difficulty |
| --- | --- | --- | --- | --- | --- |
| 14 | Asansor Bahcesi | Vertical lifts | Ride/wait/exit | After first safe lift | 3/10 |
| 15 | Kirik Taslar | Crumbling tiles | Commit timing | Before crumble-over-pit | 4/10 |
| 16 | Ruzgar Kuyusu | Fans/wind | Adjusted jump arc | After safe fan tutorial | 4/10 |
| 17 | Tek Yon Gecit | One-way gates | Route commitment | Midpoint after gate chain | 5/10 |
| 18 | Anahtar Patikasi | Keys/locks | Tiny branch exploration | After first key/lock | 5/10 |
| 19 | Tik Tak Kopru | Timed hazards | Cycle reading | Before second timed beat | 6/10 |
| 20 | Sekme Makinesi | Bumpers | Recovery/angle reading | After first bumper bay | 6/10 |
| 21 | Ay Hafifligi | Low gravity | Air control | After first low-gravity climb | 6/10 |
| 22 | Gizli Hat | Secret coins | Observation/replay routing | At route split | 7/10 |
| 23 | Kirmizi Zirve | Final remix | Combined mastery | Two checkpoints | 8/10 |

Full design details live in `docs/ai-production/reports/level-design-expansion-v1.md`.

## Sprint 01 Recommendation

Implement first:

- **Vertical lift Level 14 prototype.** Highest confidence because moving platforms already exist and only need vertical layout tuning.
- **Crumbling tile component plus Level 15 teaching section.** Highest novelty/value ratio; it makes levels feel less static while staying readable.
- **Secret coin support only if cheap.** Prefer metadata-light optional coins in Sprint 01. Defer full secret coin save/reward UI if it expands scope.

Defer:

- Fans/wind.
- One-way gates.
- Keys/locks.
- Timed hazards.
- Bumpers.
- Low gravity zones.
- Final remix levels after Level 15 until tutorial mechanics pass QA.

## Risk Check

- Shared systems touched: future implementation will likely touch level builders, object helpers, collision/damage, checkpoint respawn, collectible tracking, and possibly save data.
- Regression risks: crumbling tiles may not reset after checkpoint respawn; stateful gates/keys may persist across restart incorrectly; secret coins may confuse all-coins mastery if not defined.
- Rollback idea: implement each mechanic behind a single helper/component and ship only Level 14-15 first; keep existing 13 levels untouched except shared helper regressions found in QA.

## Activity Log

- 2026-05-12 00:00 - Level Design Lead created Level Pack 01 design task and report from PM request.

## Changed Files

- `docs/ai-production/reports/level-design-expansion-v1.md`
- `docs/ai-production/tasks/TASK-0018-level-pack-01-design.md`

## Verification

- Documentation-only task. Verified by static review of the two created markdown files.
- Existing code and Unity asset files were not edited.

## QA Notes

QA should use the checklist in `docs/ai-production/reports/level-design-expansion-v1.md` when Sprint 01 implementation starts. First playable QA target should be Level 14 vertical lift prototype, then Level 15 crumbling tile tutorial.

## Repo Controller Notes

Documentation-only change. No runtime code, Unity scene, prefab, sprite, or meta files intentionally changed.

## PM Closure

Pending.

