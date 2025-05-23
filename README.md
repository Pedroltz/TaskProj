# Documentação da API - TaskApi (.NET 9 + EF Core)

Esta é uma API RESTful para gerenciamento de **usuários** e **tarefas** utilizando ASP.NET Core e Entity Framework Core.

## Base URL

```
http://localhost:5261/api
```

---

## Rotas de Usuário (`/Users`)

### 🔹 GET `/Users`

Retorna todos os usuários com suas tarefas.

**Resposta:**

```json
[
  {
    "id": "...",
    "name": "Lucas",
    "email": "lucas@example.com",
    "password": "...",
    "tasks": [ ... ]
  }
]
```

### 🔹 GET `/Users/{id}`

Retorna um usuário específico com suas tarefas.

**Exemplo:**

```
GET /Users/791a221e-be35-4982-b4e2-53cc157b4972
```

---

### 🔸 POST `/Users`

Cria um novo usuário.

**Body:**

```json
{
  "name": "Beatriz",
  "email": "bea@example.com",
  "password": "Senha123!"
}
```

**Resposta:** `201 Created`

---

### 🔸 PUT `/Users/{id}`

Atualiza os dados de um usuário.

**Body:**

```json
{
  "id": "...",
  "name": "Beatriz Silva",
  "email": "bea@example.com",
  "password": "NovaSenha123"
}
```

---

### 🔸 DELETE `/Users/{id}`

Remove o usuário e todas suas tarefas.

**Exemplo:**

```
DELETE /Users/791a221e-be35-4982-b4e2-53cc157b4972
```

---

## Rotas de Tarefas vinculadas a Usuários (`/Users/{userId}/tasks`)

### 🔸 POST `/Users/{userId}/tasks`

Cria uma nova tarefa para o usuário.

**Body:**

```json
{
  "title": "Estudar EF Core",
  "description": "Revisar migrations",
  "dueDate": "2025-05-25T10:00:00",
  "priority": 2,
  "status": "pending"
}
```

**Resposta:** `201 Created`

---

### 🔹 PUT `/Users/{userId}/tasks/{taskId}`

Edita uma tarefa do usuário.

**Body:**

```json
{
  "title": "Atualizar slides",
  "description": "Incluir exemplos de código",
  "dueDate": "2025-05-25T15:00:00",
  "priority": 1,
  "status": "completed"
}
```

---

### 🔸 DELETE `/Users/{userId}/tasks/{taskId}`

Remove uma tarefa do usuário.

**Exemplo:**

```
DELETE /Users/ce83d089-16fa-430e-a9e2-457537e9e3aa/tasks/7d3d5b8c-4e06-408f-bdea-e83488e9c97e
```

**Resposta:** `204 No Content`

---

## Códigos de Status Comuns

* `200 OK` - Requisição bem-sucedida
* `201 Created` - Recurso criado com sucesso
* `204 No Content` - Recurso atualizado ou removido com sucesso
* `400 Bad Request` - Erro de validação
* `404 Not Found` - Usuário ou Tarefa não encontrados

---

## Observações

* Todas as tarefas estão associadas a um usuário via `UserId`.
* Todos os endpoints retornam JSON.
* Swagger UI está disponível em: `http://localhost:5261/swagger`
