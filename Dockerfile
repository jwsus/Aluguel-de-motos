# Use uma imagem base que contém o SDK do .NET 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Defina o diretório de trabalho no contêiner
WORKDIR /app

# Copie apenas os arquivos de projeto e restaure as dependências
COPY src/Mottu.Api/*.csproj ./src/Mottu.Api/
RUN dotnet restore "./src/Mottu.Api/Mottu.Api.csproj"

# Copie todos os arquivos do projeto e compile a aplicação
COPY . ./
RUN dotnet publish "./src/Mottu.Api/Mottu.Api.csproj" -c Release -o out

# Use uma imagem runtime mais leve para a aplicação publicada
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Defina o diretório de trabalho no contêiner
WORKDIR /app

# Copie os binários publicados da etapa anterior
COPY --from=build-env /app/out .

# Defina o comando de entrada para o contêiner
ENTRYPOINT ["dotnet", "Mottu.Api.dll"]
