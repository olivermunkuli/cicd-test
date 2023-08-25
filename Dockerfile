FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app

FROM base AS final
WORKDIR /app
RUN apt-get update && apt-get install -y nano bash

# Copy the already built and published output of the projects from GitHub Actions
COPY DockerTester/bin/Release/net7.0/publish /app/DockerTester
COPY AutogasSA.Common.Logging/bin/Release/net7.0/publish /app/AutogasSA.Common.Logging
COPY AutogasSA.Common.Logging.Interfaces/bin/Release/net7.0/publish /app/AutogasSA.Common.Logging.Interfaces
COPY AutogasSA.Common.Utilities/bin/Release/net7.0/publish /app/AutogasSA.Common.Utilities

COPY DockerTester/*.json . 
RUN chmod g+w /app/appsettings.json && chown -R appuser:root /app/appsettings.json

USER appuser
ENTRYPOINT ["dotnet", "DockerTester/DockerTester.dll"]
