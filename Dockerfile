# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY visingsobiodlarna-backend/*.csproj ./visingsobiodlarna-backend/
RUN dotnet restore visingsobiodlarna-backend/visingsobiodlarna-backend.csproj

# Copy everything else and build
COPY . .
WORKDIR /src/visingsobiodlarna-backend
RUN dotnet publish -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "visingsobiodlarna-backend.dll"]
