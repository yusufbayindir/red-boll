# Prod Sprint 01 Grooming

## Meeting

- Date: 2026-05-12
- Moderator: Product Grooming Lead
- Request / theme: Production Sprint 01 roadmap to take Red Ball toward release.
- Related task(s): `TASK-0016`, `TASK-0015`, `TASK-0004`, `TASK-0005`, `TASK-0013`, `TASK-0006`, `TASK-0007`, `TASK-0008`, `TASK-0014`
- Roles present: PM Moderator, Product Ops, Realtime Dashboard Engineer, Gameplay Developer, Game Design Differentiation, Level Designer, Asset Pipeline, QA Lead, Ads Pipeline Engineer, Marketing Producer, Git Repo Controller
- Status: Groomed / awaiting Sprint 01 kickoff approval.

## PM Intent

Yusuf wants the game moved toward a real production launch path. PM will own grooming, sequencing, acceptance criteria, debate rules, and closure notes, but will not directly code product/runtime/dashboard changes.

## Problem Statement

Red Ball has useful production docs, dashboard work, gameplay research, level expansion plans, asset research, ads architecture, marketing research, and git readiness notes. Sprint 01 needs to turn those pieces into a hard production sequence, with blockers and parallel work made explicit so the team does not implement flashy pieces before the core launch path is safe.

## Scope

In:

- Dashboard/ops first.
- Gameplay differentiation before broad content promises.
- Checkpoint-first level expansion.
- Asset provenance and curated imports.
- QA as a gate, not a late note.
- Fake-service-first monetization boundary.
- Marketing that only shows implemented features.
- Git/release readiness with explicit staging approval.

Out:

- Runtime or dashboard code edits by PM.
- Real SDK import without fake lifecycle proof and privacy decision.
- Broad asset replacement.
- Store claims for unimplemented systems.
- Git initialization/push without Yusuf approval.

## Proposed Task Breakdown

| Task | Owner | Output | Dependency | Priority |
| --- | --- | --- | --- | --- |
| S01-01 Dashboard/Ops Closure | Realtime Dashboard Engineer + Product Ops | TASK-0015 closure note, relay smoke, dashboard-source links | TASK-0015 | P0 |
| S01-02 Gameplay Differentiation Slice | Gameplay Developer + Game Design Differentiation | First mastery/Trickline-compatible playable slice | TASK-0004, TASK-0009, TASK-0010 | P0 |
| S01-03 Checkpoint-First Level Content | Level Designer + Gameplay Developer | Levels 14-16 production slice; 17-20 gated | TASK-0005, TASK-0010 | P0 |
| S01-04 Asset Pipeline Execution | Asset Pipeline + UI/Effects Developer | Approved downloads/import manifest/checksums | TASK-0013 | P1 |
| S01-05 QA Systemization | QA Lead | Sprint QA matrix and closure gates | Existing QA reports/checklist | P0 |
| S01-06 Monetization / Ad Boundary | Ads Pipeline Engineer + Gameplay Economy Developer | Fake rewarded-heart boundary, no SDK leakage | TASK-0006 | P1 |
| S01-07 Marketing Production | Marketing Producer + Capture QA | Store copy, ASO, capture plan, real screenshots later | TASK-0007, TASK-0014 | P1 |
| S01-08 Git / Release Control | Git Repo Controller + Release Manager | Safe first push/release checklist | TASK-0008, Yusuf approval | P0 |

## Acceptance Criteria

- `TASK-0016` contains roadmap order, owners, acceptance criteria, dependencies, risks, QA gates, and technical debt fields for each workstream.
- Meeting notes record objections and unresolved decisions without automatic agreement.
- Today/overnight parallel work and blockers are explicit.
- `STATUS_BOARD.md` has a Sprint 01 section and TASK-0016 row.
- Technical debt register references are recorded.

## Risks

| Risk | Owner | Mitigation |
| --- | --- | --- |
| Product becomes generic if content ships before differentiation. | PM + Game Design | Make gameplay differentiation P0 and block marketing claims on it. |
| Longer levels frustrate players without checkpoints. | Level Designer + QA | Build Levels 14-16 first with checkpoint spacing and warning signs. |
| Asset work creates repo bloat or mixed style. | Asset Pipeline | Stage raw downloads with provenance; import only approved subset. |
| Ads damage trust if tied to failure pressure. | Ads Pipeline | Fake-service-first rewarded hearts only; no interstitials/death prompts. |
| Marketing overclaims future features. | Marketing Producer + QA | Capture QA rejects unsupported copy/screens. |
| Git first push includes generated Unity outputs. | Git Repo Controller | Review staged files before commit; avoid `git add .`. |

## 4-Round Debate Notes

Round 1 - positions:

- Product Ops: Dashboard/ops must close first, otherwise overnight parallel work will create undocumented handoffs.
- Game Design Differentiation: A "more levels" sprint will not create launch identity; mastery/Trickline must be the product hook.
- Level Designer: Levels 14-20 are tempting, but Levels 14-16 are the safe first implementation slice because checkpoints and teaching beats need proof.
- Ads Pipeline: Monetization should not enter runtime until fake service and placement policy are testable.
- Marketing: Store work can start, but screenshots and claims must wait for implemented visible systems.
- Git Repo Controller: No release path exists until repo initialization and remote spelling are confirmed.

Round 2 - response to strongest objection:

- Objection: "If we do dashboard/ops first, the player-facing game does not move tonight."
- Response: Ops closure is short and unlocks safe parallel work. Gameplay planning can start in parallel, but implementation should not outrun QA gates and handoff records.
- Objection: "A real launch needs more levels, so build all 20."
- Response: Shipping seven weak or unfair levels hurts release quality. Levels 14-16 with checkpoints are a better production proof than a rushed 20-level count.
- Objection: "Ads are needed for production, so import the SDK now."
- Response: SDK import before consent, fake lifecycle, and gameplay boundary creates privacy and architecture debt. Fake rewarded-heart flow is the Sprint 01 ceiling unless Yusuf explicitly changes risk appetite.

Round 3 - compromise options:

- Dashboard/Ops: Close TASK-0015 and keep dispatcher gap as known deferred technical debt.
- Gameplay: Pick the smallest visible mastery loop first: Clear / All Coins / Clean Run, then evolve toward Trickline contracts.
- Levels: Implement or spec Levels 14-16 first; keep Levels 17-20 as designed backlog gated by QA.
- Assets: Download/stage approved particles/UI/audio with provenance, but import runtime subset only through implementation tasks.
- Marketing: Prepare copy/storyboards now; capture only after real build scenes exist.
- Git: Prepare exact commands and checks now; wait for Yusuf before first push.

Round 4 - final positions:

- PM Moderator: Sprint 01 order is dashboard/ops, gameplay differentiation, level content, asset pipeline, QA, monetization boundary, marketing, git/release.
- QA Lead: No workstream closes without evidence and explicit QA verdict.
- Git Repo Controller: Release remains blocked until source control is initialized safely.
- Ads Pipeline: No real SDK in Sprint 01 without Yusuf approval after fake-service QA.
- Marketing: No fake feature claims.

Decision after round 4:

- PM decision: Adopt TASK-0016 roadmap as Sprint 01 grooming source.
- Ask Yusuf? yes, only for unresolved product/release decisions listed below.
- Reason: The roadmap resolves execution order but several business/product risk choices require owner approval if specialists cannot align.

## Decisions To Ask Yusuf If Still Unresolved

- Product title direction: `Red Ball: Trickline`, `Trickline Ball`, or another safer title.
- Whether Sprint 01 success requires all Levels 14-20 or a QA-proven Levels 14-16 slice.
- Whether fake-service monetization belongs in Sprint 01 implementation or should remain architecture-only.
- Whether the GitHub remote spelling `red-boll.git` is intentional and should receive the first push.
- Whether the product should be treated as child-directed for ads/privacy posture.

## Today / Overnight Production Mode

Parallel:

- Dashboard/Ops closure with QA relay smoke.
- QA matrix creation across current tasks.
- Asset provenance preparation after exact subset approval.
- Marketing copy/storyboard cleanup using current truthful feature set.
- Git readiness review and staged-set checklist preparation.

Blocking:

- Gameplay differentiation blocks mastery/Trickline marketing claims.
- Checkpoint reliability blocks long post-13 level rollout.
- Fake ad lifecycle plus privacy/product decision blocks SDK import.
- Real screenshots block on visible gameplay/content/asset changes.
- First release candidate blocks on QA verdicts and safe git initialization.

## Technical Debt Notes

- Technical debt source of truth is `docs/ai-production/technical-debt/TECHNICAL_DEBT_REGISTER.md`.
- Sprint 01 references existing entries TD-0001 through TD-0008 where relevant.
- No new technical debt entries were added in this meeting.

## Agent Limit / Handoff Notes

- Agents to close: none in this meeting.
- Handoff received: existing TASK-0015 dashboard QA pass and prior reports.
- New role/agent: Product Grooming Lead owns TASK-0016 docs.
- Transfer note added to task MD: yes, via TASK-0016 ownership and activity log.

## Decision Log Entries To Add

- 2026-05-12: Sprint 01 production order adopted: dashboard/ops, gameplay differentiation, level content, asset pipeline, QA, monetization/ad boundary, marketing, git/release.
- 2026-05-12: Sprint 01 will use challenge/debate notes and 4-round escalation; unresolved business/product decisions go to Yusuf.
- 2026-05-12: Sprint 01 references `TECHNICAL_DEBT_REGISTER.md`; no new debt entry added by grooming.

## Follow-Ups

| Action | Owner | Due / trigger |
| --- | --- | --- |
| Confirm Sprint 01 kickoff and P0 order. | PM / Yusuf | Before implementation agents start. |
| Close or reopen TASK-0015 based on latest dashboard verification. | Realtime Dashboard Engineer + QA | Sprint 01 start. |
| Choose first gameplay differentiation slice. | PM + Game Design + Gameplay Developer | Before S01-02 implementation. |
| Confirm Levels 14-16 vs 14-20 Sprint 01 scope. | PM / Yusuf if debated | Before level implementation. |
| Approve asset download subset. | PM / Yusuf | Before raw downloads. |
| Confirm git remote spelling and first staged set. | Yusuf + Git Repo Controller | Before `git init` / push. |
