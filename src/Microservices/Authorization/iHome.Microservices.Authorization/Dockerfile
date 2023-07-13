#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["iHome.Microservices.Authorization/NuGet.config", "iHome.Microservices.Authorization/"]
COPY ["iHome.Microservices.Authorization/NuGet.config", "iHome.Microservices.Authorization.Infrastructure/"]
COPY ["iHome.Microservices.Authorization/iHome.Microservices.Authorization.csproj", "iHome.Microservices.Authorization/"]
COPY ["iHome.Microservices.Authorization.Contract/iHome.Microservices.Authorization.Contract.csproj", "iHome.Microservices.Authorization.Contract/"]
COPY ["iHome.Microservices.Authorization.Infrastructure/iHome.Microservices.Authorization.Infrastructure.csproj", "iHome.Microservices.Authorization.Infrastructure/"]
RUN dotnet restore "iHome.Microservices.Authorization/iHome.Microservices.Authorization.csproj"
COPY . .
WORKDIR "/src/iHome.Microservices.Authorization"
RUN dotnet build "iHome.Microservices.Authorization.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "iHome.Microservices.Authorization.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "iHome.Microservices.Authorization.dll"]