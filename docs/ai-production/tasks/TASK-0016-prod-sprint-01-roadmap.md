# TASK-0016 - Prod Sprint 01 Roadmap

## Status

Open - Groomed for Sprint 01

## Owner

Product Grooming Lead + PM Moderator. PM owns scope, sequencing, acceptance criteria, debate notes, and closure. PM does not implement product code.

## PM Intent

Move Red Ball from prototype production flow toward a production-ready launch candidate through a sequenced Sprint 01 roadmap: ops/dashboard first, then differentiating gameplay, content, assets, QA, monetization/ad boundary, marketing, and git/release readiness.

## Sprint 01 North Star

Ship a credible production path for `Red Ball: Trickline` or its safer naming alternative: a compact, readable physics platformer about clean rolling routes, fair retries, and replayable mastery. Sprint 01 should avoid fake launch claims, broad art churn, and SDK imports before runtime boundaries and QA gates exist.

## Scope

In:

- Dashboard/ops closure and relay/handoff process.
- Gameplay differentiation through mastery/Trickline-compatible systems.
- Level content path for Levels 14-20 with checkpoint-first sequencing.
- Curated asset pipeline with provenance before runtime import.
- QA gates for gameplay, UI/dashboard, content, ads boundary, marketing, and release.
- Monetization/ad pipeline architecture using fake service before SDK integration.
- Marketing launch artifacts tied to real gameplay captures only.
- Git/release readiness and first safe repository push path.

Out:

- Direct PM implementation in runtime or dashboard code.
- Real ad SDK import in Sprint 01 unless fake-service QA passes and Yusuf approves provider choice.
- Store claims for bosses, hundreds of levels, daily challenges, skins, leaderboards, or mastery badges before visible implementation.
- Full art replacement or broad tileset swap.
- `git add .`, broad Unity generated file staging, or first push before remote spelling and staged set are approved.

## Roadmap Sequence

| Order | Workstream | Sprint 01 Outcome | Why Now |
| --- | --- | --- | --- |
| 1 | Dashboard / Ops | TASK-0015 closure, live dashboard source-of-truth links, handoff protocol visible. | Production coordination must be reliable before parallel work scales. |
| 2 | Gameplay Differentiation | First playable mastery slice: clear/all-coins/clean-run or Trickline contract foundation. | The game needs an identity stronger than generic red-ball levels. |
| 3 | Level Content | Levels 14-16 playable with checkpoint and warning-sign readability; Levels 17-20 remain gated by QA. | Content should teach new mechanics before scaling volume. |
| 4 | Asset Pipeline | Approved download/import subset, licenses/checksums/manifest updated, curated runtime import plan. | Visual feedback supports mastery, but provenance must precede shipping assets. |
| 5 | QA | Regression checklist, mobile fairness gates, dashboard/relay gates, release smoke checklist. | Production readiness is defined by repeatable evidence, not optimistic handoff. |
| 6 | Monetization / Ad Pipeline | Fake rewarded-heart service and policy boundary designed or implemented behind config; no SDK leakage. | Monetization must not corrupt gameplay fairness or privacy posture. |
| 7 | Marketing | Store copy, ASO, screenshots/video plan using only real implemented features. | Launch positioning should match the actual build. |
| 8 | Git / Release | Repo initialized/pushed only after staged set approval; release candidate checklist established. | Release work is blocked until source control is safe and intentional. |

## Sprint 01 Task Breakdown

| Task | Owner Role | Acceptance Criteria | Dependencies | Risk | QA Gate | Technical Debt |
| --- | --- | --- | --- | --- | --- | --- |
| S01-01 Dashboard/Ops Closure | Realtime Dashboard Engineer + Product Ops | TASK-0015 remains QA Pass or receives a closure note; dashboard links to source-of-truth task/meeting docs; relay message queue behavior is documented; active handoff/agent-limit signals are visible. | TASK-0015, `OPERATING_MODEL.md`, `STATUS_BOARD.md`. | Dashboard can look complete while real dispatch remains a manual queue. | Run dashboard relay smoke: `/api/health`, `/api/state`, `/api/events`, `POST /api/messages`; confirm docs links resolve. | Reference TD-0001, TD-0002, TD-0007, TD-0008 in `TECHNICAL_DEBT_REGISTER.md`. |
| S01-02 Gameplay Differentiation Slice | Gameplay Developer + Game Design Differentiation | One implemented mastery-friendly slice is selected and scoped: Clear, All Coins, Clean Run, or first Trickline contract; player-visible feedback exists; no marketing claim exceeds implementation. | TASK-0004, TASK-0009, TASK-0010, architecture split status. | Clean-run/contract systems may expose inconsistent physics or weak level design. | Editor play smoke for success/failure states, save/load, replay, and no regression in existing 13 levels. | Reference TD-0006 for play-mode QA; add new debt only if contract logic expands monolithic runtime without closure plan. |
| S01-03 Checkpoint-First Level Content | Level Designer + Gameplay Developer | Levels 14-16 are built or implementation-ready with checkpoint support, warning signs, vertical lifts, and mobile-fair teaching beats; Levels 17-20 remain design-approved but not forced into Sprint 01 if QA fails. | TASK-0005, TASK-0010, S01-02 if mastery feedback is required on completion. | Adding long levels before checkpoint reliability creates frustration and test noise. | Mobile fairness pass: first exposure safe, checkpoint spacing 25-35 world units after Level 13, no surprise hazards, touch controls remain readable. | Reference TD-0006 for play-mode QA; add level-authoring duplication debt only if helpers are copied instead of consolidated. |
| S01-04 Asset Pipeline Execution | Asset Pipeline + UI/Effects Developer | Approved raw asset subset is downloaded/staged with source URL, license, original zip, and checksums; `assets-manifest.md` updated; runtime import subset is curated and minimal. | TASK-0013, asset download plan, PM approval of exact subset. | Broad imports create style churn, repo bloat, and unclear provenance. | Verify license files/checksums; confirm no unapproved CC-BY/NC assets; runtime smoke only after implementation task imports selected files. | Reference TD-0005 until every shipped asset has manifest provenance. |
| S01-05 QA Systemization | QA Lead | QA checklist maps to Sprint 01 workstreams; each implementation task has compile/play-mode smoke, mobile readability checks, regression notes, and rollback idea. | TASK-0015 QA reports, `qa-checklist.md`, all implementation tasks. | QA can become a final ceremony instead of a blocking production gate. | No task closes without verification notes and QA verdict; failures reopen the owning task instead of being hidden in summaries. | Reference TD-0006 until play-mode smoke evidence is recorded for affected scenes/features. |
| S01-06 Monetization / Ad Boundary | Ads Pipeline Engineer + Gameplay Economy Developer | Rewarded-heart product is fake-service-first; SDK types do not enter gameplay; caps/cooldowns/idempotent grants are specified; no interstitials or death-pressure ads. | TASK-0006, heart/economy boundaries, privacy decision. | Monetization can damage trust if tied to failure pressure or level unlocks. | Fake service tests for completed/no-fill/skipped/failed/double-tap/cap/cooldown; game remains playable when ads unavailable. | Add a new register entry if real provider/privacy work is deferred after fake-service implementation starts. |
| S01-07 Marketing Production | Marketing Producer + Capture QA | Store copy, ASO metadata, screenshot storyboard, and short video scripts are aligned to implemented features; first screenshots use real gameplay captures only. | TASK-0007, TASK-0014, S01-02/S01-03/S01-04 visibility. | Marketing may promise future mastery before it is in game. | Capture QA rejects fake UI, unsupported claims, unreadable mobile controls, or screenshots from unapproved builds. | Reference TD-0005 for asset provenance and TD-0006 for unverified runtime captures. |
| S01-08 Git / Release Control | Git Repo Controller + Release Manager | Remote spelling/empty state confirmed with Yusuf; safe staged set approved; first commit/push avoids generated Unity outputs; release checklist created. | TASK-0008, PM/Yusuf approval. | Wrong remote name or broad staging can create bad first history. | `git diff --cached --name-only` reviewed before commit; excludes `Library/`, `Builds/`, `.DS_Store`, generated IDE files, and mobile package outputs. | Reference TD-0003 and TD-0004 until launcher/source-control release scope is resolved. |

## Today / Overnight Work Mode

Parallel tonight:

- S01-01 Dashboard/Ops closure can run with S01-05 QA systemization because both are docs/process plus dashboard verification.
- S01-04 Asset Pipeline provenance work can run in parallel after PM approves the exact subset; it should not wait for level implementation.
- S01-07 Marketing copy/storyboard can proceed using current truthful claims, but final screenshots/video are blocked on visible gameplay/content changes.
- S01-08 Git readiness can prepare checks and staged-set review in parallel, but must not push without Yusuf confirmation.

Blocking sequence:

- S01-02 gameplay differentiation blocks any marketing claim about mastery/Trickline rewards.
- S01-03 post-13 level content blocks on checkpoint reliability from TASK-0010 and must start with Levels 14-16, not all 14-20.
- S01-06 real ad SDK work blocks on fake-service QA, consent/privacy decision, and Yusuf/provider approval.
- Release candidate blocks on QA verdicts, git initialization/push approval, and truthful marketing assets.

## Debate / Challenge Policy

- Role agents must challenge weak scope, fake launch claims, broad imports, and monetization that pressures failed players.
- "Aynen haklisin" style automatic agreement is not accepted as a grooming outcome.
- Debate is capped at 4 rounds. If the owner roles cannot resolve after round 4, PM either records a decision or asks Yusuf with a concrete tradeoff.

## Decisions To Ask Yusuf If Unresolved After 4 Rounds

- Product name: `Red Ball: Trickline` vs `Trickline Ball` vs a safer title.
- Whether Sprint 01 must include Levels 17-20 or stop at a QA-proven Levels 14-16 slice.
- Whether monetization can enter Sprint 01 as fake-service implementation only or must wait until after gameplay differentiation.
- Whether the first git remote is intentionally `red-boll.git` and empty.
- Whether the product is child-directed for ad/privacy configuration.

## Technical Debt Register

Technical debt source of truth is `docs/ai-production/technical-debt/TECHNICAL_DEBT_REGISTER.md`. Sprint 01 references existing entries TD-0001 through TD-0008 where applicable. No new debt entry was added by this grooming task.

## Activity Log

- 2026-05-12 - Created Sprint 01 production roadmap from existing dashboard, gameplay differentiation, level design, asset pipeline, QA, ads, marketing, and git readiness work.

## Changed Files

- `docs/ai-production/tasks/TASK-0016-prod-sprint-01-roadmap.md`
- `docs/ai-production/meetings/2026-05-12-prod-sprint-01-grooming.md`
- `docs/ai-production/STATUS_BOARD.md`

## Verification

- Documentation-only change. No code verification required.
- Verify by reading task, meeting, and status board links.

## QA Notes

Pending QA review of the roadmap as a production grooming artifact.

## Repo Controller Notes

No repo action taken. First push remains blocked by TASK-0008 approvals.

## PM Closure

Pending Sprint 01 kickoff approval.
