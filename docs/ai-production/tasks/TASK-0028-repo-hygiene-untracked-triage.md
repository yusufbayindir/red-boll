# TASK-0028 - Repo Hygiene Untracked Triage

## Status

Completed - triage report written / no git actions performed

## Owner

Repo Hygiene / Untracked File Triage

## PM Intent

Classify the remaining untracked working tree items after the Sprint 01 git push report so the next repo-controller pass can make safe source-control decisions without accidentally committing raw asset packages, generated files, or stray tooling.

## Scope

In:

- Inspect `git status --short --ignored`.
- Inspect file sizes and lightweight contents for the reported untracked items.
- Classify each item as track now, ignore, keep local, or needs provenance.
- Decide whether `ProjectSettings/SceneTemplateSettings.json` is Unity source-controlled configuration or local generated output.
- Decide whether root `package.json` is needed for dashboard/tooling.
- Tie raw asset folders back to TD-0005 and the asset manifest.
- Update docs/status only.

Out:

- `git add`, commit, push, reset, checkout, delete, or destructive cleanup.
- PM code/runtime/dashboard implementation.
- Broad `.gitignore` additions for source-like files or raw asset folders.

## Acceptance Criteria

- Triage report exists at `docs/ai-production/reports/repo-hygiene-untracked-triage.md`.
- Every reported untracked item has a recommendation.
- Scene template setting and root package findings are explicit.
- Asset folders are linked to TD-0005/provenance.
- `.gitignore` is changed only if there is a clear generated/log/cache miss.
- Final technical debt line is present.

## Risk Check

- Shared systems touched: production docs/status only.
- Regression risks: low; no runtime, Unity, dashboard, git index, or ignore-rule changes were made.
- Rollback idea: revert this task/report/status documentation only.

## Activity Log

- 2026-05-12 - Created task for untracked file triage.
- 2026-05-12 - Reviewed `git status --short --ignored`, item sizes, Unity scene template JSON, root `package.json`, `.gitignore`, asset folder summaries, local license files, asset manifest, Sprint 01 git release report references, and TD-0005.
- 2026-05-12 - Classified `ProjectSettings/SceneTemplateSettings.json` as track-now project setting; `package.json` as keep-local / owner-decision; raw Kenney folders as provenance-gated; `Logs/` as already ignored.

## Changed Files

- `docs/ai-production/reports/repo-hygiene-untracked-triage.md`
- `docs/ai-production/tasks/TASK-0028-repo-hygiene-untracked-triage.md`
- `docs/ai-production/STATUS_BOARD.md`

## Verification

Read-only verification commands used:

```sh
git status --short --ignored
du -sh ProjectSettings/SceneTemplateSettings.json package.json kenney_game-icons-expansion red-ball-assets Logs
sed -n '1,220p' ProjectSettings/SceneTemplateSettings.json
sed -n '1,220p' package.json
sed -n '1,220p' .gitignore
find kenney_game-icons-expansion red-ball-assets -maxdepth 3 -type f
find kenney_game-icons-expansion red-ball-assets -type f | wc -l
rg -n "package.json|npm|node|dashboard|red-ball-assets|kenney_game-icons-expansion|TD-0005|SceneTemplateSettings|Logs/" docs .gitignore ProjectSettings package.json
```

No test suite was run because this was docs-only repo hygiene triage.

## QA Notes

No app QA required. The report is the deliverable.

## Repo Controller Notes

Recommended next source-control action is to review and track `ProjectSettings/SceneTemplateSettings.json` in the next safe repo-controller pass. Do not track root `package.json` unless a tooling owner claims it. Do not track raw asset folders until the asset manifest/provenance package decision is complete. No `.gitignore` update is recommended from this triage.

## PM Closure

Completed as docs-only triage. PM/repo controller can use the report as the decision input for the next safe staging pass.

## Technical Debt Added

Technical debt added: none
