FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
RUN apk update && apk upgrade && apk add --no-cache ca-certificates && rm -rf /var/cache/apk/* && \
    addgroup -g 1001 -S appgroup && adduser -u 1001 -S appuser -G appgroup
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["UserService.csproj", "."]
RUN dotnet restore "./UserService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "UserService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN chown -R appuser:appgroup /app
USER appuser
ENTRYPOINT ["dotnet", "UserService.dll"]