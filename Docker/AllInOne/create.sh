#!/bin/bash

# Step 1: Start the first service
echo "Starting SQL Server..."
docker-compose up --build --force-recreate -d sql_server

echo "Waiting for sql_server to be fully running..."
while ! docker inspect --format "{{.State.Health.Status}}" sql_server | grep -q "healthy"; do
    sleep 1
done

echo "SQL Server is running."

echo "Building and starting task..."
docker-compose up --build --force-recreate -d task

echo "Waiting for task to be fully running..."
until [ "$(docker inspect -f {{.State.Running}} task)" == "true" ]; do
    sleep 1
done

echo "Task is running"

docker-compose up --build --force-recreate -d task_migration

docker-compose ps

$SHELL