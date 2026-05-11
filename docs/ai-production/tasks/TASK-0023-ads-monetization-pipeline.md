# TASK-0023 - Ads Monetization Pipeline

## Status

Proposed / Ready for PM Review

## Owner

Monetization / Ads Pipeline Lead

## PM Intent

Define the monetization strategy, SDK-readiness pipeline, privacy decision gates, and QA acceptance criteria for Red Ball without importing an SDK or touching Unity gameplay code. Protect Sprint 01 mastery from ad pressure.

## Scope

In:

- Rewarded, interstitial, and banner monetization recommendation.
- Ethical retry/mastery policy.
- SDK option decision list for Unity LevelPlay, AdMob, and Unity Ads.
- User/Yusuf unresolved decision list.
- Privacy, ATT, GDPR, COPPA, and store-disclosure control checklist.
- Assets/config/app ID/test mode plan before SDK import.
- Acceptance criteria and QA gate.
- Sprint 01 mastery-loop binding.

Out:

- SDK import.
- Unity code, scene, prefab, asset, package, or project setting changes.
- Production ad unit setup inside provider consoles.
- Legal or financial guarantees.
- Monetization that blocks retry, badges, checkpoints, or mastery.

## Acceptance Criteria

- Rewarded-first strategy is documented without punishing Sprint 01 retry/mastery.
- Interstitial and banner MVP decisions are explicit.
- SDK choices are framed as decisions, tradeoffs, and prerequisites.
- Privacy/ATT/GDPR/COPPA items are documented as controls and review gates, not legal certainty.
- Pre-import assets/config/app ID/test mode checklist exists.
- QA gate includes no ad before first meaningful play, no energy/timer gating, offline behavior, test ads only, and privacy prompt timing.
- Sprint 01 protected flows are named: badge replay, Clean Run retry, All Coins retry, checkpoint learning, completion feedback.
- Technical debt outcome is stated.

## Risk Check

Shared systems touched:

- Documentation only: task, report, and status board.

Regression risks:

- Future implementers could over-read this as approval to import an SDK. The report explicitly requires PM/Yusuf approval, fake-service-first implementation, and privacy/config readiness first.
- Privacy requirements can change. The report cites official sources checked on 2026-05-12 and requires legal/privacy review before release.

Rollback idea:

- Revert or supersede `ads-monetization-pipeline-v1.md` with a later monetization decision if Yusuf changes the product posture.

## Activity Log

- 2026-05-12 - Monetization / Ads Pipeline Lead read existing `TASK-0006` ads architecture, Sprint 01 mastery report, Sprint 01 implementation task, status board, and technical debt register.
- 2026-05-12 - Checked official Apple, Google AdMob, Unity LevelPlay, Unity child-directed, and FTC COPPA references for privacy boundary planning.
- 2026-05-12 - Created docs-only monetization pipeline v1 and TASK-0023. No Unity code, assets, packages, project settings, or SDK files touched.

## Changed Files

- `docs/ai-production/reports/ads-monetization-pipeline-v1.md`
- `docs/ai-production/tasks/TASK-0023-ads-monetization-pipeline.md`
- `docs/ai-production/STATUS_BOARD.md`

## Verification

- Docs-only task.
- Confirmed no SDK import or Unity file edits were required for this task.

## QA Notes

Future QA must validate fake ad outcomes before any real SDK import:

- Free retry, Clean Run retry, All Coins replay, checkpoint learning, and badge progress remain ad-free.
- Offline/no-fill/skipped/failed/consent-blocked outcomes do not punish the player.
- Test ads only in development and QA.
- No ad before first meaningful play.
- Privacy prompt appears before ad requests and after the player has context unless legally required earlier.

## Repo Controller Notes

Docs-only change. Stage only the report, task, and status board updates from this task. Do not stage unrelated untracked asset folders or package files.

## PM Closure

Pending PM/Yusuf review.

## Technical Debt Added

None.
