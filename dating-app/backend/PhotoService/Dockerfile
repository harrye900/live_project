FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PhotoService.csproj", "."]
RUN dotnet restore "./PhotoService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "PhotoService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PhotoService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/uploads
ENTRYPOINT ["dotnet", "PhotoService.dll"]