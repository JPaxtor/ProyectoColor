# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy the solution and project files
COPY ["DetectorColores.sln", "."]
COPY ["DetectorColores/DetectorColores.csproj", "DetectorColores/"]

# Restore dependencies
RUN dotnet restore "DetectorColores.sln"

# Copy the rest of the source code
COPY ["DetectorColores/", "DetectorColores/"]

# Build the application
WORKDIR "/src/DetectorColores"
RUN dotnet build "DetectorColores.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "DetectorColores.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Expose the default ASP.NET Core port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/ || exit 1

# Run the application
ENTRYPOINT ["dotnet", "DetectorColores.dll"]

