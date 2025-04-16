FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FakeUser.WebApi/FakeUser.WebApi.csproj", "FakeUser.WebApi/"]
COPY ["FakeUser.Model/FakeUser.Model.csproj", "FakeUser.Model/"]
RUN dotnet restore "FakeUser.WebApi/FakeUser.WebApi.csproj"
COPY . .
WORKDIR "/src/FakeUser.WebApi"
RUN dotnet build "FakeUser.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FakeUser.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FakeUser.WebApi.dll"]
