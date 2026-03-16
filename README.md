<img align="center" alt="OrbisCatholicusWiki" height="175" width="175" src="https://media.discordapp.net/attachments/746489793646428160/1333501523249991792/logo_for_a_Catholic_church-related_wiki_company_with_a_book_and_the_cross_of_Christ.jpg?ex=67991f92&is=6797ce12&hm=c46ce817fd1354cc5d3c0e6348a91d82e5cb4199229906de892456348555bf82&=&format=webp" />

# Orbis Catholicus Wiki

Enciclopédia católica colaborativa estilo Wikipedia, onde usuários podem criar, editar e validar artigos sobre a fé católica.
Conteúdo revisado por moderadores para garantir fidelidade doutrinária e rigor histórico.

---

## **Tecnologias Utilizadas** ⚙️

### **Backend**
- **.NET 8** (ASP.NET Core Web API): API RESTful para servir o frontend e gerenciar toda a lógica de negócios.
- **Entity Framework Core 8**: ORM para acesso ao banco de dados PostgreSQL com migrations automáticas.
- **ASP.NET Core Identity**: Autenticação e autorização com suporte a JWT tokens.
- **FluentValidation**: Validação de dados de entrada nas requisições.
- **AutoMapper**: Mapeamento entre entidades e DTOs.
- **Serilog**: Logging estruturado para monitoramento e diagnóstico.

### **Frontend**
- **HTML5 / CSS3**: Estrutura semântica e estilização estilo Wikipedia com tema católico.
- **JavaScript Vanilla**: Interatividade sem dependência de frameworks pesados.
- **Font Awesome 6**: Iconografia consistente.
- **Design Responsivo**: Mobile-first com breakpoints para tablet e desktop.

### **Banco de Dados**
- **PostgreSQL 16+**: Banco relacional com Full Text Search (pg_trgm + unaccent).
- **Views Materializadas**: Para consultas complexas de performance.
- **Soft Delete**: Todas as entidades usam `is_active` + `deactivated_at`.

---

## **Arquitetura do Sistema** 🏗️

### **Estrutura de Pastas (.NET)**
```
OrbisCatholicusWiki/
├── src/
│   ├── OrbisCatholicus.API/           # ASP.NET Core Web API
│   │   ├── Controllers/               # Endpoints REST
│   │   ├── Middlewares/                # Exception handling, logging
│   │   ├── Filters/                   # Action filters, validação
│   │   └── Program.cs
│   │
│   ├── OrbisCatholicus.Application/   # Camada de aplicação
│   │   ├── DTOs/                      # Data Transfer Objects
│   │   ├── Interfaces/                # Contratos de serviços
│   │   ├── Services/                  # Implementação dos serviços
│   │   ├── Mappings/                  # Profiles do AutoMapper
│   │   └── Validators/               # FluentValidation
│   │
│   ├── OrbisCatholicus.Domain/        # Camada de domínio
│   │   ├── Entities/                  # Entidades do domínio
│   │   ├── Enums/                     # Enumeradores
│   │   └── Interfaces/               # Repositórios (contratos)
│   │
│   └── OrbisCatholicus.Infrastructure/ # Infraestrutura
│       ├── Data/                      # DbContext, Configurations
│       ├── Repositories/             # Implementação dos repos
│       └── Migrations/               # EF Core Migrations
│
├── tests/
│   ├── OrbisCatholicus.UnitTests/
│   └── OrbisCatholicus.IntegrationTests/
│
├── frontend/                          # Frontend estático
│   ├── index.html                     # Página principal (portal)
│   ├── article.html                   # Template de artigo
│   ├── category.html                  # Listagem de categorias
│   ├── search.html                    # Resultados de pesquisa
│   ├── css/style.css                  # Tema Wikipedia católico
│   └── js/app.js                      # Interatividade
│
├── Documentation/
│   ├── RegrasGit                      # Convenções de Git
│   ├── resumoBaseWiki.txt             # Resumo do banco
│   └── ArquiteturaDotNet.md           # Documentação da arquitetura
│
├── scriptTabelas.sql                  # Schema PostgreSQL completo
└── README.md
```

### **Padrão Arquitetural**
- **Clean Architecture**: Separação em camadas (API → Application → Domain ← Infrastructure).
- **Repository Pattern**: Abstração de acesso a dados via interfaces + UnitOfWork pattern.
- **Dependency Injection**: Registrado em Program.cs (Autofac não necessário).
- **CQRS Leve**: Separação de leitura e escrita nos serviços (ArticleService, PortalService).

### **Configuração Atual**
- **Autenticação**: JWT Bearer com HS256, roles integrados ao `ClaimsPrincipal`
- **Banco de Dados**: PostgreSQL com EF Core 8 e soft delete habilitado
- **ORM**: Entity Framework Core 8 (Npgsql provider)
- **Validação**: FluentValidation com pipelines
- **Mapeamento**: AutoMapper (MappingProfile)
- **Logging**: Serilog estruturado com rotação de logs diária

### **Evolução Planejada**
- **Fase 1** ✅ (Atual - 85%): API REST .NET 8 com CRUD, autenticação JWT, endpoints principais.
- **Fase 2**: Integração completa do frontend com API, testes e bug fixes.
- **Fase 3**: Sistema de revisão e versionamento completo, permissões granulares.
- **Fase 4**: Busca Full Text otimizada, cache com Redis, possível Elasticsearch.

---

## **Schema do Banco de Dados** 📊

| Tabela | Descrição |
|---|---|
| `access_levels` | Níveis de acesso (Leitor, Autor, Revisor, Admin) |
| `users` | Usuários com perfil e autenticação |
| `categories` | Categorias hierárquicas com slugs |
| `articles` | Artigos com slug, sumário e contagem de views |
| `article_versions` | Versionamento completo de conteúdo |
| `review_status` | Status de revisão com cores |
| `article_review_history` | Histórico de decisões de revisão |
| `article_tags` | Tags dos artigos |
| `article_references` | Referências bibliográficas |
| `related_articles` | Artigos relacionados |
| `comments` | Comentários com suporte a respostas (threads) |
| `comment_reactions` | Likes/dislikes nos comentários |
| `user_favorites` | Artigos favoritos do usuário |
| `activity_log` | Log de auditoria com JSONB |

### **Estado do Banco de Dados**
- ✅ **Schema definido**: Todas as entidades mapeadas em `OrbisCatholicus.Infrastructure/Data/AppDbContext.cs`
- ✅ **Relacionamentos**: Configurados com `FluentAPI` e navegações
- ⚠️ **Migrations**: Nenhuma criada ainda - banco vazio
- ⚠️ **Seed Data**: Não implementado - precisa inserir dados iniciais
- ✅ **Soft Delete**: Query filters habilitados (Article, User, Category, Comment)
- ✅ **Connection String**: Configurada em `appsettings.json` (PostgreSQL local)

---

## **Funcionalidades** 🌟

- **Portal Wikipedia-style**: Página principal com artigo em destaque, categorias, artigos recentes e mais lidos.
- **Artigos com Infobox**: Estilo Wikipedia com tabela lateral de dados rápidos.
- **12 Categorias Católicas**: Doutrina, Santos, Liturgia, História, Papado, Ordens, Sacramentos, Direito Canônico, Filosofia, Arte Sacra, Mariologia, Concílios.
- **Sistema de Revisão**: Fluxo completo: Rascunho → Pendente → Em Revisão → Aprovado/Rejeitado.
- **Versionamento**: Histórico completo de edições com diff entre versões.
- **Busca Full Text**: Preparado para pg_trgm + unaccent (implementação pendente).
- **Controle de Acesso**: 4 níveis (Leitor, Autor, Revisor, Administrador) com autorização por endpoint.
- **Soft Delete**: Nenhum dado é excluído permanentemente (é_ativo + desativado_em).
- **Log de Auditoria**: Conceitual (ActivityLog entity criada, implementação pendente).
- **Responsivo**: Frontend mobile-first com breakpoints.

---

---

## **Status Atual do Projeto** 📊

### **Backend API (.NET 8)**

#### Componentes Implementados (85% de Progresso):
- ✅ **Program.cs**: DI, middlewares, autenticação JWT, CORS, Swagger/OpenAPI
- ✅ **17 Endpoints**: Articles, Auth, Categories, Search, Portal, ArticleVersions
- ✅ **5 Serviços**: ArticleService, AuthService, CategoryService, ArticleVersionService, PortalService
- ✅ **Autenticação JWT**: HS256, roles (Leitor, Autor, Revisor, Admin), refresh tokens
- ✅ **DTOs**: Estruturados para todos os endpoints
- ✅ **Validação**: FluentValidation integrado
- ✅ **Logging**: Serilog (console + arquivo com rotação)
- ✅ **Exceções**: GlobalExceptionMiddleware para tratamento centralizado

#### Dependências do Banco de Dados (Crítico - 40%):
- ⚠️ **AppDbContext**: Configurado para PostgreSQL (Npgsql)
- ⚠️ **Migrations**: Nenhuma criada (próximo passo essencial)
- ⚠️ **Seed Data**: Não implementado (precisa inserir AccessLevels, ReviewStatuses)

#### Relação de Endpoints Implementados:

| Endpoint | Autenticado | Descrição |
|----------|------------|-----------|
| `GET /api/articles/featured` | ❌ | 5 artigos em destaque |
| `GET /api/articles/recent` | ❌ | 10 artigos recentes |
| `GET /api/articles/popular` | ❌ | 10 artigos mais vistos |
| `GET /api/articles/{slug}` | ❌ | Detalhe do artigo (incrementa views) |
| `POST /api/articles` | ✅ (Autor+) | Criar artigo |
| `PUT /api/articles/{id}` | ✅ (Autor+) | Editar artigo |
| `DELETE /api/articles/{id}` | ✅ (Revisor+) | Desativar artigo |
| `POST /api/auth/register` | ❌ | Registrar usuário |
| `POST /api/auth/login` | ❌ | Login (JWT + refresh token) |
| `POST /api/auth/refresh` | ❌ | Renovar access token |
| `GET /api/auth/profile` | ✅ | Perfil do usuário autenticado |
| `GET /api/categories` | ❌ | Listar categorias |
| `GET /api/categories/{slug}` | ❌ | Detalhe + artigos paginados |
| `GET /api/categories/{slug}/articles` | ❌ | Artigos da categoria (paginado) |
| `GET /api/search` | ❌ | Busca por termo (paginado) |
| `GET /api/portal/stats` | ❌ | Dashboard stats |
| `GET /api/articles/{id}/versions` | ❌ | Histórico de versões |
| `POST /api/articles/{id}/versions` | ✅ (Autor+) | Criar nova versão |
| `PUT /api/versions/{id}/review` | ✅ (Revisor+) | Revisar/comentar versão |

### **Frontend (Estático - Fase 1)**
- ✅ HTML/CSS: Templates para portal, artigos, categorias, busca
- ✅ JavaScript: Integração básica com a API
- ⚠️ Interatividade: Parcialmente testável (depende da API funcionar)

---

## **Como Executar** 🚀

### **1. Preparar Banco de Dados PostgreSQL**
```bash
# Criar database
createdb orbis_catholicus

# Executar migrations (cria schema)
cd src/OrbisCatholicus.API
dotnet ef database update

# OU executar script SQL manualmente
psql -U postgres -d orbis_catholicus -f scriptTabelas.sql
```

### **2. Seed Data Inicial (Crítico!)**
O banco precisa dos dados iniciais de `AccessLevels` e `ReviewStatuses`. Executar:
```sql
-- Insert AccessLevels
INSERT INTO public.access_levels (name, description, is_active)
VALUES 
('Leitor', 'Somente visualizar artigos', true),
('Autor', 'Criar e editar artigos', true),
('Revisor', 'Revisar e aprovar artigos', true),
('Admin', 'Acesso total ao sistema', true);

-- Insert ReviewStatuses
INSERT INTO public.review_statuses (name, display_name, color, is_active)
VALUES 
('Draft', 'Rascunho', '#d3d3d3', true),
('PendingReview', 'Pendente de Revisão', '#ffa500', true),
('UnderReview', 'Em Revisão', '#87ceeb', true),
('Approved', 'Aprovado', '#228b22', true),
('Rejected', 'Rejeitado', '#dc143c', true);
```

### **3. Executar Backend API**
```bash
cd src/OrbisCatholicus.API
dotnet restore
dotnet run

# API disponível em https://localhost:7289
# Swagger em https://localhost:7289/swagger
```

### **4. Executar Frontend (Estático)**
```bash
# Com Python
python -m http.server 8080

# Com Node.js
npx serve .

# Frontend em http://localhost:8080
```

---

## **Testes Recomendados** ✅

1. **Registrar usuário**: `POST /api/auth/register`
2. **Fazer login**: `POST /api/auth/login`
3. **Listar artigos**: `GET /api/articles/featured`
4. **Criar artigo** (autenticado): `POST /api/articles`
5. **Visualizar Swagger**: `https://localhost:7289/swagger`

---

## **Próximos Passos Críticos** ⚠️

1. **[URGENTE]** Criar migrations EF Core
2. **[URGENTE]** Implementar seed data (AccessLevels, ReviewStatuses)
3. Testar todos os endpoints com Postman/Swagger
4. Implementar autorização detalhada nos endpoints protegidos
5. Integrar frontend com endpoints da API
6. Validar lógica completa dos services
7. Testes unitários para serviços
8. Testes de integração da API

---

## **Regras de Git** 📋

Consulte [Documentation/RegrasGit](Documentation/RegrasGit) para as convenções de branches, commits e pull requests.
