# Build Frontend
FROM node:20 AS frontend
WORKDIR /app
COPY client-app/ ./client-app/
WORKDIR /app/client-app

RUN npm install
RUN npm run build

# Build Backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ParcelTracker.Api/*.csproj ParcelTracker.Api/
RUN dotnet restore ParcelTracker.Api/ParcelTracker.Api.csproj

COPY parceltracker-key.json /app/parceltracker-key.json

COPY . .
RUN dotnet publish ParcelTracker.Api/ParcelTracker.Api.csproj -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "ParcelTracker.Api.dll"]
