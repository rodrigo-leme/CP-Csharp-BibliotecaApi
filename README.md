# 📚 BibliotecaApi

API RESTful para gerenciar o acervo de livros de uma biblioteca comunitária, desenvolvida com **C# .NET 10**, **ASP.NET Core Minimal APIs** e **Entity Framework Core**.

## 👥 Integrantes

| Nome | RM |
|------|----|
| Rodrigo Leme | RM550266 |


## 📝 Contexto do Projeto

**O que é:** uma API para cadastrar, consultar, atualizar e remover livros do acervo de uma biblioteca comunitária.

**Qual problema resolve:** muitas bibliotecas comunitárias (de bairros, escolas e ONGs) controlam seus livros em cadernos ou planilhas soltas, o que dificulta saber quais livros existem e quais estão disponíveis para empréstimo. A API centraliza essas informações em um banco de dados.

**Para quem é destinado:** voluntários e responsáveis por bibliotecas comunitárias que precisam organizar o acervo de forma simples.

## 🛠️ Tecnologias

- C# / .NET 10
- ASP.NET Core Minimal APIs (`MapGet`, `MapPost`, `MapPut`, `MapDelete`) — sem Controllers
- Entity Framework Core 10
- OpenAPI + Scalar (documentação e testes dos endpoints)
- Programação assíncrona (`async`/`await`) em todas as operações com o banco

## 🗄️ Banco de Dados

**SQLite** — o banco é um arquivo local (`biblioteca.db`) criado automaticamente ao aplicar as migrations. Não é necessário instalar nenhum servidor de banco.

String de conexão (em `appsettings.json`):

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=biblioteca.db"
}
```

## 📁 Estrutura

```
BibliotecaApi/
├── Data/
│   └── BibliotecaContext.cs   # DbContext do EF Core
├── Models/
│   ├── Livro.cs               # Entidade mapeada
│   └── LivroRequest.cs        # Dados recebidos no POST e PUT
├── Migrations/                # Gerada pelo comando de migration
├── docs/prints/               # Prints dos testes
├── Program.cs                 # Configuração e endpoints
└── appsettings.json
```

## ▶️ Como rodar localmente

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Ferramenta do EF Core:

```bash
dotnet tool install --global dotnet-ef
```

### Passo a passo

1. Clone o repositório e entre na pasta:

```bash
git clone <url-do-repositorio>
cd BibliotecaApi
```

2. Restaure os pacotes:

```bash
dotnet restore
```

3. Crie a migration (apenas se a pasta `Migrations` ainda não existir):

```bash
dotnet ef migrations add CriacaoInicial
```

4. Aplique a migration para criar o banco:

```bash
dotnet ef database update
```

5. Rode o projeto:

```bash
dotnet run
```

6. Acesse a documentação:

- Scalar: http://localhost:5074/scalar
- OpenAPI (JSON): http://localhost:5074/openapi/v1.json

## 🔗 Endpoints

| Método | Rota | Descrição | Respostas |
|--------|------|-----------|-----------|
| GET | `/livros` | Lista todos os livros | 200 |
| GET | `/livros/{id}` | Busca um livro pelo id | 200, 404 |
| POST | `/livros` | Cadastra um novo livro | 201, 400 |
| PUT | `/livros/{id}` | Atualiza um livro existente | 200, 400, 404 |
| DELETE | `/livros/{id}` | Remove um livro | 204, 404 |

### Exemplo de corpo (POST e PUT)

```json
{
  "titulo": "Dom Casmurro",
  "autor": "Machado de Assis",
  "anoPublicacao": 1899,
  "disponivel": true
}
```

### Exemplo de resposta (POST — 201 Created)

```json
{
  "id": 1,
  "titulo": "Dom Casmurro",
  "autor": "Machado de Assis",
  "anoPublicacao": 1899,
  "disponivel": true
}
```

### Tratamento de erros

| Situação | Status | Resposta |
|----------|--------|----------|
| Título ou autor vazio | 400 | `{ "mensagem": "O título é obrigatório." }` |
| Ano de publicação inválido | 400 | `{ "mensagem": "O ano de publicação é inválido." }` |
| Livro não encontrado | 404 | `{ "mensagem": "Livro com id 99 não encontrado." }` |

## 📸 Evidências de Teste

Prints dos testes realizados no Scalar, disponíveis na pasta `docs/prints`:

### GET /livros
![GET livros](docs/prints/get-livros.png)

### GET /livros/{id}
![GET livro por id](docs/prints/get-livro-por-id.png)

### POST /livros
![POST livro](docs/prints/post-livro.png)

### PUT /livros/{id}
![PUT livro](docs/prints/put-livro.png)

### DELETE /livros/{id}
![DELETE livro](docs/prints/delete-livro.png)
