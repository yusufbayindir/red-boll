# Status Board

## Setup

| Item | Status | Owner | Notes |
| --- | --- | --- | --- |
| AI production docs | Complete | PM | Role, protocol, roster, and task files created. |
| Git repo binding | Open | Git Repo Controller | Local folder is not a git repo; remote exists as requested but returned no refs during initial check. |
| Runtime architecture | In progress | Architecture/Developers | First split completed: `RedBallPlayer` extracted. |
| QA workflow | Complete | QA | QA checklist created; code tasks require compile and play-mode smoke evidence. |
| Agent dashboard | In progress - post-PlayMode roster synced | PM + Live Dashboard + Agent State Recorder | `dashboard/data/dashboard.json` now has versioned polling fields, stable statuses, events, snippets, task rows, PM decisions, artifact links, and the latest Euclid/Hilbert/McClintock/Locke/Confucius roster sync. Manual state sync remains covered by TD-0007 and TD-0008. |
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
| TD-0012 | iOS Simulator build verification is blocked because the generated Unity Xcode target exposes `iphoneos` only and no eligible simulator destination; TASK-0027 recommends keeping Sprint 01 device-only/open and deferring simulator SDK export remediation to Sprint 02 unless simulator evidence becomes a release gate. | iOS Build / Unity Export Owner | Before simulator baseline evidence, before release-candidate build verification, or whenever the Unity iOS workspace is regenerated |
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
| TASK-0008 | Git repo control setup | Pushed to origin/main + docs follow-up pushed | Git Repo Controller + Release Manager + Wegener |
| TASK-0009 | Mastery Badge MVP | Draft | PM |
| TASK-0010 | Checkpoints and warning signs | Implementation Complete - QA Pending | PM |
| TASK-0011 | Agent Dashboard Site | Accepted | PM + Dashboard Site Developer |
| TASK-0012 | RedBall UI Bridge First Slice | Implementation Complete - QA Pending | PM + Developer UI/Polish |
| TASK-0013 | Asset Pipeline And Downloads | In Progress | PM + Asset Pipeline |
| TASK-0014 | Marketing Production Assets | Aligned - Capture Gated | PM + Marketing Production |
| TASK-0015 | Live Agent Dashboard System | QA Pass - Post-PlayMode Roster Synced | PM + Live Dashboard UX + Agent State Recorder |
| TASK-0016 | Prod Sprint 01 Roadmap | Open - Groomed for Sprint 01 | Product Grooming Lead + PM |
| TASK-0019 | Sprint 01 Unity Implementation | QA PARTIAL - Compile Pass / Play-mode Pending | Gameplay Integration + UI/Badge + Mechanic Component + QA |
| TASK-0021 | Sprint 01 QA Playtest | QA PARTIAL - Compile/EditMode/PlayMode Smoke PASS / Manual-Mobile Pending / iOS Simulator BLOCKED TD-0012 | QA Scenario Designer / QA Lead |
| TASK-0022 | Unity Play/EditMode Smoke Harness | Implementation Complete - EditMode Smoke PASS | Unity Test Harness Developer | `TestResults/editmode-smoke-results.xml` has 6/6 passing tests for LevelCount, badge mask helper, UI badge summary, and Level 1/14/15 load construction. |
| TASK-0023 | Ads Monetization Pipeline | Completed - Ads Deferred / No SDK Import | Monetization / Ads Pipeline Lead |
| TASK-0024 | Sprint 01 Manual Playtest / Mobile QA | Completed - 25 MQA scenarios ready / xcodebuild -list PASS | Manual Playtest / Mobile QA Planner |
| TASK-0025 | iOS Workspace Build Verification | Completed - workspace/list PASS / simulator build BLOCKED TD-0012 | iOS Workspace Build Verifier |
| TASK-0026 | PlayMode Traversal Smoke | Completed - PlayMode Smoke PASS 5/5 | PlayMode Traversal Automation Developer | `TestResults/playmode-traversal-smoke-results.xml` has 5/5 passing PlayMode tests for Level 1/14/15 object contracts, Level 14 lift motion, Level 15 crumble collapse, Level 14 goal completion badges, and badge PlayerPrefs roundtrip. TD-0006 remains open for real route/mobile QA. |
| TASK-0027 | iOS Simulator Build Strategy | Completed - Sprint 02 simulator SDK export recommended | iOS Simulator Build Strategy Engineer |
| TASK-0031 | Open Source Model Setup Research | Completed - local model install deferred | Open Source Generative Model Researcher | Report recommends imagegen/manual-vector first, optional local R&D only after license, hardware, disk, and provenance gates. SDXL is the safest first local candidate; SD3 Medium and FLUX.1 dev are not recommended for production without separate permissions. Report: `reports/open-source-image-model-options.md`. |
| TASK-0029 | AI Asset Generation Pipeline | Open - Brief Ready | AI Asset Pipeline Product Owner + Asset Production |
| TASK-0030 | Ad Creative Generation Pipeline | Open - Brief Ready | AI Asset Pipeline Product Owner + Marketing Production |
| TASK-0033 | Runtime Generated Asset Pack | Implementation Complete - Smoke PASS | Runtime Asset Producer / Unity Integrator | Eight generated Sprint02 PNGs are imported under `Assets/Resources/Generated/Sprint02/` and used in runtime UI/gameplay: main menu and level select mastery badges, Level 14 lift overlay, Level 15 crumbling tiles, warning/checkpoint/dust polish. Unity import/compile PASS, EditMode 6/6 PASS, PlayMode 5/5 PASS. Report: `reports/runtime-generated-asset-pack-report.md`. |
| TASK-0034 | Dashboard Button QA And Fixes | Fix Complete - Awaiting PM Review | Dashboard Web QA Runner + Dashboard Button Fix Developer |
| TASK-0035 | Existing Level Polish Pass | Implementation Complete - Smoke PASS | Existing Level Art Polish Developer | Level 1-13 now show Sprint02 generated polish: route `dust_sparkle`, hazard `warning_spark`, existing `checkpoint_spark` on Level 13, and `lift_platform_polish` on existing moving platforms. Unity import/compile PASS, EditMode 19/19 PASS, PlayMode 5/5 PASS. Report: `reports/existing-level-polish-pass-report.md`. |
| TASK-0036 | Combined Runtime Dashboard QA | Completed - PASS | Combined QA Runner - Runtime Assets + Dashboard Fix | Combined no-code QA passed for Sprint02 runtime assets, Level 1-13 polish evidence, Unity smoke evidence, dashboard syntax/HTTP routes, traversal blocking, Codex handoff separation, Build Workspace dry-run, and clean outbox/events-live data. Report: `reports/combined-runtime-dashboard-qa-2026-05-12.md`. |

## Sprint 01

| Order | Workstream | Status | Owner | Notes |
| --- | --- | --- | --- | --- |
| 1 | Dashboard / Ops | QA Pass - post-PlayMode roster synced | Realtime Dashboard Engineer + Product Ops + Dashboard State Sync Maintainer | TASK-0015 data now shows Euclid, Hilbert, McClintock, Locke, and Confucius after PlayMode/iOS follow-up push `1238284`. Manual roster sync remains existing debt under TD-0007 and TD-0008. |
| 2 | Gameplay Differentiation | Groomed / blocked on slice selection | Gameplay Developer + Game Design Differentiation | Choose first mastery/Trickline-compatible slice before marketing claims. |
| 3 | Level Content | Groomed / checkpoint-first | Level Designer + Gameplay Developer | Levels 14-16 first; Levels 17-20 gated by QA. |
| 4 | Asset Pipeline | Groomed / awaiting subset approval | Asset Pipeline | Stage approved assets with source URLs, licenses, zips, and checksums before runtime import. |
| 5 | QA | QA PARTIAL - compile pass, EditMode smoke pass, PlayMode smoke pass, manual/mobile play pending; iOS simulator build BLOCKED / Needs Config | QA Lead + PlayMode Traversal Automation Developer + iOS Workspace Build Verifier + iOS Simulator Build Strategy Engineer | Compile/import gate passed on 2026-05-12; TASK-0022 added 6/6 passing EditMode smoke XML for Level 1/14/15 construction. TASK-0026 added 5/5 passing PlayMode XML for Level 1/14/15 object contracts, Level 14 lift motion, Level 15 crumble collapse, Level 14 goal completion badge persistence, and badge PlayerPrefs roundtrip. TASK-0024 now provides the ready-to-run manual/mobile script and confirms the existing Xcode workspace is readable. TASK-0025 confirms `xcodebuild -list` PASS but simulator build is blocked before compile because `Unity-iPhone` has no eligible iOS Simulator destination and reports `SUPPORTED_PLATFORMS = iphoneos`. TASK-0027 recommends keeping the current device-only workspace open for Sprint 01 and deferring Unity Simulator SDK re-export to Sprint 02 unless PM/Yusuf require simulator evidence as a release gate. Full safe-route/mobile traversal evidence remains pending under TD-0006; simulator config debt is tracked under TD-0012. |
| 6 | Monetization / Ad Pipeline | Proposed / Ready for PM Review | Monetization / Ads Pipeline Lead | TASK-0023 recommends no real ads in Sprint 01, no banners/interstitials in MVP, fake-service-first SDK boundary, and optional rewarded ads only for future revive/cosmetic/currency value after privacy/config approval. No retry, badge, Clean Run, All Coins, checkpoint, or mastery progress monetization. |
| 7 | Marketing | Aligned / capture gated | Marketing Producer + Capture QA | Store copy, ASO, storyboard, video scripts, and checklist now reflect Sprint 01 mastery badges, Clean Run, skill route, Level 14 lift, and Level 15 crumbling tile. Public claims require real-play verification before launch while TD-0006 remains open. Ads pipeline claims remain pending. Report: `reports/marketing-sprint-01-alignment.md`. |
| 8 | Git / Release | Pushed to origin/main + post-PlayMode follow-up pushed | Git Repo Controller + Release Manager + Wegener + Confucius | Primary Sprint 01 release commit `5d7ee44`, docs follow-up push `5811a55`, and post-PlayMode/iOS strategy push `1238284` are recorded. Scope is constrained to Unity source/tests/project settings, dashboard/docs, tracked launcher, and committed smoke XML. Generated Unity folders, logs, raw asset holding folders, `.DS_Store`, and parent launcher remain excluded. Report: `reports/sprint-01-git-release-report.md`. |
| 9 | AI Asset / Ad Creative Pipeline | Brief ready / first concept board archived / production gated | AI Asset Pipeline Product Owner + Asset Production + Marketing Production + Capture QA | TASK-0029/TASK-0030 define generated/manual/vector asset strategy, first batch, ad still/video guardrails, export targets, and manifest/provenance schema. First ChatGPT/imagegen creative board is archived at `generated-assets/2026-05-12/red-ball-creative-board-v1.png` with MD/JSON provenance and manifest entry; no Unity import, final production art, store asset, or paid creative approval performed. Generated creative automation/provenance enforcement is tracked under TD-0013. Report: `reports/ai-asset-generation-brief.md`. |

Implementation queue note: `TASK-0019` is the Sprint 01 Unity implementation handoff. `Assets/Scripts/RedBallRuntime.cs` should have one Gameplay Integration Owner during the task; badge UI helpers and crumbling tile behavior can be split into `RedBallUi.cs` and a new component file after the runtime model is fixed.

QA queue note: `TASK-0021`, `TASK-0022`, `TASK-0024`, `TASK-0025`, `TASK-0026`, `TASK-0027`, `reports/sprint-01-qa-test-plan.md`, `reports/sprint-01-qa-run-2026-05-12.md`, `reports/unity-playmode-smoke-harness.md`, `reports/playmode-traversal-smoke.md`, `reports/sprint-01-manual-playtest-plan.md`, `reports/ios-workspace-build-verification.md`, and `reports/ios-simulator-build-strategy.md` define the current QA state. Compile/import passed, static blocker review found no confirmed blocker, TASK-0022 provides EditMode smoke XML for Level 1/14/15 construction, and TASK-0026 provides PlayMode smoke XML for Level 1/14/15 object contracts plus Level 14 lift, Level 15 crumble, goal completion badge persistence, and badge PlayerPrefs roundtrip. TASK-0024 adds the ready-to-run Level 14/15, badge, checkpoint, crumble, and iOS/mobile manual script, plus static confirmation that `Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace` is readable by `xcodebuild -list`. TASK-0025 reran `xcodebuild -list` successfully but records iOS Simulator build as BLOCKED / Needs Config before compile because the scheme has no eligible simulator destination. TASK-0027 recommends PM keep Sprint 01 device-only/open and move simulator SDK export remediation to Sprint 02 unless simulator evidence is promoted to a release gate. Manual/mobile safe-route traversal evidence remains required before Sprint 01 PASS. Bugs block `TASK-0021`/`TASK-0024` and return to the owning implementation task; accepted limitations must be registered as technical debt before closure.

Marketing queue note: Sprint 01 public claims are gated on real-play/capture QA. Use `reports/marketing-sprint-01-alignment.md`, `marketing/screenshot-storyboard-v1.md`, `reports/creative-asset-prompt-pack-v1.md`, and `marketing/ad-creative-storyboards-v1.md` for the preferred 5-frame set, first AI generation prompts, six ad still concepts, and three short video storyboards. Do not make ads, rewarded recovery, ad-free, fake gameplay, or monetization claims while the ads pipeline is pending.

AI asset/ad creative queue note: `TASK-0029`, `TASK-0030`, and `reports/ai-asset-generation-brief.md` define the next production handoff for generated and hand-made creative. First batch candidates are mastery badge icons, vertical lift polish, crumbling tile, checkpoint/warning polish, app icon/key art, store screenshots, and ad stills. `RB-AI-20260512-001` is archived as a concept board only, not final/imported art. Public or paid creative must be truthful to verified gameplay, avoid UGC/deceptive formats, and carry a manifest with prompt/model/license/source/seed/date/owner/usage-rights/review-status fields before approval.

Runtime generated asset note: `TASK-0033` completed the first gameplay-used generated asset pack. `Assets/Resources/Generated/Sprint02/` now contains eight original Pillow-generated PNGs plus Unity `.meta` files. Runtime integration is fallback-safe: missing generated assets fall back to existing RedBall/UI sprites. Compile/import, EditMode smoke, and PlayMode smoke all passed on 2026-05-12. Technical debt added: none; existing manual/mobile evidence remains under TD-0006.

Existing level polish note: `TASK-0035` extended Sprint02 generated asset visibility to Level 1-13 without gameplay balance changes. Level 1-13 each have route `dust_sparkle` and `warning_spark` polish, Level 13 keeps `checkpoint_spark`, and existing moving-platform levels show `lift_platform_polish`. Compile/import, EditMode 19/19, and PlayMode 5/5 passed on 2026-05-12. Technical debt added: none.

Dashboard button fix note: `TASK-0034`, `reports/dashboard-button-qa-2026-05-12.md`, and `reports/dashboard-button-fix-report.md` record the 2026-05-12 QA plus implementation follow-up. The local dashboard server now serves docs through safe `/docs/<path>` routing, dashboard task/report/artifact/root markdown links normalize to `/docs/...`, agent card selection stays transcript-only, and Build Workspace feedback remains visible in a persistent topbar inline status after the menu closes. Technical debt added: none.

Combined runtime/dashboard QA note: `TASK-0036` and `reports/combined-runtime-dashboard-qa-2026-05-12.md` record a no-code PASS over Sprint02 asset presence, Level 1-13 polish static/test evidence, TASK-0035 Unity import/EditMode/PlayMode evidence, dashboard `node --check`/`py_compile`, health/docs/traversal HTTP checks, explicit Codex and Build Workspace dry-runs, and unchanged clean dashboard outbox/events-live files. Technical debt added: none.

Sprint 01 technical debt note: source of truth is `docs/ai-production/technical-debt/TECHNICAL_DEBT_REGISTER.md`; TASK-0016 references existing TD-0001 through TD-0008 where relevant. Dashboard roster/state sync is still manual and remains covered by TD-0007 and TD-0008. TASK-0027 updated TD-0012 closure criteria but added no new debt ID. Post-PlayMode dashboard sync added no new debt ID. TASK-0033, TASK-0035, and TASK-0036 added no new debt ID.

Git release note: Sprint 01 release execution treats `TestResults/editmode-smoke-results.xml` and `TestResults/playmode-traversal-smoke-results.xml` as committed QA evidence and leaves local logs unpushed. Primary release push was `5d7ee44`; docs follow-up push was `5811a55`; post-PlayMode/iOS strategy push was `1238284`. Parent `/Users/yusufbayindir/Desktop/ai game/open-ai-production-dashboard.command` is outside the repo and cannot be pushed; repo launcher is `open-ai-production-dashboard.command`.

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
| 2026-05-12 | Dashboard roster synced after Sprint 01 docs follow-up: James, Arendt, Hume, and Wegener are visible in dashboard state; manual sync remains covered by TD-0007 and TD-0008. | Dashboard State Sync Maintainer |
| 2026-05-12 | Recommended Sprint 01 iOS build strategy: keep current device-only Xcode workspace open, use physical-device QA if available plus Editor/CI smoke evidence, and defer Unity Simulator SDK re-export to Sprint 02 under TD-0012 unless PM/Yusuf make simulator evidence a release gate. | iOS Simulator Build Strategy Engineer |
| 2026-05-12 | Dashboard roster synced after PlayMode/iOS follow-up: Euclid, Hilbert, McClintock, Locke, and Confucius are visible in dashboard state; timeline records PlayMode smoke 5/5 PASS, iOS simulator BLOCKED / TD-0012, and push commit `1238284`. Manual sync remains covered by TD-0007 and TD-0008. | Dashboard State Sync Maintainer |
| 2026-05-12 | Open-source image model setup decision: do not download large models now; use ChatGPT/imagegen plus manual/vector production assets first. Optional local R&D starts with SDXL in ComfyUI/diffusers only after hardware, disk, license, and TD-0005 provenance gates are accepted. | Open Source Generative Model Researcher |
| 2026-05-12 | AI asset and ad creative production may use manual/vector work, ChatGPT image generation, or local open-source generation, but every output requires manifest/provenance review and no public creative may show fake gameplay, fake UI, or unverified mechanics. | AI Asset Pipeline Product Owner |
