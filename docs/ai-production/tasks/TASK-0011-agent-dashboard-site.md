# TASK-0011 - Agent Dashboard Site

## Status

Accepted

## Owner

PM, Dashboard Site Developer, QA

## PM Intent

Create a simple local website where Yusuf can see all AI production agents, their roles, outputs, decisions, tasks, and next implementation queue in one place.

## Scope

In:

- Static local dashboard page.
- Agent cards with role, agent nickname, status, task, output report.
- PM decisions.
- Task status summary.
- Next implementation queue.
- Links to relevant MD files.

Out:

- Backend/server requirement.
- Authentication.
- Editing tasks from the website.
- Complex build tooling.

## Acceptance Criteria

- Site opens directly as a static HTML file.
- Every initial agent is represented.
- PM decisions and next queue are visible without reading multiple MD files.
- Links point to local repo docs using relative paths.
- Design is readable in desktop and mobile widths.
- Related task MD is updated before and after code steps.

## Risk Check

- Shared systems touched: docs only.
- Regression risks: broken relative links, stale agent statuses.
- Rollback idea: delete `docs/ai-production/dashboard/` without touching game code.

## Activity Log

- 2026-05-11 23:59 - Task created by PM before dashboard site code edits.
- 2026-05-11 23:59 - Dashboard Site Developer starting implementation; intended touches are `docs/ai-production/dashboard/index.html`, `docs/ai-production/dashboard/styles.css`, and this task log only.
- 2026-05-11 23:59 - Added static dashboard scaffold and content in `docs/ai-production/dashboard/index.html`, covering agents, task statuses, PM decisions, queue, and report/task links.
- 2026-05-11 23:59 - Added responsive dashboard styling in `docs/ai-production/dashboard/styles.css` for desktop/mobile readability without build tooling.
- 2026-05-11 23:59 - Finalized scope to only dashboard HTML/CSS plus this task record; no backend, server, authentication, or build tooling added.
- 2026-05-12 00:04 - PM reviewed dashboard source, fixed the displayed `TASK-0011` status to `QA Pending`, and verified all local links resolve.
- 2026-05-12 00:06 - PM updated dashboard to include new `TASK-0012` and mark `TASK-0011` accepted.
- 2026-05-12 00:58 - Follow-on `TASK-0015` upgraded the static dashboard into a local-server live dashboard. Static content remains the fallback; live mode is now owned by the realtime dashboard engineer, not PM direct coding.

## Changed Files

- `docs/ai-production/dashboard/index.html`
- `docs/ai-production/dashboard/styles.css`
- `docs/ai-production/dashboard/dashboard.js`
- `docs/ai-production/tasks/TASK-0011-agent-dashboard-site.md`

## Verification

- Static source review against `docs/ai-production/AGENT_DASHBOARD.md`: every listed agent is represented.
- Static source review confirmed report/task links from `AGENT_DASHBOARD.md` are included as relative local paths.
- No backend, server, or build tooling required; page opens directly from `docs/ai-production/dashboard/index.html`.
- PM link check using a Node script returned `all links ok` for every local `href` in `docs/ai-production/dashboard/index.html`.

## QA Notes

PM source QA passed for static links and content coverage. Visual browser QA is still recommended when convenient.

## Repo Controller Notes

Pending.

## PM Closure

Accepted as a lightweight local dashboard. Keep it updated when new agents/tasks are added.
