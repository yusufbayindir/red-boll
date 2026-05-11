# Gameplay Differentiation / Retention Report v1

## Source

Agent: Gameplay Differentiation / Retention Designer  
Task: `TASK-0017`  
Date: 2026-05-12

## Product Position

This game should not try to win by being "another Red Ball with more levels." The strongest product direction is a compact physics mastery platformer where each level has a readable main route, optional skill routes, and replay goals that feel earned rather than coerced.

Retention should come from:

- Mastery: the player understands why they failed and wants one more clean attempt.
- Short goals: each replay has a clear, bite-sized target.
- Fair repetition: no punitive energy, forced ads, streak loss, or artificial waiting.
- Visible progress: level select and completion results show what improved.
- Optional challenge: harder goals are aspirational, not required to continue.

## Differentiation Mechanics

1. **Trickline Contracts**  
   Each level has optional route goals such as `All Coins`, `Clean Run`, `Par Time`, `Bounce Chain`, or `No Stop`. This turns a simple finish line into a stunt puzzle without blocking casual progression.

2. **Safe Route vs Skill Route**  
   Levels expose a stable main path plus one risky shortcut or coin lane. The player can clear safely, then return for mastery rewards when ready.

3. **Momentum Windows**  
   Certain downhill, jump, or bounce sequences reward keeping speed through a visible lane. This creates movement mastery without requiring complex new controls.

4. **Player-Placed Checkpoint Choice**  
   Once per level, the player can activate one of two checkpoint sockets: safer recovery or closer-to-reward route. This adds agency and risk/reward without dark-pattern punishment.

5. **Clean Run Badge**  
   Awarded for clearing without taking damage or dying. It is easy to understand, compatible with existing platforming, and creates fair replay motivation.

6. **Coin Route Economy**  
   Coins are not just scattered filler; coin clusters communicate optional routes. Early economy should unlock cosmetics or badge frames only, not power upgrades that damage level balance.

7. **Ghost Replay Rival**  
   After a clear, the player can race a translucent version of their best run. Later, daily or friend ghosts can extend retention, but the first version can be local-only.

8. **Daily Micro Course**  
   A small rotating challenge with one contract and no streak pressure. The reward should be pride, badge progress, or cosmetic currency, not fear of missing out.

9. **Run Modifiers**  
   Optional variants such as `Low Gravity`, `Heavy Ball`, `Slippery Floor`, or `Fragile Run`. These can recycle existing levels, but should wait until baseline physics feel consistent.

10. **Puzzle Hazards With Readable Timing**  
   Timed gates, crumble platforms, bumpers, one-way doors, and switch blocks create identity beyond generic spikes/enemies. The design rule: hazards must preview their behavior before punishing the player.

11. **Enemy Stomp Chains**  
   Optional enemy placement lets skilled players chain stomps for coins or route skips. This gives enemies a mastery role rather than only being obstacles.

12. **Assist Toggles With Reward Clarity**  
   Accessibility options such as extra checkpoint, slower moving hazards, high-contrast hazards, and reduced camera shake. Assisted clears should still progress; mastery badges can note standard/assisted mode without shaming.

13. **Route Preview Camera**  
   Before risky segments, a quick non-intrusive camera nudge or signposting shot shows what the player is committing to. This reduces cheap-feeling failure.

14. **Post-Level "Almost There" Results**  
   Completion UI shows `1 coin missed`, `0.8s over par`, or `Clean Run lost at checkpoint 2`. This makes replay goals concrete instead of vague.

15. **Level Badge Shelf**  
   Level select shows three to five compact badges per level. This is a low-content-cost retention layer and makes progress legible.

## Ethical Retention Guardrails

- Do not use energy systems, forced wait timers, mandatory ad continues, streak loss, or manipulative scarcity as the main loop.
- Do not hide progress behind unclear currencies or random rewards.
- Do not make failure expensive; a physics platformer needs fast retry to stay fun.
- Do not require mastery goals to unlock basic level progression.
- Reward skill, curiosity, and route discovery rather than compulsive checking.

## Top 3 Mechanics

### 1. Mastery Badge Loop

Recommended first badges:

- `Clear`: finish the level.
- `All Coins`: route discovery and collection.
- `Clean Run`: no death and no damage.

Why it is worth building:

- It uses mechanics the game likely already has: finish, coins, hazards, death/damage.
- It gives every existing level replay value without demanding a large new content batch.
- It is understandable in one screen and easy to market in screenshots.
- It supports ethical retention because badges are optional and skill-based.

Production decision needed:

- Should `Clean Run` mean no damage only, or no damage plus no death? Recommendation: no damage and no death for clarity.
- Should assisted mode still earn badges? Recommendation: assisted clears progress normally; mastery badges should record assist state only if necessary for future leaderboards.

### 2. Safe Route vs Skill Route

Why it is worth building:

- It differentiates level design immediately: the game becomes about choosing routes, not only surviving obstacles.
- It lets casual players continue while advanced players chase mastery.
- It creates better coin placement rules and screenshot/video moments.

Production decision needed:

- How many skill routes per level in Sprint 01? Recommendation: one visible optional route in one pilot level only.
- What reward should the skill route provide? Recommendation: coins plus a shortcut, not exclusive progression.

### 3. Trickline Contract

Recommended first contract:

- `Bounce Chain`: touch two or three marked bounce pads in sequence before finishing.

Why it is worth building:

- It gives the game a nameable hook: levels are not only cleared, they are performed.
- It can become the long-term identity once the safer badge loop is proven.
- It creates compact daily challenge and ghost replay opportunities later.

Production decision needed:

- Should Trickline be part of Sprint 01? Recommendation: only as a data/UI placeholder or one prototype contract after badges work. Do not block Sprint 01 on a full contract system.

## Sprint 01 Recommendation

Build the first slice around the **Mastery Badge Loop** and a single **Safe Route vs Skill Route** pilot. This is the fastest path to product differentiation with the lowest code and content risk.

Defer full ghost runs, daily challenge, run modifiers, and broad Trickline contracts until badge tracking, completion UI, and level-select progress are stable.

## Risks

- If physics are inconsistent, mastery goals will feel unfair.
- If level design remains linear, badges may feel like chores instead of discovery.
- If completion UI is unclear, players will not understand why to replay.
- If ads interrupt retries or mastery attempts, the retention loop will become hostile.

## Dependencies

- Gameplay: reliable detection for level clear, coin collection, damage/death, and retry.
- UI: completion results and level-select badge state.
- Level design: one pilot level with a safe route and optional skill route.
- QA: repeatability checks for badge awarding and reset behavior.
- Ads/telemetry: no forced ad gate in the core retry loop; track badge/retry behavior before tuning monetization.

