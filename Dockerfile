# 1. Etapa de construcción usando el SDK de .NET 9
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["API/API.csproj", "API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infraestructure/Infraestructure.csproj", "Infraestructure/"]
RUN dotnet restore "API/API.csproj"

# Copiamos todo el resto del código y compilamos
COPY . .
WORKDIR "/src/API"
RUN dotnet publish "API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Etapa de ejecución usando el runtime de .NET 9
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]