FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

ARG BRANCH=master
ENV BRANCH=$BRANCH

WORKDIR /
RUN git clone --single-branch --branch ${BRANCH} https://github.com/harrisonfishco/Task.git

WORKDIR /Task/Task
RUN dotnet restore ./Task.csproj
RUN dotnet publish ./Task.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App

COPY --from=build-env /Task/Task/out .
ENTRYPOINT ["dotnet", "Task.dll"]