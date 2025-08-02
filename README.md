# FIAP.PLAY

FIAP.PLAY é uma aplicação desenvolvida com .NET Core com o objetivo de gerenciar **usuários** e **jogos**. O projeto está em fase inicial e visa evoluir para uma plataforma mais completa de gerenciamento e consulta de conteúdo interativo.

---

## Objetivos

Atualmente, a aplicação permite:

- Cadastro e gerenciamento de **usuários**  
- Cadastro e gerenciamento de **jogos**

---

## Tecnologias Utilizadas

- **.NET Core**
- **C#**
- **SQL Server**
- **Swagger** (para documentação e testes de API)

---

## Como Executar o Projeto

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/FIAP.PLAY.git
cd FIAP.PLAY
```

### 2. Configure o banco de dados

Certifique-se de que o SQL Server está em execução e que a string de conexão está correta no appsettings.json.

### 3. Aplique as migrations

Abra o terminal do Package Manager Console no Visual Studio e execute:

```bash
Update-Database
```

### 4. Execute o projeto

Pressione F5 no Visual Studio ou clique em Run.

O projeto será iniciado na URL padrão e você poderá acessar o Swagger em:

```bash
https://localhost:{porta}/swagger
```

### 5. Documentação

A documentação interativa está disponível via Swagger.
Você pode utilizar o Swagger UI para testar todas as rotas da API diretamente no navegador.
