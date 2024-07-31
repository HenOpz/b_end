# Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Clear NuGet cache (optional, if needed)
RUN dotnet nuget locals all --clear

# Copy the entire project
COPY . .

# Optionally, you can run tests here if needed
# RUN dotnet test --no-build

# Publish the application
RUN dotnet publish -c Release -o out

# Stage 2: Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build-env /app/out .

# Expose port 7129
EXPOSE 80

# Copy Swagger JSON files (assuming they are in the root directory)
COPY *.json ./

# Entry point for the application
ENTRYPOINT ["dotnet", "CPOC-AIMS-II-Backend.dll"]
