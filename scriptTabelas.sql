-- ============================================================
-- OrbisCatholicusWiki - Schema PostgreSQL
-- Arquitetura: .NET 8 + Entity Framework Core + PostgreSQL
-- ============================================================

-- Extensão para UUIDs (opcional, caso queira usar no futuro)
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Extensão para busca textual em português
CREATE EXTENSION IF NOT EXISTS pg_trgm;
CREATE EXTENSION IF NOT EXISTS unaccent;

-- ============================
-- Tabela de Níveis de Acesso
-- ============================
CREATE TABLE access_levels (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) UNIQUE NOT NULL,
    description TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NULL
);

INSERT INTO access_levels (name, description) VALUES 
('Leitor', 'Pode visualizar artigos e comentar'),
('Autor', 'Pode criar e editar artigos'),
('Revisor', 'Pode aprovar ou rejeitar artigos pendentes'),
('Administrador', 'Acesso total ao sistema');

-- ============================
-- Tabela de Usuários
-- ============================
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(100) UNIQUE NOT NULL,
    display_name VARCHAR(150) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash TEXT NOT NULL,
    avatar_url VARCHAR(500) DEFAULT NULL,
    bio TEXT DEFAULT NULL,
    access_level_id INT NOT NULL REFERENCES access_levels(id) ON DELETE RESTRICT,
    is_active BOOLEAN DEFAULT TRUE,
    email_confirmed BOOLEAN DEFAULT FALSE,
    last_login_at TIMESTAMP WITH TIME ZONE DEFAULT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NULL,
    deactivated_at TIMESTAMP WITH TIME ZONE DEFAULT NULL
);

CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_users_username ON users(username);
CREATE INDEX idx_users_access_level ON users(access_level_id);

-- ============================
-- Tabela de Categorias
-- ============================
CREATE TABLE categories (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) UNIQUE NOT NULL,
    slug VARCHAR(120) UNIQUE NOT NULL,
    description TEXT,
    icon_class VARCHAR(100) DEFAULT NULL,
    parent_category_id INT DEFAULT NULL REFERENCES categories(id) ON DELETE SET NULL,
    display_order INT DEFAULT 0,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NULL,
    deactivated_at TIMESTAMP WITH TIME ZONE DEFAULT NULL
);

CREATE INDEX idx_categories_slug ON categories(slug);
CREATE INDEX idx_categories_parent ON categories(parent_category_id);

INSERT INTO categories (name, slug, description, icon_class, display_order) VALUES
('Doutrina', 'doutrina', 'Ensinamentos da fé católica, dogmas e teologia', 'fas fa-book-bible', 1),
('Santos e Beatos', 'santos-e-beatos', 'Biografias e hagiografias dos santos da Igreja', 'fas fa-cross', 2),
('Liturgia', 'liturgia', 'Ritos, sacramentos e celebrações litúrgicas', 'fas fa-church', 3),
('História da Igreja', 'historia-da-igreja', 'Eventos históricos e períodos da história eclesiástica', 'fas fa-landmark', 4),
('Papado', 'papado', 'Papas, conclaves e a Sé Apostólica', 'fas fa-crown', 5),
('Ordens Religiosas', 'ordens-religiosas', 'Congregações, ordens monásticas e institutos religiosos', 'fas fa-hands-praying', 6),
('Sacramentos', 'sacramentos', 'Os sete sacramentos e sua teologia', 'fas fa-dove', 7),
('Direito Canônico', 'direito-canonico', 'Legislação eclesiástica e normas canônicas', 'fas fa-scale-balanced', 8),
('Filosofia Cristã', 'filosofia-crista', 'Pensamento filosófico e teológico cristão', 'fas fa-lightbulb', 9),
('Arte Sacra', 'arte-sacra', 'Iconografia, arquitetura e arte religiosa', 'fas fa-palette', 10),
('Mariologia', 'mariologia', 'Estudo e devoção à Virgem Maria', 'fas fa-star', 11),
('Concílios', 'concilios', 'Concílios ecumênicos e regionais da Igreja', 'fas fa-users', 12);

-- ============================
-- Tabela de Status de Revisão
-- ============================
CREATE TABLE review_status (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) UNIQUE NOT NULL,
    description TEXT,
    color_hex VARCHAR(7) DEFAULT '#6c757d'
);

INSERT INTO review_status (name, description, color_hex) VALUES
('Rascunho', 'Artigo em elaboração pelo autor', '#6c757d'),
('Pendente', 'Aguardando revisão de um moderador', '#ffc107'),
('Em Revisão', 'Sendo revisado por um moderador', '#17a2b8'),
('Aprovado', 'Aprovado e publicado', '#28a745'),
('Rejeitado', 'Rejeitado pelo revisor, necessita correções', '#dc3545'),
('Revisão Solicitada', 'Autor solicitou nova revisão após correções', '#fd7e14');

-- ============================
-- Tabela de Artigos
-- ============================
CREATE TABLE articles (
    id SERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    slug VARCHAR(300) UNIQUE NOT NULL,
    summary TEXT DEFAULT NULL,
    category_id INT REFERENCES categories(id) ON DELETE SET NULL,
    current_version_id INT DEFAULT NULL,
    created_by INT NOT NULL REFERENCES users(id) ON DELETE SET NULL,
    featured_image_url VARCHAR(500) DEFAULT NULL,
    is_featured BOOLEAN DEFAULT FALSE,
    is_active BOOLEAN DEFAULT TRUE,
    views INT DEFAULT 0,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NULL,
    deactivated_at TIMESTAMP WITH TIME ZONE DEFAULT NULL
);

CREATE INDEX idx_articles_slug ON articles(slug);
CREATE INDEX idx_articles_category ON articles(category_id);
CREATE INDEX idx_articles_created_by ON articles(created_by);
CREATE INDEX idx_articles_featured ON articles(is_featured) WHERE is_featured = TRUE;
CREATE INDEX idx_articles_active ON articles(is_active) WHERE is_active = TRUE;

-- Índice GIN para busca textual (Full Text Search)
CREATE INDEX idx_articles_title_trgm ON articles USING gin (title gin_trgm_ops);

-- ============================
-- Tabela de Versões dos Artigos
-- ============================
CREATE TABLE article_versions (
    id SERIAL PRIMARY KEY,
    article_id INT NOT NULL REFERENCES articles(id) ON DELETE CASCADE,
    version_number INT NOT NULL DEFAULT 1,
    content TEXT NOT NULL,
    content_html TEXT NOT NULL,
    edit_summary VARCHAR(500) DEFAULT NULL,
    created_by INT NOT NULL REFERENCES users(id) ON DELETE SET NULL,
    review_status_id INT REFERENCES review_status(id) ON DELETE SET NULL,
    reviewed_by INT DEFAULT NULL REFERENCES users(id) ON DELETE SET NULL,
    reviewed_at TIMESTAMP WITH TIME ZONE DEFAULT NULL,
    notes TEXT,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deactivated_at TIMESTAMP WITH TIME ZONE DEFAULT NULL,
    UNIQUE(article_id, version_number)
);

CREATE INDEX idx_article_versions_article ON article_versions(article_id);
CREATE INDEX idx_article_versions_status ON article_versions(review_status_id);

-- FK de versão atual do artigo
ALTER TABLE articles ADD CONSTRAINT fk_articles_current_version 
FOREIGN KEY (current_version_id) REFERENCES article_versions(id) ON DELETE SET NULL;

-- ============================
-- Tabela de Histórico de Revisão
-- ============================
CREATE TABLE article_review_history (
    id SERIAL PRIMARY KEY,
    article_version_id INT NOT NULL REFERENCES article_versions(id) ON DELETE CASCADE,
    reviewer_id INT NOT NULL REFERENCES users(id) ON DELETE SET NULL,
    previous_status_id INT REFERENCES review_status(id),
    new_status_id INT NOT NULL REFERENCES review_status(id),
    notes TEXT,
    reviewed_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_review_history_version ON article_review_history(article_version_id);

-- ============================
-- Tabela de Tags dos Artigos
-- ============================
CREATE TABLE article_tags (
    id SERIAL PRIMARY KEY,
    article_id INT NOT NULL REFERENCES articles(id) ON DELETE CASCADE,
    tag VARCHAR(50) NOT NULL,
    UNIQUE(article_id, tag)
);

CREATE INDEX idx_article_tags_article ON article_tags(article_id);
CREATE INDEX idx_article_tags_tag ON article_tags(tag);

-- ============================
-- Tabela de Referências Bíblicas
-- ============================
CREATE TABLE article_references (
    id SERIAL PRIMARY KEY,
    article_id INT NOT NULL REFERENCES articles(id) ON DELETE CASCADE,
    reference_type VARCHAR(50) NOT NULL DEFAULT 'bibliografica',
    reference_text TEXT NOT NULL,
    reference_url VARCHAR(500) DEFAULT NULL,
    display_order INT DEFAULT 0,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_article_references_article ON article_references(article_id);

-- ============================
-- Tabela de Artigos Relacionados
-- ============================
CREATE TABLE related_articles (
    id SERIAL PRIMARY KEY,
    article_id INT NOT NULL REFERENCES articles(id) ON DELETE CASCADE,
    related_article_id INT NOT NULL REFERENCES articles(id) ON DELETE CASCADE,
    relation_type VARCHAR(50) DEFAULT 'ver_tambem',
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(article_id, related_article_id),
    CHECK (article_id != related_article_id)
);

-- ============================
-- Tabela de Comentários
-- ============================
CREATE TABLE comments (
    id SERIAL PRIMARY KEY,
    article_id INT NOT NULL REFERENCES articles(id) ON DELETE CASCADE,
    user_id INT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    parent_comment_id INT DEFAULT NULL REFERENCES comments(id) ON DELETE CASCADE,
    content TEXT NOT NULL,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NULL,
    deactivated_at TIMESTAMP WITH TIME ZONE DEFAULT NULL
);

CREATE INDEX idx_comments_article ON comments(article_id);
CREATE INDEX idx_comments_user ON comments(user_id);
CREATE INDEX idx_comments_parent ON comments(parent_comment_id);

-- ============================
-- Tabela de Reações nos Comentários
-- ============================
CREATE TABLE comment_reactions (
    id SERIAL PRIMARY KEY,
    comment_id INT NOT NULL REFERENCES comments(id) ON DELETE CASCADE,
    user_id INT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    reaction_type VARCHAR(10) NOT NULL CHECK (reaction_type IN ('like', 'dislike')),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(comment_id, user_id)
);

CREATE INDEX idx_comment_reactions_comment ON comment_reactions(comment_id);

-- ============================
-- Tabela de Favoritos do Usuário
-- ============================
CREATE TABLE user_favorites (
    id SERIAL PRIMARY KEY,
    user_id INT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    article_id INT NOT NULL REFERENCES articles(id) ON DELETE CASCADE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(user_id, article_id)
);

-- ============================
-- Tabela de Log de Atividades
-- ============================
CREATE TABLE activity_log (
    id SERIAL PRIMARY KEY,
    user_id INT REFERENCES users(id) ON DELETE SET NULL,
    action VARCHAR(100) NOT NULL,
    entity_type VARCHAR(50) NOT NULL,
    entity_id INT NOT NULL,
    details JSONB DEFAULT NULL,
    ip_address INET DEFAULT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_activity_log_user ON activity_log(user_id);
CREATE INDEX idx_activity_log_entity ON activity_log(entity_type, entity_id);
CREATE INDEX idx_activity_log_created ON activity_log(created_at DESC);

-- ============================
-- View: Artigos com informações completas
-- ============================
CREATE OR REPLACE VIEW vw_articles_full AS
SELECT 
    a.id,
    a.title,
    a.slug,
    a.summary,
    a.featured_image_url,
    a.is_featured,
    a.views,
    a.created_at,
    a.updated_at,
    c.name AS category_name,
    c.slug AS category_slug,
    c.icon_class AS category_icon,
    u.display_name AS author_name,
    u.username AS author_username,
    av.content_html,
    av.version_number,
    rs.name AS review_status_name,
    rs.color_hex AS review_status_color
FROM articles a
LEFT JOIN categories c ON a.category_id = c.id
LEFT JOIN users u ON a.created_by = u.id
LEFT JOIN article_versions av ON a.current_version_id = av.id
LEFT JOIN review_status rs ON av.review_status_id = rs.id
WHERE a.is_active = TRUE;

-- ============================
-- View: Estatísticas do portal
-- ============================
CREATE OR REPLACE VIEW vw_portal_stats AS
SELECT 
    (SELECT COUNT(*) FROM articles WHERE is_active = TRUE) AS total_articles,
    (SELECT COUNT(*) FROM users WHERE is_active = TRUE) AS total_users,
    (SELECT COUNT(*) FROM categories WHERE is_active = TRUE) AS total_categories,
    (SELECT SUM(views) FROM articles WHERE is_active = TRUE) AS total_views,
    (SELECT COUNT(*) FROM article_versions av 
     JOIN review_status rs ON av.review_status_id = rs.id 
     WHERE rs.name = 'Pendente') AS pending_reviews;
