# 🐳 Guia Docker - TaskAPI (.NET 9 + SQL Server)

Este documento explica como rodar a API TaskAPI dentro de containers usando Docker e Docker Compose. Ele cobre desde o build da imagem até a conexão com o banco e a exposição da API na porta `5261`.

---

## 📦 Estrutura esperada

```
TaskApi/
├─ Controllers/
├─ Models/
├─ Program.cs
├─ TaskApi.csproj
├─ appsettings.json
├─ Dockerfile
├─ docker-compose.yml
└─ docker-guide.md ← este arquivo
```

---

## 🚀 Subir o projeto (API + Banco)

Dentro da pasta onde está o `docker-compose.yml`, rode:

```bash
docker-compose up --build
```

Esse comando:

* Builda a imagem da API
* Sobe a API na porta `http://localhost:5261`
* Sobe o SQL Server (porta `1433`) com o banco `TaskDb`

---

## 📍 Acessar a API

Após subir os containers, abra no navegador:

```
http://localhost:5261/swagger
```

---

## 🔄 Atualizar a API após mudanças

Sempre que modificar o código da API (ou as migrations), execute:

```bash
docker-compose down
docker-compose up --build
```

---

## 🧹 Derrubar os containers

Para parar e remover todos os containers da aplicação:

```bash
docker-compose down
```

---

## 🐳 Ver containers ativos

```bash
docker ps
```

---

## 🔍 Ver logs da API

```bash
docker-compose logs -f
```

---

## 👥 Acessar o terminal dentro do container

**API:**

```bash
docker exec -it task-api-container sh
```

**Banco de Dados (SQL Server):**

```bash
docker exec -it sqlserver /bin/bash
```

---

## 🛠️ Configuração atual de portas

* `localhost:5261` → API (.NET rodando na porta 8080 dentro do container)
* `localhost:1433` → SQL Server

---

## 🔐 String de conexão usada (na API)

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=sqlserver;Database=TaskDb;User Id=sa;Password=StrongPass123!;TrustServerCertificate=True;MultipleActiveResultSets=True"
}
```

---

## 📌 Observações finais

* O banco SQL Server está rodando em um container separado com persistência opcional.
* Migrations são aplicadas automaticamente na inicialização da API.
* Porta `5261` é a que o front-end deve usar pra se comunicar com a API.
