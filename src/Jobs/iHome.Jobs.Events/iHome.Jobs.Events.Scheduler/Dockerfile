#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["iHome.Jobs.Events.Scheduler/NuGet.config", "iHome.Jobs.Events.Scheduler/"]
COPY ["iHome.Jobs.Events.Scheduler/NuGet.config", "iHome.Jobs.Events.Infrastructure/"]
COPY ["iHome.Jobs.Events.Scheduler/iHome.Jobs.Events.Scheduler.csproj", "iHome.Jobs.Events.Scheduler/"]
COPY ["iHome.Jobs.Events.Infrastructure/iHome.Jobs.Events.Infrastructure.csproj", "iHome.Jobs.Events.Infrastructure/"]
RUN dotnet restore "iHome.Jobs.Events.Scheduler/iHome.Jobs.Events.Scheduler.csproj"
COPY . .
WORKDIR "/src/iHome.Jobs.Events.Scheduler"
RUN dotnet build "iHome.Jobs.Events.Scheduler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "iHome.Jobs.Events.Scheduler.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "iHome.Jobs.Events.Scheduler.dll"]