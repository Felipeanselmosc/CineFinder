# 🎬 CineFinder

Sistema de recomendação e gerenciamento de filmes que permite aos usuários descobrir, avaliar e organizar seus favoritos.

---

## 📋 Sobre o Projeto

### Objetivo
Resolver a dificuldade dos usuários em encontrar filmes adequados aos seus gostos pessoais e manter um histórico organizado de suas preferências cinematográficas.

### Escopo
O que foi desenvolvido: Sistema web completo para descoberta, avaliação e organização de filmes

Funcionalidades principais:
- Busca e descoberta de filmes
- Sistema de avaliação e comentários
- Criação e gerenciamento de listas personalizadas
- Recomendações baseadas em gêneros preferidos
- Perfil de usuário com histórico de avaliações

### Requisitos Funcionais e Não Funcionais

**Requisitos Funcionais**
- RF01: O sistema deve permitir o cadastro e autenticação de usuários
- RF02: O sistema deve permitir buscar filmes por título, gênero ou ano
- RF03: O sistema deve permitir que os usuários avaliem filmes com notas
- RF04: O sistema deve permitir criar e gerenciar listas personalizadas de filmes
- RF05: O sistema deve exibir informações detalhadas dos filmes
- RF06: O sistema deve permitir definir gêneros preferidos no perfil do usuário

**Requisitos Não Funcionais**
- RNF01: O sistema deve ser desenvolvido em .NET 8.0
- RNF02: O sistema deve usar SQL Server como banco de dados
- RNF03: O sistema deve seguir os princípios da Arquitetura Limpa
- RNF04: O sistema deve ter tempo de resposta inferior a 2 segundos
- RNF05: O sistema deve ser compatível com navegadores modernos
- RNF06: O sistema deve implementar CORS para integração com frontend
- RNF07: O sistema deve implementar monitoramento e observabilidade
- RNF08: O sistema deve possuir testes automatizados com padrão AAA

---

## 🏗️ Desenho da Arquitetura

O projeto utiliza **Clean Architecture** para separar responsabilidades e manter o código desacoplado, facilitando manutenção e testes.

### Camadas da Aplicação

**📱 Apresentação**
- Controladores (API REST)
- DTOs (Objetos de Transferência de Dados)
- Validações de entrada
- Documentação Swagger

**🎯 Aplicação**
- Serviços (lógica de negócio)
- Interfaces de serviços
- Orquestração entre domínio e infraestrutura

**💼 Domínio**
- Entidades (Usuario, Filme, Genero, Lista, Avaliacao)
- Interfaces de repositório
- Regras de negócio do domínio
- Relacionamentos entre entidades

**🔧 Infraestrutura**
- Repositórios (implementação com Entity Framework Core)
- DbContext e configurações de mapeamento
- Migrações do banco de dados

---

## 🛠️ Tecnologias Utilizadas

- .NET 8.0 - Framework principal
- ASP.NET Core - Framework web
- Entity Framework Core - ORM
- SQL Server / LocalDB - Banco de dados
- Swagger/OpenAPI - Documentação da API
- Serilog - Logging estruturado
- OpenTelemetry - Tracing e métricas distribuídas
- xUnit - Framework de testes
- Moq - Mocking para testes unitários
- FluentAssertions - Asserções para testes

---

## 📦 Estrutura do Projeto

```
CineFinder/
├── CineFinder.API/                  # API REST
│   ├── Controller/                  # Controllers
│   ├── Helpers/                     # HateoasLinkGenerator
│   ├── Models/                      # ResourceDto, Link
│   └── Program.cs                   # Configuração da aplicação
├── CineFinder.Application/          # Camada de aplicação
│   ├── Services/                    # Serviços de negócio
│   ├── Interfaces/                  # Contratos
│   └── DTOs/                        # Objetos de transferência
├── CineFinder.Domain/               # Camada de domínio
│   ├── Entities/                    # Entidades
│   └── Interfaces/                  # Contratos de repositório
├── CineFinder.Infrastructure/       # Infraestrutura
│   ├── Context/                     # DbContext
│   ├── Repository/                  # Repositórios
│   └── Configurations/              # Configurações EF Core
├── CineFinder.Tests.Unit/           # Testes unitários
│   └── Services/
│       ├── FilmeServiceTests.cs
│       ├── AvaliacaoServiceTests.cs
│       └── UsuarioServiceTests.cs
└── CineFinder.Tests.Integration/    # Testes de integração
    └── Controllers/
        ├── FilmesControllerIntegrationTests.cs
        └── HealthCheckIntegrationTests.cs
```

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
- SDK do .NET 8.0
- SQL Server (LocalDB, Express ou versão completa)
- Visual Studio 2022 ou VS Code
- Git

### Passo a Passo

**1. Clonar o repositório**
```bash
git clone https://github.com/Felipeanselmosc/CineFinder.git
cd CineFinder
```

**2. Configurar a connection string** em `CineFinder.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CineFinderDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**3. Restaurar pacotes**
```bash
dotnet restore
```

**4. Aplicar migrations**
```bash
dotnet ef database update --project CineFinder.Infrastructure --startup-project CineFinder.API
```

**5. Executar a API**
```bash
dotnet run --project CineFinder.API
```

**6. Acessar o Swagger**
```
http://localhost:5062
```

---

## 🩺 Monitoramento e Observabilidade

### Health Check Endpoints

| Endpoint | Descrição |
|---|---|
| `GET /health` | Status completo em JSON (API + banco de dados) |
| `GET /health/live` | Liveness probe — verifica se a aplicação está no ar |
| `GET /health/ready` | Readiness probe — verifica se o banco está disponível |

### Exemplo de resposta do `/health`
```json
{
  "status": "Healthy",
  "duration": "00:00:00.045",
  "checks": [
    { "name": "sqlserver", "status": "Healthy", "duration": "00:00:00.030" },
    { "name": "dbcontext", "status": "Healthy", "duration": "00:00:00.010" }
  ]
}
```

### Logging Estruturado (Serilog)
- **Console** — com CorrelationId e template enriquecido
- **Arquivo** — em `logs/cinefinder-YYYYMMDD.log` com rotação diária
- Níveis: `Information` (padrão), `Warning` para Microsoft.AspNetCore

### Tracing e Métricas (OpenTelemetry)
- Requisições HTTP (ASP.NET Core)
- Consultas ao banco de dados (EF Core)
- Chamadas HTTP de saída
- Métricas de runtime .NET

---

## 🧪 Testes

```bash
# Executar todos os testes
dotnet test

# Apenas testes unitários
dotnet test CineFinder.Tests.Unit

# Apenas testes de integração
dotnet test CineFinder.Tests.Integration

# Com cobertura de código
dotnet test --collect:"XPlat Code Coverage"
```

### Estrutura dos Testes
- **Padrão AAA** — Arrange, Act, Assert
- **Nomenclatura** — `MetodoTeste_Cenario_ResultadoEsperado`
- **Unitários** — Moq para mocking, FluentAssertions para asserções
- **Integração** — WebApplicationFactory com banco InMemory

---

## 🔌 Endpoints da API

### Usuários
| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/usuarios` | Lista todos os usuários |
| GET | `/api/usuarios/{id}` | Busca usuário por ID |
| GET | `/api/usuarios/search` | Busca com filtros e paginação |
| POST | `/api/usuarios` | Cria novo usuário |
| PUT | `/api/usuarios/{id}` | Atualiza usuário |
| DELETE | `/api/usuarios/{id}` | Remove usuário |

### Filmes
| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/filmes` | Lista todos os filmes |
| GET | `/api/filmes/{id}` | Busca filme por ID |
| GET | `/api/filmes/search` | Busca com filtros e paginação |
| GET | `/api/filmes/top-rated` | Filmes mais bem avaliados |
| GET | `/api/filmes/genero/{generoId}` | Filmes por gênero |
| POST | `/api/filmes` | Cria novo filme |
| PUT | `/api/filmes/{id}` | Atualiza filme |
| DELETE | `/api/filmes/{id}` | Remove filme |

### Gêneros
| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/generos` | Lista todos os gêneros |
| GET | `/api/generos/{id}` | Busca gênero por ID |
| GET | `/api/generos/populares` | Gêneros mais populares |
| GET | `/api/generos/search` | Busca com filtros |
| POST | `/api/generos` | Cria novo gênero |
| PUT | `/api/generos/{id}` | Atualiza gênero |
| DELETE | `/api/generos/{id}` | Remove gênero |

### Listas
| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/listas` | Lista todas as listas |
| GET | `/api/listas/{id}` | Busca lista por ID |
| GET | `/api/listas/usuario/{usuarioId}` | Listas de um usuário |
| POST | `/api/listas` | Cria nova lista |
| PUT | `/api/listas/{id}` | Atualiza lista |
| DELETE | `/api/listas/{id}` | Remove lista |
| POST | `/api/listas/{listaId}/filmes/{filmeId}` | Adiciona filme à lista |
| DELETE | `/api/listas/{listaId}/filmes/{filmeId}` | Remove filme da lista |

### Avaliações
| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/avaliacoes` | Lista todas as avaliações |
| GET | `/api/avaliacoes/{id}` | Busca avaliação por ID |
| GET | `/api/avaliacoes/filme/{filmeId}` | Avaliações de um filme |
| GET | `/api/avaliacoes/usuario/{usuarioId}` | Avaliações de um usuário |
| GET | `/api/avaliacoes/search` | Busca com filtros |
| POST | `/api/avaliacoes` | Cria avaliação |
| PUT | `/api/avaliacoes/{id}` | Atualiza avaliação |
| DELETE | `/api/avaliacoes/{id}` | Remove avaliação |

---

## 📊 Modelo de Dados

### Entidades
- **Usuario** — Gerenciamento de usuários do sistema
- **Filme** — Informações sobre filmes
- **Genero** — Categorias de filmes
- **Lista** — Listas personalizadas de filmes por usuário
- **Avaliacao** — Avaliações e comentários de usuários sobre filmes

### Relacionamentos
- Um usuário pode ter várias listas
- Uma lista pode conter múltiplos filmes
- Um filme pode estar em múltiplas listas
- Um usuário pode fazer várias avaliações
- Um filme pode ter várias avaliações
- Um filme pode ter múltiplos gêneros
- Um usuário pode ter gêneros preferidos

---

## 📝 Comandos Úteis

### Entity Framework Core
```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration --project CineFinder.Infrastructure --startup-project CineFinder.API

# Aplicar migrations
dotnet ef database update --project CineFinder.Infrastructure --startup-project CineFinder.API

# Remover última migration
dotnet ef migrations remove --project CineFinder.Infrastructure --startup-project CineFinder.API
```

### Build e Deploy
```bash
# Build em modo Release
dotnet build -c Release

# Publicar aplicação
dotnet publish -c Release -o ./publish
```

---

## 🤝 Contribuindo

1. Faça um fork do projeto
2. Crie um branch para seu feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova feature'`)
4. Push para o branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

---

## 👥 Autores

- **Felipe Anselmo** — Desenvolvimento
- **Matheus Mariotto** — Desenvolvimento
- **João Vinicius** — Desenvolvimento

---
