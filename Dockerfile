FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 18000

ENV ASPNETCORE_URLS=http://+:18000
ENV ASPNETCORE_ENVIRONMENT=Development
ENV PORT=3200
ENV CLIENT_URL=http://localhost
ENV IS_TITAN=false

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# copy all the layers' csproj files into respective folders
COPY ["src/Resrcify.DataProvider.Domain/*.csproj", "src/Resrcify.DataProvider.Domain/"]
COPY ["src/Resrcify.DataProvider.Application/*.csproj", "src/Resrcify.DataProvider.Application/"]
COPY ["src/Resrcify.DataProvider.Infrastructure/*.csproj", "src/Resrcify.DataProvider.Infrastructure/"]
COPY ["src/Resrcify.DataProvider.Presentation/*.csproj", "src/Resrcify.DataProvider.Presentation/"]
COPY ["src/Resrcify.DataProvider.Web/*.csproj", "src/Resrcify.DataProvider.Web/"]
COPY "*.sln" .

RUN dotnet restore

COPY src/ src/

FROM build AS debug-build
RUN apt-get update && \
    apt-get install -y --no-install-recommends unzip && \
    rm -rf /var/lib/apt/lists/*
RUN dotnet publish -c Debug --property:PublishDir=/app/publish --no-restore
RUN curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /app/vsdbg

FROM base AS debug
COPY --from=build /src src/
COPY --from=debug-build /app/publish .
COPY --from=debug-build /app/vsdbg /app/vsdbg
ENTRYPOINT ["dotnet", "Resrcify.DataProvider.Web.dll"]

FROM build AS release-build
RUN dotnet publish -c Release --property:PublishDir=/app/publish --no-restore

FROM base AS release
COPY --from=release-build /app/publish .
ENTRYPOINT ["dotnet", "Resrcify.DataProvider.Web.dll"]