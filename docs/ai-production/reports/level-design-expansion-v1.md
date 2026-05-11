# Level Design Expansion V1

## Source

Agent: Level Design Lead  
Task: `TASK-0018`  
Date: 2026-05-12

## Design Goal

Level Pack 01 should make the current Red Ball prototype feel less like a sequence of simple horizontal routes and more like a compact mobile platformer campaign. The pack should still respect the current strengths: readable physics, short attempts, coin motivation, bounce pads, moving platforms, patrol enemies, hearts, checkpoints, and warning signs.

The pack should add variety through one new idea at a time, then remix ideas in small combinations. Do not solve the "levels are plain" feedback with bigger empty levels or dense hazards.

## Level Pack 01 Progression

Target: 10 new sections after the current Level 13. Each section should be playable in roughly 45-85 seconds on a clean first clear, with one checkpoint in medium/long sections and two checkpoints only for the finale.

| Section | Name | Theme | New hazard / obstacle | Skill focus | Checkpoint | Collectible placement | Difficulty target |
| --- | --- | --- | --- | --- | --- | --- | --- |
| 14 | Asansor Bahcesi | Lift garden with safe vertical movement | Vertical moving platforms using existing moving platform helper | Riding, waiting, stepping off cleanly | After the first safe lift and before the first gap lift | Coin stack above first lift, coin arc over exit jump, 1 optional coin on a slower upper ledge | 3/10 |
| 15 | Kirik Taslar | Old stone bridge | Crumbling tiles with visible shake delay | Commit timing without panic | Before the first crumble-over-pit sequence | Coins teach safe stepping rhythm; optional 3-coin line across the crumbling bridge | 4/10 |
| 16 | Ruzgar Kuyusu | Wind shaft / cave mouth | Fan/wind zone pushing upward or sideways | Adjusting jump arc while rolling | After safe fan tutorial and before side-wind gap | Vertical coin column shows lift force; secret coin above a fan for curious players | 4/10 |
| 17 | Tek Yon Gecit | One-way gate ridge | One-way gates / pass-through flaps | Route commitment and backtracking awareness | At midpoint after first gate chain | Main route coins remain visible; optional coins behind a one-way risk path that rejoins later | 5/10 |
| 18 | Anahtar Patikasi | Key path in a small ruin | Key and lock gate | Explore a tiny branch, return to main path | After collecting first key and opening first lock | Key path has a 3-coin breadcrumb trail; locked reward alcove holds secret coin | 5/10 |
| 19 | Tik Tak Kopru | Timed bridge | Timed hazard cycles, such as spike blocks or closing gates | Read cycle, wait, go | Before the second timed beat | Coins are placed during safe windows, not over damage zones; one optional fast-line coin at cycle edge | 6/10 |
| 20 | Sekme Makinesi | Bumper workshop | Bumpers that redirect ball without damage | Recovery, angle reading, controlled bounce | After first bumper tutorial bay | Coin arcs preview bumper launch path; optional high coin requires bumper plus jump | 6/10 |
| 21 | Ay Hafifligi | Low-gravity pocket | Low gravity zone | Air control and patience | After first low-gravity climb | Wide coin arcs in low gravity; avoid tiny precision coin gaps | 6/10 |
| 22 | Gizli Hat | Split-route hillside | Secret coins and hidden alcoves, no new damage hazard | Observation, replay routing, mastery | At route split | 3 secret coins: one above camera-high ledge, one behind one-way gate, one after optional crumble path | 7/10 |
| 23 | Kirmizi Zirve | Final remix summit | Lift + crumble + timed hazard + key gate, no brand-new mechanic | Fair final exam with readable combinations | Two checkpoints: after lift/crumble half and before final timed/key remix | Required coins trace safe path; optional secret coin on risk route after second checkpoint | 8/10 |

## System Candidates

| System | Design value | First-use rule | Implementation priority |
| --- | --- | --- | --- |
| Moving platforms / vertical lifts | High. Uses existing player expectation and existing helper pattern. | First lift over safe floor, speed <= 1.4, coin stack shows direction. | Sprint 01 |
| Crumbling tiles | High. Adds urgency without complex input. | First two crumble tiles over safe ground, warning sign before pit version, delay >= 0.6s. | Sprint 01 |
| Fans / wind | Medium-high. Changes arc feel and creates visual variety. | First fan should lift the player upward, not push into damage. | Sprint 02 |
| One-way gates | Medium. Good for route puzzles and secret coins. | Gate direction must be obvious; no forced damage after commitment. | Sprint 02 |
| Keys / locks | Medium-high. Adds light exploration. | Key and lock visible within the same route segment when taught. | Sprint 02 |
| Timed hazards | Medium. Adds rhythm but can punish mobile controls. | Always show warning sign, cycle must be readable before player commits. | Sprint 03 |
| Bumpers | Medium. Fun, but physics tuning risk. | First bumper launches to a safe landing with coin arc preview. | Sprint 03 |
| Low gravity zones | Medium. Memorable, but camera/readability risk. | Use in a bounded zone with clear entry/exit treatment. | Sprint 03 |
| Secret coins | High. Low code risk if built on collectible variants or metadata. | Never required for basic clear; should reward observation and replay. | Sprint 01 if cheap, otherwise Sprint 02 |

## Sprint 01 Feature Selection

Best first implementation slice:

1. **Level 14 vertical lift prototype** using the existing moving platform system, plus checkpoint and warning sign placement. This proves longer sections can stay fair with already-supported behavior.
2. **Crumbling tile component and Level 15 teaching section.** This is the highest-value new obstacle because it changes player rhythm without requiring a complex puzzle system.
3. **Secret coin placement support if it can be metadata-only.** If secret coins require new save UI or reward plumbing, defer the reward layer and only place optional hard-to-reach normal coins in Sprint 01.

Keep for later:

- Fans/wind, because force tuning and camera framing need dedicated QA.
- One-way gates and keys/locks, because they create stateful route logic.
- Timed hazards, bumpers, and low gravity, because each needs new readability rules and mobile playtest time.
- Level 20+ remix sections, because remix levels should wait until the teaching sections pass QA.

## Pack Rules

- Each new mechanic gets one safe tutorial beat, one basic test, then one optional risk/reward beat.
- Checkpoints appear every 25-35 world units in post-13 levels unless the section is very short.
- Warning signs appear before crumble-over-pit, timed hazards, blind drops, and forced moving-platform timing.
- Coin lines should teach the intended arc. Optional coins can ask for risk; required completion cannot.
- No autoscroll, chase sequences, crushers, or blind high-speed hazards in Pack 01.
- Mobile readability wins over novelty: hazards must be understandable at phone landscape size before the player commits.

## QA Playtest Checklist

Fairness:

- First exposure to each mechanic cannot kill the player unless they ignore a clearly safe tutorial beat.
- Damage after a checkpoint respawns at the latest checkpoint and spends hearts according to current rules.
- Optional coins do not require damage, perfect sub-pixel jumps, or blind leaps.
- The player can recover from normal physics variance after bounce, bumper, fan, or lift interactions.

Camera:

- Next landing, hazard, or gate is visible before the player jumps or commits.
- Vertical lifts and low-gravity zones do not push the player beyond useful camera framing.
- Checkpoints, warning signs, keys, locks, and gates are not hidden by HUD or screen edges.

Checkpoint:

- Checkpoint trigger is visually distinct from goal.
- Restart behavior still returns to level start unless PM changes the rule.
- Completing a level clears previous checkpoint state.
- Finale checkpoint spacing does not let the player respawn into an immediate hazard.

Completion time:

- Section 14 target: 45-60 seconds first clear.
- Sections 15-18 target: 55-75 seconds first clear.
- Sections 19-23 target: 65-85 seconds first clear.
- Clean replay should be at least 15-25% faster than cautious first clear.

Mobile readability:

- Every new object is readable on mobile landscape without relying on text labels.
- Warning sign, coin arc, and object motion together explain the next action.
- Touch controls have room for waiting, reversing, and jump timing.
- No sequence requires rapid alternating left/right inputs while also timing a jump.

## Open Design Notes

- Secret coins need a product decision: cosmetic extra, mastery badge input, or just harder normal coins for now.
- Crumbling tiles need a clear reset behavior on checkpoint respawn.
- Stateful systems such as keys and one-way gates should be introduced only after checkpoint QA is accepted.

