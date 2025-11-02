FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MatchService.csproj", "."]
RUN dotnet restore "./MatchService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MatchService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MatchService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MatchService.dll"]