# Lista de Tarefas (ToDo/Kanban)

Aplicação full stack para gestão de tarefas no formato Kanban, com:

- **Back-end:** .NET 8, API REST, Entity Framework Core, SQLite
- **Front-end:** Angular 21

Ela permite criar, listar, atualizar, mover de status (Pendente, EmAndamento, Concluido) e remover tarefas.

## Como a aplicação funciona

### Fluxo funcional
- O front consulta e envia dados para a API em `/api/tarefas`.
- A API valida entradas com FluentValidation.
- A camada de aplicação executa as regras de caso de uso.
- A infraestrutura persiste dados no SQLite com EF Core.
- Na inicialização da API, as migrations são aplicadas automaticamente.

### Funcionalidades principais
- Quadro Kanban com colunas por status
- Criar tarefa
- Editar tarefa
- Alterar status (inclusive via arrastar/soltar no board)
- Excluir tarefa
- Filtrar por status e período de vencimento

## Arquitetura e estrutura

```
back/
  Tarefas.Domain/          entidades, enums, exceções, contratos
  Tarefas.Application/     DTOs, serviços, validações, mapeamentos
  Tarefas.Infrastructure/  EF Core + SQLite, repositórios, Unit of Work, IoC, migrations
  Tarefas.Api/             controllers, middleware de exceções, Program.cs
  Tests/                   testes xUnit

Front/src/app/
  core/                    DTOs, services e utilitários
  pages/                   páginas (board Kanban)
  shered/                  componentes reutilizáveis
```

## Endpoints da API

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/tarefas?status=&vencimentoDe=&vencimentoAte=` | Lista com filtros |
| GET | `/api/tarefas/{id}` | Obtém uma tarefa por id |
| POST | `/api/tarefas` | Cria tarefa |
| PUT | `/api/tarefas/{id}` | Atualiza tarefa |
| PATCH | `/api/tarefas/{id}/status` | Atualiza somente o status |
| DELETE | `/api/tarefas/{id}` | Remove tarefa |

---

## Rodando com Docker (recomendado)

### Pré-requisitos
- Docker Desktop (ou Docker Engine + Compose)

### Subir toda a aplicação
Na raiz do projeto:

```bash
docker compose up --build
```

### Acessos
- Front-end: http://localhost:4200
- Swagger da API: http://localhost:5146/swagger
- Base da API: http://localhost:5146/api/tarefas

### Parar containers
```bash
docker compose down
```

### Parar e remover volume do banco (reset de dados)
```bash
docker compose down -v
```

> O SQLite fica persistido no volume Docker `tarefas-data`.

---

## Rodando sem Docker

### Pré-requisitos
- .NET SDK 8+
- Node.js 20+ e npm

### 1) Subir a API
```bash
cd back/Tarefas.Api
dotnet run --launch-profile http
```

API em `http://localhost:5146` com Swagger em `http://localhost:5146/swagger`.

### 2) Subir o front
Em outro terminal:

```bash
cd Front
npm install
npm start
```

Front-end em `http://localhost:4200`.

> Em ambiente local, o Angular usa proxy para encaminhar `/api` para `http://localhost:5146`, evitando problemas de CORS.

---

## Testes

### Back-end
```bash
cd back
dotnet test
```

## Decisões técnicas
- **EF Core + SQLite:** simples para executar localmente, sem dependência de banco externo.
- **Repository + Unit of Work + DI:** separação de responsabilidades e baixo acoplamento.
- **FluentValidation:** validação consistente dos DTOs de entrada.
- **Middleware global de exceções:** retorna `ProblemDetails` padronizado para erros de domínio/validação e falhas inesperadas.
