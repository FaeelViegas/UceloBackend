# ================================
# Stage 1: Base runtime image (ASP.NET Core)
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# ================================
# Stage 2: Build (SDK)
# ================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia os arquivos de projeto para restaurar dependências
COPY ["Ucelo.API/Ucelo.API.csproj", "Ucelo.API/"]
COPY ["Ucelo.Application/Ucelo.Application.csproj", "Ucelo.Application/"]
COPY ["Ucelo.Domain/Ucelo.Domain.csproj", "Ucelo.Domain/"]
COPY ["Ucelo.Infrastructure/Ucelo.Infrastructure.csproj", "Ucelo.Infrastructure/"]

# Restaura dependências
RUN dotnet restore "Ucelo.API/Ucelo.API.csproj"

# Copia o restante do código-fonte
COPY . .

# Compila o projeto
WORKDIR "/src/Ucelo.API"
RUN dotnet build "Ucelo.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ================================
# Stage 3: Publicação (publish)
# ================================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ucelo.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# ================================
# Stage 4: Imagem final (runtime)
# ================================
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "Ucelo.API.dll"]