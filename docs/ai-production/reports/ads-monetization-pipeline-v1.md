# Ads Monetization Pipeline v1

## Source

Agent: Monetization / Ads Pipeline Lead  
Task: `TASK-0023`  
Date: 2026-05-12  
Scope: Docs-only monetization and ad SDK readiness plan. No Unity files changed and no SDK imported.

## Executive Decision

Red Ball should remain mastery-first in Sprint 01. Ads must not make retries, clean runs, badge chasing, checkpoints, level unlocks, or movement mastery feel punished.

Recommended monetization posture:

1. Ship no real ads in Sprint 01.
2. Prepare a fake-service-first ad boundary before importing any SDK.
3. First real placement, only after PM/Yusuf approval: opt-in rewarded revive or recovery outside pressure moments, with a strong preference for future optional revive, cosmetic, or soft-currency use once those systems exist.
4. Do not ship banners in the MVP.
5. Do not ship interstitials in the MVP.

This updates the earlier `TASK-0006` rewarded-heart idea to fit the Sprint 01 mastery loop. Hearts or retries must not become a scarcity monetization pillar unless the product direction explicitly changes.

## Product Strategy

### Rewarded Ads

Allowed future rewarded placements:

- Optional revive after a failed attempt, if and only if the retry button remains free, immediate, and visually primary.
- Optional cosmetic or currency reward after a durable cosmetic/economy system exists.
- Optional daily bonus from menu, if it never blocks level access or badge progress.

Rejected rewarded placements:

- Watch ad to retry.
- Watch ad to preserve Clean Run.
- Watch ad to recover a checkpoint state.
- Watch ad to unlock a level, badge, route, or movement advantage.
- Watch ad to double mastery badge progress.

Revive design boundary:

- A revive can be offered later as a convenience, not as the expected way to finish a level.
- No revive offer before the player has had meaningful play in the session.
- No revive offer if it would invalidate the skill language of Clean Run; a revived attempt cannot earn Clean Run unless game design explicitly creates a separate badge category.
- Failed, skipped, no-fill, or offline ad outcomes must return the player to the same free retry path.

### Interstitials

Recommendation: disabled for MVP.

Possible future placement, only after approval:

- Natural break after returning to menu or level select, never before first gameplay.
- Minimum session maturity rule: no interstitial in the first session and no interstitial until at least several meaningful level attempts or completions have happened.
- Frequency cap: conservative global cap and cool-down controlled by remote/config values.

Hard bans:

- No interstitial after death, damage, fall, restart, no-heart/no-resource state, locked-level tap, checkpoint use, or failed skill route.
- No interstitial covering completion feedback, badge reveal, Clean Run feedback, or level-select mastery review.

### Banners

Recommendation: no banners for MVP.

Reasoning:

- Red Ball is a touch-control physics platformer. Persistent banners compete with landscape HUD, joystick, jump button, level visibility, and hazard readability.
- Banner revenue is unlikely to justify degraded control comfort during the core mastery loop.
- If banners are revisited later, restrict them to non-gameplay surfaces such as a shop/cosmetic screen, and require mobile safe-area QA.

## Ethical Retry And Mastery Principle

The core promise is: failure teaches, retry is free, mastery is respected.

Policy:

- Retry remains instant and ad-free.
- Badge improvement remains ad-free.
- Clean Run and All Coins are never protected, restored, multiplied, or sold through ads.
- Checkpoint use remains a gameplay affordance, not a monetization surface.
- Any rewarded ad must be player-initiated, clearly labeled, optional, and non-punitive when unavailable.
- No energy, timer, stamina, or wait gate may be added to Sprint 01 retry/mastery play.

## SDK Options And Technical Decision List

### Option A: Unity LevelPlay Mediation

Best fit if Red Ball wants mediation across multiple networks, waterfall/bidding controls, and one long-term monetization console.

Pros:

- Mediation-first architecture can support AdMob, Unity Ads, and other networks later.
- Unity ecosystem fit is strong for a Unity mobile game.
- Regulation flags exist for GDPR, U.S. privacy, and child-directed handling.

Risks / controls:

- More integration surface than a single-network SDK.
- Adapter versions and privacy API changes need release discipline.
- Requires a final network list before store privacy disclosures are reliable.

### Option B: Google AdMob

Best fit if Red Ball wants a common single-network starting point with mature test ad flows and Google Play alignment.

Pros:

- Strong documentation for Unity, test ads, child-directed treatment, and under-age-of-consent flags.
- Straightforward for a narrow rewarded-only MVP.

Risks / controls:

- Less mediation flexibility if used alone.
- If later migrating to mediation, placement IDs, reporting, and consent flow may need rework.
- Requires careful App Store privacy/data disclosure review, especially around identifiers and ad personalization.

### Option C: Unity Ads Direct

Best fit if Red Ball wants the smallest Unity-native ad experiment and does not need mediation at first.

Pros:

- Unity-native mental model.
- Can be simpler than mediation for a single rewarded placement.

Risks / controls:

- May be limiting if revenue optimization later requires multiple networks.
- Direct SDK choice should not bypass the same privacy, test mode, and placement policy gates.

### Recommendation

Do not pick or import an SDK yet.

Default pipeline recommendation:

1. Implement fake ad service and placement policy first in a later engineering task.
2. Confirm privacy/age positioning, platform targets, and monetization placements.
3. Choose either AdMob-only for a narrow test or LevelPlay if mediation is part of the near-term business plan.
4. Import real SDK only after app IDs, test IDs, consent flow, store disclosures, and QA gates are ready.

## User Decisions Required Before SDK Import

- Target platforms: iOS, Android, or both for first ad-enabled build.
- Monetization start: no ads until after Sprint 01 QA PASS, or fake-service implementation during Sprint 02.
- First placement: optional revive, cosmetic/currency reward, menu daily bonus, or no rewarded placement yet.
- SDK path: AdMob-only MVP, Unity LevelPlay mediation, Unity Ads direct, or defer.
- Is Red Ball child-directed, mixed-audience, or general audience?
- Will the app be submitted to Google Play Designed for Families or any kids/family category?
- Does Yusuf want personalized ads where legally allowed, or non-personalized/limited ads by default?
- Should ATT be requested on iOS, or should the ad stack avoid IDFA-dependent tracking where possible?
- Which legal/privacy owner approves privacy policy, store privacy labels, data safety forms, and consent copy?
- What analytics provider, if any, will coexist with ads and consent state?
- What age gate, if any, is appropriate before requesting ad consent or showing personalized ads?

## Privacy, ATT, GDPR, COPPA Boundary

This is not legal advice. Final policy decisions need a qualified legal/privacy review before release.

Controls to require:

- Do not initialize a real ad SDK until age/privacy/consent state is resolved for that user and region.
- Keep a privacy options entry available from settings/menu once ads are enabled.
- Use test mode and test ad unit IDs in development and QA builds only.
- Maintain store disclosures before release: Apple privacy details, Google Play Data Safety, SDK list, network adapters, and data categories.
- On iOS, request App Tracking Transparency only if the selected ad/analytics stack tracks users across companies' apps or websites or needs IDFA. Apple requires ATT permission before tracking; denial must still leave the app usable.
- For GDPR/UK GDPR regions, use a consent management flow before personalized ad requests.
- For COPPA/child-directed or under-age users, configure child-directed/under-age flags where provider docs require them and default to limited/non-personalized ads.
- For U.S. state privacy regimes, document whether "do not sell/share" or limited data use controls are needed for the selected provider.

Reference sources checked on 2026-05-12:

- Apple App Tracking Transparency: https://developer.apple.com/documentation/apptrackingtransparency
- Apple App Store privacy and data use: https://developer.apple.com/app-store/user-privacy-and-data-use/
- Google AdMob Unity targeting / TFCD / TFUA: https://developers.google.com/admob/unity/targeting
- Unity LevelPlay regulation settings: https://docs.unity.com/grow/en-us/levelplay/sdk/unity/regulation-advanced-settings
- Unity child-directed apps guidance: https://docs.unity.com/en-us/grow/levelplay/legal-resources/children-child-directed-apps
- FTC COPPA FAQ: https://www.ftc.gov/business-guidance/resources/complying-coppa-frequently-asked-questions
- FTC COPPA six-step plan: https://www.ftc.gov/business-guidance/resources/childrens-online-privacy-protection-rule-six-step-compliance-plan-your-business

## Pre-Import Assets And Configuration Checklist

Required before SDK import:

- Chosen SDK/provider and version policy.
- iOS bundle ID and Android package name confirmed.
- Store/app records created where provider requires them.
- Provider app IDs created for dev/test and production.
- Rewarded placement/ad unit IDs created for dev/test and production.
- Interstitial and banner IDs not created unless PM approves those formats.
- Test devices registered for AdMob/LevelPlay/Unity Ads as required.
- Test mode switch documented and verified.
- Build flags/config keys defined, for example fake ads vs provider ads.
- Consent provider or consent UX decision documented.
- Privacy policy URL draft available before real ad build distribution.
- Store privacy/data safety draft prepared from selected SDK/network data collection docs.
- SKAdNetwork IDs and iOS privacy manifest requirements collected for selected SDKs/adapters.
- Android manifest permissions and Gradle dependency impact reviewed.
- QA matrix includes offline, no-fill, skipped, failed, completed, duplicate callback, slow load, and consent-blocked outcomes.

Configuration rule:

- No production ad unit ID may be committed or enabled in a development build by default.
- Secrets or console-only credentials must not be stored in repo docs.
- Placement IDs can be documented as placeholders until PM/Yusuf approves real provider setup.

## Acceptance Criteria

- No ad is shown before first meaningful play.
- No forced ad, energy, timer, stamina, or wait gate blocks retry or level access.
- Badge, Clean Run, All Coins, checkpoint retry, and replay improvement remain ad-free.
- Rewarded ad offers are optional, manually tapped, clearly labeled, and safe to ignore.
- Failed, skipped, no-fill, consent-blocked, or offline ad outcomes do not consume progress, hearts, currency, attempts, or badge eligibility.
- Interstitials remain disabled unless a future task explicitly approves them.
- Banners remain disabled in gameplay and MVP.
- Development and QA builds use test ads only.
- Real SDK initialization waits for consent/age/privacy readiness.
- Privacy prompt timing is not front-loaded before the player understands the game unless legally required; consent prompts should appear before ad requests, not at app boot by habit.
- QA can simulate fake outcomes before real SDK import.

## QA Gate

Before fake-service implementation closes:

- Free retry works with fake ads disabled.
- Clean Run retry path remains ad-free.
- All Coins replay path remains ad-free.
- No ad UI appears in active gameplay unless a future approved optional revive exists.
- Offline mode hides or disables ad offers calmly.
- Fake `Completed` grants exactly one reward and is idempotent.
- Fake `Skipped`, `NoFill`, `Failed`, and `BlockedByConsent` grant nothing and do not punish.
- Double-tap or duplicate callback cannot grant twice.
- Cooldowns/caps, if configured, are visible in logs and do not block normal play.

Before real SDK import closes:

- Test mode verified on every target platform.
- No production ads in Editor/dev/QA builds.
- Consent flow tested in personalized allowed, non-personalized, under-age/child-directed, denied ATT, and offline states as applicable.
- First-session flow proves no ad before meaningful play.
- Store privacy/data safety review completed for the exact SDK/adapters imported.

## Sprint 01 Binding

Sprint 01 shipped a mastery loop direction: Clear, All Coins, Clean Run, replay improvement, Level 14/15 prototypes, and QA gates. Monetization must support that product truth.

Sprint 01 protected flows:

- Retry after failure: no ad.
- Restart for Clean Run: no ad.
- Replay for missing coins: no ad.
- Badge reveal and completion feedback: no ad.
- Checkpoint learning: no ad.
- Level select mastery review: no ad interruption.

Future rewarded ads can exist only around optional convenience or cosmetic/economy value. They cannot become a toll on mastery.

## Technical Debt Policy

Technical debt added: none.

Reason: this task is a docs-only planning task that explicitly defers SDK import and real implementation until required decisions, configs, and QA gates exist. No accepted shortcut or temporary runtime behavior was introduced.
