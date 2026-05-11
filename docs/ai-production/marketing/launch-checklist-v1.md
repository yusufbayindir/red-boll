# Launch Checklist V1 - Red Ball

## Source

Owner: Marketing Production  
Date: 2026-05-12  
Inputs: marketing research, PM decisions, current status board, ads architecture, QA constraints.

## Purpose

This checklist is for a practical MVP store test, not a scale launch. The launch goal is to learn whether the store page and short gameplay clips can make players understand the game, install it, and complete early levels without frustration.

## 1. Product Truth And Positioning

- [ ] PM chooses public listing name: `Red Ball Roll & Jump`, `Red Ball: Trickline`, or `Trickline Ball`.
- [ ] Product/brand review confirms the name does not look like an impersonation of a larger Red Ball brand.
- [ ] Final copy avoids bosses, skins, daily challenges, leaderboards, offline play, ad rewards, and big level-count claims unless implemented and verified.
- [ ] Public claims for mastery badges, `Clean Run`, skill route, Level 14 vertical lift, and Level 15 crumbling tile are verified before launch with real-play evidence because TD-0006 is open.
- [ ] First screenshot shows gameplay action, not a menu.
- [ ] Store promise matches the build: quick physics platformer levels, roll/jump/bounce, coins, hazards, bounce pads, moving platforms, patrol enemies, touch controls, level select, saved progress, and only QA-approved Sprint 01 mechanics.

## 2. Build Readiness

- [ ] Unity compile passes on the launch branch/build.
- [ ] No blocking console errors on first launch.
- [ ] First 3 levels teach movement, jump, coins, hazards, and enemies without text-heavy explanation.
- [ ] Touch controls are comfortable on at least one small phone and one larger phone.
- [ ] Level select opens reliably and starts the selected level.
- [ ] Saved progress survives app restart.
- [ ] Hearts do not block early learning during the first test session.
- [ ] Checkpoint/warning-sign behavior is either QA-approved or not shown in marketing materials.
- [ ] Mastery badges are implemented, persistent, readable on mobile, and QA-approved before appearing in public materials.
- [ ] Level 14 vertical lift and Level 15 crumbling tile have real traversal evidence before being used as public screenshot/video claims.
- [ ] Clean Run failure paths are verified after hazard damage, enemy side contact, fall/death, and restart before `Clean Run` is used as a public claim.

## 3. Store Assets

- [ ] App icon is readable at small sizes and not too close to Red Ball 4 or major clone icons.
- [ ] 5-7 gameplay screenshots captured from real device or approved simulator footage.
- [ ] Screenshot captions match `screenshot-storyboard-v1.md`.
- [ ] No screenshot shows fake features, future badges, fake menus, or placeholder debug UI.
- [ ] Sprint 01 screenshot set includes only verified frames: badge progress, vertical lift, crumbling tile, skill route, and quick retry/clean run.
- [ ] 1 vertical gameplay trailer is cut from real footage.
- [ ] Trailer first frame is gameplay, not logo or menu.
- [ ] Store copy uses `store-copy-v1.md` or a PM-approved revision.
- [ ] ASO fields use `aso-metadata-v1.md` or a PM-approved revision.

## 4. Compliance And Monetization

- [ ] PM decides whether the game is child-directed or general audience before ads/analytics configuration.
- [ ] Store age rating questionnaire is completed from actual content.
- [ ] Privacy policy URL is ready before public store submission.
- [ ] Analytics SDK, ads SDK, and data disclosures match the real build.
- [ ] If ads are present, they follow PM decision: rewarded heart recovery only, no death-pressure ads.
- [ ] If ads are not present, store disclosures and copy do not imply ad-free unless PM approves that claim.
- [ ] Ads pipeline remains pending until the monetization lead approves implementation, disclosures, and allowed public wording.
- [ ] Any third-party assets used in screenshots or builds have license-safe attribution handling.

## 5. Analytics And Test Events

- [ ] Install source is tracked.
- [ ] First launch is tracked.
- [ ] Level start is tracked.
- [ ] Level complete is tracked.
- [ ] Level fail/death cause is tracked.
- [ ] Coin collection is tracked.
- [ ] Heart empty is tracked.
- [ ] Session length is tracked.
- [ ] Level unlock is tracked.
- [ ] Ad impression/reward events are tracked only if ads exist.
- [ ] Crash reporting is enabled for closed testing.

## 6. Closed Test

- [ ] Recruit 20-50 known testers in Turkey and the developer network.
- [ ] Testers receive a short task list: install, play until Level 3 or 10 minutes, report control confusion, note hardest moment.
- [ ] Collect device model, OS version, session length, level reached, and crash notes.
- [ ] Review Level 1 completion and Level 3 reached before open testing.
- [ ] Fix any first-session control or progress blockers before adding paid traffic.

## 7. Soft Launch Store Test

- [ ] Android open test starts in Turkey plus one or two lower-cost Android-heavy markets if PM approves.
- [ ] iOS TestFlight runs with 25-100 testers before public launch.
- [ ] No paid user acquisition begins until real gameplay screenshots, trailer, analytics, and crash reporting are ready.
- [ ] If a tiny paid test is approved later, spend only to test creative and store conversion, not scale.

## 8. Creative Test Plan

- [ ] Produce 10 vertical clips from `short-video-scripts-v1.md`.
- [ ] Post clips with gameplay in the first frame.
- [ ] Track first-second hold.
- [ ] Track 3-second hold.
- [ ] Track completion rate.
- [ ] Track comments about difficulty, timing, coins, and controls.
- [ ] Track store clicks where links are available.
- [ ] Recut winning clips with faster first payoff.
- [ ] Kill generic clips where viewers do not understand the action.

## 9. Decision Gates

- [ ] Crash-free sessions are 99% or better before wider launch.
- [ ] Level 1 completion is 70% or better.
- [ ] Level 3 reached is 45% or better.
- [ ] D1 retention is around 30% or better for a promising casual prototype.
- [ ] D7 retention is 8-12% or better before meaningful paid spend.
- [ ] If store clicks are weak, revise icon, first screenshot, and short-video hooks.
- [ ] If store conversion is weak but social hooks work, rewrite the listing around the best-performing mechanic.
- [ ] If D1 is weak after install, fix onboarding, controls, early friction, or hearts before more marketing.
- [ ] If players replay for coins, prioritize visible mastery badges.
- [ ] If clean-line clips outperform generic adventure clips, move the product identity toward `Red Ball: Trickline`.

## Launch Blockers

- [ ] Store copy claims a feature not in the build.
- [ ] Store copy claims Sprint 01 badge, Clean Run, skill route, lift, or crumble behavior before real-play verification.
- [ ] First screenshot is unclear or menu-led.
- [ ] Touch controls fail on common phone sizes.
- [ ] Progress does not save after restart.
- [ ] Early hearts block practice and cause tester churn.
- [ ] Crash rate or console errors are unresolved.
- [ ] Privacy, age rating, data disclosure, or asset license questions are unanswered.
- [ ] Ads or monetization claims are made before the ads pipeline decision is approved.

Technical debt added: none.
