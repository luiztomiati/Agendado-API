# 💈 Agendado API

API RESTful desenvolvida em .NET para gerenciamento de **agendamentos de serviços**, projetada para atender múltiplas empresas, como barbearias, salões de beleza e clínicas.

---

## 📖 Sobre o projeto

O **Agendado API** permite que empresas gerenciem seus profissionais, serviços e horários, possibilitando que clientes realizem agendamentos de forma organizada e escalável.

A aplicação foi construída seguindo boas práticas de arquitetura, separação de responsabilidades e segurança, visando um ambiente preparado para crescimento e uso em produção.

---

## 🧩 Principais funcionalidades

### 🏢 Gestão de Empresas

* Criação de empresas
* Criação de usuário com perfil **SUPER** ao cadastrar uma empresa
* Isolamento de dados por empresa

---

### 👤 Gestão de Usuários

* Criação de usuários (restrito ao perfil SUPER)
* Edição de usuários (restrito ao perfil SUPER e ao próprio usuário)
* Remoção de usuários (restrito ao perfil SUPER)
* Associação automática à empresa do usuário autenticado

---

### 📅 Gestão de Agendamentos *(em evolução)*

* Criação de agendamentos
* Seleção de profissional
* Definição de horários
* Associação de serviços ao agendamento

---

### 💼 Serviços e Profissionais *(roadmap)*

* Cadastro de serviços por empresa
* Definição de preços por profissional
* Gestão de disponibilidade de agenda

---

## 🔐 Autenticação e Segurança

A API utiliza autenticação baseada em **JWT (JSON Web Token)** com ASP.NET Identity.

### 🔑 Características:

* Senhas armazenadas com hash (Identity)
* Geração de tokens assinados com chave secreta
* Controle de acesso por perfil (ex: SUPER)
* Uso de variáveis de ambiente para dados sensíveis

### 📌 Uso do token:

```http
Authorization: Bearer {seu_token}
```

---

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas, promovendo organização e manutenibilidade:

* **Controllers** → Entrada da API (HTTP)
* **Services** → Regras de negócio
* **Repositories** → Acesso a dados
* **Domain/Entities** → Modelagem do sistema

---

## ⚙️ Tecnologias utilizadas

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* ASP.NET Identity
* JWT (Json Web Token)
* Docker

---

## 🧪 Configuração do ambiente

### 📁 Variáveis de ambiente (.env)

Crie um arquivo `.env` na raiz do projeto:

```env
JWT__KEY=sua_chave_super_secreta
JWT__ISSUER=seu_issuer
JWT__AUDIENCE=seu_audience

ConnectionStrings__Connection=Host=localhost;Port=5432;Database=SeuBanco;Username=usuario;Password=senha
```

---

## ▶️ Execução do projeto

```bash
dotnet restore
dotnet ef database update
dotnet run
```

---

## 📌 Boas práticas aplicadas

* Separação de responsabilidades (Controller / Service / Repository)
* Uso de DTOs para comunicação externa
* Proteção de dados sensíveis via `.env`
* Autenticação segura com JWT
* Estrutura preparada para multi-tenant

---

## 🚀 Roadmap

* [ ] Implementação completa de agendamentos
* [ ] Controle de disponibilidade por profissional
* [ ] Refresh Token
* [ ] Logs estruturados
* [ ] Deploy em ambiente cloud

---

## 📄 Licença

Este projeto foi desenvolvido para fins de estudo, evolução técnica e demonstração de habilidades em desenvolvimento backend.
