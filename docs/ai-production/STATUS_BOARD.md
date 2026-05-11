# Status Board

## Setup

| Item | Status | Owner | Notes |
| --- | --- | --- | --- |
| AI production docs | Complete | PM | Role, protocol, roster, and task files created. |
| Git repo binding | Open | Git Repo Controller | Local folder is not a git repo; remote exists as requested but returned no refs during initial check. |
| Runtime architecture | In progress | Architecture/Developers | First split completed: `RedBallPlayer` extracted. |
| QA workflow | Complete | QA | QA checklist created; code tasks require compile and play-mode smoke evidence. |
| Agent dashboard | In progress - data layer ready | PM + Live Dashboard + Agent State Recorder | `dashboard/data/dashboard.json` now has versioned polling fields, stable statuses, events, snippets, task rows, PM decisions, and artifact links. Runtime/UI verification remains separate. |
| Company-like operating model | Complete | Product Ops / Grooming Process Lead | `OPERATING_MODEL.md`, grooming, intake, decision log, and daily/overnight run templates created. PM directs process; specialists implement. |
| Technical debt register | Open | Technical Debt Registrar | Source of truth: `technical-debt/TECHNICAL_DEBT_REGISTER.md`. Every worker final report must state debt added or none. |

## Process

- Source of truth lives in task MDs, meeting MDs, decision logs, and this status board.
- PM/moderator owns grooming, prioritization, debate moderation, acceptance criteria, and closure notes. PM does not directly implement dashboard UI/runtime or product code.
- Specialists must challenge weak proposals and defend their role-based judgment; automatic agreement is not acceptable.
- Debate is capped at 4 rounds. If no agreement exists after round 4, PM documents a decision or asks Yusuf.
- Agent limit handling requires a final agent comment, written handoff, closure/replacement note, and task/meeting MD update before a new role inherits the work.
- Dashboard chat/grooming visibility is part of TASK-0015 process: dashboard should surface current tasks, agent state, recent meeting/debate notes, handoffs, and PM decisions while task/meeting docs remain source of truth.
- Technical debt tracking is mandatory. Register: `technical-debt/TECHNICAL_DEBT_REGISTER.md`; entry template: `templates/technical-debt-entry-template.md`.

## Technical Debt Watchlist

Top critical debts to review first:

| Debt | Why critical | Owner | Trigger |
| --- | --- | --- | --- |
| TD-0004 | Unity source files are not yet committed into first release scope, so a fresh checkout may not reproduce the project. | Git / Release + Developer | Before first release commit or build verification |
| TD-0006 | Play-mode QA is pending, so runtime behavior may still fail even where docs or implementation notes look complete. | QA | Before closing implementation tasks or release candidate |
| TD-0001 | Real Codex subagent dispatch is still manual, so PM queue and handoffs can drift under load. | PM / Dashboard | Before overnight runs or more than two active specialist agents |

## Open Tasks

| Task | Title | Status | Owner |
| --- | --- | --- | --- |
| TASK-0001 | Modular architecture plan and first split | QA Pending | Developer Gameplay + PM |
| TASK-0002 | Menu and UI redesign direction | Ready for implementation task | UI/Menu Designer |
| TASK-0003 | Free asset research report | Accepted | Asset Researcher |
| TASK-0004 | Differentiation and retention design | Accepted | Game Design |
| TASK-0005 | Level design expansion pack | Accepted | Level Designer |
| TASK-0006 | Ads pipeline architecture | Accepted | Ads Pipeline |
| TASK-0007 | Marketing and launch research | Accepted | Marketing Researcher |
| TASK-0008 | Git repo control setup | Awaiting approval | Git Repo Controller |
| TASK-0009 | Mastery Badge MVP | Draft | PM |
| TASK-0010 | Checkpoints and warning signs | Implementation Complete - QA Pending | PM |
| TASK-0011 | Agent Dashboard Site | Accepted | PM + Dashboard Site Developer |
| TASK-0012 | RedBall UI Bridge First Slice | Implementation Complete - QA Pending | PM + Developer UI/Polish |
| TASK-0013 | Asset Pipeline And Downloads | In Progress | PM + Asset Pipeline |
| TASK-0014 | Marketing Production Assets | Aligned - Capture Gated | PM + Marketing Production |
| TASK-0015 | Live Agent Dashboard System | In Progress - Data Layer Ready | PM + Live Dashboard UX + Agent State Recorder |
| TASK-0016 | Prod Sprint 01 Roadmap | Open - Groomed for Sprint 01 | Product Grooming Lead + PM |
| TASK-0019 | Sprint 01 Unity Implementation | QA PARTIAL - Compile Pass / Play-mode Pending | Gameplay Integration + UI/Badge + Mechanic Component + QA |
| TASK-0021 | Sprint 01 QA Playtest | QA PARTIAL - Compile Pass / Play-mode Pending | QA Scenario Designer / QA Lead |
| TASK-0022 | Unity Play/EditMode Smoke Harness | Implementation Complete - EditMode Smoke PASS | Unity Test Harness Developer | `TestResults/editmode-smoke-results.xml` has 6/6 passing tests for LevelCount, badge mask helper, UI badge summary, and Level 1/14/15 load construction. |
| TASK-0023 | Ads Monetization Pipeline | Proposed / Ready for PM Review | Monetization / Ads Pipeline Lead |

## Sprint 01

| Order | Workstream | Status | Owner | Notes |
| --- | --- | --- | --- | --- |
| 1 | Dashboard / Ops | Ready for closure QA | Realtime Dashboard Engineer + Product Ops | Finish TASK-0015 closure, relay smoke, source-of-truth links, and handoff visibility. |
| 2 | Gameplay Differentiation | Groomed / blocked on slice selection | Gameplay Developer + Game Design Differentiation | Choose first mastery/Trickline-compatible slice before marketing claims. |
| 3 | Level Content | Groomed / checkpoint-first | Level Designer + Gameplay Developer | Levels 14-16 first; Levels 17-20 gated by QA. |
| 4 | Asset Pipeline | Groomed / awaiting subset approval | Asset Pipeline | Stage approved assets with source URLs, licenses, zips, and checksums before runtime import. |
| 5 | QA | QA PARTIAL - compile pass, EditMode smoke pass, play-mode pending | QA Lead | Compile/import gate passed on 2026-05-12; TASK-0022 added 6/6 passing EditMode smoke XML for Level 1/14/15 construction. Full PlayMode traversal evidence remains pending under TD-0006. |
| 6 | Monetization / Ad Pipeline | Proposed / Ready for PM Review | Monetization / Ads Pipeline Lead | TASK-0023 recommends no real ads in Sprint 01, no banners/interstitials in MVP, fake-service-first SDK boundary, and optional rewarded ads only for future revive/cosmetic/currency value after privacy/config approval. No retry, badge, Clean Run, All Coins, checkpoint, or mastery progress monetization. |
| 7 | Marketing | Aligned / capture gated | Marketing Producer + Capture QA | Store copy, ASO, storyboard, video scripts, and checklist now reflect Sprint 01 mastery badges, Clean Run, skill route, Level 14 lift, and Level 15 crumbling tile. Public claims require real-play verification before launch while TD-0006 remains open. Ads pipeline claims remain pending. Report: `reports/marketing-sprint-01-alignment.md`. |
| 8 | Git / Release | Pushed to origin/main | Git Repo Controller + Release Manager | Primary Sprint 01 release commit `5d7ee44` pushed successfully. Scope is constrained to Unity source/tests/project settings, dashboard/docs, tracked launcher, and committed EditMode smoke XML. Generated Unity folders, logs, raw asset holding folders, `.DS_Store`, and parent launcher remain excluded. Report: `reports/sprint-01-git-release-report.md`. |

Implementation queue note: `TASK-0019` is the Sprint 01 Unity implementation handoff. `Assets/Scripts/RedBallRuntime.cs` should have one Gameplay Integration Owner during the task; badge UI helpers and crumbling tile behavior can be split into `RedBallUi.cs` and a new component file after the runtime model is fixed.

QA queue note: `TASK-0021`, `TASK-0022`, `reports/sprint-01-qa-test-plan.md`, `reports/sprint-01-qa-run-2026-05-12.md`, and `reports/unity-playmode-smoke-harness.md` define the current QA state. Compile/import passed, static blocker review found no confirmed blocker, and TASK-0022 now provides EditMode smoke XML for Level 1/14/15 construction. PlayMode/manual traversal evidence remains required before Sprint 01 PASS. Bugs block `TASK-0021` and return to the owning implementation task; accepted limitations must be registered as technical debt before closure.

Marketing queue note: Sprint 01 public claims are gated on real-play/capture QA. Use `reports/marketing-sprint-01-alignment.md` and `marketing/screenshot-storyboard-v1.md` for the preferred 5-frame set: badge progress, vertical lift, crumbling tile, skill route, and quick retry/clean run. Do not make ads, rewarded recovery, ad-free, or monetization claims while the ads pipeline is pending.

Sprint 01 technical debt note: source of truth is `docs/ai-production/technical-debt/TECHNICAL_DEBT_REGISTER.md`; TASK-0016 references existing TD-0001 through TD-0008 where relevant. No new Sprint 01 grooming debt entry added.

Git release note: Sprint 01 release execution treats `TestResults/editmode-smoke-results.xml` as committed QA evidence and leaves `Logs/editmode-smoke.log` local-only. Parent `/Users/yusufbayindir/Desktop/ai game/open-ai-production-dashboard.command` is outside the repo and cannot be pushed; repo launcher is `open-ai-production-dashboard.command`.

## Decision Log

| Date | Decision | Owner |
| --- | --- | --- |
| 2026-05-11 | Use strict task MD + QA + repo controller flow for implementation. | PM |
| 2026-05-11 | Do not keep growing the single runtime file without a modularization plan. | PM |
| 2026-05-11 | Product direction candidate is mastery-first: badges now, Trickline-style contracts later. | PM |
| 2026-05-11 | Checkpoints are a prerequisite for longer post-13 levels. | PM |
| 2026-05-12 | Live dashboard data source uses versioned local JSON with explicit agent statuses and dashboard-ready event/task/decision/artifact collections. | Agent State Recorder |
| 2026-05-12 | Adopt company-like AI production operating model with grooming templates, 4-round debate, agent limit handoff, and overnight run protocol. | Product Ops / Grooming Process Lead |
| 2026-05-12 | Adopt Sprint 01 production order: dashboard/ops, gameplay differentiation, level content, asset pipeline, QA, monetization/ad boundary, marketing, git/release. | Product Grooming Lead |
| 2026-05-12 | Monetization posture for Sprint 01 is mastery-protective: no SDK import, no real ads, no banners/interstitials in MVP, and future rewarded ads only as optional convenience/cosmetic/economy value after fake-service, privacy, config, and Yusuf approval gates. | Monetization / Ads Pipeline Lead |
