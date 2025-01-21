# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy and restore dependencies
COPY revisifyBackened.csproj ./
RUN dotnet restore

# Copy the remaining files and publish
COPY . ./
RUN dotnet publish revisifyBackened.csproj -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published output
COPY --from=build-env /app/out .

# Copy templates folder
COPY ./Templates /app/Templates

# Set the entry point
ENTRYPOINT [ "dotnet", "revisifyBackened.dll" ]
