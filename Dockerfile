FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 18000

ENV ASPNETCORE_URLS=http://+:18000
ENV ASPNETCORE_ENVIRONMENT Development
ENV PORT 3200
ENV CLIENT_URL http://localhost
ENV IS_TITAN false

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# copy all the layers' csproj files into respective folders
COPY ["src/Core/Titan.DataProvider.Domain/Titan.DataProvider.Domain.csproj", "Core/Titan.DataProvider.Domain/"]
COPY ["src/Core/Titan.DataProvider.Application/Titan.DataProvider.Application.csproj", "Core/Titan.DataProvider.Application/"]
COPY ["src/Infrastructure/Titan.DataProvider.Infrastructure/Titan.DataProvider.Infrastructure.csproj", "Infrastructure/Titan.DataProvider.Infrastructure/"]
COPY ["src/API/Titan.DataProvider.API/Titan.DataProvider.API.csproj", "API/Titan.DataProvider.API/"]

RUN dotnet restore "API/Titan.DataProvider.API/Titan.DataProvider.API.csproj"
COPY . .
RUN dotnet build -c Release --property:OutputPath=/app/build

FROM build AS publish
RUN dotnet publish -c Release --property:PublishDir=/app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Titan.DataProvider.API.dll"]