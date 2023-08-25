FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DockerTester/DockerTester.csproj", "DockerTester/"]
COPY ["AutogasSA.Common.Logging/AutogasSA.Common.Logging.csproj", "AutogasSA.Common.Logging/"]
COPY ["AutogasSA.Common.Logging.Interfaces/AutogasSA.Common.Logging.Interfaces.csproj", "AutogasSA.Common.Logging.Interfaces/"]
COPY ["AutogasSA.Common.Utilities/AutogasSA.Common.Utilities.csproj", "AutogasSA.Common.Utilities/"]
RUN dotnet restore "DockerTester/DockerTester.csproj"
COPY . .
WORKDIR "/src/DockerTester"
RUN dotnet build "DockerTester.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerTester.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN apt-get update && apt-get install -y nano bash

# Copy the published output of the projects from GitHub Actions
COPY --from=publish /app/publish/DockerTester /app/DockerTester
COPY --from=publish /app/publish/AutogasSA.Common.Logging /app/AutogasSA.Common.Logging
COPY --from=publish /app/publish/AutogasSA.Common.Logging.Interfaces /app/AutogasSA.Common.Logging.Interfaces
COPY --from=publish /app/publish/AutogasSA.Common.Utilities /app/AutogasSA.Common.Utilities

COPY DockerTester/*.json . 
RUN chmod g+w /app/appsettings.json && chown -R appuser:root /app/appsettings.json

USER appuser
ENTRYPOINT ["dotnet", "DockerTester/DockerTester.dll"]
