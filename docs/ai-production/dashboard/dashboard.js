const DATA_URL = "data/dashboard.json";
const API_STATE_URL = "/api/state";
const API_EVENTS_URL = "/api/events";
const API_MESSAGES_URL = "/api/messages";
const API_OPEN_WORKSPACE_URL = "/api/open-workspace";
const API_OPEN_CODEX_AGENT_URL = "/api/open-codex-agent";
const REFRESH_MS = 1000;
const MAX_EVENT_ITEMS = 12;
const MAX_MESSAGE_ITEMS = 4;

const agentGrid = document.querySelector("#agentGrid");
const visibleAgentCount = document.querySelector("#visibleAgentCount");
const emptyState = document.querySelector("#agentEmptyState");
const filterButtons = Array.from(document.querySelectorAll("[data-filter]"));
const dashboardRefreshStatus = document.querySelector("#dashboardRefreshStatus");
const workspaceTopbarStatus = document.querySelector("#workspaceTopbarStatus");
const railRefreshStatus = document.querySelector("#railRefreshStatus");
const railAgentList = document.querySelector("#railAgentList");
const railMessageStream = document.querySelector("#railMessageStream");
const railThinkingLogo = document.querySelector("#railThinkingLogo");
const liveMessageStream = document.querySelector("#liveMessageStream");
const taskGrid = document.querySelector("#taskGrid");
const decisionGrid = document.querySelector("#decisionGrid");
const selectedAgentAvatar = document.querySelector("#selectedAgentAvatar");
const selectedAgentName = document.querySelector("#selectedAgentName");
const selectedAgentRole = document.querySelector("#selectedAgentRole");
const selectedAgentStatus = document.querySelector("#selectedAgentStatus");
const agentTranscript = document.querySelector("#agentTranscript");
const agentMessageForm = document.querySelector("#agentMessageForm");
const agentMessageInput = document.querySelector("#agentMessageInput");
const agentMessageFeedback = document.querySelector("#agentMessageFeedback");
const openCodexChatButton = document.querySelector("#openCodexChatButton");
const codexHandoffStatus = document.querySelector("#codexHandoffStatus");
const meetingNotesList = document.querySelector("#meetingNotesList");
const groomingQueue = document.querySelector("#groomingQueue");
const workflowDecisionList = document.querySelector("#workflowDecisionList");
const redBallMenu = document.querySelector("#redBallMenu");
const openWorkspaceButton = document.querySelector("#openWorkspaceButton");
const workspaceOpenStatus = document.querySelector("#workspaceOpenStatus");

let activeFilter = document.querySelector(".segment.is-active")?.dataset.filter || "all";
let isRefreshing = false;
let relayAvailable = false;
let sseConnected = false;
let eventSource = null;
let latestEventKey = "";
let latestData = null;
let selectedAgentId = localStorage.getItem("redBallSelectedAgentId") || "";
let codexHandoffMessage = "";
let codexHandoffAgentId = "";
const OUTBOUND_QUEUE_KEY = "redBallPendingOutboundMessages";

function canTryRelay() {
  return window.location.protocol === "http:" || window.location.protocol === "https:";
}

function safeStatus(status) {
  const normalized = String(status || "done").toLowerCase().replace(/[^a-z0-9-]/g, "-");
  if (normalized === "running") return "talking";
  return normalized;
}

function statusText(status) {
  const normalized = safeStatus(status);
  if (normalized === "talking") return "Talking";
  if (normalized === "thinking") return "Thinking";
  if (normalized === "blocked") return "Blocked";
  if (normalized === "done") return "Done";
  if (normalized === "idle") return "Idle";
  if (normalized === "qa") return "QA Pending";
  if (normalized === "accepted") return "Accepted";
  if (normalized === "ready") return "Ready";
  if (normalized === "waiting") return "Waiting";
  if (normalized === "draft") return "Draft";
  return status || "Unknown";
}

function eventStatus(kind) {
  const normalized = safeStatus(kind);
  if (normalized.includes("reply") || normalized.includes("message") || normalized.includes("status")) return "talking";
  if (normalized.includes("start") || normalized.includes("think")) return "thinking";
  if (normalized.includes("done") || normalized.includes("complete")) return "done";
  if (normalized.includes("block")) return "blocked";
  return "ready";
}

function taskStatus(status) {
  const normalized = String(status || "").toLowerCase();
  if (normalized.includes("qa") || normalized.includes("pending")) return "qa";
  if (normalized.includes("progress") || normalized.includes("opening")) return "thinking";
  if (normalized.includes("accepted")) return "accepted";
  if (normalized.includes("complete")) return "done";
  if (normalized.includes("ready")) return "ready";
  if (normalized.includes("await")) return "waiting";
  if (normalized.includes("draft")) return "draft";
  return "ready";
}

function initials(name) {
  const parts = String(name || "?").trim().split(/\s+/).filter(Boolean);
  if (!parts.length) return "?";
  if (parts.length === 1) return parts[0].slice(0, 2).toUpperCase();
  return parts.slice(0, 2).map(part => part[0]).join("").toUpperCase();
}

function createElement(tag, className, text) {
  const element = document.createElement(tag);
  if (className) element.className = className;
  if (text !== undefined && text !== null) element.textContent = text;
  return element;
}

function createStatus(status, label = statusText(status)) {
  const pill = createElement("span", `status ${safeStatus(status)}`, label);
  return pill;
}

function createThinkingLogo() {
  const logo = createElement("span", "thinking-logo is-inline");
  logo.setAttribute("aria-hidden", "true");
  logo.append(createElement("span"));
  return logo;
}

function formatClock(date = new Date()) {
  return date.toLocaleTimeString([], {
    hour: "2-digit",
    minute: "2-digit",
    second: "2-digit"
  });
}

function setWorkspaceStatus(message) {
  if (workspaceOpenStatus) workspaceOpenStatus.textContent = message;
  if (workspaceTopbarStatus) {
    workspaceTopbarStatus.textContent = message;
    workspaceTopbarStatus.hidden = !message;
    workspaceTopbarStatus.dataset.state = message ? "visible" : "hidden";
  }
}

function formatSourceTime(value) {
  if (!value) return "source time unknown";
  const parsed = new Date(value);
  if (Number.isNaN(parsed.getTime())) return value;
  return parsed.toLocaleTimeString([], {
    hour: "2-digit",
    minute: "2-digit",
    second: "2-digit"
  });
}

function eventKey(event) {
  return [event?.time, event?.displayTime, event?.speaker, event?.kind, event?.message, event?.snippet].filter(Boolean).join("|");
}

function eventDisplayTime(event) {
  return event?.displayTime || event?.time || "";
}

function eventMessage(event) {
  return event?.message || event?.snippet || event?.body || "";
}

function itemTime(item) {
  return item?.time || item?.createdAt || "";
}

function sortByTime(items) {
  return items.slice().sort((a, b) => String(itemTime(a)).localeCompare(String(itemTime(b))));
}

function dashboardHref(href) {
  if (!href) return "#";
  if (/^(https?:|mailto:|#)/.test(href)) return href;
  if (href.startsWith("/docs/") || href.startsWith("/repo/")) return href;
  if (href.startsWith("../../../../")) return `/repo/${href.slice(12)}`;
  if (href.startsWith("../../")) return `/docs/${href.slice(6)}`;
  if (href.startsWith("../")) return `/docs/${href.slice(3)}`;
  if (href === "README.md") return "/docs/dashboard/data/README.md";
  if (href === "dashboard.schema.json") return "/docs/dashboard/data/dashboard.schema.json";
  return href;
}

function normalizeDocumentLinks(root = document) {
  for (const link of root.querySelectorAll("a[href]")) {
    const href = link.getAttribute("href");
    const normalized = dashboardHref(href);
    if (normalized !== href) link.setAttribute("href", normalized);
  }
}

function allAgents(data) {
  const liveAgents = Array.isArray(data?.liveAgents) ? data.liveAgents : [];
  const agents = Array.isArray(data?.agents) ? data.agents : [];
  const byId = new Map();

  for (const agent of [...liveAgents, ...agents]) {
    const id = agent.id || String(agent.agent || "").toLowerCase().replace(/[^a-z0-9]+/g, "-");
    byId.set(id, { ...agent, id });
  }

  return Array.from(byId.values());
}

function findAgent(data, agentId = selectedAgentId) {
  return allAgents(data).find(agent => agent.id === agentId) || allAgents(data)[0] || null;
}

function setSelectedAgent(agentId) {
  if (!agentId) return;
  selectedAgentId = agentId;
  localStorage.setItem("redBallSelectedAgentId", selectedAgentId);
  if (agentMessageFeedback) {
    agentMessageFeedback.textContent = relayAvailable
      ? "Relay ready. PM messages will queue through /api/messages."
      : "Local draft only; relay API unavailable.";
  }
  renderAgentDetail(latestData);
  renderSelectableState();
  document.querySelector("#agentDetailPanel")?.scrollIntoView({ block: "nearest", behavior: "smooth" });
}

async function openCodexAgent(agent, options = {}) {
  if (!agent) return;

  const agentName = agent.agent || "agent";
  if (codexHandoffStatus) {
    codexHandoffStatus.textContent = `Opening Codex for ${agentName}...`;
  }
  if (openCodexChatButton && !options.passive) {
    openCodexChatButton.disabled = true;
  }

  try {
    if (!canTryRelay()) {
      if (codexHandoffStatus) {
        codexHandoffStatus.textContent = "Open from the local dashboard server to use Codex handoff.";
      }
      return;
    }

    const response = await fetch(API_OPEN_CODEX_AGENT_URL, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      cache: "no-store",
      body: JSON.stringify({
        agentId: agent.id,
        codexAgentId: agent.codexAgentId || agent.agentPath || agent.id
      })
    });
    const payload = await response.json().catch(() => ({}));

    if (!response.ok || payload.ok === false) {
      throw new Error(payload.error || `HTTP ${response.status}`);
    }

    if (codexHandoffStatus) {
      codexHandoffMessage = payload.message || "Codex opened; direct agent deep link unavailable.";
      codexHandoffAgentId = agent.id;
      codexHandoffStatus.textContent = codexHandoffMessage;
    }
  } catch (error) {
    if (codexHandoffStatus) {
      codexHandoffStatus.textContent = `Codex handoff unavailable (${error.message}).`;
    }
  } finally {
    if (openCodexChatButton) {
      openCodexChatButton.disabled = !findAgent(latestData);
    }
  }
}

function outboundQueue() {
  try {
    const parsed = JSON.parse(localStorage.getItem(OUTBOUND_QUEUE_KEY) || "[]");
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
}

function saveOutboundQueue(queue) {
  localStorage.setItem(OUTBOUND_QUEUE_KEY, JSON.stringify(queue));
}

function addOutboundMessage(agent, message) {
  const now = new Date();
  const queue = outboundQueue();
  queue.push({
    id: `local-${now.getTime()}`,
    agentId: agent.id,
    speaker: "You",
    kind: "queued-outbound",
    message,
    time: now.toISOString(),
    displayTime: now.toLocaleString([], {
      month: "short",
      day: "2-digit",
      hour: "2-digit",
      minute: "2-digit",
      second: "2-digit"
    }),
    status: "PM dispatch pending"
  });
  saveOutboundQueue(queue);
  return queue.at(-1);
}

function outboxToTranscriptItem(item) {
  return {
    id: item.id,
    agentId: item.agentId,
    speaker: item.author || item.speaker || "PM",
    kind: item.kind || "pm-dispatch-queued",
    message: item.body || item.message || "",
    time: item.createdAt || item.time || "",
    displayTime: item.displayTime || item.createdAt || item.time || "",
    status: item.status || "PM dispatch pending"
  };
}

function eventIdentity(item) {
  return item?.id || eventKey(item);
}

function mergeByIdentity(...groups) {
  const seen = new Set();
  const merged = [];

  for (const group of groups) {
    if (!Array.isArray(group)) continue;
    for (const item of group) {
      const key = eventIdentity(item);
      if (key && seen.has(key)) continue;
      if (key) seen.add(key);
      merged.push(item);
    }
  }

  return sortByTime(merged);
}

function normalizeRelayState(payload) {
  if (!payload?.dashboard) return payload;

  const dashboard = { ...payload.dashboard };
  const liveEvents = Array.isArray(payload.liveEvents) ? payload.liveEvents : [];
  const outbox = Array.isArray(payload.outbox) ? payload.outbox : [];
  const outboxEvents = outbox.map(outboxToTranscriptItem);

  dashboard.relay = payload.relay || null;
  dashboard.outbox = outbox;
  dashboard.liveEvents = liveEvents;
  dashboard.events = mergeByIdentity(dashboard.events, liveEvents);
  dashboard.transcriptSnippets = mergeByIdentity(dashboard.transcriptSnippets, outboxEvents);

  return dashboard;
}

function applyRelayPatch(patch) {
  if (!latestData) return;

  const outbox = Array.isArray(patch?.outbox) ? patch.outbox : latestData.outbox || [];
  const liveEvents = Array.isArray(patch?.events) ? patch.events : latestData.liveEvents || [];
  renderLiveData(normalizeRelayState({
    relay: latestData.relay,
    dashboard: {
      ...latestData,
      events: mergeByIdentity(
        (latestData.events || []).filter(item => !String(item?.kind || "").startsWith("pm-dispatch")),
        liveEvents
      ),
      transcriptSnippets: (latestData.transcriptSnippets || []).filter(item => !outbox.some(message => message.id === item.id))
    },
    outbox,
    liveEvents
  }));
}

function applyAgentFilter(filter = activeFilter) {
  activeFilter = filter;
  const cards = Array.from(document.querySelectorAll("[data-agent-status]"));
  let visible = 0;

  for (const card of cards) {
    const shouldShow = filter === "all" || card.dataset.agentStatus === filter;
    card.hidden = !shouldShow;
    if (shouldShow) visible += 1;
  }

  if (visibleAgentCount) visibleAgentCount.textContent = String(visible);
  if (emptyState) emptyState.hidden = visible !== 0;
}

function renderSelectableState() {
  for (const item of document.querySelectorAll("[data-agent-select]")) {
    const isSelected = item.dataset.agentSelect === selectedAgentId;
    item.classList.toggle("is-selected", isSelected);
    if (item.tagName === "BUTTON") {
      item.setAttribute("aria-pressed", isSelected ? "true" : "false");
    }
  }
}

function makeAgentSelectable(element, agent) {
  if (!agent?.id) return element;

  element.dataset.agentSelect = agent.id;
  if (element.tagName !== "BUTTON") {
    element.setAttribute("role", "button");
    element.tabIndex = 0;
    element.addEventListener("keydown", event => {
      if (event.key === "Enter" || event.key === " ") {
        event.preventDefault();
        setSelectedAgent(agent.id);
      }
    });
  }

  element.addEventListener("click", event => {
    if (event.target.closest("a, button, input, textarea, select, summary, label")) return;
    setSelectedAgent(agent.id);
  });

  return element;
}

for (const button of filterButtons) {
  button.addEventListener("click", () => {
    for (const other of filterButtons) {
      other.classList.toggle("is-active", other === button);
      other.setAttribute("aria-pressed", other === button ? "true" : "false");
    }

    applyAgentFilter(button.dataset.filter);
  });
}

async function postRelayMessage(agent, message) {
  const response = await fetch(API_MESSAGES_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      agentId: agent.id,
      body: message,
      author: "PM"
    })
  });

  const payload = await response.json().catch(() => ({}));
  if (!response.ok || payload.ok === false) {
    throw new Error(payload.error || `HTTP ${response.status}`);
  }
  return payload;
}

async function openBuildWorkspace() {
  if (!openWorkspaceButton) return;

  setWorkspaceStatus("Opening workspace...");
  openWorkspaceButton.disabled = true;

  try {
    const response = await fetch(API_OPEN_WORKSPACE_URL, {
      method: "POST",
      cache: "no-store"
    });
    const payload = await response.json().catch(() => ({}));

    if (response.ok && payload.ok) {
      setWorkspaceStatus(payload.workspaceName
        ? `Opened ${payload.workspaceName}.`
        : "Workspace open request sent.");
      redBallMenu?.removeAttribute("open");
      return;
    }

    if (response.status === 404 || payload.status === "needs-build") {
      setWorkspaceStatus("No .xcworkspace found. Build workspace first.");
      return;
    }

    setWorkspaceStatus(payload.error || `Workspace open failed (HTTP ${response.status}).`);
  } catch (error) {
    setWorkspaceStatus(canTryRelay()
      ? `Workspace relay unavailable (${error.message}).`
      : "Open from the local dashboard server to use this.");
  } finally {
    openWorkspaceButton.disabled = false;
  }
}

openWorkspaceButton?.addEventListener("click", openBuildWorkspace);

openCodexChatButton?.addEventListener("click", () => {
  const agent = findAgent(latestData);
  if (agent) openCodexAgent(agent);
});

agentMessageForm?.addEventListener("submit", async event => {
  event.preventDefault();
  const agent = findAgent(latestData);
  const message = agentMessageInput?.value.trim();
  if (!agent || !message) return;

  const previousDisabled = agentMessageInput.disabled;
  agentMessageInput.disabled = true;

  try {
    if (canTryRelay()) {
      const payload = await postRelayMessage(agent, message);
      relayAvailable = true;
      agentMessageInput.value = "";
      if (payload.event || payload.message) {
        applyRelayPatch({
          events: payload.event ? [...(latestData?.liveEvents || []), payload.event] : latestData?.liveEvents || [],
          outbox: payload.message ? [...(latestData?.outbox || []), payload.message] : latestData?.outbox || []
        });
      }
      await refreshLiveData({ force: true });
      if (agentMessageFeedback) {
        agentMessageFeedback.textContent = `PM dispatch queued via relay for ${agent.agent}.`;
      }
      return;
    }
  } catch (error) {
    relayAvailable = false;
    if (agentMessageFeedback) {
      agentMessageFeedback.textContent = `Local draft only; relay unavailable (${error.message}).`;
    }
  } finally {
    agentMessageInput.disabled = previousDisabled;
  }

  addOutboundMessage(agent, message);
  agentMessageInput.value = "";
  if (agentMessageFeedback) {
    agentMessageFeedback.textContent = `Local draft only for ${agent.agent}. Relay API unavailable.`;
  }
  renderAgentDetail(latestData);
});

function mountLiveSection() {
  let section = document.querySelector("#live-json");
  if (section) return section;

  section = document.createElement("section");
  section.id = "live-json";
  section.className = "dashboard-section live-json-section";
  section.setAttribute("aria-labelledby", "live-json-title");
  section.innerHTML = `
    <div class="section-head compact">
      <div>
        <p class="eyebrow">Live JSON</p>
        <h2 id="live-json-title">Active Thinking Room</h2>
        <p class="summary" id="liveJsonSummary">Waiting for local dashboard data...</p>
      </div>
      <span class="status thinking" id="liveJsonState">Connecting</span>
    </div>
    <div class="live-json-grid">
      <div>
        <h3>Active / Recent Agents</h3>
        <div class="live-agent-list" id="liveJsonAgents" aria-live="polite"></div>
      </div>
      <div>
        <h3>Conversation Events</h3>
        <div class="live-event-list" id="liveJsonEvents" aria-live="polite"></div>
      </div>
    </div>
  `;

  const overview = document.querySelector("#overview");
  overview?.after(section);
  return section;
}

function linksForAgent(agent) {
  if (Array.isArray(agent.links)) return agent.links.map(dashboardHref);
  if (agent.link) return [dashboardHref(agent.link)];
  return [];
}

function createAgentCard(agent, isLive = false) {
  const agentRecord = { ...agent, id: agent.id || initials(agent.agent).toLowerCase() };
  const status = safeStatus(agent.status);
  const card = createElement("article", `agent-card${isLive ? " is-live" : ""} is-${status}`);
  card.dataset.agentStatus = status;

  const top = createElement("div", "agent-card-header");
  const avatar = createElement("span", "avatar", initials(agent.agent));
  top.append(avatar, createStatus(status));

  const role = createElement("p", "role", agent.role || "Production Agent");
  const title = createElement("h3", "", agent.agent || "Agent");
  const body = createElement("p", "", agent.lastUpdate || agent.output || agent.task || "No update yet.");

  card.append(top, role, title, body);

  if (agent.task && agent.task !== body.textContent) {
    card.append(createElement("p", "agent-task", agent.task));
  }

  const links = linksForAgent(agent);
  if (links.length) {
    const foot = createElement("div", "agent-foot");
    for (const href of links) {
      const link = createElement("a", "", href.includes("/reports/") ? "Report" : agent.task || "Record");
      link.href = href;
      foot.append(link);
    }
    card.append(foot);
  }

  return makeAgentSelectable(card, agentRecord);
}

function renderAgentGrid(liveAgents, agents) {
  if (!agentGrid) return;

  const liveNames = new Set(liveAgents.map(agent => String(agent.agent || "").toLowerCase()));
  const referenceAgents = agents.filter(agent => !liveNames.has(String(agent.agent || "").toLowerCase()));
  const cards = [
    ...liveAgents.map(agent => createAgentCard(agent, true)),
    ...referenceAgents.map(agent => createAgentCard(agent, false))
  ];

  agentGrid.replaceChildren(...cards);
  applyAgentFilter(activeFilter);
  renderSelectableState();
}

function createMiniAgent(agent) {
  const agentRecord = { ...agent, id: agent.id || initials(agent.agent).toLowerCase() };
  const status = safeStatus(agent.status);
  const item = createElement("button", `mini-agent is-${status}`);
  item.type = "button";
  const mark = status === "thinking"
    ? createThinkingLogo()
    : createElement("span", "mini-agent-mark", initials(agent.agent));

  const copy = createElement("span");
  copy.append(
    createElement("strong", "", agent.agent || "Agent"),
    createElement("small", "", `${statusText(status)} - ${agent.role || "Production"}`)
  );

  item.append(mark, copy);
  return makeAgentSelectable(item, agentRecord);
}

function createMiniMessage(event) {
  const item = createElement("article", "mini-message");
  const top = createElement("div", "mini-message-top");
  top.append(
    createElement("strong", "", event.speaker || "Agent"),
    createElement("time", "", eventDisplayTime(event))
  );
  item.append(top, createElement("p", "", eventMessage(event)));
  return item;
}

function renderSidebar(liveAgents, events, data) {
  const activeAgents = liveAgents.filter(agent => safeStatus(agent.status) !== "done");
  const thinkingAgents = liveAgents.filter(agent => safeStatus(agent.status) === "thinking");

  if (railThinkingLogo) {
    railThinkingLogo.hidden = thinkingAgents.length === 0;
  }

  if (railRefreshStatus) {
    const source = relayAvailable ? (sseConnected ? "relay SSE + state" : "relay state polling") : "JSON polling fallback";
    railRefreshStatus.textContent = `${source} - ${activeAgents.length} active - source ${formatSourceTime(data.updatedAt)}`;
  }

  if (railAgentList) {
    if (liveAgents.length) {
      railAgentList.replaceChildren(...liveAgents.map(createMiniAgent));
    } else {
      railAgentList.replaceChildren(createElement("p", "mini-empty", "No live agents in dashboard JSON."));
    }
  }

  if (railMessageStream) {
    const latest = events.slice(-MAX_MESSAGE_ITEMS).reverse();
    if (latest.length) {
      railMessageStream.replaceChildren(...latest.map(createMiniMessage));
    } else {
      railMessageStream.replaceChildren(createElement("p", "mini-empty", "No events logged yet."));
    }
  }

  renderSelectableState();
}

function renderLiveAgents(liveAgents) {
  const agentsRoot = document.querySelector("#liveJsonAgents");
  if (!agentsRoot) return;

  if (!liveAgents.length) {
    agentsRoot.replaceChildren(createElement("p", "empty-state", "No live agents in dashboard JSON."));
    return;
  }

  agentsRoot.replaceChildren(...liveAgents.map(agent => {
    const agentRecord = { ...agent, id: agent.id || initials(agent.agent).toLowerCase() };
    const status = safeStatus(agent.status);
    const card = createElement("article", `live-json-card is-${status}`);

    const top = createElement("div", "agent-card-header");
    const avatar = status === "thinking" ? createThinkingLogo() : createElement("span", "avatar", initials(agent.agent));
    top.append(avatar, createStatus(status));

    card.append(
      top,
      createElement("p", "role", agent.role || ""),
      createElement("h4", "", agent.agent || "Agent"),
      createElement("p", "", agent.task || ""),
      createElement("small", "", agent.lastUpdate || "")
    );
    return makeAgentSelectable(card, agentRecord);
  }));
  renderSelectableState();
}

function createLiveEvent(event, isLatest = false) {
  const item = createElement("article", `live-event${isLatest ? " is-new" : ""}`);

  const meta = createElement("div", "event-line");
  meta.append(
    createElement("time", "", eventDisplayTime(event)),
    createElement("strong", "", event.speaker || ""),
    createStatus(eventStatus(event.kind), event.kind || "event")
  );

  item.append(meta, createElement("p", "", eventMessage(event)));
  return item;
}

function renderEvents(events) {
  const eventsRoot = document.querySelector("#liveJsonEvents");
  if (!eventsRoot) return;

  const latestKey = eventKey(events.at(-1));
  const newestChanged = latestKey && latestKey !== latestEventKey;
  latestEventKey = latestKey || latestEventKey;

  const visibleEvents = events.slice(-MAX_EVENT_ITEMS).reverse();
  if (!visibleEvents.length) {
    eventsRoot.replaceChildren(createElement("p", "empty-state", "No events logged yet."));
    return;
  }

  eventsRoot.replaceChildren(...visibleEvents.map((event, index) => createLiveEvent(event, newestChanged && index === 0)));
}

function createMessageCard(event) {
  const card = createElement("article", "message-card");
  const head = createElement("div", "message-head");
  head.append(
    createElement("span", "avatar", initials(event.speaker)),
    createElement("strong", "", event.speaker || "Agent"),
    createStatus(eventStatus(event.kind), event.kind || "event")
  );
  card.append(head, createElement("p", "", eventMessage(event)));
  return card;
}

function renderMessageStream(events) {
  if (!liveMessageStream) return;

  const latest = events.slice(-MAX_MESSAGE_ITEMS).reverse();
  if (!latest.length) {
    liveMessageStream.replaceChildren(createElement("p", "empty-state", "No conversation messages yet."));
    return;
  }

  liveMessageStream.replaceChildren(...latest.map(createMessageCard));
}

function taskLink(task) {
  if (task.link) return dashboardHref(task.link);
  if (Array.isArray(task.links) && task.links.length) return dashboardHref(task.links[0]);
  if (task.id === "TASK-0013") return dashboardHref("../tasks/TASK-0013-asset-pipeline-downloads.md");
  if (task.id === "TASK-0014") return dashboardHref("../tasks/TASK-0014-marketing-production-assets.md");
  if (task.id === "TASK-0015") return dashboardHref("../tasks/TASK-0015-live-agent-dashboard-system.md");
  return dashboardHref(`../tasks/${task.id?.toLowerCase()}-${String(task.title || "").toLowerCase().replace(/[^a-z0-9]+/g, "-").replace(/^-|-$/g, "")}.md`);
}

function renderTasks(tasks) {
  if (!taskGrid || !Array.isArray(tasks) || !tasks.length) return;

  taskGrid.replaceChildren(...tasks.map(task => {
    const status = taskStatus(task.status);
    const card = createElement("article", `task-card ${status === "qa" || status === "thinking" || status === "waiting" ? "priority" : ""}`.trim());
    const top = createElement("div", "task-topline");
    const link = createElement("a", "", task.id || "Task");
    link.href = taskLink(task);
    top.append(link, createStatus(status, task.status || statusText(status)));
    card.append(top, createElement("h3", "", task.title || "Untitled task"));
    if (task.owner) card.append(createElement("p", "", `Owner: ${task.owner}`));
    return card;
  }));
}

function renderDecisions(decisions) {
  if (!decisionGrid || !Array.isArray(decisions) || !decisions.length) return;

  decisionGrid.replaceChildren(...decisions.map((decision, index) => {
    const card = createElement("article", "decision-card");
    card.append(
      createElement("span", "", String(index + 1).padStart(2, "0")),
      createElement("h3", "", decision.title || decision.area || "PM Decision"),
      createElement("p", "", typeof decision === "string" ? decision : decision.decision || decision.reason || "")
    );
    return card;
  }));
}

function conversationItemsForAgent(data, agent) {
  if (!data || !agent) return [];

  const agentId = agent.id;
  const storedConversations = data.agentConversations;
  let conversationItems = [];

  if (Array.isArray(storedConversations)) {
    conversationItems = storedConversations
      .filter(group => group.agentId === agentId)
      .flatMap(group => Array.isArray(group.entries) ? group.entries : []);
  } else if (storedConversations && Array.isArray(storedConversations[agentId])) {
    conversationItems = storedConversations[agentId];
  }

  const snippets = Array.isArray(data.transcriptSnippets)
    ? data.transcriptSnippets.filter(item => item.agentId === agentId)
    : [];
  const events = Array.isArray(data.events)
    ? data.events.filter(item => item.agentId === agentId)
    : [];
  const queued = outboundQueue().filter(item => item.agentId === agentId);
  const relayQueued = Array.isArray(data.outbox)
    ? data.outbox.filter(item => item.agentId === agentId).map(outboxToTranscriptItem)
    : [];

  const fallback = conversationItems.length || snippets.length || events.length || relayQueued.length || queued.length
    ? []
    : [{
      id: `${agentId}-fallback`,
      time: agent.lastMessageAt || agent.statusSince || "",
      speaker: agent.agent || "Agent",
      kind: "status",
      message: agent.lastUpdate || agent.output || agent.task || "No transcript entries yet."
    }];

  return mergeByIdentity(conversationItems, snippets, events, relayQueued, queued, fallback);
}

function createTranscriptItem(item, agent) {
  const kind = safeStatus(item.kind || item.status || "message");
  const bubble = createElement("article", `transcript-item is-${kind}`);
  const meta = createElement("div", "transcript-meta");
  meta.append(
    createElement("strong", "", item.speaker || agent.agent || "Agent"),
    createElement("time", "", eventDisplayTime(item)),
    createStatus(item.status === "PM dispatch pending" ? "waiting" : eventStatus(item.kind), item.status || item.kind || "message")
  );
  bubble.append(meta, createElement("p", "", eventMessage(item)));
  return bubble;
}

function renderAgentDetail(data) {
  const agent = findAgent(data);
  if (!agent) return;

  selectedAgentId = agent.id;
  localStorage.setItem("redBallSelectedAgentId", selectedAgentId);

  const status = safeStatus(agent.status);
  if (selectedAgentAvatar) {
    selectedAgentAvatar.className = `avatar${status === "thinking" ? " is-thinking-avatar" : ""}`;
    selectedAgentAvatar.textContent = initials(agent.agent);
  }
  if (selectedAgentName) selectedAgentName.textContent = agent.agent || "Agent";
  if (selectedAgentRole) selectedAgentRole.textContent = `${agent.role || "Production Agent"} - ${agent.task || "No assigned task"}`;
  if (selectedAgentStatus) {
    selectedAgentStatus.className = `status ${status}`;
    selectedAgentStatus.textContent = statusText(status);
  }
  if (openCodexChatButton) {
    openCodexChatButton.disabled = false;
    openCodexChatButton.dataset.agentId = agent.id;
  }
  if (codexHandoffStatus) {
    const chatStatus = agent.codexChat?.status || "Open Codex Chat opens Codex; dashboard transcript remains visible here.";
    codexHandoffStatus.textContent = codexHandoffAgentId === agent.id && codexHandoffMessage
      ? codexHandoffMessage
      : chatStatus;
  }
  if (agentMessageInput) {
    agentMessageInput.disabled = false;
    agentMessageInput.placeholder = `Write a note for ${agent.agent || "this agent"}...`;
  }
  if (
    agentMessageFeedback
    && !agentMessageFeedback.textContent.includes("PM dispatch queued via relay")
    && !agentMessageFeedback.textContent.includes("Local draft only for")
  ) {
    agentMessageFeedback.textContent = relayAvailable
      ? "Relay ready. PM dispatches queue through /api/messages."
      : "Local draft only; relay API unavailable.";
  }

  if (agentTranscript) {
    const items = conversationItemsForAgent(data, agent);
    agentTranscript.replaceChildren(...items.map(item => createTranscriptItem(item, agent)));
  }
}

function renderWorkflowList(root, items, emptyText) {
  if (!root) return;
  if (!Array.isArray(items) || !items.length) {
    root.replaceChildren(createElement("p", "empty-state", emptyText));
    return;
  }

  root.replaceChildren(...items.map(item => {
    const entry = createElement("article", "workflow-item");
    const label = item.title || item.area || item.id || item.taskId || "Workflow item";
    const body = item.note || item.decision || item.reason || item.status || item.message || "";
    entry.append(
      createElement("strong", "", label),
      createElement("p", "", body)
    );
    if (item.owner || item.time || item.priority) {
      entry.append(createElement("small", "", [item.owner, item.priority, item.displayTime || item.time].filter(Boolean).join(" - ")));
    }
    return entry;
  }));
}

function renderWorkflow(data) {
  const meetingNotes = Array.isArray(data.meetingNotes) ? data.meetingNotes : [];
  const groomingItems = Array.isArray(data.groomingQueue) && data.groomingQueue.length
    ? data.groomingQueue
    : (Array.isArray(data.tasks) ? data.tasks.slice(0, 6) : []);
  const decisions = Array.isArray(data.pmDecisionLog) ? data.pmDecisionLog.slice(-6).reverse() : [];

  renderWorkflowList(meetingNotesList, meetingNotes, "No meeting notes in dashboard data yet.");
  renderWorkflowList(groomingQueue, groomingItems, "No task grooming items in dashboard data yet.");
  renderWorkflowList(workflowDecisionList, decisions, "No PM decisions in dashboard data yet.");
}

function renderRefreshStatus(data, liveAgents, events) {
  const activeCount = liveAgents.filter(agent => safeStatus(agent.status) !== "done").length;
  const thinkingCount = liveAgents.filter(agent => safeStatus(agent.status) === "thinking").length;
  const localTime = formatClock();

  if (dashboardRefreshStatus) {
    const transport = relayAvailable ? (sseConnected ? "relay SSE" : "relay state") : "JSON fallback";
    dashboardRefreshStatus.textContent = `Live - ${transport} - last pull ${localTime}`;
    dashboardRefreshStatus.className = "refresh-meter is-live";
  }

  const summary = document.querySelector("#liveJsonSummary");
  if (summary) {
    summary.textContent = `${activeCount} active agents, ${thinkingCount} thinking, ${events.length} logged events. Source updated ${data.updatedAt || "locally"}.`;
  }

  const state = document.querySelector("#liveJsonState");
  if (state) {
    state.className = "status talking";
    state.textContent = relayAvailable
      ? `Relay live ${localTime}`
      : `Fallback live ${localTime}`;
  }
}

function renderLiveData(data) {
  mountLiveSection();
  latestData = data;
  normalizeDocumentLinks();

  const liveAgents = Array.isArray(data.liveAgents) ? data.liveAgents : [];
  const agents = Array.isArray(data.agents) ? data.agents : [];
  const events = Array.isArray(data.events) ? data.events : [];
  const transcriptSnippets = Array.isArray(data.transcriptSnippets) ? data.transcriptSnippets : [];
  const conversationItems = transcriptSnippets.length ? transcriptSnippets : events;

  if (!selectedAgentId || !findAgent(data, selectedAgentId)) {
    selectedAgentId = findAgent(data)?.id || "";
  }

  renderRefreshStatus(data, liveAgents, events);
  renderSidebar(liveAgents, events, data);
  renderLiveAgents(liveAgents);
  renderEvents(events);
  renderMessageStream(conversationItems);
  renderAgentGrid(liveAgents, agents);
  renderTasks(data.tasks);
  renderDecisions(data.pmDecisionLog || data.decisions);
  renderAgentDetail(data);
  renderWorkflow(data);
  renderSelectableState();
}

function renderOfflineState(error) {
  mountLiveSection();

  if (dashboardRefreshStatus) {
    dashboardRefreshStatus.textContent = "Static file mode - launch with .command for live updates";
    dashboardRefreshStatus.className = "refresh-meter is-waiting";
  }

  if (railRefreshStatus) {
    railRefreshStatus.textContent = "Live JSON unavailable. Use open-ai-production-dashboard.command.";
  }

  if (railThinkingLogo) railThinkingLogo.hidden = true;

  const summary = document.querySelector("#liveJsonSummary");
  if (summary) {
    summary.textContent = `Live JSON is unavailable from this page mode. Open with the local server launcher. ${error?.message || ""}`.trim();
  }

  const state = document.querySelector("#liveJsonState");
  if (state) {
    state.className = "status waiting";
    state.textContent = "Static mode";
  }
}

normalizeDocumentLinks();

async function fetchRelayState() {
  const response = await fetch(`${API_STATE_URL}?t=${Date.now()}`, { cache: "no-store" });
  if (!response.ok) throw new Error(`HTTP ${response.status}`);
  return normalizeRelayState(await response.json());
}

async function fetchDashboardJson() {
  const response = await fetch(`${DATA_URL}?t=${Date.now()}`, { cache: "no-store" });
  if (!response.ok) throw new Error(`HTTP ${response.status}`);
  return response.json();
}

async function refreshLiveData(options = {}) {
  if (isRefreshing && !options.force) return;
  isRefreshing = true;

  try {
    if (canTryRelay()) {
      try {
        const relayData = await fetchRelayState();
        relayAvailable = true;
        renderLiveData(relayData);
        connectEventStream();
        return;
      } catch (relayError) {
        relayAvailable = false;
        closeEventStream();
        if (agentMessageFeedback) {
          agentMessageFeedback.textContent = `Local draft only; relay unavailable (${relayError.message}).`;
        }
      }
    }

    renderLiveData(await fetchDashboardJson());
  } catch (error) {
    renderOfflineState(error);
  } finally {
    isRefreshing = false;
  }
}

function connectEventStream() {
  if (!canTryRelay() || eventSource) return;

  eventSource = new EventSource(API_EVENTS_URL);
  eventSource.addEventListener("open", () => {
    sseConnected = true;
    if (latestData) renderLiveData(latestData);
  });
  eventSource.addEventListener("state", event => {
    sseConnected = true;
    try {
      applyRelayPatch(JSON.parse(event.data));
    } catch (error) {
      console.warn("Unable to parse relay SSE state", error);
    }
  });
  eventSource.addEventListener("error", () => {
    sseConnected = false;
    closeEventStream();
  });
}

function closeEventStream() {
  if (eventSource) {
    eventSource.close();
    eventSource = null;
  }
  sseConnected = false;
}

applyAgentFilter(activeFilter);
refreshLiveData();
window.setInterval(refreshLiveData, REFRESH_MS);
