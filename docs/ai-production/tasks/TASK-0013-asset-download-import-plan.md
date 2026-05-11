# TASK-0013 - Asset Download And Import Plan

## Status

Plan Ready - Awaiting PM Download Approval

## Owner

Asset Pipeline, PM

## PM Intent

Turn the free asset research and design reports into a concrete, license-safe download/import plan for the next Red Ball polish ideas: UI, particles, checkpoint feedback, clean-line/Trickline readability, and marketing screenshots.

## Scope

In:

- Read the asset research, UI, level design, checkpoint, retention, Trickline, and marketing reports.
- Decide which assets are safe and useful to download now versus later.
- Record license notes and import destinations before any asset import work.
- Update the project asset manifest.

Out:

- Runtime implementation.
- Large full-pack imports.
- Paid assets.
- CC-BY, CC-BY-NC, unclear, or personal-use assets.
- Store screenshots or trailer capture.

## Acceptance Criteria

- Download/import plan covers UI, particles, checkpoint effects, clean-line/Trickline visuals, and marketing screenshots.
- Plan separates "download now" from "download later".
- License notes include source URLs and attribution obligations.
- Manifest records current local third-party sources and planned approved candidates.
- No large or unclear-license assets are downloaded.

## Risk Check

- Shared systems touched: documentation only in this pass.
- Future import risks: asset bloat, duplicated Resource paths, drifting source-page license text, third-party art that makes Red Ball look more generic, and marketing captures showing unimplemented features.
- Rollback idea: remove staged files under `docs/ai-production/downloads/` or `Assets/ThirdParty/` and remove the matching manifest entries before runtime code references them.

## Activity Log

- 2026-05-12 00:45 +0300 - Created TASK-0013. Intended touches: this task, `docs/ai-production/reports/asset-download-plan.md`, and `docs/ai-production/assets-manifest.md`. No asset downloads planned during this documentation pass.
- 2026-05-12 00:45 +0300 - Read asset research, level expansion, checkpoint, UI, retention, Trickline, and marketing reports.
- 2026-05-12 00:45 +0300 - Checked local asset folders and existing license files for `red-ball-assets/` and `kenney_game-icons-expansion/`.
- 2026-05-12 00:45 +0300 - Verified current source pages for the main license-safe candidate packs before writing the plan.

## Changed Files

- `docs/ai-production/tasks/TASK-0013-asset-download-import-plan.md`
- `docs/ai-production/reports/asset-download-plan.md`
- `docs/ai-production/assets-manifest.md`

## Verification

- Static documentation review only.
- Confirmed no new downloads were created under `docs/ai-production/downloads/`.
- Confirmed no files were added under `Assets/ThirdParty/`.
- License checks were limited to official/current source pages and local license files; final legal review remains a PM/release responsibility.

## QA Notes

QA pending after assets are downloaded/imported and wired into runtime scenes.

## Repo Controller Notes

Workspace is not currently a Git worktree, so no Git diff or commit verification was available from this folder. File changes were kept inside the assigned documentation ownership.

## PM Closure

Pending.
