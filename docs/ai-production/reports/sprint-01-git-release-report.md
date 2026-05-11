# Sprint 01 Git Release Report

Owner: Git / Release Executor  
Workspace: `/Users/yusufbayindir/Desktop/ai game/red_ball`  
Date: 2026-05-12  
Branch: `main`  
Remote: `origin` -> `https://github.com/yusufbayindir/red-boll.git`

## Verdict

Safe release staging completed for Sprint 01 gameplay loop, Unity smoke tests, dashboard handoff, and production docs.

## Staged Scope

- Unity project source required for a fresh checkout/open: `Assets/`, `Packages/`, `ProjectSettings/`.
- Sprint 01 EditMode smoke evidence: `TestResults/editmode-smoke-results.xml`.
- Dashboard launcher tracked inside the repo: `open-ai-production-dashboard.command`.
- AI production dashboard/docs/task/report/debt/status changes under `docs/ai-production/`.
- Repo hygiene update: `.gitignore` now ignores generated TestResults output except the committed Sprint 01 smoke XML.

## Explicitly Excluded

- Generated Unity/runtime folders: `Library/`, `Temp/`, `Obj/`, `Builds/`, `Logs/`, `UserSettings/`.
- Runtime logs: `Logs/editmode-smoke.log`, `Logs/shadercompiler-UnityShaderCompiler-0.log`, and `docs/ai-production/dashboard/dashboard-server.log`.
- macOS metadata: `.DS_Store` files.
- Raw/downloaded asset holding folders pending asset-pipeline approval: `kenney_game-icons-expansion/`, `red-ball-assets/`.
- Root `package.json`, because it is not needed for the Unity project, dashboard runtime, or Sprint 01 release scope.
- Parent launcher outside this repository: `/Users/yusufbayindir/Desktop/ai game/open-ai-production-dashboard.command`. It cannot be pushed from this repo; the tracked equivalent is `open-ai-production-dashboard.command`.

## Test Evidence Decision

`TestResults/editmode-smoke-results.xml` is committed because it is small, stable Sprint 01 evidence referenced by TASK-0022 and STATUS_BOARD, with 6/6 passing EditMode smoke tests. Broader Test Runner output remains ignored to avoid committing generated evidence churn.

`Logs/editmode-smoke.log` is not committed. The log path remains local evidence only; the committed XML and reports are the durable repo evidence.

## Pre-Commit Review

- `git status --short --ignored` was reviewed before staging.
- Staging used explicit paths only; `git add .` was not used.
- Generated/large Unity folders were not staged.
- `git diff --cached --stat` and `git diff --cached --name-only` were reviewed before commit.

## Push Result

- Primary release commit: `5d7ee44` (`Add Sprint 01 gameplay loop and dashboard handoff`).
- Push target: `origin/main`.
- Push result: success, `9db5292..5d7ee44 main -> main`.
- Remote link: `https://github.com/yusufbayindir/red-boll/tree/main`.
- Follow-up report/status commit may appear after `5d7ee44` only to record this completed push result in-repo.

## Technical Debt

Technical debt added: none.
