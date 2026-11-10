# Bygg ASP.NET Core-applikationen
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["visingsobiodlarna-backend.csproj", "./"]
RUN dotnet restore "visingsobiodlarna-backend.csproj"

COPY . .
RUN dotnet publish "visingsobiodlarna-backend.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# Kör applikationen
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .
COPY --from=frontend / /app/wwwroot/

EXPOSE 8080

ENTRYPOINT ["dotnet", "visingsobiodlarna-backend.dll"]