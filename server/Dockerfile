# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Weather.API/Weather.API.csproj", "src/Weather.API/"]
COPY ["src/Weather.Domain/Weather.Domain.csproj", "src/Weather.Domain/"]
COPY ["src/Weather.Application/Weather.Application.csproj", "src/Weather.Application/"]
COPY ["src/Weather.Infrastructure/Weather.Infrastructure.csproj", "src/Weather.Infrastructure/"]
RUN dotnet restore "src/Weather.API/Weather.API.csproj"

COPY . .
WORKDIR "/src/src/Weather.API"
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Weather.API.dll"]