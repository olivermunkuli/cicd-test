FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app

FROM base AS final
WORKDIR /app
RUN apt-get update && apt-get install -y nano bash

# Copy the published output of the projects from GitHub Actions
COPY --from=build /src/DockerTester/bin/Release/net7.0/publish /app/DockerTester
COPY --from=build /src/AutogasSA.Common.Logging/bin/Release/net7.0/publish /app/AutogasSA.Common.Logging
COPY --from=build /src/AutogasSA.Common.Logging.Interfaces/bin/Release/net7.0/publish /app/AutogasSA.Common.Logging.Interfaces
COPY --from=build /src/AutogasSA.Common.Utilities/bin/Release/net7.0/publish /app/AutogasSA.Common.Utilities

COPY DockerTester/*.json . 
RUN chmod g+w /app/appsettings.json && chown -R appuser:root /app/appsettings.json

USER appuser
ENTRYPOINT ["dotnet", "DockerTester/DockerTester.dll"]
