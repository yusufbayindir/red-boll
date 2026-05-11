# Screenshot Storyboard V1 - Red Ball

## Source

Owner: Marketing Production  
Date: 2026-05-12  
Inputs: marketing launch research, PM product decisions, current task status, Sprint 01 implementation and QA reports.

## Capture Rules

- Use real gameplay from a QA-approved build.
- Sprint 01 claims require verify-before-launch review because TD-0006 is open and Unity real play traversal is still pending.
- Capture on an actual phone or device simulator at store-safe aspect ratios.
- Do not use fake UI, fake bosses, fake badges, fake skins, or future feature mockups.
- Keep captions short and outside the ball's immediate path.
- First screenshot must show action in under 2 seconds: ball, platform, coins, and danger.
- Menus can appear later in the set, never first.

## Sprint 01 Launch Candidate Set

Use these five frames as the preferred Sprint 01 store set after capture QA. If any frame cannot be captured from real gameplay without ambiguity, fall back to the older generic action set below.

| Slot | Working Filename | Caption | Frame To Capture | Composition | Production Notes | Pass Criteria |
| --- | --- | --- | --- | --- | --- | --- |
| 1 | `01-badge-progress.png` | `Earn the clean run` | Completion or level-select view showing `Clear`, `All Coins`, and `Clean Run` badge progress. | Badge text/icons must be readable at thumbnail size, with level context visible. | Capture only after badge persistence, Clean Run invalidation, and mobile readability pass QA. | Viewer understands replay mastery without believing badges are fake UI. |
| 2 | `02-vertical-lift.png` | `Ride the lift` | Level 14 ball riding or stepping off a vertical moving platform. | Ball, lift, destination platform, and danger/height cue visible together. | Use first safe lift if possible; do not crop out the landing. | Vertical timing mechanic is obvious. |
| 3 | `03-crumbling-tile.png` | `Crumble, then jump` | Level 15 tile visibly crumbling as the ball crosses toward safe ground. | Crumbling tile, safe landing, and route direction readable in one frame. | Avoid implying checkpoint reset unless QA verifies it; capture a normal restart/reload-safe moment. | Viewer sees a temporary platform challenge. |
| 4 | `04-skill-route.png` | `Find the skill route` | Optional coin route or higher-risk path separated from the safe route. | Safe route and skill route visible together with coins marking the optional path. | Do not claim formal Trickline contracts. | Viewer understands optional mastery/replay choice. |
| 5 | `05-quick-retry-clean-run.png` | `Retry for mastery` | A restart/second attempt or early clean section after a mistake. | Keep retry state natural; no fake fail screen or ad prompt. | Pair with video script `Clean Run Or Reset?`; no ad/monetization UI. | Communicates quick retry and clean-run motivation honestly. |

## Baseline Launch Screenshot Set

| Slot | Working Filename | Caption | Frame To Capture | Composition | Production Notes | Pass Criteria |
| --- | --- | --- | --- | --- | --- | --- |
| 1 | `01-core-action.png` | `Roll. Jump. Bounce.` | Ball mid-jump between platforms, coins ahead, hazard visible below or nearby. | Ball slightly left of center, destination platform visible, hazard readable. | Best from an early or mid level with clear background and no clutter. | A viewer understands this is a physics platformer from the thumbnail. |
| 2 | `02-bounce-route.png` | `Find the clean line` | Ball launching from a bounce pad toward a coin arc. | Bounce pad visible at bottom third, coin line curves toward the landing. | Use the cleanest bounce-pad level; avoid a crop where the landing is offscreen. | The screenshot communicates motion and an optional route. |
| 3 | `03-moving-platform.png` | `Time every move` | Ball landing on or waiting for a moving platform over danger. | Moving platform and danger both visible in the same frame. | Capture at the moment the platform is just reachable. | Timing challenge is clear without explanation. |
| 4 | `04-risky-coins.png` | `Collect the risky coins` | Coin trail that pulls the player away from the obvious safe route. | Safe route and riskier coin route visible together. | This should sell replay value without claiming badges. | Viewer can see a choice: finish safely or chase coins. |
| 5 | `05-patrol-stomp.png` | `Stomp patrols` | Ball bouncing off or about to land on a patrol enemy. | Enemy, ball, and next platform all visible. | Capture the action frame, not the aftermath. | Adds action variety beyond rolling and jumping. |
| 6 | `06-level-select.png` | `Clear levels, unlock more` | Level select showing available, completed, locked, continue-target states, and badge progress if QA-approved. | UI fills the screen with no tiny unreadable text. | Use only after level-select visual states and compact badge display pass QA. | Progression is understandable and polished. |
| 7 | `07-touch-controls.png` | `Built for quick runs` | Gameplay with touch controls visible while the ball approaches a simple jump. | Controls visible but not covering the key obstacle. | Use a clean early level; avoid dense hazards behind UI. | Shows mobile readiness and reduces control anxiety. |

## Shot Order

Default Sprint 01 store order after QA:

1. Badge progress / clean run.
2. Vertical lift.
3. Crumbling tile.
4. Skill route.
5. Quick retry / clean run.
6. Core action fallback if the set needs a broader first-frame platforming read.
7. Touch controls or level select after mobile UI QA.

Baseline fallback order:

1. Core action.
2. Bounce route.
3. Moving platform timing.
4. Risky coins.
5. Patrol stomp.
6. Level select.
7. Touch controls.

If the level-select screen still looks less polished than gameplay, swap slot 6 and 7 or omit level select from paid creative tests.

## Art Direction Notes

- Use gameplay-first crops, not scenic empty backgrounds.
- Prefer bright readable frames with the ball fully visible.
- Avoid text-heavy overlays.
- Avoid a first screenshot that looks like a menu, prototype, or tutorial.
- If a screenshot needs arrows or circles to explain the action, choose a clearer frame instead.
- Ads pipeline is pending. Do not include ad UI, rewarded recovery, ad-free labels, or monetization captions.
- Public screenshot captions for badge progress, Clean Run, Level 14 lift, Level 15 crumbling tile, and skill route must be verified before launch while TD-0006 remains open.

Technical debt added: none.
