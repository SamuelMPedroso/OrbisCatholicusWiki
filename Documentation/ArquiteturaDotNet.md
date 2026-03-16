# Arquitetura .NET — Orbis Catholicus Wiki

## Visão Geral

O backend do Orbis Catholicus Wiki segue **Clean Architecture** com .NET 8, separando responsabilidades em 4 camadas independentes.

```
┌─────────────────────────────────────────────┐
│                  Frontend                   │
│         (HTML/CSS/JS - Wikipedia-style)     │
└──────────────────┬──────────────────────────┘
                   │ HTTP/REST (JSON)
┌──────────────────▼──────────────────────────┐
│            OrbisCatholicus.API              │
│  ● Controllers (REST endpoints)             │
│  ● Middlewares (Auth, Error Handling, CORS) │
│  ● Filters (Validation, Logging)            │
└──────────────────┬──────────────────────────┘
                   │ Dependency Injection
┌──────────────────▼──────────────────────────┐
│        OrbisCatholicus.Application          │
│  ● Services (Business Logic)                │
│  ● DTOs (Data Transfer Objects)             │
│  ● Validators (FluentValidation)            │
│  ● Mappings (AutoMapper Profiles)           │
└──────────────────┬──────────────────────────┘
                   │ Interfaces
┌──────────────────▼──────────────────────────┐
│          OrbisCatholicus.Domain             │
│  ● Entities (Article, User, Category...)    │
│  ● Enums (ReviewStatus, AccessLevel...)     │
│  ● Interfaces (IArticleRepository...)       │
│  ● Value Objects                            │
└──────────────────┬──────────────────────────┘
                   │ Implementations
┌──────────────────▼──────────────────────────┐
│       OrbisCatholicus.Infrastructure        │
│  ● AppDbContext (EF Core)                   │
│  ● Repositories (Data Access)               │
│  ● Configurations (Fluent API)              │
│  ● Migrations                               │
│  ● External Services                        │
└──────────────────┬──────────────────────────┘
                   │
            ┌──────▼──────┐
            │ PostgreSQL  │
            │   16+       │
            └─────────────┘
```

## Entidades Principais

### User
```csharp
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public int AccessLevelId { get; set; }
    public bool IsActive { get; set; }
    public bool EmailConfirmed { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeactivatedAt { get; set; }

    // Navigation
    public AccessLevel AccessLevel { get; set; }
    public ICollection<Article> Articles { get; set; }
    public ICollection<Comment> Comments { get; set; }
}
```

### Article
```csharp
public class Article
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string? Summary { get; set; }
    public int? CategoryId { get; set; }
    public int? CurrentVersionId { get; set; }
    public int CreatedBy { get; set; }
    public string? FeaturedImageUrl { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
    public int Views { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeactivatedAt { get; set; }

    // Navigation
    public Category? Category { get; set; }
    public ArticleVersion? CurrentVersion { get; set; }
    public User Author { get; set; }
    public ICollection<ArticleVersion> Versions { get; set; }
    public ICollection<ArticleTag> Tags { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<ArticleReference> References { get; set; }
}
```

### ArticleVersion
```csharp
public class ArticleVersion
{
    public int Id { get; set; }
    public int ArticleId { get; set; }
    public int VersionNumber { get; set; }
    public string Content { get; set; }      // Markdown
    public string ContentHtml { get; set; }   // Rendered HTML
    public string? EditSummary { get; set; }
    public int CreatedBy { get; set; }
    public int? ReviewStatusId { get; set; }
    public int? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Article Article { get; set; }
    public User Author { get; set; }
    public ReviewStatus? ReviewStatus { get; set; }
}
```

## Endpoints da API (Fase 2)

### Artigos
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/articles` | Listar artigos (paginado) |
| GET | `/api/articles/{slug}` | Obter artigo por slug |
| GET | `/api/articles/featured` | Artigos em destaque |
| GET | `/api/articles/recent` | Artigos recentes |
| GET | `/api/articles/popular` | Mais visualizados |
| POST | `/api/articles` | Criar artigo (Auth: Autor+) |
| PUT | `/api/articles/{id}` | Editar artigo (Auth: Autor+) |
| DELETE | `/api/articles/{id}` | Desativar artigo (Auth: Admin) |

### Versões
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/articles/{id}/versions` | Listar versões |
| POST | `/api/articles/{id}/versions` | Nova versão |
| PUT | `/api/versions/{id}/review` | Revisar versão (Auth: Revisor+) |

### Categorias
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/categories` | Listar categorias |
| GET | `/api/categories/{slug}` | Artigos por categoria |

### Busca
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/search?q={term}` | Full Text Search |

### Autenticação
| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/auth/register` | Registro |
| POST | `/api/auth/login` | Login (retorna JWT) |
| POST | `/api/auth/refresh` | Refresh token |

## Pacotes NuGet Recomendados

```xml
<!-- API -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" />
<PackageReference Include="Swashbuckle.AspNetCore" />  <!-- Swagger -->
<PackageReference Include="Serilog.AspNetCore" />

<!-- Application -->
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" />
<PackageReference Include="FluentValidation.AspNetCore" />

<!-- Infrastructure -->
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" />
```

## Configuração do DbContext

```csharp
public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<AccessLevel> AccessLevels => Set<AccessLevel>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<ArticleVersion> ArticleVersions => Set<ArticleVersion>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ReviewStatus> ReviewStatuses => Set<ReviewStatus>();
    public DbSet<ArticleReviewHistory> ArticleReviewHistories => Set<ArticleReviewHistory>();
    public DbSet<ArticleTag> ArticleTags => Set<ArticleTag>();
    public DbSet<ArticleReference> ArticleReferences => Set<ArticleReference>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<CommentReaction> CommentReactions => Set<CommentReaction>();
    public DbSet<UserFavorite> UserFavorites => Set<UserFavorite>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        // Global Query Filter para Soft Delete
        modelBuilder.Entity<Article>().HasQueryFilter(a => a.IsActive);
        modelBuilder.Entity<User>().HasQueryFilter(u => u.IsActive);
        modelBuilder.Entity<Category>().HasQueryFilter(c => c.IsActive);
        modelBuilder.Entity<Comment>().HasQueryFilter(c => c.IsActive);
    }
}
```

## Fluxo de Revisão de Artigos

```
  Autor cria artigo
        │
        ▼
  ┌─────────────┐
  │  Rascunho   │ ◄── Autor pode editar livremente
  └──────┬──────┘
         │ Autor submete para revisão
         ▼
  ┌─────────────┐
  │  Pendente   │ ◄── Entra na fila de revisão
  └──────┬──────┘
         │ Revisor assume
         ▼
  ┌─────────────┐
  │ Em Revisão  │ ◄── Revisor analisa o conteúdo
  └──────┬──────┘
         │
    ┌────┴────┐
    ▼         ▼
┌────────┐ ┌──────────┐
│Aprovado│ │Rejeitado │
└────────┘ └────┬─────┘
                │ Autor corrige
                ▼
         ┌───────────────┐
         │Rev. Solicitada│ ──► volta para "Pendente"
         └───────────────┘
```

## Segurança

- **Autenticação**: JWT Bearer tokens com refresh tokens.
- **Autorização**: Claims-based com 4 níveis de acesso.
- **Senhas**: BCrypt hashing via ASP.NET Core Identity.
- **CORS**: Configurado para origens específicas.
- **Rate Limiting**: Proteção contra abuso de endpoints.
- **Input Sanitization**: Validação de todo input do usuário.
- **SQL Injection**: Prevenido pelo EF Core (parameterized queries).
- **XSS**: Sanitização de HTML no conteúdo dos artigos.
