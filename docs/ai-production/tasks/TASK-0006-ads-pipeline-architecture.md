# TASK-0006 - Ads Pipeline Architecture

## Status

Accepted

## Owner

Ads Pipeline Architect, Developer Gameplay, QA, PM

## PM Intent

Prepare monetization architecture before any ad SDK is wired into gameplay code.

## Scope

In:

- Ad service abstraction.
- Rewarded ad use cases.
- Interstitial policy recommendation.
- Unity implementation boundary.
- Config/test mode.
- QA checklist.

Out:

- SDK import before PM approval.
- Hardcoding ad calls into current gameplay methods.
- Any monetization that blocks fair play.

## Acceptance Criteria

- Interfaces/pseudocode are clear.
- Rewarded ads are first priority.
- Failure/no-fill behavior is player-safe.
- Privacy/consent concerns are listed.
- QA can test with a fake ad service.

## Activity Log

- 2026-05-11 23:20 - Task created by PM.
- 2026-05-11 23:31 - Ads Pipeline Architect read protocol, role brief, README, retention report, TASK-0006, and current heart/progress methods in `Assets/Scripts/RedBallRuntime.cs`.
- 2026-05-11 23:31 - Created architecture report for rewarded-first ad abstraction, fake service testing, placement policy, and privacy/config notes. No Unity code or SDK files touched.

## Changed Files

- `docs/ai-production/reports/ads-pipeline-architecture.md`
- `docs/ai-production/tasks/TASK-0006-ads-pipeline-architecture.md`

## Verification

- Architecture task only; no SDK imported and no Unity code changed.

## QA Notes

Architecture task only. SDK/fake-service QA will happen in a future ads implementation task.

## Repo Controller Notes

No SDK imported and no runtime code changed in this architecture task.

## PM Closure

Accepted. PM direction: rewarded heart recovery only for MVP, from menu/level select, never death-pressure ads or badge/checkpoint monetization.
