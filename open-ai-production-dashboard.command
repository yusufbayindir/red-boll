#!/bin/zsh
set -e

PROJECT_DIR="/Users/yusufbayindir/Desktop/ai game/red_ball"
DASHBOARD_DIR="$PROJECT_DIR/docs/ai-production/dashboard"
BASE_PORT="${RED_BALL_DASHBOARD_PORT:-8765}"
PORT="$BASE_PORT"

cd "$DASHBOARD_DIR"

while lsof -iTCP:"$PORT" -sTCP:LISTEN >/dev/null 2>&1; do
  if command -v curl >/dev/null 2>&1 && curl -fs "http://127.0.0.1:${PORT}/api/health" >/dev/null 2>&1; then
    URL="http://127.0.0.1:${PORT}/index.html"
    open "$URL"
    exit 0
  fi
  PORT=$((PORT + 1))
  if [ "$PORT" -gt $((BASE_PORT + 20)) ]; then
    echo "No available local dashboard port found from $BASE_PORT to $((BASE_PORT + 20))."
    exit 1
  fi
done

URL="http://127.0.0.1:${PORT}/index.html"

if ! lsof -iTCP:"$PORT" -sTCP:LISTEN >/dev/null 2>&1; then
  if command -v python3 >/dev/null 2>&1; then
    nohup python3 "$DASHBOARD_DIR/server.py" --port "$PORT" --host 127.0.0.1 > "$PROJECT_DIR/docs/ai-production/dashboard/dashboard-server.log" 2>&1 &
    sleep 1
  else
    open "$DASHBOARD_DIR/index.html"
    exit 0
  fi
fi

open "$URL"
