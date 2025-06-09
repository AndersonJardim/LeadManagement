# Lead Management

Estou criando um controle de gerenciamento de Lead - Lead Management.
Um webapi .net.

# Alguns comandos

## 1- Criando um novo projeto .NET Core Web API:

```bash
dotnet new webapi -n LeadManagementApi
cd LeadManagementApi
```

## 2- Adicionando packages ao backend:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package MediatR
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
dotnet add package Swashbuckle.AspNetCore
```

## 3- Adicionando pastas a arquitetura do projeto:

Crie as pastas:

```bash
md Controllers
md Data
md Entities
md Features
md Models

cd Features
md Leads

cd Leads

md Commands
md Queries
```

# 4- Arquivos da API

- Criei os arquivos da API

# 5- Connectar a SQL Server ou criar um novo

- Network do SQL Server

```bash
docker network create leadmanagement-network
docker run -d --name sqlserver4 --hostname sqlserver4 --network leadmanagement-network -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=leadmanagement#2025" -v sql_data:/var/opt/mssql3 --restart unless-stopped -p 11444:1433 mcr.microsoft.com/mssql/server
```

- Gerar o Migration

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

# 6- Arquivos Docker

- Criei o Dockerfile na raiz do seu projeto LeadManagementApi, para facilitar o testes completo.
- Lembrar de comfigurar atualizar a ConnectionStrings:DefaultConnection no seu appsettings.json para apontar para o endereço IP ou nome do host do seu servidor SQL Server.

```bash
docker build -t leadmanagementapi .
docker run -d -p 7780:80 --name lead-api leadmanagementapi
```

```bash
"DefaultConnection": "Server=localhost,11444;Database=LeadManagementDb;User Id=sa;Password=leadmanagement#2025;Encrypt=false;TrustServerCertificate=true;MultipleActiveResultSets=true;"
```

```bash
"DefaultConnection": "Server=sqlserver4;Database=LeadManagementDb;User Id=sa;Password=leadmanagement#2025;Encrypt=false;TrustServerCertificate=true;MultipleActiveResultSets=true;"
```

# TESTE 1 - Server=localhost,11433

Em appsettings.json:
"DefaultConnection": "Server=localhost,11444;Database=LeadManagementDb;User Id=sa;Password=leadmanagement#2025;Encrypt=false;TrustServerCertificate=true;MultipleActiveResultSets=true;",

# build da imagem

```bash
docker build -t leadmanagementapi -f ./LeadManagementApi/Dockerfile .
docker build -t lead-api -f ./LeadManagementApi/Dockerfile .
```

# execução da imagem

```bash
docker run -d -p 7780:8080 --network leadmanagement-network --name lead-api leadmanagementapi
```

# TESTE 2 - Server=sqlserver

Em appsettings.json:

```bash
"DefaultConnection": "Server=sqlserver;Database=LeadManagementApi;User Id=sa;Password=leadmanagement#2025;Encrypt=false;TrustServerCertificate=true;MultipleActiveResultSets=true;",
```

```bash
docker build leadmanagementapi
docker build -t leadmanagementapi -f ./src/LeadManagementApi/Dockerfile .
docker run -d -p 7780:8080 --network leadmanagement-network --name leadmanagementapi_container leadmanagementapi
http://localhost:8080

```
