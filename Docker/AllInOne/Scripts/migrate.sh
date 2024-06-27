#!/bin/bash

echo "Creating Migration"
dotnet ef migrations add Initial -p /Task/Task/Task.csproj 

echo "Applying Migration"
dotnet ef database update -p /Task/Task/Task.csproj