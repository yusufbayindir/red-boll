# ASO Metadata V1 - Red Ball

## Source

Owner: Marketing Production  
Date: 2026-05-12  
Inputs: marketing launch research and PM product decisions.

## Metadata Strategy

Use readable keyword coverage, not keyword stuffing. The first metadata pass should index for red ball, bounce ball, roller ball, ball jump, physics platformer, 2D platformer, clean run, and skill route while preserving trust. Sprint 01 terms may be used only where screenshots and real-play QA support them.

Default MVP title:

**Red Ball Roll & Jump**

Future test title after mastery/clean-route systems are verified and visible:

**Red Ball: Trickline**

Brand-risk fallback:

**Trickline Ball**

## Google Play Metadata

| Field | Value | Count / Limit | Notes |
| --- | --- | --- | --- |
| App name | `Red Ball Roll & Jump` | 20 / 30 | MVP-safe, keyword-readable. |
| Short description | `Roll, jump, and bounce through quick physics platformer levels.` | 63 / 80 | Clear and honest. |
| Category | `Game - Arcade` or `Game - Action` | Store choice | Use the category that best matches actual store review guidance. |
| Tags | `Platformer`, `Arcade`, `Casual`, `Single player`, `Stylized` | Store tags | Keep tags aligned with actual gameplay. |
| Content notes | Family-readable action, hazards, simple enemies | Internal | Confirm final age rating through store questionnaire. |

### Google Play Full Description

Use the full description in `store-copy-v1.md`.

### Google Play Custom Store Listing Tests

| Test | Title | Short Description | Screenshot Lead | Hypothesis |
| --- | --- | --- | --- | --- |
| A | `Red Ball Roll & Jump` | `Roll, jump, and bounce through quick physics platformer levels.` | Core action | Baseline keyword/trust listing. |
| B | `Red Ball: Trickline` | `Earn clean runs through quick physics levels.` | Badge progress / skill route | Tests whether clean-line identity improves conversion. Use only if mastery badges and Clean Run are QA-approved for public claim. |
| C | `Trickline Ball` | `Roll the skill route in quick physics runs.` | Skill route / risky coins | Tests lower Red Ball clone adjacency and a more ownable name. |

## App Store Metadata

| Field | Value | Count / Limit | Notes |
| --- | --- | --- | --- |
| Name | `Red Ball Roll & Jump` | 20 / 30 | Same as Google baseline. |
| Subtitle | `Physics platformer runs` | 23 / 30 | Clean category signal. |
| Promotional text | `Find the cleanest rolling line in quick physics platformer levels.` | 66 / 170 | Baseline safe. After QA, test `Earn clean runs and master quick physics routes.` |
| Keywords | `red ball,bounce ball,roller ball,ball jump,platformer,clean run,skill route,coins,hazards` | 90 / 100 | No repeated spaces. Avoid competitor trademarks. |
| Category | `Games - Action` or `Games - Casual` | Store choice | Pick based on final gameplay and rating strategy. |

### App Store Description

Use the App Store draft in `store-copy-v1.md`.

### App Store Keyword Backup Set

Use if the default keyword field underperforms:

`red ball,bounce ball,roller ball,jump game,platform game,2d game,arcade,coins,hazards`

Internal reason: trades `physics platformer` for broader `jump game`, `platform game`, and `arcade`.

Sprint 01 backup set after QA approves badge/route claims:

`red ball,bounce ball,roller ball,clean run,skill route,mastery,badges,lift,crumble,coins`

## Keyword Theme Map

| Theme | Include In Copy | Avoid |
| --- | --- | --- |
| Core discovery | red ball, bounce ball, roller ball, ball jump | title stuffing such as `Red Ball Bounce Roller Ball Hero` |
| Mechanics | physics platformer, bounce pads, moving platforms, coins, obstacles, patrol enemies | bosses, lasers, power-ups unless implemented |
| Session promise | quick runs, simple controls, mobile platformer | relaxing idle, endless runner, adventure RPG |
| Differentiation | clean line, cleaner route, risky coins, Clean Run, mastery badges, skill route | Clean Run rewards, ad rewards, route contracts, ghosts, daily challenges before implementation |
| Sprint 01 mechanics | vertical lift, crumbling tiles, badge progress | level-count inflation or claims that every level has a unique mechanic |

## Localization Notes

English is the primary launch copy. Add Turkish localization after the English positioning is stable and the first screenshot set is approved.

Do not translate by keyword stuffing. Keep the same practical promise: quick physics levels, roll/jump/bounce, risky coins, cleaner routes.

## Metadata Guardrails

- Do not use competitor names in public metadata.
- Do not use `offline` until explicitly verified and approved for store use.
- Do not use `for kids` as a casual keyword before PM resolves child-directed compliance and ad settings.
- Do not mention `ads`, `ad-free`, or rewarded recovery until monetization is implemented and approved.
- Do not make ad performance, ad reward, or monetization claims while the ads pipeline is pending.
- Do not claim `daily challenges` or `leaderboards` until those systems exist in product.
- Do not use `mastery badges`, `Clean Run`, `skill route`, `vertical lift`, or `crumbling tiles` in public metadata until real-play traversal, badge persistence, Clean Run failure paths, and mobile readability are verified before launch under TD-0006.

Technical debt added: none.
