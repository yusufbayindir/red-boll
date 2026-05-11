# Dashboard Data

This folder is the live dashboard data contract. Runtime/UI work reads these files, but data ownership stays here.

The dashboard can be opened directly as a file, but live refresh works best through:

`open-ai-production-dashboard.command`

That command starts a local HTTP server for `docs/ai-production/dashboard/` and opens the page. The page reads `data/dashboard.json` every one second, so Codex can update agent status, event history, and task state while work continues without a manual refresh.

## Files

- `dashboard.json` - current live state for agents, tasks, events, PM decisions, transcripts, and artifacts.
- `dashboard.schema.json` - JSON Schema for the dashboard state shape.
- `outbox.json` - local PM dispatch queue written by `POST /api/messages`.
- `events-live.json` - relay-generated live events, including queued PM messages.

## Local Relay Server

`docs/ai-production/dashboard/server.py` serves the static dashboard and exposes the local relay API. The launcher starts it through:

`open-ai-production-dashboard.command`

Default URL:

`http://127.0.0.1:8765/index.html`

If port `8765` is already serving this relay, the launcher reuses it. If the port is occupied by something else, it tries the next ports up to `8785`.

## HTTP API Contract

### `GET /api/health`

Returns a small readiness payload. Launch scripts use this to detect an existing relay.

Example response:

```json
{
  "ok": true,
  "name": "red-ball-dashboard-relay",
  "root": "/Users/yusufbayindir/Desktop/ai game/red_ball/docs/ai-production/dashboard"
}
```

### `GET /api/state`

Returns the source dashboard JSON plus relay-owned queue/event state. This is the recommended endpoint for UI work that needs both the dashboard state and messages queued from the composer.

Example response shape:

```json
{
  "relay": {
    "name": "red-ball-dashboard-relay",
    "mode": "local-pm-dispatch-queue",
    "realSubagentDispatch": false,
    "updatedAt": "2026-05-12T01:20:00+03:00"
  },
  "dashboard": {
    "schemaVersion": "1.0.0",
    "version": 4,
    "updatedAt": "2026-05-12T01:05:00+03:00"
  },
  "outbox": [],
  "liveEvents": []
}
```

### `GET /api/events`

Opens a Server-Sent Events stream containing the current relay event/outbox state. The server sends a `state` event when `events-live.json` or `outbox.json` changes, plus heartbeat comments while idle. Clients may reconnect or fall back to polling `/api/state`.

Example SSE frame:

```text
event: state
data: {"events":[],"outbox":[]}
```

### `POST /api/messages`

Queues a PM-authored message for a selected agent. This does not dispatch to a real Codex subagent API. It appends to `outbox.json`, appends an event to `events-live.json`, and returns both records.

Request body:

```json
{
  "agentId": "pascal-dashboard-runtime",
  "body": "Please wire the composer to POST /api/messages.",
  "author": "PM"
}
```

Example response:

```json
{
  "ok": true,
  "message": {
    "id": "msg-20260511222000-1234abcd",
    "agentId": "pascal-dashboard-runtime",
    "body": "Please wire the composer to POST /api/messages.",
    "author": "PM",
    "createdAt": "2026-05-12T01:20:00+03:00",
    "status": "PM dispatch pending",
    "dispatchMode": "local-pm-dispatch-queue",
    "realSubagentDispatch": false
  },
  "event": {
    "id": "evt-msg-20260511222000-1234abcd",
    "time": "2026-05-12T01:20:00+03:00",
    "displayTime": "2026-05-12 01:20",
    "speaker": "PM",
    "agentId": "pascal-dashboard-runtime",
    "kind": "pm-dispatch-queued",
    "message": "Please wire the composer to POST /api/messages.",
    "status": "PM dispatch pending",
    "dispatchMode": "local-pm-dispatch-queue"
  }
}
```

Validation errors return:

```json
{
  "ok": false,
  "error": "agentId is required"
}
```

## Polling Contract

Clients should poll `dashboard.json` on a short interval and compare:

- `version` - monotonically increases when the data changes.
- `updatedAt` - ISO 8601 timestamp of the latest state write.
- `events[].id` - stable event ids for timeline de-duplication.

The current recommended interval is exposed at `polling.recommendedIntervalMs`.

## Agent Status Values

Use only these values for `liveAgents[].status` and `agents[].status`:

- `thinking` - agent is reasoning or preparing output.
- `running` - agent is actively producing work or speaking.
- `done` - task/output is complete.
- `blocked` - waiting on approval, dependency, or missing evidence.
- `idle` - available but not currently active.

`liveAgents[].indicator.showThinkingLogo` tells the UI whether to show the small left-panel thinking/logo indicator requested by the user.

## Primary Collections

- `liveAgents` - currently active or recently active agents.
- `agents` - completed/reference agents from the production thread.
- `events` - timeline entries for requests, starts, decisions, and data updates.
- `transcriptSnippets` - short conversation excerpts suitable for a compact dashboard panel.
- `agentConversations` - per-agent transcript entries used by the selected-agent detail panel.
- `tasks` - task board rows with owners, priorities, statuses, and links.
- `meetingNotes` - current meeting/grooming context for the live workflow board.
- `groomingQueue` - ticket-like work items surfaced in the workflow board.
- `pmDecisionLog` - structured PM/moderator decisions with reason text.
- `artifactLinks` - reports, schemas, status docs, and marketing/asset deliverables.

## Outbound Message Queue

The dashboard UI has a message composer for the selected agent, but there is no real Codex subagent dispatch API attached to this local runtime yet. The relay endpoint `POST /api/messages` stores submitted messages in `outbox.json`, records `pm-dispatch-queued` events in `events-live.json`, and labels them `PM dispatch pending`.

Older/static UI builds may still store pending messages in browser `localStorage` under `redBallPendingOutboundMessages`. Runtime/UI can migrate to the relay API without changing the dashboard data source shape.
