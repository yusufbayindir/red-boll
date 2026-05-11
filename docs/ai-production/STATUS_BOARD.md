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

## Process

- Source of truth lives in task MDs, meeting MDs, decision logs, and this status board.
- PM/moderator owns grooming, prioritization, debate moderation, acceptance criteria, and closure notes. PM does not directly implement dashboard UI/runtime or product code.
- Specialists must challenge weak proposals and defend their role-based judgment; automatic agreement is not acceptable.
- Debate is capped at 4 rounds. If no agreement exists after round 4, PM documents a decision or asks Yusuf.
- Agent limit handling requires a final agent comment, written handoff, closure/replacement note, and task/meeting MD update before a new role inherits the work.
- Dashboard chat/grooming visibility is part of TASK-0015 process: dashboard should surface current tasks, agent state, recent meeting/debate notes, handoffs, and PM decisions while task/meeting docs remain source of truth.

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
| TASK-0014 | Marketing Production Assets | In Progress | PM + Marketing Production |
| TASK-0015 | Live Agent Dashboard System | In Progress - Data Layer Ready | PM + Live Dashboard UX + Agent State Recorder |

## Decision Log

| Date | Decision | Owner |
| --- | --- | --- |
| 2026-05-11 | Use strict task MD + QA + repo controller flow for implementation. | PM |
| 2026-05-11 | Do not keep growing the single runtime file without a modularization plan. | PM |
| 2026-05-11 | Product direction candidate is mastery-first: badges now, Trickline-style contracts later. | PM |
| 2026-05-11 | Checkpoints are a prerequisite for longer post-13 levels. | PM |
| 2026-05-12 | Live dashboard data source uses versioned local JSON with explicit agent statuses and dashboard-ready event/task/decision/artifact collections. | Agent State Recorder |
| 2026-05-12 | Adopt company-like AI production operating model with grooming templates, 4-round debate, agent limit handoff, and overnight run protocol. | Product Ops / Grooming Process Lead |
