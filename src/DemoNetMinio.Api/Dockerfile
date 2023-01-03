FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/DemoNetMinio.Api/DemoNetMinio.Api.csproj", "DemoNetMinio.Api/"]
RUN dotnet restore "src/DemoNetMinio.Api/DemoNetMinio.Api.csproj"

COPY ["src/DemoNetMinio.Storage/DemoNetMinio.Storage.csproj", "DemoNetMinio.Storage/"]
RUN dotnet restore "src/DemoNetMinio.Storage/DemoNetMinio.Storage.csproj"

COPY . .
WORKDIR "/src/DemoNetMinio.Api"
RUN dotnet build "DemoNetMinio.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoNetMinio.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoNetMinio.Api.dll"]
