# TASK-0029 - AI Asset Generation Pipeline

## Status

Open - Brief Ready

## Owner

AI Asset Pipeline Product Owner, Asset Production, Art Direction, QA

## PM Intent

Create a controlled, license-safe production path for generated and hand-made game assets without importing anything into Unity until a later implementation task approves exact files.

## Scope

In:

- Game asset generation brief and first batch recommendation.
- Style targets for mastery badges, vertical lift polish, crumbling tile, checkpoint/warning polish, app icon/key art, store screenshots, and ad stills.
- Model strategy across local open-source generation, ChatGPT image generation, and manual/vector production.
- Manifest/provenance requirements for generated, edited, and manually made assets.
- Acceptance criteria for creative review, license review, and reproducibility.

Out:

- Unity import, `.meta` generation, prefab wiring, runtime code, or scene edits.
- Downloading model weights or third-party asset packs in this task.
- Shipping CC-BY, NonCommercial, unclear-license, or unreviewed AI-generated output.
- Claims or visuals that show fake mechanics, fake UI, or unverified future gameplay.

## Acceptance Criteria

- `docs/ai-production/reports/ai-asset-generation-brief.md` defines target art direction, asset families, production methods, export targets, and review gates.
- Every proposed game asset type has a first-pass production method: manual/vector, local open-source generation, ChatGPT image generation, or hybrid.
- Generated asset provenance format includes prompt, negative prompt where relevant, model/provider/version, seed or reproducibility note, source inputs, license/usage-rights, date, owner, review status, and intended use.
- First batch recommendation covers mastery badge icons, vertical lift polish, crumbling tile, checkpoint/warning polish, app icon/key art, store screenshots, and ad stills.
- License policy rejects NonCommercial assets and requires source/license/notice tracking before runtime or marketing use.
- Task explicitly confirms no Unity asset import or code work was performed.

## Risk Check

- Shared systems touched: Documentation only: task docs, status board, report, technical debt register.
- Regression risks: Low. Main risk is future teams treating concept output as shippable without provenance/review.
- Rollback idea: Revert the docs additions and keep existing asset manifest policy as source of truth.

## Activity Log

- 2026-05-12 02:38 - Task opened for overnight asset/ad creative pipeline ownership.
- 2026-05-12 02:38 - Product brief created with generation strategy, manifest schema, first batch, and acceptance criteria.
- 2026-05-12 - First ChatGPT/imagegen creative board archived under `docs/ai-production/generated-assets/2026-05-12/` with MD/JSON provenance, SHA-256 checksum, and manifest entry. Asset remains concept-only; no Unity import, runtime code, store asset, or paid creative approval performed.

## Changed Files

- `docs/ai-production/tasks/TASK-0029-ai-asset-generation-pipeline.md`
- `docs/ai-production/reports/ai-asset-generation-brief.md`
- `docs/ai-production/generated-assets/2026-05-12/red-ball-creative-board-v1.png`
- `docs/ai-production/generated-assets/2026-05-12/red-ball-creative-board-v1.provenance.md`
- `docs/ai-production/generated-assets/2026-05-12/red-ball-creative-board-v1.provenance.json`
- `docs/ai-production/assets-manifest.md`
- `docs/ai-production/STATUS_BOARD.md`
- `docs/ai-production/technical-debt/TECHNICAL_DEBT_REGISTER.md`

## Verification

- Documentation/archive-only review. Generated creative board was copied from the local ChatGPT/imagegen output cache into docs generated-assets and provenance was recorded. No code, Unity asset import, model download, package install, runtime file change, store asset approval, or paid creative approval performed.

## QA Notes

- Future visual QA must compare generated assets against the style targets in the brief and verify readability at gameplay and store-thumbnail sizes.
- Future license QA must block assets with missing manifest fields before import, capture, store submission, or paid creative use.

## Repo Controller Notes

- Generated source files, prompts, seeds, exports, and review manifests need an approved folder plan before bulk production.
- Large model weights and caches must not be committed without explicit repo-controller approval.

## PM Closure

Pending.

## Technical Debt Added

TD-0013 referenced. No new technical debt added by the archive pass.
