FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY . .
# TODO: improve using multi-stage build
RUN dotnet restore && dotnet build && dotnet test --no-build && dotnet publish -c Release -o /app/out


FROM mcr.microsoft.com/dotnet/runtime:9.0 AS prod
WORKDIR /app
RUN useradd -m dotnetuser
USER dotnetuser
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "TradingSystem.App.dll"]
