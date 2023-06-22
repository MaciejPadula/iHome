#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["iHome.Microservices.Widgets/NuGet.config", "iHome.Microservices.Widgets/"]
COPY ["iHome.Microservices.Widgets/NuGet.config", "iHome.Microservices.Widgets.Infrastructure/"]
COPY ["iHome.Microservices.Widgets/iHome.Microservices.Widgets.csproj", "iHome.Microservices.Widgets/"]
COPY ["iHome.Microservices.Widgets.Contract/iHome.Microservices.Widgets.Contract.csproj", "iHome.Microservices.Widgets.Contract/"]
COPY ["iHome.Microservices.Widgets.Infrastructure/iHome.Microservices.Widgets.Infrastructure.csproj", "iHome.Microservices.Widgets.Infrastructure/"]
RUN dotnet restore "iHome.Microservices.Widgets/iHome.Microservices.Widgets.csproj"
COPY . .
WORKDIR "/src/iHome.Microservices.Widgets"
RUN dotnet build "iHome.Microservices.Widgets.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "iHome.Microservices.Widgets.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "iHome.Microservices.Widgets.dll"]