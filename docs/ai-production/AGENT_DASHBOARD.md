# Agent Dashboard

This is the visible dashboard for every specialist agent used in this production thread.

Note: Codex background agents do not appear as separate user-visible sidebar chats in the app. This file is the canonical visible record of who worked on what, what they concluded, and what PM decided.

## Live Dashboard State

`docs/ai-production/dashboard/data/dashboard.json` is now the canonical browser-readable live state source for the dashboard. It includes:

- Version/change fields: `schemaVersion`, `version`, `updatedAt`, `polling`.
- Agent statuses limited to `thinking`, `running`, `done`, `blocked`, and `idle`.
- Active agent indicators for the small thinking/logo UI state.
- Event timeline, transcript snippets, task rows, PM decision log, and artifact/report links.

Runtime/UI ownership remains with the live dashboard worker; this document records the state contract and production history.

## Agents

| Role | Agent | Status | Output |
| --- | --- | --- | --- |
| Developer Gameplay | Kant | Completed | `TASK-0001`, extracted `RedBallPlayer` |
| UI/Menu Designer | Leibniz | Completed | `reports/TASK-0002-menu-ui-redesign-direction.md` |
| Developer UI / Polish | Aristotle | Completed | `reports/TASK-0002-menu-ui-redesign-implementation-breakdown.md` |
| Asset Researcher | Zeno | Completed | `reports/asset-research.md` |
| Game Design / Retention | Mencius | Completed | `reports/retention-mastery-badge-loop.md` |
| Game Design / Differentiation Challenger | Erdos | Completed | `reports/differentiation-challenger-red-ball-trickline.md` |
| Level Designer | Banach | Completed | `reports/level-design-expansion-plan.md` |
| Ads Pipeline Architect | Carver | Completed | `reports/ads-pipeline-architecture.md` |
| Marketing Researcher | Chandrasekhar | Completed | `reports/marketing-launch-research.md` |
| Git Repo Controller | Averroes | Completed | `TASK-0008` |
| QA | Socrates | Completed | `reports/qa-checklist.md` |

## PM Decisions So Far

| Area | Decision | Reason |
| --- | --- | --- |
| Product hook | Mastery-first, Trickline later | Safer MVP is `Clear`, `All Coins`, `Clean Run`; full route contracts/ghosts can come later. |
| Hearts | Do not make hearts the main retention loop | Physics platformer failure is practice; scarcity-first retention would feel hostile. |
| UI direction | Playable Hill Menu | Stronger first-screen identity using existing assets, no large art dependency. |
| UI implementation | Start with one `RedBallUi` bridge | Better than exploding the project into many UI files immediately. |
| Level expansion | Checkpoints before long levels | Longer post-13 levels need fairness support first. |
| Ads | Rewarded heart recovery only for MVP | Offer from menu/level select, never death-pressure ads. |
| Assets | CC0-first, no import yet | Kenney-heavy stack is safest; import waits until repo/mvp priorities are stable. |
| Repo | Initialize existing folder only after approval | Remote appears empty; local Unity folder is the real project. |
| Architecture | Small splits only | First split extracted `RedBallPlayer`; no broad rewrite. |

## Next Implementation Queue

1. Close or risk-accept `TASK-0001` after play-mode smoke.
2. Ask Yusuf for approval before `git init` / first commit.
3. Open UI implementation task for `RedBallUi` bridge + level-select continue highlight fix.
4. Open checkpoint/warning-sign implementation task before Levels 14-20.
5. Open Mastery Badge MVP after UI bridge and save boundaries are clearer.
6. Open Ads fake-service task only after heart/economy boundary exists.

## Report Links

- `docs/ai-production/reports/TASK-0002-menu-ui-redesign-direction.md`
- `docs/ai-production/reports/TASK-0002-menu-ui-redesign-implementation-breakdown.md`
- `docs/ai-production/reports/ads-pipeline-architecture.md`
- `docs/ai-production/reports/asset-research.md`
- `docs/ai-production/reports/differentiation-challenger-red-ball-trickline.md`
- `docs/ai-production/reports/level-design-expansion-plan.md`
- `docs/ai-production/reports/marketing-launch-research.md`
- `docs/ai-production/reports/qa-checklist.md`
- `docs/ai-production/reports/retention-mastery-badge-loop.md`

## New Task Opened

- `TASK-0012`: RedBall UI Bridge First Slice.
