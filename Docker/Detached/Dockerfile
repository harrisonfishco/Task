FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

ARG BRANCH
ENV BRANCH=$BRANCH

WORKDIR /
RUN git clone --single-branch --branch master https://github.com/harrisonfishco/Task.git

WORKDIR /App
RUN cp /Task/Task/* ./ -r

RUN dotnet restore ./Task.csproj
RUN dotnet publish ./Task.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App

COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "Task.dll"]