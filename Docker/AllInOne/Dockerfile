FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
ARG BRANCH=master
ARG TASK_CONNECTIONSTRING
ENV TASK_CONNECTIONSTRING=${TASK_CONNECTIONSTRING}
WORKDIR /App

WORKDIR /
RUN git clone --single-branch --branch ${BRANCH} https://github.com/harrisonfishco/Task.git

WORKDIR /Task/Task
RUN dotnet restore ./Task.csproj
RUN dotnet publish ./Task.csproj -c Release -o out

WORKDIR /

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App

COPY --from=build-env /Task/Task/out .

WORKDIR /App
ENTRYPOINT ["dotnet", "Task.dll"]
