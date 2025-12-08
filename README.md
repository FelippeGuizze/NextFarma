# NextFarma 💊

> **Projeto desenvolvido durante o Hackathon utilizando ASP.NET Core MVC (C#).**

O **NextFarma** é um sistema web para gerenciamento farmacêutico, utilizando **MySQL** como banco de dados e **Entity Framework Core**. O projeto conta com sistema de Autenticação (Login/Cadastro) e controle de permissões.

---

## 🚀 Tecnologias
* **ASP.NET Core MVC** (.NET 10)
* **C#**
* **Entity Framework Core**
* **MySQL** (Pomelo)
* **Bootstrap 5**

---

## ⚙️ Configuração Rápida

### 1. Conexão com o Banco
No arquivo `appsettings.json`, ajuste a `DefaultConnection` com seu usuário e senha do MySQL:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=NextFarmaDb;user=root;password=SUA_SENHA"
}
```

### 2. Instalação do Entity Framework Core
Caso precise instalar, use este comando:

```bash
dotnet tool install --global dotnet-ef --version 8.0.0
```

### 3. 🗄️ Comandos do Banco (Migrations)

Execute no terminal na raiz do projeto:

1. Criar Migration (Gera o arquivo de histórico):

```bash
dotnet ef migrations add NomeDaMudanca
```
2. Atualizar Banco (Aplica as mudanças no MySQL):

```bash
dotnet ef database update
```

3. Remover Última Migration (Desfaz a criação, caso não tenha atualizado o banco):
```bash
dotnet ef migrations remove
```

### 4. 🌱 Dados de Acesso (Admin)

O sistema cria automaticamente um usuário administrador ao rodar pela primeira vez:

    Email: admin@nextfarma.com


    Senha: 1234

### 5. ▶️ Como Rodar

Após configurar o banco e rodar o database update, inicie o projeto:

```bash
dotnet run
```
Acesse: https://localhost:7260
