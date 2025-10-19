# Stage 1: Build the application using the .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies first. This leverages Docker's layer caching.
# If the .csproj file hasn't changed, Docker will reuse the cached layer, speeding up future builds.
COPY ["TrovTver.csproj", "."]
RUN dotnet restore "./TrovTver.csproj"

# Copy the rest of the application's source code
COPY . .
WORKDIR "/src/."
RUN dotnet build "TrovTver.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "TrovTver.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Create the final, smaller runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "TrovTver.dll"]
