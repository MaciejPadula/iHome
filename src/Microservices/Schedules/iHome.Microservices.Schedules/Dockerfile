#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["iHome.Microservices.Schedules/NuGet.config", "iHome.Microservices.Schedules/"]
COPY ["iHome.Microservices.Schedules/NuGet.config", "iHome.Microservices.Schedules.Infrastructure/"]
COPY ["iHome.Microservices.Schedules/iHome.Microservices.Schedules.csproj", "iHome.Microservices.Schedules/"]
COPY ["iHome.Microservices.Schedules.Contract/iHome.Microservices.Schedules.Contract.csproj", "iHome.Microservices.Schedules.Contract/"]
COPY ["iHome.Microservices.Schedules.Infrastructure/iHome.Microservices.Schedules.Infrastructure.csproj", "iHome.Microservices.Schedules.Infrastructure/"]
RUN dotnet restore "iHome.Microservices.Schedules/iHome.Microservices.Schedules.csproj"
COPY . .
WORKDIR "/src/iHome.Microservices.Schedules"
RUN dotnet build "iHome.Microservices.Schedules.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "iHome.Microservices.Schedules.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "iHome.Microservices.Schedules.dll"]