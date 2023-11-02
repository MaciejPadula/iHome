#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["iHome/NuGet.config", "iHome.Core/"]
COPY ["iHome/NuGet.config", "iHome/"]
COPY ["iHome/iHome.csproj", "iHome/"]
COPY ["iHome.Core/iHome.Core.csproj", "iHome.Core/"]
RUN dotnet restore "iHome/iHome.csproj"
COPY . .
WORKDIR "/src/iHome"
RUN dotnet build "iHome.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "iHome.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "iHome.dll"]