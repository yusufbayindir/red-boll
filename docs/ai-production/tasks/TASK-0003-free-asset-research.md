# TASK-0003 - Free Asset Research

## Status

Open - report ready for PM review

## Owner

Asset Researcher, PM

## PM Intent

Find license-safe free assets that can make the game look, sound, and market better.

## Scope

In:

- UI kits.
- Platformer tiles.
- Character/enemy options.
- Effects.
- Audio/music.
- Fonts/icons.
- License and attribution notes.

Out:

- Downloading/importing assets without PM approval.
- Paid assets.
- Assets with unclear licenses.

## Acceptance Criteria

- At least 20 relevant candidate assets or sources.
- Each item includes source link, license note, and fit score.
- Report separates "use now" from "maybe later".
- Report warns about attribution requirements.

## Activity Log

- 2026-05-11 23:20 - Task created by PM.
- 2026-05-11 - Asset Researcher reviewed protocol/role/task docs and prepared to touch only TASK-0003 plus a new asset research report.
- 2026-05-11 - Asset Researcher checked current source/license pages for free UI, platformer, character/enemy, effects, audio/music, font, and icon candidates.
- 2026-05-11 - Asset Researcher created `docs/ai-production/reports/asset-research.md` with 35 use-now candidates, 5 maybe-later candidates, license cautions, and fit scores.

## Changed Files

- `docs/ai-production/tasks/TASK-0003-free-asset-research.md`
- `docs/ai-production/reports/asset-research.md`

## Verification

- Checked source/license pages on 2026-05-11.
- Acceptance criteria met: 20+ candidates/sources, source links, license notes, fit scores, "use now" vs "maybe later", and attribution warnings.
- No assets downloaded or imported.

## QA Notes

Research-only task. No runtime QA needed until assets are imported.

## Repo Controller Notes

No asset files downloaded or imported. Future imports require license entry in `THIRD_PARTY_ASSETS.md` or equivalent approval record.

## PM Closure

Report accepted. Do not import new assets yet. Short-term approved direction is CC0-first, especially Kenney UI/particles/audio and OFL fonts after repo hygiene is established.
