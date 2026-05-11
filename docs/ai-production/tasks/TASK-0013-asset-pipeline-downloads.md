# TASK-0013 - Asset Pipeline And Downloads

## Status

In Progress

## Owner

PM, Asset Pipeline, Git Repo Controller

## PM Intent

Turn the asset research into a controlled asset pipeline for the new product direction: Playable Hill UI, mastery badges, checkpoints, Trickline visuals, particles, SFX, and marketing images.

## Scope

In:

- Choose license-safe assets to download/import first.
- Create asset manifest requirements.
- Prefer CC0 and OFL/MIT/ISC with notice tracking.
- Download only modest, clearly licensed assets if the pipeline worker confirms source/license safety.
- Keep raw downloads separate from Unity imports.

Out:

- Unclear-license assets.
- Paid assets.
- Huge asset dumps without PM approval.
- Importing assets into gameplay before QA/repo hygiene.

## Acceptance Criteria

- Asset download/import plan exists.
- Manifest format exists.
- Any downloaded asset has source URL, license, creator, date, and intended use.
- Root generated/cache folders remain excluded.

## Activity Log

- 2026-05-12 00:38 - Task opened by PM after user requested asset continuation and downloads based on new ideas.
- 2026-05-12 00:38 - Asset Pipeline worker Carson started.
- 2026-05-12 - Asset Pipeline read the existing asset download plan, manifest, and TASK-0013 scope, then prepared a Sprint 01 manual-download list for vertical lift, crumbling tile, mastery badge UI, checkpoint/warning polish, and SFX.
- 2026-05-12 - Asset Pipeline checked primary/official source pages for Kenney Particle Pack, UI Pack, UI Audio, Interface Sounds, Impact Sounds, Music Jingles, Simplified Platformer Pack, New Platformer Pack, Game Icons Expansion, and Shape Characters.
- 2026-05-12 - No assets were downloaded or imported. New raw downloads should not use `red-ball-assets/downloads/`; if approved, stage raw zips under `docs/ai-production/downloads/2026-05-12/<pack>/` with license files and checksums.

## Changed Files

- `docs/ai-production/reports/asset-pipeline-sprint-01-execution.md`
- `docs/ai-production/assets-manifest.md`
- `docs/ai-production/tasks/TASK-0013-asset-pipeline-downloads.md`

## Verification

- Documentation-only verification complete.
- Confirmed no Unity code, scene, prefab, runtime asset, `.meta`, or import folder changes were made intentionally.
- License/source facts were verified against primary pages where possible:
  - `https://www.kenney.nl/assets/particle-pack`
  - `https://kenney-assets.itch.io/ui-pack`
  - `https://kenney.nl/assets/ui-audio`
  - `https://kenney.nl/assets/interface-sounds`
  - `https://kenney.nl/assets/impact-sounds`
  - `https://kenney.nl/assets/music-jingles`
  - `https://www.kenney.nl/assets/simplified-platformer-pack`
  - `https://www.kenney.nl/assets/new-platformer-pack`
  - `https://opengameart.org/content/game-icons-expansion`
  - `https://kenney-assets.itch.io/shape-characters`

## QA Notes

- Runtime QA is not applicable until assets are actually imported and wired by implementation tasks.
- Asset QA for the future download pass should verify original zip retention, license/notice retention, source URL capture, SHA256 checksums, and manifest status before any Unity import.

## Repo Controller Notes

- Keep raw downloads out of Unity import folders until PM approves the exact subset.
- If raw zips are staged later, repo inclusion should be an explicit PM/repo-controller decision to avoid large or unreviewed asset history.
- TD-0005 remains active for unknown existing provenance under `Assets/Resources/Imported/`.

## PM Closure

Pending.
