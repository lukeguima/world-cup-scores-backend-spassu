# World Cup Scores

Backend para gerenciamento de partidas e placares da Copa do Mundo.

## Como Rodar

### 1. Subir o PostgreSQL

Na raiz do projeto:

```bash
cd C:\dev\world-cup-scores
docker compose up -d
```

Conferir se o container esta rodando:

```bash
docker ps
```

Container esperado:

```text
world-cup-scores-postgres
```

### 2. Aplicar migrations

Entre na pasta do backend:

```bash
cd C:\dev\world-cup-scores\backend
```

Aplicar migrations:

```bash
dotnet ef database update --project src/WorldCupScores.Infrastructure --startup-project src/WorldCupScores.Api
```

Listar migrations:

```bash
dotnet ef migrations list --project src/WorldCupScores.Infrastructure --startup-project src/WorldCupScores.Api
```

### 3. Rodar o backend

```bash
dotnet run --project src/WorldCupScores.Api --launch-profile http
```

A API ficara disponivel em:

```text
http://localhost:8080
```

Swagger:

```text
http://localhost:8080/swagger
```

### 4. Rodar testes

Na pasta `backend`:

```bash
dotnet test
```

## Tecnologias

- .NET 10 no projeto atual
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL 16
- Npgsql.EntityFrameworkCore.PostgreSQL
- Swagger / Swashbuckle
- xUnit
- FluentAssertions
- Docker Compose

## Estrutura

```text
world-cup-scores/
+-- docker-compose.yml
+-- backend/
|   +-- WorldCupScores.slnx
|   +-- src/
|   |   +-- WorldCupScores.Domain
|   |   +-- WorldCupScores.Application
|   |   +-- WorldCupScores.Infrastructure
|   |   +-- WorldCupScores.Api
|   +-- tests/
|       +-- WorldCupScores.Tests
+-- frontend/
```

## Arquitetura

### Domain

Principais itens:

- `Entities/WorldCupMatch.cs`
- `Enums/MatchStage.cs`
- `Enums/MatchStatus.cs`
- `Exceptions/DomainException.cs`
- `Repositories/IWorldCupMatchRepository.cs`

A entidade `WorldCupMatch` encapsula regras e comportamentos:

- `Create`
- `Update`
- `UpdateScore`
- `ChangeStatus`

Regras principais:

- Time mandante e visitante sao obrigatorios.
- Mandante e visitante nao podem ser iguais.
- Toda partida inicia como `Scheduled`.
- Partidas `Scheduled` nao podem ter placar.
- Ao atualizar placar de uma partida `Scheduled`, o backend altera automaticamente para `InProgress`.
- Partidas `Finished` precisam ter placar.
- Placar nao pode ser negativo.
- Partida `Finished` nao volta para `Scheduled`.
- Partida `Canceled` nao pode ser marcada como `Finished`.
- `CreatedAt` e `UpdatedAt` sao controlados pela entidade.

### Application

Commands:

- `CreateWorldCupMatchCommand`
- `UpdateWorldCupMatchCommand`
- `UpdateWorldCupMatchScoreCommand`
- `ChangeWorldCupMatchStatusCommand`
- `DeleteWorldCupMatchCommand`

Queries:

- `GetWorldCupMatchesQuery`
- `GetWorldCupMatchByIdQuery`

### Infrastructure

Principais itens:

- `Persistence/AppDbContext.cs`
- `Persistence/Configurations/WorldCupMatchConfiguration.cs`
- `Persistence/Migrations`
- `Repositories/WorldCupMatchRepository.cs`
- `DependencyInjection.cs`

O banco e registrado via:

```csharp
options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
```

### Api

Principais configuracoes:

- Controllers REST
- Swagger em `/swagger`
- CORS para `http://localhost:5173`
- Backend em `http://localhost:8080`
- Tratamento simples de erros de dominio
- Controllers chamam handlers da Application, sem acessar `DbContext` diretamente


## Banco de Dados

O PostgreSQL roda via Docker Compose na raiz do projeto.

Arquivo:

```text
docker-compose.yml
```

Configuracao:

```yaml
services:
  postgres:
    image: postgres:16
    container_name: world-cup-scores-postgres
    restart: unless-stopped
    environment:
      POSTGRES_DB: world_cup_scores
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
    ports:
      - "5432:5432"
    volumes:
      - world_cup_scores_data:/var/lib/postgresql/data

volumes:
  world_cup_scores_data:
```

Connection string da API:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=world_cup_scores;Username=postgres;Password=postgres"
  },
  "AllowedHosts": "*"
}
```

## Endpoints

Base URL:

```text
http://localhost:8080/api/world-cup-matches
```

Endpoints:

- `GET /api/world-cup-matches`
- `GET /api/world-cup-matches/{id}`
- `POST /api/world-cup-matches`
- `PUT /api/world-cup-matches/{id}`
- `PATCH /api/world-cup-matches/{id}/score`
- `PATCH /api/world-cup-matches/{id}/status`
- `DELETE /api/world-cup-matches/{id}`

## Exemplo de Criacao

```http
POST http://localhost:8080/api/world-cup-matches
Content-Type: application/json
```

```json
{
  "homeTeam": "Brasil",
  "awayTeam": "Argentina",
  "matchDate": "2026-06-03T18:46:00.000Z",
  "stage": 5,
  "stadium": "Arena MRV"
}
```

## Enums

### MatchStage

| Valor | Nome |
| --- | --- |
| 0 | GroupStage |
| 1 | RoundOf16 |
| 2 | QuarterFinal |
| 3 | SemiFinal |
| 4 | ThirdPlace |
| 5 | Final |

### MatchStatus

| Valor | Nome |
| --- | --- |
| 0 | Scheduled |
| 1 | InProgress |
| 2 | Finished |
| 3 | Canceled |


## Observacoes Operacionais

```bash
dotnet ef migrations add NomeDaMigration --project src/WorldCupScores.Infrastructure --startup-project src/WorldCupScores.Api --output-dir Persistence/Migrations
```

Depois aplique:

```bash
dotnet ef database update --project src/WorldCupScores.Infrastructure --startup-project src/WorldCupScores.Api
```
