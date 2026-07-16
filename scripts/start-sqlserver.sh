#!/usr/bin/env bash
set -euo pipefail

CONTAINER_NAME="tasktracking-sqlserver"
IMAGE_NAME="mcr.microsoft.com/mssql/server:2022-latest"
SA_PASSWORD="${MSSQL_SA_PASSWORD:-YourStrong!Passw0rd}"

if ! command -v docker >/dev/null 2>&1; then
  echo "Docker is not installed or not available in PATH."
  exit 1
fi

if docker ps --format '{{.Names}}' | grep -qx "$CONTAINER_NAME"; then
  echo "SQL Server container is already running: $CONTAINER_NAME"
  exit 0
fi

if docker ps -a --format '{{.Names}}' | grep -qx "$CONTAINER_NAME"; then
  echo "Starting existing SQL Server container: $CONTAINER_NAME"
  docker start "$CONTAINER_NAME" >/dev/null
else
  echo "Creating SQL Server container: $CONTAINER_NAME"
  docker run \
    -e "ACCEPT_EULA=Y" \
    -e "MSSQL_SA_PASSWORD=$SA_PASSWORD" \
    -e "MSSQL_PID=Developer" \
    -p 1433:1433 \
    --name "$CONTAINER_NAME" \
    --hostname "$CONTAINER_NAME" \
    -d "$IMAGE_NAME" >/dev/null
fi

echo "Waiting for SQL Server to become ready..."
for attempt in {1..60}; do
  logs="$(docker logs "$CONTAINER_NAME" 2>&1 || true)"
  if grep -q "SQL Server is now ready for client connections" <<< "$logs"; then
    echo "SQL Server is ready."
    echo "Connection string:"
    echo "Server=localhost,1433;Database=TaskTrackingDb;User Id=sa;Password=$SA_PASSWORD;TrustServerCertificate=True;"
    exit 0
  fi

  sleep 2
done

echo "SQL Server container started, but readiness was not confirmed within 120 seconds."
echo "Check logs with: docker logs $CONTAINER_NAME"
