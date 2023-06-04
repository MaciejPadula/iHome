#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["iHome.Microservices.RoomsManagement/iHome.Microservices.RoomsManagement.csproj", "iHome.Microservices.RoomsManagement/"]
COPY ["iHome.Microservices.RoomsManagement.Contract/iHome.Microservices.RoomsManagement.Contract.csproj", "iHome.Microservices.RoomsManagement.Contract/"]
COPY ["iHome.Microservices.RoomsManagement.Infrastructure/iHome.Microservices.RoomsManagement.Infrastructure.csproj", "iHome.Microservices.RoomsManagement.Infrastructure/"]
RUN dotnet restore "iHome.Microservices.RoomsManagement/iHome.Microservices.RoomsManagement.csproj"
COPY . .
WORKDIR "/src/iHome.Microservices.RoomsManagement"
RUN dotnet build "iHome.Microservices.RoomsManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "iHome.Microservices.RoomsManagement.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "iHome.Microservices.RoomsManagement.dll"]