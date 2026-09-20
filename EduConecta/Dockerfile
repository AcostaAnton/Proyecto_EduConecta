dockerfile: # Etapa 1: compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY EduConecta.sln ./
COPY src/EduConecta.Api/EduConecta.Api.csproj src/EduConecta.Api/
COPY src/EduConecta.Application/EduConecta.Application.csproj src/EduConecta.Application/
COPY src/EduConecta.Domain/EduConecta.Domain.csproj src/EduConecta.Domain/
COPY src/EduConecta.Infrastructure/EduConecta.Infrastructure.csproj src/EduConecta.Infrastructure/

RUN dotnet restore EduConecta.sln

COPY src/ src/
RUN dotnet publish src/EduConecta.Api/EduConecta.Api.csproj -c Release -o /app/publish --no-restore

# Etapa 2: correr
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "EduConecta.Api.dll"]