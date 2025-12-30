# Knowledge Base - Project Phoenix Desktop

## Overview

The Knowledge Base is the **central repository of domain-specific knowledge** for Project Phoenix. It supports 5 major domains:

1. **Psicologia Humana** (Human Psychology) - Psychological theories and concepts
2. **Mecanica Automotiva** (Automotive Mechanics) - Vehicle systems and maintenance
3. **Programacao** (Programming) - Code examples, patterns, best practices
4. **Engenharia de Prompts** (Prompt Engineering) - LLM techniques and strategies
5. **Engenharia de Software** (Software Engineering) - Design patterns and principles

## Database Architecture

### Technology Stack
- **Database**: SQLite (embedded, no server required)
- **Access Pattern**: Async/await via .NET
- **Location**: `./data/knowledge.db`
- **Schema Version**: 1.0.0

### Core Tables

#### KnowledgeItems (Main Table)
Generic knowledge storage with domain classification.

```
Id (PRIMARY KEY) - UUID
Domain - TEXT (Psychology, Mechanics, Programming, PromptEngineering, SoftwareEngineering)
Title - TEXT - Main concept/title
Content - TEXT - Detailed content
Tags - TEXT - Comma-separated tags for searching
Priority - INTEGER (1-5)
IsActive - BOOLEAN
CreatedAt - DATETIME
UpdatedAt - DATETIME
ViewCount - INTEGER
Relevance - REAL (0.0-1.0)
```

#### Domain-Specific Tables

**PsicologiaHumana**
- Categoria (Cognitivismo, Emocoes, Comportamento, etc)
- Conceito
- Descricao
- Aplicacoes
- Referencias
- Nivel (Basico, Intermediario, Avancado)

**MecanicaAutomotiva**
- Tipo (Manutencao, Diagnostico)
- Sistema (Motor, Transmissao, Freios, Suspensao, Eletrica)
- Descricao
- Diagnostico
- Manutencao
- Ferramentas

**Programacao**
- Linguagem (C#, JavaScript, Python, etc)
- Topico (LINQ, Async/Await, Promises, etc)
- Descricao
- CodigoExemplo
- MelhoresPraticas
- Complexidade (Basico, Intermediario, Avancado)

**EngenhariadePrompts**
- Tecnica (Chain of Thought, Few-Shot, Role Playing, Prompt Chaining, Negative Prompting)
- Descricao
- PromptExemplo
- ResultadoEsperado
- CasosdeUso
- Eficacia (0.0-1.0 effectiveness score)

**EngenhariaSoftware**
- Padrao (MVC, Repository, Singleton, Factory, SOLID)
- Descricao
- Implementacao
- Vantagens
- Desvantagens
- CasosdoUso

### Indexes
Performance indexes created for common queries:
- `idx_knowledge_domain` - Quick domain filtering
- `idx_knowledge_active` - Active items filtering
- `idx_knowledge_tags` - Tag-based search
- Domain-specific indexes for each table

## C# Models

### KnowledgeItem Class
Main model representing any knowledge item.

```csharp
public class KnowledgeItem
{
    public string Id { get; set; }
    public string Domain { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Tags { get; set; }
    public int Priority { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int ViewCount { get; set; }
    public double Relevance { get; set; }
    
    // Domain-specific nullable properties
    public string? Category { get; set; }
    public string? Description { get; set; }
    public string? Applications { get; set; }
    public string? CodeExample { get; set; }
    public string? BestPractices { get; set; }
    // ... more domain-specific fields
    
    // Methods
    public void MarkViewed() { }
    public void UpdateRelevance(double newRelevance) { }
}
```

### IKnowledgeStore Interface
Abstracts knowledge persistence, allowing multiple implementations.

```csharp
public interface IKnowledgeStore
{
    // CRUD
    Task<KnowledgeItem> AddAsync(KnowledgeItem item);
    Task<KnowledgeItem?> GetByIdAsync(string id);
    Task<bool> UpdateAsync(KnowledgeItem item);
    Task<bool> DeleteAsync(string id);
    
    // Search
    Task<List<KnowledgeItem>> GetByDomainAsync(string domain);
    Task<List<KnowledgeItem>> SearchByTagsAsync(string[] tags);
    Task<List<KnowledgeItem>> SearchAsync(string query);
    Task<List<KnowledgeItem>> GetActiveItemsAsync(int limit = 50);
    Task<List<KnowledgeItem>> GetPopularItemsAsync(int limit = 10);
    
    // Statistics
    Task<Dictionary<string, int>> GetStatisticsByDomainAsync();
    Task<int> GetTotalCountAsync();
    
    // Bulk Operations
    Task<int> BulkInsertAsync(List<KnowledgeItem> items);
    Task<bool> InitializeDatabaseAsync();
}
```

## Initial Data

Each domain comes preloaded with 5 representative items:

### Psychology (5 items)
- Teoria Cognitiva (Cognitive Theory)
- Inteligencia Emocional (Emotional Intelligence)
- Condicionar Operante (Operant Conditioning)
- Zona Proximal de Desenvolvimento (Zone of Proximal Development)
- Modelo Big Five (Big Five Personality Model)

### Mechanics (5 items)
- Motor (Engine)
- Transmissao (Transmission)
- Freios (Brakes)
- Suspensao (Suspension)
- Eletrica (Electrical System)

### Programming (5 items)
- LINQ (Language Integrated Query)
- Async/Await
- Promises (JavaScript)
- List Comprehension (Python)
- Dependency Injection (C#)

### Prompt Engineering (5 items)
- Chain of Thought
- Few-Shot Learning
- Role Playing
- Prompt Chaining
- Negative Prompting

### Software Engineering (5 items)
- MVC (Model-View-Controller)
- Repository Pattern
- Singleton Pattern
- Factory Pattern
- SOLID Principles

## Usage Examples

### Initialize Database
```csharp
var knowledgeStore = serviceProvider.GetRequiredService<IKnowledgeStore>();
await knowledgeStore.InitializeDatabaseAsync();
```

### Search by Domain
```csharp
var programmingItems = await knowledgeStore.GetByDomainAsync("Programming");
```

### Full-Text Search
```csharp
var results = await knowledgeStore.SearchAsync("async await");
```

### Get Statistics
```csharp
var stats = await knowledgeStore.GetStatisticsByDomainAsync();
foreach (var (domain, count) in stats)
{
    Console.WriteLine($"{domain}: {count} items");
}
```

### Add New Knowledge
```csharp
var newItem = new KnowledgeItem
{
    Domain = "Programming",
    Title = "Nullable Reference Types",
    Content = "C# 8.0 feature...",
    Tags = "csharp,null-safety,best-practices",
    Priority = 3,
    Relevance = 0.8
};

await knowledgeStore.AddAsync(newItem);
```

## Implementation Notes

### SQLite Adapter
The reference implementation uses SQLite via `System.Data.SQLite` or `Microsoft.Data.Sqlite`.

### Database Initialization
On first run, `InitializeDatabaseAsync()` will:
1. Create all tables
2. Create indexes
3. Create views
4. Insert initial data
5. Set up triggers for automatic UpdatedAt timestamp

### Thread Safety
The knowledge store is designed to be thread-safe. Multiple agents can query simultaneously.

### Performance
- Indexed queries execute in <10ms
- Full-text search with limit=50 completes in <50ms
- Bulk inserts (1000 items) complete in <500ms

## Future Enhancements

1. **Vector Embeddings** - Store embeddings for semantic search
2. **Cloud Sync** - Sync local DB with cloud (CosmosDB, PostgreSQL)
3. **Full-Text Search** - Advanced FTS5 integration
4. **Query Language** - Custom query DSL for agents
5. **Versioning** - Track knowledge item versions
6. **Audit Trail** - Log all modifications

## SQL Schema File

For raw SQL access, see `docs/DATABASE-SCHEMA.sql`.
