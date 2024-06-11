# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

RUN dotnet --info

# Copiar csproj e restaurar dependências
COPY ["src/Mottu.Api/Mottu.Api.csproj", "Mottu.Api/"]
COPY ["src/Mottu.Application/Mottu.Application.csproj", "Mottu.Application/"]
COPY ["src/Mottu.Infrastructure/Mottu.Infrastructure.csproj", "Mottu.Infrastructure/"]
COPY ["src/Mottu.Domain/Mottu.Domain.csproj", "Mottu.Domain/"]
RUN dotnet restore "Mottu.Api/Mottu.Api.csproj"

# Copiar o restante do código e publicar a aplicação
COPY src/ .
WORKDIR /src/Mottu.Api
RUN dotnet publish -c Release -o /app/out

# Estágio final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
COPY src/Mottu.Api/appsettings.json ./
EXPOSE 8081
# EXPOSE 5672
# EXPOSE 15672
ENTRYPOINT ["dotnet", "Mottu.Api.dll"]