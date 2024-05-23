FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StudentEnrollment01.InMemory/StudentEnrollment01.InMemory.csproj", "StudentEnrollment01.InMemory/"]
RUN dotnet restore "StudentEnrollment01.InMemory/StudentEnrollment01.InMemory.csproj"
COPY . .
WORKDIR "/src/StudentEnrollment01.InMemory"
RUN dotnet build "StudentEnrollment01.InMemory.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StudentEnrollment01.InMemory.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentEnrollment01.InMemory.dll"]
