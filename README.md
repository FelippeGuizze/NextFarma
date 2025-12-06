# NextFarma 💊

Sistema web desenvolvido em **ASP.NET Core MVC** (.NET 10) para gerenciamento farmacêutico, utilizando **MySQL** como banco de dados e **Entity Framework Core** para ORM.

O projeto conta com um sistema completo de Autenticação (Login e Cadastro) e controle de permissões de usuário.

## 🚀 Tecnologias Utilizadas

* **ASP.NET Core MVC** (Framework Web)
* **C#** (Linguagem)
* **Entity Framework Core** (ORM)
* **MySQL** (Banco de Dados - Driver Pomelo)
* **Bootstrap 5** (Frontend/Layout)

---

## ⚙️ Configuração do Ambiente

### 1. Pré-requisitos
Certifique-se de ter instalado em sua máquina:
* [.NET SDK](https://dotnet.microsoft.com/download) (Versão compatível com o projeto)
* **MySQL Server** rodando (pode ser via XAMPP, Workbench ou Docker).
* Ferramenta de linha de comando do EF (`dotnet ef`).

### 2. Configurar a Conexão (Connection String)
Abra o arquivo `appsettings.json` na raiz do projeto e configure a seção `ConnectionStrings` com os dados do seu banco MySQL local:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=NextFarmaDb;user=root;password=SUA_SENHA_AQUI"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}