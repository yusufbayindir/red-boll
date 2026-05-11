# TASK-0008 - Git Repo Control Setup

## Status

Repo Controller readiness refresh complete - awaiting PM approval

## Owner

Git Repo Controller, PM

## PM Intent

Connect work to the requested GitHub repo safely and define what should be tracked.

## Scope

In:

- Verify local git state.
- Verify remote repo state.
- Recommend initialize-vs-clone path.
- Review `.gitignore`.
- Define include/exclude files.
- Prepare safe command sequence for PM approval.

Out:

- Destructive git actions.
- Pushing without explicit PM/Yusuf approval.
- Adding generated Unity cache/build output.

## Acceptance Criteria

- Local repo status is understood.
- Remote repo status is understood.
- Safe setup plan exists.
- `.gitignore` recommendation exists.
- Generated folders are excluded.

## Activity Log

- 2026-05-11 23:20 - Task created by PM.
- 2026-05-11 23:20 - Initial observation: current workspace is not a git repository.
- 2026-05-11 23:20 - Initial remote probe: `git ls-remote` returned success but no refs/output.
- 2026-05-11 23:25 - PM reviewed `.gitignore`; Unity cache/build folders are already excluded.
- 2026-05-11 23:25 - PM checked top-level folders; generated folders `Library`, `Temp`, `Logs`, `Builds`, and `UserSettings` exist locally and must stay out of history unless explicitly approved.
- 2026-05-11 23:29 - Git Repo Controller reviewed protocol, role brief, task, `.gitignore`, and `README.md`; intended doc change limited to this task file.
- 2026-05-11 23:29 - Git Repo Controller verified local folder is still not a git repository.
- 2026-05-11 23:29 - Git Repo Controller verified requested remote is reachable but has no advertised refs.
- 2026-05-11 23:29 - Git Repo Controller reviewed project folders and generated/cache folder sizes before preparing the setup plan.
- 2026-05-11 23:44 - PM applied safe `.gitignore` additions for memory captures, recordings, mobile packages, Xcode archives, IDE folders, and swap files. No repo init/push performed.
- 2026-05-12 - Git Repo Controller refreshed repo readiness for the AI production system push request. Workspace is still not a git repository; requested remote is reachable with no refs/output; `.gitignore` still excludes Unity generated folders and `.DS_Store` files.
- 2026-05-12 - Git Repo Controller added a readiness report at `docs/ai-production/reports/TASK-0008-git-readiness-report.md` with exact add/include guidance and safe PM-executed setup/push steps.
- 2026-05-12 - Git Release Executor began approved first release execution for the AI production dashboard/docs scope.
- 2026-05-12 - Confirmed local folder was not a git repository before execution; initialized git in place, preserving the requested empty remote target.
- 2026-05-12 - Added dashboard runtime ignores for `docs/ai-production/dashboard/*.log`, dashboard `__pycache__`, and dashboard `.pyc` files before staging.
- 2026-05-12 - Staged only `.gitignore`, `README.md`, `docs/ai-production/**`, and repo-local `open-ai-production-dashboard.command`; Unity project folders, generated folders, external asset folders, `package.json`, dashboard logs, `.DS_Store`, and pycache remained unstaged/ignored as applicable.

## Changed Files

Previously recorded by PM:

- `.gitignore`

Changed by 2026-05-12 Git Repo Controller refresh:

- `docs/ai-production/tasks/TASK-0008-git-repo-control-setup.md`
- `docs/ai-production/reports/TASK-0008-git-readiness-report.md`

Changed by 2026-05-12 Git Release Executor:

- `.gitignore`
- `docs/ai-production/tasks/TASK-0008-git-repo-control-setup.md`

## Verification

- Initial shell checks run by PM:
- `git status --short --branch` failed because local folder is not a git repository.
- `git ls-remote --heads --tags https://github.com/yusufbayindir/red-boll.git` returned success with empty output.
- `.gitignore` currently excludes Library, Temp, Obj, Build, Builds, Logs, UserSettings, solution/project files, temp files, `.DS_Store`, and `unity-import.log`.
- Git Repo Controller checks:
- `git rev-parse --is-inside-work-tree` failed with `fatal: not a git repository`.
- `git status --short --branch` failed with `fatal: not a git repository`.
- `git ls-remote --symref https://github.com/yusufbayindir/red-boll.git` returned success with empty output.
- `git ls-remote --heads --tags https://github.com/yusufbayindir/red-boll.git` returned success with empty output.
- `git ls-remote --exit-code https://github.com/yusufbayindir/red-boll.git` exited with code 2 and no output, consistent with an accessible remote that currently has no refs.
- `curl -I -L https://github.com/yusufbayindir/red-boll.git` reached GitHub and resolved to the repository web page with HTTP 200 after redirect.
- Top-level size check found `Builds` about 523 MB, `Library` about 223 MB, `Logs` about 252 KB, `Temp` about 48 KB, and `UserSettings` about 60 KB; these should remain excluded.
- 2026-05-12 refresh:
- `git status --short --branch` still fails with `fatal: not a git repository`.
- `git ls-remote --symref https://github.com/yusufbayindir/red-boll.git` returned success with no refs/output.
- Current `.gitignore` excludes Unity generated/cache folders, build output, user settings, generated project files, `.DS_Store`, mobile package outputs, IDE folders, and swap files.
- Current `.DS_Store` files found at `.DS_Store`, `docs/ai-production/.DS_Store`, `Builds/.DS_Store`, `kenney_game-icons-expansion/.DS_Store`, `kenney_game-icons-expansion/PNG/.DS_Store`, and `kenney_game-icons-expansion/PNG/Black/.DS_Store`; these should not be staged.
- 2026-05-12 release execution:
- `git status --short` failed before initialization with `fatal: not a git repository`.
- Safe file discovery found `.gitignore`, `README.md`, repo-local `open-ai-production-dashboard.command`, and `docs/ai-production/**`, including dashboard server/UI/data/reports/tasks.
- Dashboard runtime outputs `docs/ai-production/dashboard/dashboard-server.log` and `docs/ai-production/dashboard/__pycache__/server.cpython-313.pyc` were present locally and intentionally kept out of staging.
- `git remote -v` shows `origin` as `https://github.com/yusufbayindir/red-boll.git` for fetch and push.
- `git ls-remote --heads origin` returned success with no output, consistent with an empty remote.
- `git diff --cached --name-only` after staging included only the approved first release scope.

## Current AI Production Push Recommendation

For the requested AI production system push, PM should initialize in place only after approval and use this exact first add set:

```bash
git add .gitignore README.md docs/ai-production
```

If PM decides the first push must also make the Unity project openable from a fresh clone, add this expanded source set as a separate decision:

```bash
git add Assets Packages ProjectSettings
```

If PM approves third-party/source asset package provenance, optionally add:

```bash
git add kenney_game-icons-expansion red-ball-assets
```

Do not use `git add .` for the first commit.

## PM Initial Repo Recommendation

- Treat the remote as likely empty until Git Repo Controller confirms.
- Prefer initializing this existing Unity folder as the local repo because it already contains the working project.
- Before first commit, include only: `Assets`, `Packages`, `ProjectSettings`, `README.md`, `.gitignore`, `docs/ai-production`, and selected source asset folders if PM approves their licenses.
- Exclude generated/cache/build folders: `Library`, `Temp`, `Logs`, `Builds`, `UserSettings`.
- Do not push until Yusuf approves the first commit contents.

## QA Notes

Pending.

## Repo Controller Notes

### Local State

- `/Users/yusufbayindir/Desktop/ai game/red_ball` is not currently a git repository.
- No local branch, commit history, remote config, or index exists in this folder.
- The working Unity project already exists here and contains the intended source/project files.

### Remote State

- Requested remote: `https://github.com/yusufbayindir/red-boll.git`.
- Remote is reachable from this machine.
- No refs are advertised: no branches, tags, or HEAD symref were returned.
- Treat the remote as an empty repository until PM/Yusuf confirms that this is expected.

### Initialize vs Clone Recommendation

Initialize this existing Unity folder only after PM approval.

Reasoning:

- The local folder already contains the working Unity project, docs, settings, and imported assets.
- The remote appears empty, so cloning elsewhere would likely create an empty directory and add migration/copy risk.
- Initializing in place keeps the project path stable for Unity and the existing build instructions.

Do not push until PM/Yusuf approves the exact first-commit contents.

### Include List

Recommended for the first tracked set:

- `.gitignore`
- `README.md`
- `docs/ai-production/`
- `Assets/`
- `Packages/`
- `ProjectSettings/`

Recommended only after PM/license approval:

- `kenney_game-icons-expansion/`
- `red-ball-assets/`

Note: some imported asset files already exist under `Assets/Resources/Imported/`; if those are required for the project to open/play correctly, their license provenance should be recorded before the first push.

### Exclude List

Keep untracked and ignored:

- `Library/`
- `Temp/`
- `Logs/`
- `Builds/`
- `UserSettings/`
- `Obj/`
- Generated IDE/project files such as `*.csproj`, `*.sln`, `*.user`, `*.unityproj`
- Temporary/import files such as `*.tmp`, `.DS_Store`, and `unity-import.log`

Challenge point: `Builds/` is currently about 523 MB and must not be added without explicit PM/Yusuf approval.

### `.gitignore` Recommendation

Current `.gitignore` already covers the main dangerous Unity folders:

- `Library/`
- `Temp/`
- `Obj/`
- `Build/`
- `Builds/`
- `Logs/`
- `UserSettings/`

Recommended additions before first commit:

```gitignore
[Mm]emoryCaptures/
[Rr]ecordings/

*.apk
*.aab
*.ipa
*.xcarchive

.vs/
.vscode/
.idea/
*.swp
```

Optional: if PM decides source asset downloads should not be tracked at repo root, add:

```gitignore
kenney_game-icons-expansion/
red-ball-assets/
```

Do not add those two optional ignores if the plan is to preserve third-party source asset packages in git.

### Safe Command Sequence For Approval

For the AI production system-only push, use the "Current AI Production Push Recommendation" section above. The broader command sequence below remains the expanded Unity source option if PM wants the first commit to include the playable project source as well.

Review-only commands already run:

```bash
git rev-parse --is-inside-work-tree
git status --short --branch
git ls-remote --symref https://github.com/yusufbayindir/red-boll.git
git ls-remote --heads --tags https://github.com/yusufbayindir/red-boll.git
git ls-remote --exit-code https://github.com/yusufbayindir/red-boll.git
```

Proposed setup commands after PM approval:

```bash
cd "/Users/yusufbayindir/Desktop/ai game/red_ball"
git init
git branch -M main
git remote add origin https://github.com/yusufbayindir/red-boll.git
git status --short --branch
git check-ignore -v Library Temp Logs Builds UserSettings
git add .gitignore README.md docs/ai-production Assets Packages ProjectSettings
git status --short
git diff --cached --stat
git diff --cached --name-only
git commit -m "Initial Red Ball Unity project"
git status --short --branch
```

Optional, only if PM approves tracking root source asset packages:

```bash
git add kenney_game-icons-expansion red-ball-assets
git status --short
git diff --cached --stat
git commit --amend --no-edit
```

Push command, only after separate explicit PM/Yusuf approval:

```bash
git push -u origin main
```

## PM Closure

Pending.
