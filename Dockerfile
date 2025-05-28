# --- Step 1: build frontend ---
	FROM node:20 AS frontend

	WORKDIR /app
	COPY client-app/ ./client-app/
	WORKDIR /app/client-app
	
	RUN npm install
	RUN npm run build
	
	# --- Step 2: build .NET backend ---
	FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
	WORKDIR /src
	
	COPY ParcelTracker.Api/*.csproj ParcelTracker.Api/
	RUN dotnet restore ParcelTracker.Api/ParcelTracker.Api.csproj
	
	COPY . .
	RUN dotnet publish ParcelTracker.Api/ParcelTracker.Api.csproj -c Release -o /app/publish
	
	# --- Step 3: run ---
	FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
	WORKDIR /app
	
	COPY --from=build /app/publish .
	ENV ASPNETCORE_URLS=http://+:80
	
	EXPOSE 80
	ENTRYPOINT ["dotnet", "ParcelTracker.Api.dll"]
	