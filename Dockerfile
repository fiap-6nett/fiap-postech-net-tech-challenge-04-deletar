# Imagem base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Exponha apenas a porta principal usada pela API
EXPOSE 8080

# Imagem para build do projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia a solução
COPY Contato.Delete.Web.sln . 

# Copia os projetos
COPY Contato.Delete.Web/Contato.Delete.Web.csproj Contato.Delete.Web/
COPY Contato.Delete.Application/Contato.Delete.Application.csproj Contato.Delete.Application/
COPY Contato.Delete.Domain/Contato.Delete.Domain.csproj Contato.Delete.Domain/
COPY Contato.Delete.Infra/Contato.Delete.Infra.csproj Contato.Delete.Infra/

# Restaura dependências
RUN dotnet restore Contato.Delete.Web.sln

# Copia o restante do código
COPY . .

# Build do projeto
WORKDIR /src/Contato.Delete.Web
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Publica a aplicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final: imagem de runtime
FROM base AS final
WORKDIR /app

# Garante que a aplicação escute na porta correta
ENV ASPNETCORE_URLS=http://+:8080

# Copia os arquivos publicados
COPY --from=publish /app/publish .

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "Contato.Delete.Web.dll" ]