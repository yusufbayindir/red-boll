# Red Ball AI Production Operating Model

## Purpose

Run Red Ball like a small production company: clear roles, explicit meetings, written task ownership, visible decisions, and disciplined handoff between agents.

PM/moderator does not write product code. PM owns direction, grooming, prioritization, meeting discipline, acceptance criteria, and closure.

## Core Roles

| Role | Primary responsibility | Output |
| --- | --- | --- |
| PM / Moderator | Product direction, grooming, debate moderation, task priority, acceptance decisions | Task briefs, meeting notes, decisions, closure notes |
| Grooming Lead | Turns rough ideas into scoped tasks with acceptance criteria and owners | Grooming meeting notes, task intake records |
| Developer | Implements approved code tasks only after task MD is ready | Code changes, implementation notes, verification notes |
| QA | Tests behavior against acceptance criteria and blocks unsafe closure | QA notes, pass/fail/accepted-with-risk status |
| Asset | Finds, validates, imports, or specifies art/audio assets with license notes | Asset reports, manifests, import plans |
| Marketing | Creates store, launch, screenshot, video, and positioning artifacts | Marketing docs, launch checklists, copy, scripts |
| Git / Release | Controls repo hygiene, release readiness, changed file summaries, and rollback notes | Git readiness reports, release notes, repo status |
| Dashboard | Maintains production visibility, agent/task status, event history, and cockpit usability | Dashboard task/data docs and UI implementation evidence |
| Technical Debt Registrar | Maintains the debt register, entry template, and debt policy across process docs | Technical debt register entries, template updates, process reminders |

Specialist roles must challenge weak ideas. They are not expected to agree with PM or each other by default.

## Operating Rhythm

| Rhythm | Owner | Cadence | Required artifact |
| --- | --- | --- | --- |
| Intake | PM + Grooming Lead | Whenever a new request appears | `templates/task-intake-template.md` copied into task or meeting notes |
| Grooming | PM + relevant roles | Before any meaningful implementation | `meetings/grooming-meeting-template.md` |
| Specialist debate | PM + affected roles | When direction, risk, or tradeoff is unclear | Debate notes in meeting/task MD |
| Implementation | Developer / Asset / Marketing / Dashboard owner | After task is accepted for work | Task MD activity log |
| QA review | QA | Before closure | QA section in task MD |
| Git/release review | Git / Release | Before public release or risky merge | Repo/release note |
| Daily run | PM | Start or end of work block | `templates/daily-overnight-production-run-template.md` |
| Overnight run | PM + delegated roles | Long autonomous work block | Overnight plan, limits plan, morning handoff |

## Technical Debt Policy

Technical debt is any accepted shortcut, deferred verification, manual workaround, unclear provenance, temporary process, untracked release dependency, or known complexity that future work must revisit.

The source of truth is `technical-debt/TECHNICAL_DEBT_REGISTER.md`. Use `templates/technical-debt-entry-template.md` for new entries.

Required fields for every debt entry:

- ID
- date
- source task / agent
- category
- why it became debt
- risk
- user impact
- reversal cost
- owner role
- review trigger
- closure criteria
- status

Every role can create debt, including Developer, QA, Asset, Marketing, Dashboard, Git / Release, PM, and process roles. PM does not hide or silently absorb debt; PM records process debt and routes implementation debt to the correct owner.

Worker final reports must include exactly one technical debt line:

- `Technical debt added: TD-####` when new debt was added.
- `Technical debt added: none` when no new debt was introduced or discovered.

A task cannot be cleanly closed unless its debt line is present. If debt remains open, PM may still accept the task only as accepted with documented risk, with the debt ID linked from the task, status board, or final report.

## Task Lifecycle

1. Intake: capture the request, owner, expected artifact, exclusions, and open questions.
2. Grooming: define scope, acceptance criteria, risks, dependencies, and required roles.
3. Debate: if roles disagree, run the 4-round debate rule.
4. Approval: PM marks the task ready for implementation or research.
5. Execution: assigned owner updates task MD before, during, and after work.
6. QA: QA verifies against acceptance criteria and records result.
7. Debt check: assigned owner records new debt in the register or states that no debt was added.
8. Git/release: Git / Release records repo state when relevant.
9. Closure: PM closes only after acceptance criteria, QA, technical debt line, and required notes are complete.

## Debate And Decision Rule

No role should answer with automatic agreement. A useful agent must defend its professional judgment.

4-round rule:

1. Round 1: each role states its recommendation and the strongest reason.
2. Round 2: each role responds to the strongest opposing argument.
3. Round 3: each role may offer a compromise, but only if product quality remains strong.
4. Round 4: each role gives a final position and says what evidence would change its mind.

If there is still no agreement after round 4, PM either makes a documented decision or asks Yusuf. If the decision affects cost, product identity, release timing, monetization, player trust, or broad architecture, PM should ask Yusuf instead of forcing a decision.

Every unresolved debate must leave a note in the meeting MD, task MD, or decision log.

## Agent Limit Management Protocol

When the environment reaches an active agent limit, PM must preserve continuity before closing any agent.

Required steps:

1. Ask the current agent for a final structured handoff:
   - current status
   - files touched or intended
   - decisions made
   - unresolved questions
   - risks
   - recommended next owner
2. Record the handoff in the relevant meeting MD or task MD.
3. Mark the agent as closed, paused, or replaced in the task/status notes.
4. Start the new role with the handoff, current task MD, acceptance criteria, and any constraints.
5. Add an activity log line naming the previous agent, new agent/role, reason for transfer, and timestamp.
6. If dashboard data is maintained for the run, update the dashboard-visible status source or note the dashboard update requirement in the task MD.

Agents must not be silently replaced. The production record should explain why the old agent stopped and what the new agent inherited.

## Dashboard And Grooming Process Link

The dashboard is the production cockpit, not the source of truth by itself. Source-of-truth records remain task MDs, meeting MDs, decision logs, and status docs.

Dashboard chat/grooming requirements:

- The dashboard should surface current task status, active agent state, latest meeting/debate notes, handoffs, and PM decisions.
- Grooming outcomes should be linked from dashboard-visible task rows when practical.
- Dashboard UI/runtime implementation remains owned by the dashboard role, not PM.
- PM may update process docs and task requirements for dashboard visibility but should not implement dashboard code.

This process is tied to `TASK-0015 - Live Agent Dashboard System`.

## Overnight Production Mode

Overnight work is allowed only with a written run plan:

- goal for the block
- active roles
- tasks in scope
- tasks explicitly out of scope
- agent limit strategy
- checkpoints where PM should stop and ask Yusuf
- expected morning summary format

Any overnight run must optimize for recoverability. If an agent hits uncertainty, role conflict, missing access, or high-risk implementation, it should document the blocker and pause that branch instead of guessing silently.

## Closure Criteria For Company-Like Operation

The operating model is ready when:

- Each major role has a clear responsibility and output.
- Grooming has a reusable meeting template.
- New work can enter through a reusable task intake template.
- Decisions can be recorded in a consistent decision log.
- Daily and overnight production runs have a reusable template.
- Agent limit handoff is documented before replacement.
- Technical debt is recorded in the register with owner, trigger, and closure criteria.
- The 4-round debate rule is explicit and prevents automatic agreement.
- Dashboard chat/grooming visibility is connected to TASK-0015 or status process.
- PM boundaries are clear: PM directs and accepts; specialists implement.
