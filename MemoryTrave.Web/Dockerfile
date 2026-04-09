FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MemoryTrave.Web/MemoryTrave.Web.csproj", "MemoryTrave.Web/"]
COPY ["MemoryTrave.Application/MemoryTrave.Application.csproj", "MemoryTrave.Application/"]
COPY ["MemoryTrave.Domain/MemoryTrave.Domain.csproj", "MemoryTrave.Domain/"]
COPY ["MemoryTrave.Infrastructure/MemoryTrave.Infrastructure.csproj", "MemoryTrave.Infrastructure/"]
RUN dotnet restore "MemoryTrave.Web/MemoryTrave.Web.csproj"
COPY . .
WORKDIR "/src/MemoryTrave.Web"
RUN dotnet build "./MemoryTrave.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MemoryTrave.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MemoryTrave.Web.dll"]