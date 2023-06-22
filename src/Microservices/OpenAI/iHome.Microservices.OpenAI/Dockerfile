#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["iHome.Microservices.OpenAI/NuGet.config", "iHome.Microservices.OpenAI/"]
COPY ["iHome.Microservices.OpenAI/NuGet.config", "iHome.Microservices.OpenAI.Infrastructure/"]
COPY ["iHome.Microservices.OpenAI/iHome.Microservices.OpenAI.csproj", "iHome.Microservices.OpenAI/"]
COPY ["iHome.Microservices.OpenAI.Contract/iHome.Microservices.OpenAI.Contract.csproj", "iHome.Microservices.OpenAI.Contract/"]
COPY ["iHome.Microservices.OpenAI.Infrastructure/iHome.Microservices.OpenAI.Infrastructure.csproj", "iHome.Microservices.OpenAI.Infrastructure/"]
RUN dotnet restore "iHome.Microservices.OpenAI/iHome.Microservices.OpenAI.csproj"
COPY . .
WORKDIR "/src/iHome.Microservices.OpenAI"
RUN dotnet build "iHome.Microservices.OpenAI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "iHome.Microservices.OpenAI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "iHome.Microservices.OpenAI.dll"]