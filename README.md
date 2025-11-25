# 🛍️ E-Commerce Microservices - Sistema de Gestão de Estoque e Vendas

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-316192?logo=postgresql)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-3.13-FF6600?logo=rabbitmq)
![Docker](https://img.shields.io/badge/Docker-Latest-2496ED?logo=docker)

Sistema de microserviços para gerenciamento de estoque de produtos e vendas em uma plataforma de e-commerce, desenvolvido como projeto final de bootcamp em .NET.

---

## 📋 Índice

- [Sobre o Projeto](#-sobre-o-projeto)
- [Arquitetura](#-arquitetura)
- [Tecnologias Utilizadas](#-tecnologias-utilizadas)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Pré-requisitos](#-pré-requisitos)
- [Instalação e Configuração](#-instalação-e-configuração)
- [Executando o Projeto](#-executando-o-projeto)
- [Endpoints da API](#-endpoints-da-api)
- [Funcionalidades Implementadas](#-funcionalidades-implementadas)
- [Próximas Etapas](#-próximas-etapas)
- [Contribuindo](#-contribuindo)

---

## 🎯 Sobre o Projeto

Este projeto implementa uma arquitetura de microserviços para um sistema de e-commerce, com foco na separação de responsabilidades entre gestão de estoque e vendas. O sistema utiliza práticas modernas de desenvolvimento, incluindo:

- **Clean Architecture** (camadas Domain, Application, Infrastructure e API)
- **Domain-Driven Design (DDD)**
- **CQRS Pattern** (preparado para implementação)
- **Event-Driven Architecture** (com RabbitMQ)
- **API Gateway** (para roteamento centralizado)

### 🎓 Contexto Acadêmico

Projeto desenvolvido como desafio final do bootcamp de .NET, demonstrando:
- Implementação de microserviços
- Comunicação síncrona e assíncrona entre serviços
- Padrões de arquitetura empresarial
- Boas práticas de desenvolvimento

---

## 🏗️ Arquitetura

### Diagrama de Arquitetura

```
┌─────────────────────────────────────────────────────────────┐
│                         CLIENTE                              │
└──────────────────────────┬──────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────┐
│                     API GATEWAY                              │
│                  (Autenticação JWT)                          │
└──────────────┬─────────────────────┬────────────────────────┘
               │                     │
               ▼                     ▼
┌──────────────────────┐   ┌──────────────────────┐
│  MICROSERVIÇO DE     │   │  MICROSERVIÇO DE     │
│      ESTOQUE         │   │      VENDAS          │
│                      │   │                      │
│ - Gestão Produtos    │   │ - Criação Pedidos    │
│ - Controle Estoque   │   │ - Consulta Pedidos   │
│ - Validação          │   │ - Validação Estoque  │
└──────────┬───────────┘   └───────────┬──────────┘
           │                           │
           │    ┌──────────────────┐   │
           └────► RABBITMQ         ◄───┘
                │ (Mensageria)     │
                └──────────────────┘
           │                           │
           ▼                           ▼
┌──────────────────────┐   ┌──────────────────────┐
│   PostgreSQL         │   │   PostgreSQL         │
│   (EstoqueDB)        │   │   (VendasDB)         │
└──────────────────────┘   └──────────────────────┘
```

### Arquitetura em Camadas (Clean Architecture)

Cada microserviço segue a estrutura:

```
┌─────────────────────────────────────────┐
│              API Layer                   │  ← Controllers, Middlewares
│  (Apresentação / Interface HTTP)        │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│         Application Layer                │  ← Services, DTOs, Validators
│   (Casos de Uso / Orquestração)         │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│           Domain Layer                   │  ← Entidades, Regras de Negócio
│      (Núcleo / Lógica de Negócio)       │
└──────────────────▲──────────────────────┘
                   │
┌──────────────────┴──────────────────────┐
│       Infrastructure Layer               │  ← Repositórios, DbContext, APIs
│  (Acesso a Dados / Serviços Externos)   │
└─────────────────────────────────────────┘
```

**Princípios:**
- ✅ Dependências apontam para o centro (Domain)
- ✅ Domain não conhece Infrastructure
- ✅ Application orquestra Domain e Infrastructure
- ✅ API é apenas a interface de entrada

---

## 🚀 Tecnologias Utilizadas

### Backend
- **.NET 9.0** - Framework principal
- **C# 12** - Linguagem de programação
- **ASP.NET Core** - Web API
- **Entity Framework Core 9.0** - ORM

### Banco de Dados
- **PostgreSQL 16** - Banco de dados relacional
- **Npgsql** - Driver PostgreSQL para .NET

### Mensageria
- **RabbitMQ 3.13** - Message broker para comunicação assíncrona

### Validação e Mapeamento
- **FluentValidation** - Validação de DTOs
- **AutoMapper** - Mapeamento objeto-objeto

### Documentação
- **Swagger/OpenAPI** - Documentação interativa da API

### Autenticação (Preparado)
- **JWT Bearer** - Autenticação stateless

### Containerização
- **Docker** - Containerização de serviços
- **Docker Compose** - Orquestração de containers

### Monitoramento
- **Health Checks** - Verificação de saúde dos serviços

---

## 📁 Estrutura do Projeto

```
ECommerceMicroservices/
├── src/
│   ├── ApiGateway/
│   │   └── ApiGateway/                    # (A implementar)
│   │       ├── Program.cs
│   │       └── ocelot.json
│   │
│   ├── Services/
│   │   ├── Estoque/                       # ✅ IMPLEMENTADO
│   │   │   ├── Estoque.API/               # Controllers, Endpoints
│   │   │   │   ├── Controllers/
│   │   │   │   │   └── ProdutosController.cs
│   │   │   │   ├── Program.cs
│   │   │   │   └── appsettings.json
│   │   │   │
│   │   │   ├── Estoque.Application/       # DTOs, Services, Validators
│   │   │   │   ├── DTOs/
│   │   │   │   │   ├── ProdutoDto.cs
│   │   │   │   │   ├── CriarProdutoDto.cs
│   │   │   │   │   ├── AtualizarProdutoDto.cs
│   │   │   │   │   └── AtualizarEstoqueDto.cs
│   │   │   │   ├── Services/
│   │   │   │   │   └── ProdutoService.cs
│   │   │   │   ├── Interfaces/
│   │   │   │   │   └── IProdutoService.cs
│   │   │   │   ├── Validators/
│   │   │   │   │   ├── CriarProdutoDtoValidator.cs
│   │   │   │   │   └── AtualizarProdutoDtoValidator.cs
│   │   │   │   └── Mappings/
│   │   │   │       └── ProdutoMappingProfile.cs
│   │   │   │
│   │   │   ├── Estoque.Domain/            # Entidades, Regras de Negócio
│   │   │   │   ├── Entities/
│   │   │   │   │   └── Produto.cs
│   │   │   │   ├── Interfaces/
│   │   │   │   │   └── IProdutoRepository.cs
│   │   │   │   └── Exceptions/
│   │   │   │       ├── DomainException.cs
│   │   │   │       ├── NotFoundException.cs
│   │   │   │       └── ValidationException.cs
│   │   │   │
│   │   │   └── Estoque.Infrastructure/    # Repositórios, DbContext
│   │   │       ├── Data/
│   │   │       │   ├── EstoqueDbContext.cs
│   │   │       │   ├── Configurations/
│   │   │       │   │   └── ProdutoConfiguration.cs
│   │   │       │   └── Migrations/
│   │   │       └── Repositories/
│   │   │           └── ProdutoRepository.cs
│   │   │
│   │   └── Vendas/                        # (A implementar)
│   │       ├── Vendas.API/
│   │       ├── Vendas.Application/
│   │       ├── Vendas.Domain/
│   │       └── Vendas.Infrastructure/
│   │
│   └── Shared/                            # (A implementar)
│       └── Shared.Messaging/              # RabbitMQ comum
│
├── docker-compose.yml                     # ✅ Configuração Docker
├── ECommerceMicroservices.sln            # ✅ Solution .NET
└── README.md                              # ✅ Este arquivo
```

---

## 📦 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- **.NET SDK 9.0+** - [Download](https://dotnet.microsoft.com/download)
- **Docker & Docker Compose** - [Download](https://www.docker.com/get-started)
- **Git** - [Download](https://git-scm.com/)
- **(Opcional) Visual Studio 2022** ou **VS Code** com extensão C#

### Verificar Instalações

```bash
# Verificar .NET
dotnet --version
# Saída esperada: 9.0.x

# Verificar Docker
docker --version
docker-compose --version

# Verificar Git
git --version
```

---

## 🔧 Instalação e Configuração

### 1️⃣ Clonar o Repositório

```bash
git clone https://github.com/seu-usuario/ecommerce-microservices.git
cd ecommerce-microservices
```

### 2️⃣ Subir Infraestrutura (Docker)

```bash
# Subir PostgreSQL e RabbitMQ
docker-compose up -d

# Verificar se os containers estão rodando
docker ps
```

**Serviços disponíveis:**
- **PostgreSQL:** `localhost:5432`
- **RabbitMQ AMQP:** `localhost:5672`
- **RabbitMQ Management UI:** `http://localhost:15672` (admin/admin123)

### 3️⃣ Configurar Connection String

Edite `src/Services/Estoque/Estoque.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=EstoqueDB;Username=postgres;Password=minhasenha123"
  }
}
```

**⚠️ Importante:** Substitua `minhasenha123` pela senha definida no Docker Compose.

### 4️⃣ Restaurar Dependências

```bash
# Na raiz do projeto
dotnet restore
```

### 5️⃣ Aplicar Migrations

```bash
cd src/Services/Estoque/Estoque.API

# Criar migration (se necessário)
dotnet ef migrations add InitialCreate \
  --project ../Estoque.Infrastructure/Estoque.Infrastructure.csproj \
  --startup-project Estoque.API.csproj

# Aplicar migrations
dotnet ef database update \
  --project ../Estoque.Infrastructure/Estoque.Infrastructure.csproj \
  --startup-project Estoque.API.csproj
```

---

## 🚀 Executando o Projeto

### Opção 1: Executar Manualmente

```bash
# Microserviço de Estoque
cd src/Services/Estoque/Estoque.API
dotnet run

# Em outro terminal (quando implementado)
cd src/Services/Vendas/Vendas.API
dotnet run
```

### Opção 2: Executar via Visual Studio

1. Abra `ECommerceMicroservices.sln`
2. Configure múltiplos projetos de inicialização:
   - `Estoque.API`
   - `Vendas.API` (quando implementado)
   - `ApiGateway` (quando implementado)
3. Pressione `F5` ou clique em "Start"

### Opção 3: Docker (Futuro)

```bash
# Build e execução de todos os serviços
docker-compose up --build
```

---

## 📡 Endpoints da API

### Microserviço de Estoque

**Base URL:** `http://localhost:5001`

#### 📦 Produtos

| Método | Endpoint | Descrição | Status |
|--------|----------|-----------|--------|
| `GET` | `/api/produtos` | Lista todos os produtos | ✅ |
| `GET` | `/api/produtos/{id}` | Busca produto por ID | ✅ |
| `POST` | `/api/produtos` | Cria novo produto | ✅ |
| `PUT` | `/api/produtos/{id}` | Atualiza produto completo | ✅ |
| `PATCH` | `/api/produtos/{id}/estoque` | Atualiza apenas estoque | ✅ |
| `DELETE` | `/api/produtos/{id}` | Deleta (inativa) produto | ✅ |
| `GET` | `/api/produtos/{id}/disponibilidade?quantidade=X` | Verifica disponibilidade | ✅ |

#### 🏥 Monitoramento

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/health` | Health check do serviço |
| `GET` | `/swagger` | Documentação interativa |

### Exemplos de Requisições

#### Criar Produto

```bash
curl -X POST http://localhost:5001/api/produtos \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Notebook Dell Inspiron",
    "descricao": "Notebook Dell com Intel i5, 8GB RAM, 256GB SSD",
    "preco": 3500.00,
    "quantidadeEstoque": 15
  }'
```

**Resposta (201 Created):**
```json
{
  "id": 1,
  "nome": "Notebook Dell Inspiron",
  "descricao": "Notebook Dell com Intel i5, 8GB RAM, 256GB SSD",
  "preco": 3500.00,
  "quantidadeEstoque": 15,
  "ativo": true
}
```

#### Listar Produtos

```bash
curl http://localhost:5001/api/produtos
```

#### Atualizar Estoque

```bash
curl -X PATCH http://localhost:5001/api/produtos/1/estoque \
  -H "Content-Type: application/json" \
  -d '{
    "quantidade": -5
  }'
```

#### Verificar Disponibilidade

```bash
curl http://localhost:5001/api/produtos/1/disponibilidade?quantidade=10
```

**Resposta:**
```json
{
  "disponivel": true,
  "produtoId": 1,
  "quantidadeSolicitada": 10
}
```

---

## ✨ Funcionalidades Implementadas

### ✅ Microserviço de Estoque

#### Domain Layer
- [x] Entidade `Produto` com validações de negócio
- [x] Métodos de domínio (AtualizarEstoque, AtualizarPreco, etc.)
- [x] Exceções customizadas (DomainException, NotFoundException)
- [x] Interface `IProdutoRepository`

#### Infrastructure Layer
- [x] `EstoqueDbContext` configurado com PostgreSQL
- [x] `ProdutoRepository` implementado
- [x] Configuração Fluent API para `Produto`
- [x] Migrations aplicadas

#### Application Layer
- [x] DTOs (ProdutoDto, CriarProdutoDto, AtualizarProdutoDto, AtualizarEstoqueDto)
- [x] `ProdutoService` com lógica de aplicação
- [x] AutoMapper configurado
- [x] FluentValidation para DTOs

#### API Layer
- [x] `ProdutosController` com CRUD completo
- [x] Swagger configurado
- [x] Health Checks (PostgreSQL)
- [x] CORS habilitado
- [x] Logging estruturado
- [x] Tratamento de exceções

---

## 🔜 Próximas Etapas

### Fase 2: Microserviço de Vendas
- [ ] Criar estrutura de camadas (Domain, Application, Infrastructure, API)
- [ ] Implementar entidades `Pedido` e `ItemPedido`
- [ ] CRUD de pedidos
- [ ] Validação de estoque antes de criar pedido

### Fase 3: Comunicação entre Microserviços
- [ ] Implementar RabbitMQ Publisher no Vendas
- [ ] Implementar RabbitMQ Consumer no Estoque
- [ ] Evento `VendaRealizadaEvent` para atualizar estoque
- [ ] HTTP Client no Vendas para verificar disponibilidade

### Fase 4: Autenticação e Autorização
- [ ] Implementar geração de JWT
- [ ] Endpoint de Login
- [ ] Proteger endpoints com `[Authorize]`
- [ ] Roles e Claims

### Fase 5: API Gateway
- [ ] Implementar Ocelot ou YARP
- [ ] Roteamento centralizado
- [ ] Rate Limiting
- [ ] Agregação de respostas

### Fase 6: Observabilidade
- [ ] Implementar Serilog
- [ ] Distributed Tracing (OpenTelemetry)
- [ ] Métricas (Prometheus)
- [ ] Dashboard (Grafana)

### Fase 7: Testes
- [ ] Testes unitários (xUnit)
- [ ] Testes de integração
- [ ] Testes de contrato (Pact)

### Fase 8: CI/CD
- [ ] GitHub Actions
- [ ] Deploy automatizado
- [ ] Testes automatizados

---

## 🗄️ Modelo de Dados

### Microserviço de Estoque

#### Tabela: `produtos`

| Campo | Tipo | Descrição | Restrições |
|-------|------|-----------|------------|
| `id` | int | ID único (PK) | AUTO_INCREMENT, NOT NULL |
| `nome` | varchar(200) | Nome do produto | NOT NULL |
| `descricao` | varchar(1000) | Descrição detalhada | NULL |
| `preco` | decimal(18,2) | Preço unitário | NOT NULL, > 0 |
| `quantidade_estoque` | int | Quantidade disponível | NOT NULL, >= 0 |
| `ativo` | boolean | Produto ativo/inativo | NOT NULL, DEFAULT true |
| `data_criacao` | timestamp | Data de criação | NOT NULL, DEFAULT NOW() |

---

## 🧪 Testando a Aplicação

### Via Swagger

1. Acesse: `http://localhost:5001/swagger`
2. Teste cada endpoint interativamente
3. Veja as respostas em tempo real

### Via Postman/Insomnia

Importe a collection (se disponível) ou crie manualmente as requisições seguindo a seção [Endpoints da API](#-endpoints-da-api).

### Via curl (Terminal)

Veja exemplos na seção [Exemplos de Requisições](#exemplos-de-requisições).

### Health Check

```bash
curl http://localhost:5001/health
```

**Resposta esperada:**
```
Healthy
```

---

## 🐛 Troubleshooting

### Erro: "password authentication failed"

**Solução:**
```bash
# Redefinir senha do PostgreSQL
docker exec -it ecommerce-postgres psql -U postgres
ALTER USER postgres WITH PASSWORD 'minhasenha123';
\q

# Atualizar appsettings.json com a senha correta
```

### Erro: "Port 5432 is already allocated"

**Solução:**
```bash
# Verificar o que está usando a porta
sudo lsof -i :5432

# Parar o container e liberar a porta
docker-compose down
```

### Erro: Migrations não aplicadas

**Solução:**
```bash
cd src/Services/Estoque/Estoque.API

dotnet ef database update \
  --project ../Estoque.Infrastructure/Estoque.Infrastructure.csproj \
  --startup-project Estoque.API.csproj
```

### Container do PostgreSQL não inicia

**Solução:**
```bash
# Ver logs do container
docker logs ecommerce-postgres

# Remover volumes e recriar
docker-compose down -v
docker-compose up -d
```

---

## 📚 Conceitos Aprendidos

### Clean Architecture
- Separação clara de responsabilidades
- Dependências apontando para o Domain
- Testabilidade e manutenibilidade

### Domain-Driven Design (DDD)
- Entidades ricas em comportamento
- Validações no Domain
- Linguagem ubíqua

### SOLID Principles
- **S**ingle Responsibility: Cada classe tem uma responsabilidade
- **O**pen/Closed: Extensível sem modificar código existente
- **L**iskov Substitution: Interfaces bem definidas
- **I**nterface Segregation: Interfaces específicas
- **D**ependency Inversion: Dependências em abstrações

### Microservices Patterns
- API Gateway
- Service Discovery
- Circuit Breaker (preparado)
- Event-Driven Architecture

---

## 🤝 Contribuindo

Contribuições são bem-vindas! Este é um projeto educacional.

### Como Contribuir

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

### Padrões de Código

- Seguir convenções C#
- Comentários em português
- Testes unitários (quando possível)
- Documentação atualizada

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 👨‍💻 Autor

**Seu Nome**
- GitHub: [@seu-usuario](https://github.com/seu-usuario)
- LinkedIn: [Seu Nome](https://linkedin.com/in/seu-perfil)
- Email: seu@email.com

---

## 🙏 Agradecimentos

- **DIO (Digital Innovation One)** - Bootcamp .NET
- **Comunidade .NET** - Documentação e suporte
- **Instrutores** - Orientação e conhecimento

---

## 📞 Suporte

Para dúvidas ou problemas:

1. Abra uma [Issue](https://github.com/seu-usuario/ecommerce-microservices/issues)
2. Entre em contato via email
3. Consulte a documentação oficial do [.NET](https://docs.microsoft.com/dotnet/)

---

## 🔗 Links Úteis

- [Documentação .NET](https://docs.microsoft.com/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [PostgreSQL](https://www.postgresql.org/docs/)
- [RabbitMQ](https://www.rabbitmq.com/documentation.html)
- [Docker](https://docs.docker.com/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

<div align="center">

**⭐ Se este projeto foi útil, considere dar uma estrela! ⭐**

Feito com ❤️ e ☕ durante o bootcamp .NET

</div>
