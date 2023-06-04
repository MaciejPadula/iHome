#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["iHome.Microservices.Devices/iHome.Microservices.Devices.csproj", "iHome.Microservices.Devices/"]
COPY ["iHome.Microservices.Devices.Contract/iHome.Microservices.Devices.Contract.csproj", "iHome.Microservices.Devices.Contract/"]
COPY ["iHome.Microservices.Devices.Infrastructure/iHome.Microservices.Devices.Infrastructure.csproj", "iHome.Microservices.Devices.Infrastructure/"]
RUN dotnet restore "iHome.Microservices.Devices/iHome.Microservices.Devices.csproj"
COPY . .
WORKDIR "/src/iHome.Microservices.Devices"
RUN dotnet build "iHome.Microservices.Devices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "iHome.Microservices.Devices.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "iHome.Microservices.Devices.dll"]