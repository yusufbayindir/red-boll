# Repo Hygiene Untracked Triage

Date: 2026-05-12

Owner: Repo Hygiene / Untracked File Triage

Scope: Read-only triage of untracked and ignored working tree items after the Sprint 01 git push report. No staging, commit, push, delete, Unity setting change, package install, or destructive command was performed.

## Commands Reviewed

```sh
git status --short --ignored
du -sh ProjectSettings/SceneTemplateSettings.json package.json kenney_game-icons-expansion red-ball-assets Logs
find kenney_game-icons-expansion red-ball-assets -type f
rg -n "package.json|npm|node|dashboard|red-ball-assets|kenney_game-icons-expansion|TD-0005|SceneTemplateSettings|Logs/" docs .gitignore ProjectSettings package.json
```

## Working Tree Summary

| Item | Status | Size | Content summary | Recommendation |
| --- | --- | ---: | --- | --- |
| `ProjectSettings/SceneTemplateSettings.json` | Untracked | 4.0K | Unity scene template dependency defaults for built-in Unity asset types; no user-specific path, machine path, cache, or log data found. | Track now in the next repo-controller pass. It is a Unity project setting and belongs with the tracked `ProjectSettings/*.asset` files. |
| `package.json` | Untracked | 4.0K | Root npm package stub: `name`, Turkish Unity prototype description, `main: index.js`, no dependencies, no devDependencies, and only the default failing `test` script. | Keep local / needs owner decision. Do not track now; no dashboard/runtime/tooling need was found. Do not add to `.gitignore` because source-like root tooling files should not be silently hidden. |
| `kenney_game-icons-expansion/` | Untracked | 8.4M | 813 files; Kenney game icon base + expansion source package with PNG, XML, SVG, AI, previews, and local CC0 license files. | Needs provenance gate before tracking. Link to TD-0005 and asset manifest; commit only after source URL, license, import date, checksum/original package policy, and approved local path are explicit. |
| `red-ball-assets/` | Untracked | 1.2M | 219 files; Kenney Shape Characters source package with PNG variants, `License.txt`, Kenney URL shortcuts, and preview. | Needs provenance gate before tracking. Link to TD-0005 and asset manifest; commit only after manifest/provenance completeness is accepted for the raw source package or a curated subset is moved under a tracked asset path. |
| `Logs/` | Ignored | 68K | Unity/test logs. Already matched by `[Ll]ogs/` in `.gitignore`. | Ignore. No action required. |

Ignored noise also includes `.DS_Store`, `Builds/`, `Library/`, `UserSettings/`, dashboard `__pycache__/`, and dashboard server logs. Existing `.gitignore` already covers these generated/cache/log outputs.

## Scene Template Setting Finding

`ProjectSettings/SceneTemplateSettings.json` appears to be Unity source-controlled project configuration, not a local generated cache.

Evidence:

- It sits under `ProjectSettings/`, where the repo already tracks project-wide Unity settings such as `ProjectSettings.asset`, `EditorSettings.asset`, `PackageManagerSettings.asset`, `GraphicsSettings.asset`, and `ProjectVersion.txt`.
- Its content is deterministic project/editor configuration: `templatePinStates`, `dependencyTypeInfos`, `defaultDependencyTypeInfo`, and `newSceneOverride`.
- It contains no absolute paths, usernames, timestamps, logs, temp references, or machine-local caches.

Recommendation: include it in the next safe repo-controller staging pass, subject to normal review. Do not ignore it.

## Root package.json Finding

The root `package.json` does not currently justify tracking.

Observed content:

- No `dependencies` or `devDependencies`.
- `scripts.test` is the npm initializer default that exits with failure.
- `main` points to `index.js`, but no matching project entrypoint was found in this triage scope.
- Dashboard tooling in docs uses plain Python, static HTML/CSS/JS, shell launchers, and ad hoc `npx --yes ajv-cli@5` validation in reports; no committed npm workflow requires this root package.
- The Sprint 01 release report already excluded root `package.json` because it was not needed for the Unity project, dashboard runtime, or Sprint 01 release scope.

Recommendation: keep local until its provenance is resolved. If a future dashboard/tooling owner needs npm tooling, create a purposeful package file with real scripts and dependency policy. If nobody owns it, remove it in a separate user-approved cleanup task; this triage did not delete it.

## Asset Provenance Finding

Both raw asset folders align with the existing asset manifest entries, but they should still remain outside commit scope until the asset pipeline explicitly closes the raw-source packaging decision.

Evidence:

- `docs/ai-production/assets-manifest.md` lists `red-ball-assets/` as the local Kenney Shape Characters source package and `kenney_game-icons-expansion/` as the local Kenney Game Icons + Expansion source package.
- Local license files state CC0 terms for the Kenney packages.
- `docs/ai-production/reports/sprint-01-git-release-report.md` deliberately left both raw asset holding folders out of the release commit.
- TD-0005 remains open until every shipped asset has source URL, license, attribution requirement, import date, and approval status in the asset manifest.

Recommendation: do not track the entire raw folders merely because they are local and CC0. The next asset-pipeline pass should decide one of these paths:

1. Track raw source packages in a documented third-party source location with license files and checksums.
2. Track only curated runtime/import subsets under approved `Assets/` paths with provenance beside them.
3. Keep raw packages local-only and document how to reacquire them.

## .gitignore Decision

No `.gitignore` change was made.

Reason: the only item that clearly belongs ignored from the reported list is `Logs/`, and it is already covered by `[Ll]ogs/`. The asset source folders and Unity project setting are not generated/cache outputs, and ignoring them would hide source-control decisions that should remain visible.

## Proposed Follow-Up Tasks

| Task | Owner | Purpose | Suggested acceptance criteria |
| --- | --- | --- | --- |
| TASK-0028 closeout / repo-controller pass | Git Repo Controller | Apply this triage decision without mixing unrelated cleanup. | `ProjectSettings/SceneTemplateSettings.json` is reviewed and staged if still unchanged; `package.json` remains unstaged or is dispositioned by owner; raw asset folders remain visible until asset-pipeline decision. |
| Asset raw package provenance closure | Asset Pipeline | Resolve whether `red-ball-assets/` and `kenney_game-icons-expansion/` are tracked raw sources, local-only packages, or replaced by curated imports. | Manifest includes source URL, license, attribution requirement, import/download date, checksum/original package policy, local path, runtime path, and approval status for each tracked package/subset. |
| Root npm/tooling disposition | Dashboard / Tooling Owner | Decide whether a root npm package is required. | Either a purposeful tracked `package.json` exists with real scripts and dependency policy, or the stray local file is removed in an approved cleanup. |

## Changed Files

- `docs/ai-production/reports/repo-hygiene-untracked-triage.md`
- `docs/ai-production/tasks/TASK-0028-repo-hygiene-untracked-triage.md`
- `docs/ai-production/STATUS_BOARD.md`

## Technical Debt

No new technical debt ID is required. TD-0005 already covers the asset provenance risk for the raw Kenney folders; this report adds decision evidence but does not expand the risk.

Technical debt added: none
