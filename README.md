<img align="center" alt="Samuel-Icon" height="175" width="175" src="https://media.discordapp.net/attachments/746489793646428160/1333501523249991792/logo_for_a_Catholic_church-related_wiki_company_with_a_book_and_the_cross_of_Christ.jpg?ex=67991f92&is=6797ce12&hm=c46ce817fd1354cc5d3c0e6348a91d82e5cb4199229906de892456348555bf82&=&format=webp" />

# OrbisCatholicusWiki 

Este é um sistema estilo Wiki, projetado para permitir que usuários criem, editem e validem informações de forma colaborativa.  
O projeto é construído com foco em simplicidade, escalabilidade e boas práticas de desenvolvimento.

---

## **Tecnologias Utilizadas** ⚙️ <div style="display: inline_block"><img align="center" alt="PHP" height="30" width="40" src="https://raw.githubusercontent.com/devicons/devicon/6910f0503efdd315c8f9b858234310c06e04d9c0/icons/php/php-original.svg" /> <img align="center" alt="LaraveL" height="30" width="40" src="https://raw.githubusercontent.com/devicons/devicon/6910f0503efdd315c8f9b858234310c06e04d9c0/icons/laravel/laravel-original.svg" /> <img align="center" alt="InertiaJs" height="35" width="35" src="https://avatars.githubusercontent.com/u/47703742?s=200&v=4" /> <img align="center" alt="VueJs" height="30" width="40" src="https://raw.githubusercontent.com/devicons/devicon/6910f0503efdd315c8f9b858234310c06e04d9c0/icons/vuejs/vuejs-original.svg" />  <img align="center" alt="PostegreSql" height="30" width="40" src="https://raw.githubusercontent.com/devicons/devicon/6910f0503efdd315c8f9b858234310c06e04d9c0/icons/postgresql/postgresql-original.svg" />

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

## **Evoluções Futuras** 🚧  
- Adicionar suporte à arquitetura DDD.  
- Implementar autenticação OAuth para integração com provedores externos.  
- Melhorar o sistema de busca utilizando ferramentas como Elasticsearch.  
- Adicionar testes unitários e de integração para garantir maior estabilidade.  
