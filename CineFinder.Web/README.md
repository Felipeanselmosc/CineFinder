# CineFinder

CineFinder é uma aplicação web para descoberta, organização e avaliação de filmes.  
O sistema permite que usuários consultem filmes, vejam detalhes, filtrem por gênero e mantenham listas personalizadas de filmes, além de registrar avaliações.

## Tecnologias utilizadas

- .NET / ASP.NET Core (Web MVC + API)
- Entity Framework Core
- SQL Server
- Swagger (documentação da API)
- Padrão de camadas:
  - **Domain** – Entidades e interfaces de repositório
  - **Application** – DTOs, serviços e regras de negócio
  - **Infrastructure** – Repositórios, contexto de banco (EF Core)
  - **Web** – Controllers MVC, views e endpoints da API

## Estrutura do projeto

- `CineFinder.Domain`  
  Contém as entidades principais do sistema (Filme, Gênero, Lista, Avaliação, Usuário) e contratos de repositório.

- `CineFinder.Application`  
  Contém os **DTOs**, **services** (`FilmeService`, `ListaService`, etc.) e interfaces da camada de aplicação.

- `CineFinder.Infrastructure`  
  Implementa o `CineFinderDbContext` (EF Core) e os repositórios (`FilmeRepository`, `ListaRepository`, etc.).

- `CineFinder.Web`  
  Projeto ASP.NET Core MVC responsável pelas telas, controllers e configuração da aplicação (Program.cs, rotas, Swagger, etc.).

##  Configuração e execução

1. Configure a **connection string** no `appsettings.json` do projeto `CineFinder.Web`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=SEU_SERVIDOR;Database=CineFinderDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
Aplique as migrações (se já existirem) ou crie o banco via EF Core:

bash
Copiar código
dotnet ef database update
Execute o projeto Web:

bash
Copiar código
dotnet run --project CineFinder.Web
Acesse no navegador:

Aplicação web: https://localhost:xxxx

Swagger (API): https://localhost:xxxx/swagger

Funcionalidades principais
Listagem de filmes e detalhes

Filtro por gênero

Cadastro e gerenciamento de listas de filmes

Avaliações de filmes

Listas públicas e privadas

Rotas personalizadas para filmes, listas, gêneros e avaliações