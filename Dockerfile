FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BookCatalogManagementSystem.Api/BookCatalogManagementSystem.Api/BookCatalogManagementSystem.Api.csproj", "BookCatalogManagementSystem.Api/BookCatalogManagementSystem.Api/"]
COPY ["BookCatalogManagementSystem.Models/BookCatalogManagementSystem.Models.csproj", "BookCatalogManagementSystem.Models/"]
COPY ["BookCatalogManagementSystem.Services/BookCatalogManagementSystem.Services.csproj", "BookCatalogManagementSystem.Services/"]
COPY ["BookCatalogManagementSystem.Repository/BookCatalogManagementSystem.Repository.csproj", "BookCatalogManagementSystem.Repository/"]
RUN dotnet restore "BookCatalogManagementSystem.Api/BookCatalogManagementSystem.Api/BookCatalogManagementSystem.Api.csproj"
COPY . .
WORKDIR "/src/BookCatalogManagementSystem.Api"
RUN dotnet build "BookCatalogManagementSystem.Api/BookCatalogManagementSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BookCatalogManagementSystem.Api/BookCatalogManagementSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookCatalogManagementSystem.Api.dll"]
