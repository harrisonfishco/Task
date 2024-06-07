#!/bin/bash

echo "Creating Migration"
dotnet ef migrations add Initial -p ./App/Task.csproj 

echo "Applying Migration"
dotnet ef database update -p ./App/Task.csproj