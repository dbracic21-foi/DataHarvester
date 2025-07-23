FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "DataHarvester.Worker/DataHarvester.Worker.csproj"
RUN dotnet publish "DataHarvester.Worker/DataHarvester.Worker.csproj" -c Release -o /app/publish

FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DataHarvester.Worker.dll"]
