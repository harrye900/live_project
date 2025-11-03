FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
RUN apk update && apk upgrade && apk add --no-cache ca-certificates && rm -rf /var/cache/apk/*
RUN addgroup -g 1001 -S appgroup && adduser -u 1001 -S appuser -G appgroup
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
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
RUN mkdir -p /app/uploads && chown -R appuser:appgroup /app
USER appuser
ENTRYPOINT ["dotnet", "PhotoService.dll"]