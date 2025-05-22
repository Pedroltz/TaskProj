# 🗂️ TaskAPI + Angular Frontend

Este projeto é um sistema de gerenciamento de tarefas com autenticação via Firebase. Ele permite que usuários se cadastrem, façam login e gerenciem suas tarefas de forma visual e interativa.

---

## 🛠️ Tecnologias Utilizadas

### Backend (.NET 9 - Web API)

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server (pode ser local ou remoto)
- Firebase Admin SDK (autenticação)

### Frontend (Angular 17+)

- Angular Standalone Components
- Angular Forms + HttpClient
- Firebase REST API para login

---

## 🚀 Como Rodar o Projeto

### 1. Clonar o repositório

```bash
git clone https://github.com/seu-usuario/task-app.git
cd task-app
```

## 🔧 Backend (.NET)

### Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (ou Azure SQL, LocalDB etc.)
- Firebase Admin SDK + chave JSON (arquivo `firebase-key.json`)

### Setup do banco

1. No `appsettings.json`, configure sua `DefaultConnection` para apontar para o seu banco SQL:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TaskDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

2. Execute os comandos:

```bash
cd TaskAPI/TaskApi
dotnet ef database update
dotnet run
```

O servidor estará rodando em: `http://localhost:5261`

---

## 🌐 Frontend (Angular)

### Pré-requisitos

- Node.js (18+)
- Angular CLI

### Instalação

```bash
cd TaskAngular/TaskAngular
npm install
npm start
```

A aplicação Angular estará em: `http://localhost:4200`

---

## 🔐 Firebase - Autenticação

1. Vá até o [Firebase Console](https://console.firebase.google.com/)
2. Crie um novo projeto
3. Ative o provedor de autenticação por **E-mail e Senha**
4. Crie uma chave de serviço (JSON) em:  
   `Configurações do projeto > Contas de serviço > Gerar nova chave privada`

5. Coloque esse JSON no backend e renomeie para:  
   `firebase-key.json`

---

## 🧩 Funcionalidades

- Registro de usuários (com Firebase Auth)
- Login e proteção de rotas (com Firebase token)
- Criar, editar, excluir e listar tarefas por usuário
- Atualização rápida de status
- Interface responsiva com componentes standalone

---

## 📌 Extras

- A aplicação é modular e separada por pastas de pages e componentes
- O `NavbarComponent` exibe o nome do usuário logado e botão de logout
- O `AuthGuard` protege a página de tarefas caso o usuário não esteja logado

---

## 🧪 Testes Rápidos

### Criar um novo usuário

- Acesse `/register` no frontend
- Preencha nome, email e senha válidos

### Criar uma tarefa

- Após login, clique em "Criar Nova Tarefa"
- Preencha os campos e salve

### Editar ou deletar

- Use os botões diretamente nas tarefas

