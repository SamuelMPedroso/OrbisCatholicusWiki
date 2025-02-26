-- ============================
-- Tabela de Níveis de Acesso
-- ============================
CREATE TABLE access_levels (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) UNIQUE NOT NULL,
    description TEXT
);

-- ============================
-- Tabela de Usuários
-- ============================
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(100) UNIQUE NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash TEXT NOT NULL,
    access_level_id INT NOT NULL REFERENCES access_levels(id) ON DELETE RESTRICT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ============================
-- Tabela de Categorias
-- ============================
CREATE TABLE categories (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) UNIQUE NOT NULL,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE,
    deactivated_at TIMESTAMP DEFAULT NULL
);

-- ============================
-- Tabela de Artigos
-- ============================
CREATE TABLE articles (
    id SERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    category_id INT REFERENCES categories(id) ON DELETE SET NULL,
    current_version_id INT, -- Atualizado posteriormente
    created_by INT NOT NULL REFERENCES users(id) ON DELETE SET NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE,
    deactivated_at TIMESTAMP DEFAULT NULL,
    views INT DEFAULT 0
);

-- ============================
-- Tabela de Status de Revisão
-- ============================
CREATE TABLE review_status (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) UNIQUE NOT NULL,
    description TEXT
);

-- ============================
-- Tabela de Versões dos Artigos
-- ============================
CREATE TABLE article_versions (
    id SERIAL PRIMARY KEY,
    article_id INT NOT NULL REFERENCES articles(id) ON DELETE CASCADE,
    content TEXT NOT NULL, 
    created_by INT NOT NULL REFERENCES users(id) ON DELETE SET NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    review_status_id INT REFERENCES review_status(id) ON DELETE SET NULL,
    notes TEXT, -- Observações da revisão
    is_active BOOLEAN DEFAULT TRUE,
    deactivated_at TIMESTAMP DEFAULT NULL
);

-- Atualizando a versão atual do artigo (foreign key opcional)
ALTER TABLE articles ADD CONSTRAINT fk_articles_current_version 
FOREIGN KEY (current_version_id) REFERENCES article_versions(id) ON DELETE SET NULL;

-- ============================
-- Tabela de Histórico de Revisão
-- ============================
CREATE TABLE article_review_history (
    id SERIAL PRIMARY KEY,
    article_version_id INT NOT NULL REFERENCES article_versions(id) ON DELETE CASCADE,
    reviewer_id INT NOT NULL REFERENCES users(id) ON DELETE SET NULL,
    decision VARCHAR(50) NOT NULL, -- 'Aprovado', 'Rejeitado', etc.
    reviewed_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    notes TEXT
);

-- ============================
-- Tabela de Tags dos Artigos
-- ============================
CREATE TABLE article_tags (
    id SERIAL PRIMARY KEY,
    article_id INT NOT NULL REFERENCES articles(id) ON DELETE CASCADE,
    tag VARCHAR(50) NOT NULL
);

-- ============================
-- Tabela de Comentários
-- ============================
CREATE TABLE comments (
    id SERIAL PRIMARY KEY,
    article_id INT NOT NULL REFERENCES articles(id) ON DELETE CASCADE,
    user_id INT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    content TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE,
    deactivated_at TIMESTAMP DEFAULT NULL
);

-- ============================
-- Tabela de Reações nos Comentários
-- ============================
CREATE TABLE comment_reactions (
    id SERIAL PRIMARY KEY,
    comment_id INT NOT NULL REFERENCES comments(id) ON DELETE CASCADE,
    user_id INT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    reaction_type VARCHAR(10) CHECK (reaction_type IN ('like', 'dislike')),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
