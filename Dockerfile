FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app

FROM base AS final
WORKDIR /app
RUN apt-get update && apt-get install -y nano bash

# Copy the published output of the projects from previous build stage
COPY --from=publish /app/publish/DockerTester /app/DockerTester
COPY --from=publish /app/publish/AutogasSA.Common.Logging /app/AutogasSA.Common.Logging
COPY --from=publish /app/publish/AutogasSA.Common.Logging.Interfaces /app/AutogasSA.Common.Logging.Interfaces
COPY --from=publish /app/publish/AutogasSA.Common.Utilities /app/AutogasSA.Common.Utilities

COPY DockerTester/*.json . 
RUN chmod g+w /app/appsettings.json && chown -R appuser:root /app/appsettings.json

USER appuser
ENTRYPOINT ["dotnet", "DockerTester/DockerTester.dll"]
