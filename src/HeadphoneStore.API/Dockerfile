# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/HeadphoneStore.API/HeadphoneStore.API.csproj", "src/HeadphoneStore.API/"]
COPY ["src/HeadphoneStore.Application/HeadphoneStore.Application.csproj", "src/HeadphoneStore.Application/"]
COPY ["src/HeadphoneStore.Shared/HeadphoneStore.Shared.csproj", "src/HeadphoneStore.Shared/"]
COPY ["src/HeadphoneStore.Domain/HeadphoneStore.Domain.csproj", "src/HeadphoneStore.Domain/"]
COPY ["src/HeadphoneStore.Infrastructure/HeadphoneStore.Infrastructure.csproj", "src/HeadphoneStore.Infrastructure/"]
COPY ["src/HeadphoneStore.Persistence/HeadphoneStore.Persistence.csproj", "src/HeadphoneStore.Persistence/"]
COPY ["src/HeadphoneStore.StoreFrontEnd/HeadphoneStore.StoreFrontEnd.csproj", "src/HeadphoneStore.StoreFrontEnd/"]
RUN dotnet restore "./src/HeadphoneStore.API/HeadphoneStore.API.csproj"
COPY . .
WORKDIR "/src/src/HeadphoneStore.API"
RUN dotnet build "./HeadphoneStore.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HeadphoneStore.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HeadphoneStore.API.dll"]