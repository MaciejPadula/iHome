#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["iHome.Microservices.UsersApi/NuGet.config", "iHome.Microservices.UsersApi/"]
COPY ["iHome.Microservices.UsersApi/NuGet.config", "iHome.Microservices.UsersApi.Infrastructure/"]
COPY ["iHome.Microservices.UsersApi/iHome.Microservices.UsersApi.csproj", "iHome.Microservices.UsersApi/"]
COPY ["iHome.Microservices.UsersApi.Contract/iHome.Microservices.UsersApi.Contract.csproj", "iHome.Microservices.UsersApi.Contract/"]
COPY ["iHome.Microservices.UsersApi.Infrastructure/iHome.Microservices.UsersApi.Infrastructure.csproj", "iHome.Microservices.UsersApi.Infrastructure/"]
RUN dotnet restore "iHome.Microservices.UsersApi/iHome.Microservices.UsersApi.csproj"
COPY . .
WORKDIR "/src/iHome.Microservices.UsersApi"
RUN dotnet build "iHome.Microservices.UsersApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "iHome.Microservices.UsersApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "iHome.Microservices.UsersApi.dll"]