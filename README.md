# 🎬 CineFinder

Sistema de recomendação e gerenciamento de filmes que permite aos usuários descobrir, avaliar e organizar seus filmes favoritos.

## 📋 Definição do Projeto

### Objetivo do Projeto
Esclarecer o problema que o CineFinder pretende resolver: a dificuldade dos usuários em encontrar filmes adequados aos seus gostos pessoais e manter um histórico organizado de suas preferências cinematográficas.

### Escopo
- **O que será desenvolvido:** Sistema web completo para descoberta, avaliação e organização de filmes
- **Funcionalidades principais:**
  - Busca e descoberta de filmes através da integração com TMDB API
  - Sistema de avaliação e comentários
  - Criação e gerenciamento de listas personalizadas
  - Recomendações baseadas em preferências de gênero
  - Perfil de usuário com histórico de avaliações

### Requisitos Funcionais e Não Funcionais

#### Requisitos Funcionais
- RF01: O sistema deve permitir o cadastro e autenticação de usuários
- RF02: O sistema deve permitir buscar filmes por título, gênero ou ano
- RF03: O sistema deve permitir que usuários avaliem filmes com notas
- RF04: O sistema deve permitir criar e gerenciar listas personalizadas de filmes
- RF05: O sistema deve exibir informações detalhadas dos filmes (sinopse, elenco, avaliações)
- RF06: O sistema deve permitir definir gêneros preferidos no perfil do usuário
- RF07: O sistema deve sincronizar dados com a API do TMDB

#### Requisitos Não Funcionais
- RNF01: O sistema deve ser desenvolvido em .NET 8.0
- RNF02: O sistema deve utilizar SQL Server como banco de dados
- RNF03: O sistema deve seguir os princípios de Clean Architecture
- RNF04: O sistema deve ter tempo de resposta inferior a 2 segundos
- RNF05: O sistema deve ser compatível com navegadores modernos
- RNF06: O sistema deve implementar CORS para integração com frontend

## 🏗️ Desenho da Arquitetura

### Clean Architecture
O projeto utiliza Clean Architecture para separar responsabilidades e manter o código desacoplado, facilitando manutenção e testes.

### Camadas da Aplicação

#### 📱 Apresentação
Serviços e casos de uso da aplicação, incluindo:
- Controllers (API REST)
- DTOs (Data Transfer Objects)
- Validações de entrada
- Documentação Swagger

#### 🎯 Aplicação
Serviços e casos de uso da aplicação:
- Services (lógica de negócio)
- Interfaces de serviços
- Orquestração entre domínio e infraestrutura

#### 💼 Domínio
Modelos e regras de negócio:
- Entities (Usuario, Filme, Genero, Lista, Avaliacao)
- Interfaces de repositórios
- Regras de negócio do domínio
- Relacionamentos entre entidades

#### 🔧 Infraestrutura
Acesso a dados, integração com outras APIs:
- Repositórios (implementação com Entity Framework Core)
- DbContext e configurações de mapeamento
- Migrations do banco de dados
- Integração com TMDB API

## 🛠️ Tecnologias Utilizadas

- **.NET 8.0** - Framework principal
- **ASP.NET Core** - Framework web
- **Entity Framework Core 9.0** - ORM
- **SQL Server** - Banco de dados
- **Swagger/OpenAPI** - Documentação da API
- **TMDB API** - Fonte de dados de filmes

## 📦 Estrutura do Projeto

```
CineFinder/
├── Controllers/              # API Controllers
├── Domain/
│   ├── Entities/            # Entidades do domínio
│   └── Interfaces/          # Contratos de repositórios
├── Application/
│   ├── Services/            # Serviços de aplicação
│   └── Interfaces/          # Contratos de serviços
├── Infrastructure/
│   ├── Data/
│   │   └── Context/         # DbContext
│   └── Repositories/        # Implementação dos repositórios
├── Migrations/              # Migrations do EF Core
├── Program.cs               # Configuração da aplicação
└── appsettings.json         # Configurações
```

## 🚀 Como Executar o Projeto

### Pré-requisitos
- .NET 8.0 SDK
- SQL Server (LocalDB, Express ou versão completa)
- Visual Studio 2022 ou VS Code
- Git

### Passo a Passo

1. **Clone o repositório**
```bash
git clone https://github.com/seu-usuario/cinefinder.git
cd cinefinder
```

2. **Configure a connection string**

Edite o arquivo `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CineFinderDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

3. **Restaure os pacotes**
```bash
dotnet restore
```

4. **Execute as migrations**
```bash
dotnet ef database update
```

5. **Execute o projeto**
```bash
dotnet run
```

6. **Acesse a documentação da API**
```
https://localhost:5001/swagger
```

## 📊 Modelo de Dados

### Entidades Principais

- **Usuario**: Gerenciamento de usuários do sistema
- **Filme**: Informações sobre filmes (integrado com TMDB)
- **Genero**: Categorias de filmes
- **Lista**: Listas personalizadas de filmes por usuário
- **Avaliacao**: Avaliações e comentários de usuários sobre filmes

### Relacionamentos

- Um usuário pode ter múltiplas listas
- Uma lista pode conter múltiplos filmes
- Um filme pode estar em múltiplas listas
- Um usuário pode fazer múltiplas avaliações
- Um filme pode ter múltiplas avaliações
- Um filme pode ter múltiplos gêneros
- Um usuário pode ter gêneros preferidos

## 🔌 Endpoints da API

### Usuários
- `GET /api/usuarios` - Lista todos os usuários
- `GET /api/usuarios/{id}` - Busca usuário por ID
- `POST /api/usuarios` - Cria novo usuário
- `PUT /api/usuarios/{id}` - Atualiza usuário
- `DELETE /api/usuarios/{id}` - Remove usuário

### Filmes
- `GET /api/filmes` - Lista todos os filmes
- `GET /api/filmes/{id}` - Busca filme por ID
- `POST /api/filmes` - Adiciona novo filme
- `PUT /api/filmes/{id}` - Atualiza filme
- `DELETE /api/filmes/{id}` - Remove filme

### Gêneros
- `GET /api/generos` - Lista todos os gêneros
- `GET /api/generos/{id}` - Busca gênero por ID
- `POST /api/generos` - Cria novo gênero
- `PUT /api/generos/{id}` - Atualiza gênero
- `DELETE /api/generos/{id}` - Remove gênero

### Listas
- `GET /api/listas` - Lista todas as listas
- `GET /api/listas/{id}` - Busca lista por ID
- `POST /api/listas` - Cria nova lista
- `PUT /api/listas/{id}` - Atualiza lista
- `DELETE /api/listas/{id}` - Remove lista

### Avaliações
- `GET /api/avaliacoes` - Lista todas as avaliações
- `GET /api/avaliacoes/{id}` - Busca avaliação por ID
- `POST /api/avaliacoes` - Cria nova avaliação
- `PUT /api/avaliacoes/{id}` - Atualiza avaliação
- `DELETE /api/avaliacoes/{id}` - Remove avaliação

## 🧪 Testes

```bash
# Executar todos os testes
dotnet test

# Executar com cobertura
dotnet test /p:CollectCoverage=true
```

## 📝 Comandos Úteis

### Entity Framework Core

```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration

# Aplicar migrations
dotnet ef database update

# Remover última migration
dotnet ef migrations remove

# Ver SQL gerado
dotnet ef migrations script
```

### Build e Deploy

```bash
# Build em modo Release
dotnet build -c Release

# Publicar aplicação
dotnet publish -c Release -o ./publish
```

## 🤝 Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova feature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

## 👥 Autores

- **Seu Nome** - *Desenvolvimento inicial*

## 🙏 Agradecimentos

- TMDB API pela disponibilização dos dados de filmes
- Comunidade .NET pelos recursos e documentação

