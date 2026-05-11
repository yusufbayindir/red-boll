# TASK-0015 Roster + Codex Handoff QA Report

Role: Dashboard Visual/Interaction QA  
Workspace: `/Users/yusufbayindir/Desktop/ai game/red_ball`  
Run time: 2026-05-12 01:53-01:56 Europe/Istanbul

## Verdict

**PASS**

Raman is present in live dashboard state and visible in the dashboard roster. The recent Goodall, Kierkegaard, Hubble, and Tesla cards are also visible. Selecting Raman opens the internal transcript panel and exposes the `Open Codex Chat` handoff button. Because a direct agent chat deep link is unavailable, the UI falls back silently to the in-panel status text `Codex opened; direct agent deep link unavailable.` No alert, toast, browser dialog, or notification was observed.

The in-app browser backend was unavailable in this run, so visible UI checks used headless Google Chrome through Playwright Core against the same local relay URL.

## Checks Run

```sh
node --check docs/ai-production/dashboard/dashboard.js
python3 -m py_compile docs/ai-production/dashboard/server.py
node -e 'JSON.parse(...)' # dashboard.json and dashboard.schema.json
npx --yes ajv-cli@5 validate -s docs/ai-production/dashboard/data/dashboard.schema.json -d docs/ai-production/dashboard/data/dashboard.json --spec=draft2020 --strict=false
```

`ajv-cli` returned `dashboard.json valid`; it also warned that `date-time` formats were ignored because no format plugin was installed.

## API Smoke

Local relay started on `http://127.0.0.1:8877/index.html` with macOS `open` stubbed to avoid launching Codex/Xcode during smoke tests.

| Check | Result | Evidence |
| --- | --- | --- |
| `GET /api/state` roster | PASS | Returned relay `red-ball-dashboard-relay`; roster contained Raman, Goodall, Kierkegaard, Hubble, and Tesla. |
| `POST /api/open-codex-agent` dry run | PASS | Returned `ok: true`, `status: dry-run`, `directAgentDeepLinkAvailable: false`, command `open -a Codex`, and fallback message. |
| `POST /api/open-workspace` | PASS | Returned `ok: true`, `status: opened`, `workspaceName: Unity-iPhone.xcworkspace` with `open` stubbed. |

## Browser Visibility

Headless Chrome opened `http://127.0.0.1:8877/index.html`.

| UI Check | Result | Evidence |
| --- | --- | --- |
| Left roster includes Raman | PASS | `#railAgentList` contained `Raman`. |
| Left roster includes recent completions | PASS | `#railAgentList` contained `Goodall`, `Kierkegaard`, `Hubble`, and `Tesla`. |
| Main agent cards include recent completions | PASS | `#agentGrid` contained Raman plus Goodall/Kierkegaard/Hubble/Tesla. |
| Status readability | PASS | Status pills were present on cards and visually scannable by state (`Done`, `Thinking`, `Blocked`). Measured contrast samples: Done 4.69, Thinking 4.05, Blocked 3.97. |
| Raman card interaction | PASS | Clicking Raman selected `Raman` in the detail panel and rendered Raman transcript snippets. |
| Codex handoff button | PASS | Selected-agent panel showed `Open Codex Chat`. |
| Codex fallback status | PASS | Card click posted `/api/open-codex-agent`; status text became `Codex opened; direct agent deep link unavailable.` |
| Silent fallback | PASS | Captured browser dialogs: `[]`; captured alert/confirm/prompt calls: `[]`; captured Notification calls: `[]`; toast-like elements: `0`. |
| Red Ball menu | PASS | Topbar `Red Ball` menu was visible; opening it showed `Build Workspace`. |
| Workspace endpoint from UI | PASS | Clicking `Build Workspace` produced a server-side `POST /api/open-workspace` 200 with the test `open` stub. |

## Notes

- Screenshot not required; no screenshot artifact was produced.
- The dashboard reported `visibleAgentCount: 22` during the run.
- The local relay also had an existing process on ports 8765 and 8766, so QA used port 8877 for isolated smoke coverage.

## Changed Files

- `docs/ai-production/reports/TASK-0015-roster-codex-handoff-qa.md`
- `docs/ai-production/tasks/TASK-0015-live-agent-dashboard-system.md`

Technical debt added: none.
