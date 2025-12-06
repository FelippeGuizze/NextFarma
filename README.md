# NextFarma 💊

> **Projeto desenvolvido durante o Hackathon utilizando ASP.NET Core MVC (C#).**

---

## 🚀 Tecnologias Utilizadas

O projeto foi construído sobre uma stack sólida e moderna:

* **ASP.NET Core MVC** (Framework Web)
* **.NET 10** (Plataforma de Desenvolvimento)
* **C#** (Linguagem Principal)
* **Entity Framework Core** (ORM)
* **MySQL** (Banco de Dados - Driver Pomelo)
* **Bootstrap 5** (Frontend/Layout Responsivo)

---

## ⚙️ Configuração do Ambiente

### 1. Pré-requisitos
Certifique-se de ter instalado em sua máquina:
* [**.NET SDK**](https://dotnet.microsoft.com/download) (Versão compatível com o projeto)
* **MySQL Server** em execução (via XAMPP, Workbench ou Docker).
* Ferramenta de linha de comando do EF (caso não tenha, veja abaixo).

### 2. Configurar a Conexão (Connection String)
Abra o arquivo `appsettings.json` na raiz do projeto e configure a seção `ConnectionStrings` com as credenciais do seu banco MySQL local:

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

    ⚠️ Nota: Se você utiliza o XAMPP e não configurou senha para o usuário root, deixe o campo password= vazio ou remova a chave password.

🗄️ Gerenciamento do Banco de Dados (Migrations)

Utilize os comandos abaixo na raiz do projeto para gerenciar a evolução do esquema do banco de dados.

Instalação das Ferramentas (Caso necessário):
Bash

dotnet tool install --global dotnet-ef

➤ Ciclo de Vida das Migrations

Para manter a consistência, siga esta ordem lógica: Criar ➝ Aplicar ➝ (Opcional) Desfazer.
1️⃣ Criar uma nova Migration (Add)

Sempre que você alterar um Model (ex: adicionar uma coluna em Usuario.cs), execute este comando para gerar o arquivo de histórico:
Bash

dotnet ef migrations add NomeDaAlteracao

Exemplo: dotnet ef migrations add AdicionandoCampoCPF
2️⃣ Aplicar alterações no Banco (Update)

Este passo é obrigatório após criar uma migration. Ele efetivamente cria ou atualiza as tabelas no MySQL. Deve ser rodado também ao baixar o projeto pela primeira vez.
Bash

dotnet ef database update

3️⃣ Desfazer a última Migration (Remove)

Caso tenha criado uma migration errada (e ainda não tenha aplicado o comando update), use este comando para excluí-la:
Bash

dotnet ef migrations remove

    Obs: Se já tiver aplicado no banco, você precisará rodar dotnet ef database update NomeDaMigrationAnterior antes de remover.

🌱 Dados Iniciais (Seeding Service)

O sistema possui um serviço de Seeding que popula o banco automaticamente na primeira execução, criando um usuário administrador para testes.
Tipo	Email	Senha
Administrador	admin@nextfarma.com	1234

Perfil: Administrador
🖥️ Estrutura do Projeto

Uma visão geral das principais pastas e responsabilidades:

    📂 Controllers/

        LoginController: Gerencia a autenticação e sessão do usuário.

        CadastroController: Gerencia o registro de novos usuários com validação de email único.

    📂 Models/

        Usuario: Entidade principal contendo Email, Senha, Data de Nascimento e Tipo de Pessoa.

            Enum: Administrador, Professor, Aluno.

    📂 Views/

        Telas construídas com Razor Pages e uso extensivo de Tag Helpers.

▶️ Como Executar

Após configurar o banco de dados e aplicar as migrations, execute o comando na raiz do projeto:
Bash

dotnet run

Acesse no seu navegador: 👉 https://localhost:7260 (ou a porta indicada no seu terminal).