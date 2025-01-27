# OrbisCatholicusWiki

Este é um sistema estilo Wiki, projetado para permitir que usuários criem, editem e validem informações de forma colaborativa.  
O projeto é construído com foco em simplicidade, escalabilidade e boas práticas de desenvolvimento.

---

## **Tecnologias Utilizadas** ⚙️  

### **Backend**  
- **PHP** com **Laravel**: Framework PHP robusto, utilizado para gerenciar as operações do lado do servidor e a lógica de negócios.  
- **Inertia.js**: Faz a ponte entre o backend Laravel e o frontend Vue.js, permitindo a criação de Single Page Applications (SPAs) sem a necessidade de APIs REST/GraphQL.  

### **Frontend**  
- **Vue.js**: Framework JavaScript progressivo para criar interfaces de usuário dinâmicas e reativas.

### **Banco de Dados**  
- **PostgreSQL**: Banco de dados relacional poderoso, utilizado para armazenar todas as informações do sistema.  

---

## **Arquitetura do Sistema** 🏗️  

### **Versão Inicial**  
- **Arquitetura MVC**: Implementação seguindo o padrão Model-View-Controller para separação de responsabilidades.  

### **Futuro Planejado**  
- **Arquitetura DDD (Domain-Driven Design)**: Evolução para um design focado em domínio, organizando o sistema em torno de seus contextos de negócios principais.  

---

## **Funcionalidades** 🌟  
- **Gestão de Conteúdo**:
  - Criação, edição e exclusão de páginas Wiki.  
  - Histórico de revisões para rastrear mudanças nas páginas.  

- **Sistema de Usuários**:
  - Usuários podem ser criadores de conteúdo ou validadores (moderadores).  
  - Controle de permissões baseado em papéis.  

- **Validação de Conteúdo**:
  - Paginas pendentes aguardam validação de usuários moderadores.  

- **Buscas Eficientes**:
  - Pesquisa otimizada para localizar informações rapidamente.  

---

## **Como Executar o Projeto Localmente** 🚀  

1. **Clone o Repositório**:
   ```bash
   git clone https://github.com/seu-usuario/wiki-project.git
   cd wiki-project
   ```

2. **Instale as Dependências do Laravel**:
   ```bash
   composer install
   ```

3. **Instale as Dependências do Frontend**:
   ```bash
   npm install
   ```

4. **Configure o Ambiente**:
   - Copie o arquivo `.env.example` para `.env`:
     ```bash
     cp .env.example .env
     ```
   - Configure as variáveis do banco de dados no arquivo `.env`:
     ```env
     DB_CONNECTION=pgsql
     DB_HOST=127.0.0.1
     DB_PORT=5432
     DB_DATABASE=wiki_project
     DB_USERNAME=seu_usuario
     DB_PASSWORD=sua_senha
     ```

5. **Gere a Chave da Aplicação**:
   ```bash
   php artisan key:generate
   ```

6. **Execute as Migrações**:
   ```bash
   php artisan migrate
   ```

7. **Compile os Assets do Frontend**:
   ```bash
   npm run dev
   ```

8. **Inicie o Servidor de Desenvolvimento**:
   ```bash
   php artisan serve
   ```

9. Acesse a aplicação no navegador:  
   [http://localhost:8000](http://localhost:8000)

---

## **Evoluções Futuras** 🚧  
- Adicionar suporte à arquitetura DDD.  
- Implementar autenticação OAuth para integração com provedores externos.  
- Melhorar o sistema de busca utilizando ferramentas como Elasticsearch.  
- Adicionar testes unitários e de integração para garantir maior estabilidade.  
