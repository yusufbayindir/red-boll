# Marketing Sprint 01 Alignment

Owner: Marketing Alignment Producer  
Date: 2026-05-12  
Workspace: `/Users/yusufbayindir/Desktop/ai game/red_ball`

## Scope

Aligned marketing production docs with the pushed Sprint 01 gameplay slice:

- Mastery badges: `Clear`, `All Coins`, `Clean Run`.
- Clean-run replay motivation.
- Optional skill route / risky coin route.
- Level 14 vertical lift.
- Level 15 crumbling tile.

No code, Unity scene, asset, SDK, package, or generated Unity files were changed.

## Sources Read

- `docs/ai-production/marketing/store-copy-v1.md`
- `docs/ai-production/marketing/aso-metadata-v1.md`
- `docs/ai-production/marketing/screenshot-storyboard-v1.md`
- `docs/ai-production/marketing/short-video-scripts-v1.md`
- `docs/ai-production/marketing/launch-checklist-v1.md`
- `docs/ai-production/tasks/TASK-0014-marketing-production-assets.md`
- `docs/ai-production/tasks/TASK-0019-sprint-01-unity-implementation.md`
- `docs/ai-production/tasks/TASK-0021-sprint-01-qa-playtest.md`
- `docs/ai-production/reports/sprint-01-qa-run-2026-05-12.md`
- `docs/ai-production/STATUS_BOARD.md`

## Claim Alignment

Updated public-safe positioning:

- Keep the baseline promise: quick, readable 2D physics platformer levels with roll, jump, bounce, coins, hazards, moving platforms, patrols, touch controls, level select, and saved progress.
- Add Sprint 01 wording only with launch verification gates: mastery badges, `Clean Run`, optional skill route, vertical lift, and crumbling tiles.
- Use `Clean Run` as a player mastery goal, not as a paid reward or monetized retry promise.
- Use `skill route` as an optional route/coin challenge, not as a formal Trickline contract framework.
- Use Level 14/15 mechanics as concrete screenshot/video frames only after real gameplay capture verifies traversal and readability.

Removed or kept blocked:

- No bosses, skins, daily challenges, leaderboards, multiplayer, ghosts, huge level-count claims, direct competitor comparisons, or offline claims.
- No ad, ad-free, rewarded recovery, paid retry, or monetization claim while the ads pipeline is pending.
- No claim that crumbling tiles reset at checkpoint; current QA notes say reload/restart rebuilds tiles, while checkpoint reset remains a risk/debt area.

## Store Copy Updates

Store copy now supports the Sprint 01 feature set without overstating launch readiness:

- Feature list includes vertical lifts and crumbling tiles.
- Replay copy references collecting more coins, finding the skill route, and finishing cleaner.
- Mastery goals are framed as `Clear`, `All Coins`, and `Clean Run` after final gameplay QA approves public use.
- Copy guardrails now separate allowed implemented terms from terms that need real-play/capture approval before launch.
- Ads pipeline is explicitly pending and excluded from public claims.

## ASO Updates

Metadata now has Sprint 01 keyword options with gates:

- Baseline remains `Red Ball Roll & Jump`.
- `Red Ball: Trickline` and `Trickline Ball` remain test candidates only after mastery/clean-route systems are verified and visible.
- Keyword set now can test `clean run` and `skill route`.
- Backup Sprint 01 keyword set adds `mastery`, `badges`, `lift`, and `crumble` only after QA approval.

## Screenshot Storyboard Updates

Recommended Sprint 01 frames:

1. Badge progress: completion or level-select view showing `Clear`, `All Coins`, and `Clean Run`.
2. Vertical lift: Level 14 ball riding or stepping off the lift with landing visible.
3. Crumbling tile: Level 15 tile crumbling while the safe landing remains visible.
4. Skill route: optional coin path visible beside the safer route.
5. Quick retry / clean run: second attempt or clean early route without fake fail UI or ad prompt.

Fallback baseline screenshots remain available if any Sprint 01 frame fails QA/capture readability.

## Video Script Updates

Existing `Clean Run Or Reset?` script now allows badge UI only after QA verifies Clean Run behavior and mobile readability.

New Sprint 01 script concepts added:

- `Badge Progress Check`
- `Ride The Lift`
- `Crumble Tile Timing`

All new scripts are marked verify-before-launch because TD-0006 remains open.

## Launch Verification Required

Before public launch claims use Sprint 01 wording, QA/capture must verify:

- Level 14 safe-route traversal and vertical lift readability in real play.
- Level 15 crumbling tile timing, safe first exposure, restart/reload rebuild behavior, and no misleading checkpoint-reset claim.
- Badge persistence after menu return and app relaunch.
- `Clean Run` invalidates on hazard damage, enemy side contact, fall/death, and restart/new attempt.
- Level-select badge display and completion badge summary are readable on mobile landscape.
- Skill route is optional and not required to clear the level.
- Screenshots and videos use real gameplay only, with no fake UI or future feature mockups.

## Ads And Monetization

Ads pipeline is pending. Marketing must not make claims about:

- Rewarded ads.
- Watch-to-retry or watch-to-recover.
- Ad-free play.
- Monetized badge, checkpoint, retry, or mastery benefits.
- Paid campaign performance.

## Changed Files

- `docs/ai-production/marketing/store-copy-v1.md`
- `docs/ai-production/marketing/aso-metadata-v1.md`
- `docs/ai-production/marketing/screenshot-storyboard-v1.md`
- `docs/ai-production/marketing/short-video-scripts-v1.md`
- `docs/ai-production/marketing/launch-checklist-v1.md`
- `docs/ai-production/reports/marketing-sprint-01-alignment.md`
- `docs/ai-production/tasks/TASK-0014-marketing-production-assets.md`
- `docs/ai-production/STATUS_BOARD.md`

## QA Status

Marketing status: aligned, capture gated.

Runtime QA status: Sprint 01 remains QA PARTIAL. Compile/import and static checks passed, but Unity real play traversal remains open under TD-0006. Public marketing claims for mastery badges, Clean Run, skill route, Level 14 lift, and Level 15 crumbling tile must be verified before launch.

Technical debt added: none.
