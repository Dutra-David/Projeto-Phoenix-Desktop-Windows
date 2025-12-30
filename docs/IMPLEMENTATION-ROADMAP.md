# Implementation Roadmap - Project Phoenix Desktop

## Phase 1: SQLite Adapter (Critical)

### SQLiteKnowledgeStore Implementation
Create concrete implementation of `IKnowledgeStore` using SQLite.

**File**: `src/Phoenix.Core/Knowledge/Adapters/SqliteKnowledgeStore.cs`

**Key Methods**:
```csharp
public class SqliteKnowledgeStore : IKnowledgeStore
{
    private readonly string _connectionString;
    private readonly ILogger<SqliteKnowledgeStore> _logger;
    
    // CRUD Operations
    public async Task<KnowledgeItem> AddAsync(KnowledgeItem item)
    {
        using var connection = new SqliteConnection(_connectionString);
        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO KnowledgeItems 
            (Id, Domain, Title, Content, Tags, Priority, IsActive, CreatedAt, UpdatedAt, Relevance)
            VALUES (@id, @domain, @title, @content, @tags, @priority, @active, @created, @updated, @relevance)";
        // Parameter binding...
        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
        return item;
    }
    
    public async Task<bool> InitializeDatabaseAsync()
    {
        // Read DATABASE-SCHEMA.sql
        // Execute all statements
        // Verify tables exist
        // Insert seed data
    }
}
```

**Dependencies**:
- `System.Data.Sqlite` OR `Microsoft.Data.Sqlite`
- Dapper for query mapping (optional but recommended)

**Performance Targets**:
- Single item insert: <5ms
- Search 50 items: <20ms
- Full scan 1000 items: <100ms

**Testing**:
- Unit tests with in-memory SQLite
- 100% coverage of CRUD operations
- Concurrency tests (10 parallel queries)

---

## Phase 2: Vector Search / Embeddings (High Value)

### Semantic Search with Embeddings

**File**: `src/Phoenix.Core/Knowledge/Services/EmbeddingService.cs`

**Implementation**:
```csharp
public interface IEmbeddingService
{
    Task<float[]> GenerateEmbeddingAsync(string text);
    Task<List<KnowledgeItem>> SearchSemanticAsync(string query, int topK = 5);
    Task<double> CalculateSimilarityAsync(float[] embedding1, float[] embedding2);
}

public class GeminiEmbeddingService : IEmbeddingService
{
    private readonly string _apiKey;
    
    public async Task<float[]> GenerateEmbeddingAsync(string text)
    {
        // Use Gemini Embedding API (text-embedding-004)
        // Return 768-dimensional vector
        var request = new EmbedRequest { Text = text };
        var response = await _client.EmbedAsync(request);
        return response.Embedding.Values.ToArray();
    }
    
    public async Task<List<KnowledgeItem>> SearchSemanticAsync(string query, int topK = 5)
    {
        var queryEmbedding = await GenerateEmbeddingAsync(query);
        // Calculate cosine similarity with all stored embeddings
        // Return top K results ordered by similarity score
        return results;
    }
}
```

**Database Changes**:
- Add `Embeddings` table: (Id, KnowledgeItemId, Vector, EmbeddingModel, CreatedAt)
- Add `EmbeddingModel` column to KnowledgeItems
- Index on KnowledgeItemId

**Schema**:
```sql
CREATE TABLE Embeddings (
    Id TEXT PRIMARY KEY,
    KnowledgeItemId TEXT NOT NULL,
    Vector BLOB NOT NULL,  -- 768 floats serialized
    EmbeddingModel TEXT DEFAULT 'gemini-text-embedding-004',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (KnowledgeItemId) REFERENCES KnowledgeItems(Id)
);
```

**Features**:
- On-demand embedding generation
- Caching of frequently used embeddings
- Batch embedding (100 items at once)
- Similarity scoring (0.0 - 1.0)

**Cost**: ~$0.02 per 1000 embeddings (Gemini API)

---

## Phase 3: CosmosDB Sync (Cloud Integration)

### Cloud Persistence with Azure CosmosDB

**File**: `src/Phoenix.Core/Knowledge/Adapters/CosmosDbKnowledgeStore.cs`

**Architecture**:
```
Local SQLite <-> Sync Service <-> Azure CosmosDB
                  (Bidirectional)
```

**Implementation**:
```csharp
public interface ICloudSyncService
{
    Task<SyncResult> SyncToCloudAsync(List<KnowledgeItem> items);
    Task<List<KnowledgeItem>> SyncFromCloudAsync(string domain);
    Task<SyncStatus> GetSyncStatusAsync();
}

public class CosmosDbSyncService : ICloudSyncService
{
    private readonly CosmosClient _client;
    private readonly Container _container;
    
    public async Task<SyncResult> SyncToCloudAsync(List<KnowledgeItem> items)
    {
        var batch = _container.CreateTransactionalBatch(new PartitionKey(items[0].Domain));
        foreach (var item in items)
        {
            batch.UpsertItemAsync(item, new ItemRequestOptions { IfMatchEtag = item.ETag });
        }
        var result = await batch.ExecuteAsync();
        return new SyncResult { ItemsSynced = result.Count, Success = true };
    }
    
    public async Task<SyncStatus> GetSyncStatusAsync()
    {
        // Return last sync time, pending changes, conflicts
    }
}
```

**CosmosDB Schema**:
```json
{
  "id": "PSI_001",
  "domain": "Psychology",
  "title": "Cognitive Theory",
  "content": "...",
  "tags": ["cognitivismo", "beckaellis"],
  "priority": 1,
  "isActive": true,
  "createdAt": "2025-12-30T14:00:00Z",
  "updatedAt": "2025-12-30T14:00:00Z",
  "viewCount": 5,
  "relevance": 0.85,
  "_etag": "..."
}
```

**Sync Strategy**:
1. **Periodic Sync**: Every 5 minutes
2. **Change Tracking**: Timestamp-based detection
3. **Conflict Resolution**: Last-write-wins (LWW)
4. **Batch Size**: 25 items per batch
5. **Retry Logic**: Exponential backoff (max 3 retries)

**Configuration**:
```json
{
  "CosmosDb": {
    "EndpointUri": "https://{account}.documents.azure.com:443/",
    "PrimaryKey": "...",
    "DatabaseId": "PhoenixKb",
    "ContainerId": "KnowledgeItems",
    "PartitionKey": "/domain"
  }
}
```

**Cost**: ~$25/month for standard tier (1 RU/s = $0.08/day)

---

## Phase 4: Full-Text Search with FTS5 (Performance)

### Advanced Full-Text Search

**File**: `docs/DATABASE-SCHEMA-FTS5.sql`

**Implementation**:
```sql
CREATE VIRTUAL TABLE KnowledgeItems_FTS USING fts5(
    title,
    content,
    tags,
    content = 'KnowledgeItems',
    content_rowid = 'Id'
);

-- Triggers to keep FTS in sync
CREATE TRIGGER KnowledgeItems_ai AFTER INSERT ON KnowledgeItems BEGIN
  INSERT INTO KnowledgeItems_FTS(rowid, title, content, tags)
  VALUES (new.Id, new.Title, new.Content, new.Tags);
END;

CREATE TRIGGER KnowledgeItems_ad AFTER DELETE ON KnowledgeItems BEGIN
  DELETE FROM KnowledgeItems_FTS WHERE rowid = old.Id;
END;
```

**Search Methods**:
```csharp
public async Task<List<KnowledgeItem>> SearchWithFts5Async(string query)
{
    // AND operator
    var cmd = new SqliteCommand(@"
        SELECT k.* FROM KnowledgeItems k
        WHERE k.Id IN (
            SELECT rowid FROM KnowledgeItems_FTS
            WHERE KnowledgeItems_FTS MATCH @query
        )
        ORDER BY rank
        LIMIT 50
    ", connection);
    
    cmd.Parameters.AddWithValue("@query", query);
    // Return results
}
```

**Query Syntax**:
- `"async await"` - Both words
- `"async" OR "await"` - Either word  
- `"async" AND NOT "promise"` - Complex
- `"async*"` - Prefix search

**Performance**:
- Simple query: <5ms
- Complex query: <50ms
- Phrase search: <20ms

---

## Phase 5: Versioning & Audit Trail (Compliance)

### Knowledge Item Versioning

**File**: `src/Phoenix.Core/Knowledge/Models/KnowledgeVersion.cs`

**Schema**:
```sql
CREATE TABLE KnowledgeVersions (
    Id TEXT PRIMARY KEY,
    KnowledgeItemId TEXT NOT NULL,
    Version INTEGER NOT NULL,
    Title TEXT NOT NULL,
    Content TEXT NOT NULL,
    ChangedBy TEXT,
    ChangeType TEXT,  -- Create, Update, Delete
    Reason TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (KnowledgeItemId) REFERENCES KnowledgeItems(Id),
    UNIQUE(KnowledgeItemId, Version)
);

CREATE TABLE AuditLog (
    Id TEXT PRIMARY KEY,
    EntityId TEXT NOT NULL,
    EntityType TEXT,  -- KnowledgeItem, User, etc
    Action TEXT,  -- Create, Update, Delete, View
    OldValue TEXT,
    NewValue TEXT,
    UserId TEXT,
    IpAddress TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

**Implementation**:
```csharp
public interface IVersioningService
{
    Task<KnowledgeVersion> CreateVersionAsync(KnowledgeItem item, string changeReason);
    Task<KnowledgeVersion> GetVersionAsync(string itemId, int version);
    Task<List<KnowledgeVersion>> GetHistoryAsync(string itemId);
    Task<KnowledgeItem> RestoreVersionAsync(string itemId, int version);
}

public class VersioningService : IVersioningService
{
    public async Task<KnowledgeVersion> CreateVersionAsync(KnowledgeItem item, string changeReason)
    {
        var currentVersion = await GetLatestVersionAsync(item.Id);
        var newVersion = new KnowledgeVersion
        {
            Id = Guid.NewGuid().ToString(),
            KnowledgeItemId = item.Id,
            Version = (currentVersion?.Version ?? 0) + 1,
            Title = item.Title,
            Content = item.Content,
            Reason = changeReason,
            ChangeType = "Update",
            ChangedBy = _currentUser.Id
        };
        
        await _store.SaveVersionAsync(newVersion);
        return newVersion;
    }
    
    public async Task<KnowledgeItem> RestoreVersionAsync(string itemId, int version)
    {
        var historyVersion = await GetVersionAsync(itemId, version);
        var item = new KnowledgeItem
        {
            Id = historyVersion.KnowledgeItemId,
            Title = historyVersion.Title,
            Content = historyVersion.Content,
            // ...
        };
        await _store.UpdateAsync(item);
        await CreateVersionAsync(item, $"Restored from version {version}");
        return item;
    }
}
```

**Audit Capabilities**:
- Who changed what and when
- Full history of each item
- Rollback to any version
- Change reason tracking
- User attribution

---

## Implementation Priority

| Phase | Priority | Effort | Value | Timeline |
|-------|----------|--------|-------|----------|
| 1: SQLite | 🔴 Critical | 2 days | 10/10 | Week 1 |
| 2: Embeddings | 🟠 High | 3 days | 9/10 | Week 2-3 |
| 3: CosmosDB | 🟠 High | 2 days | 8/10 | Week 3-4 |
| 4: FTS5 | 🟡 Medium | 1 day | 7/10 | Week 4 |
| 5: Versioning | 🟡 Medium | 2 days | 6/10 | Week 5 |

**Total Effort**: ~2 weeks

---

## Success Criteria

- ✅ All methods in IKnowledgeStore implemented
- ✅ 100+ items stored locally
- ✅ Semantic search <500ms
- ✅ Cloud sync <2 seconds
- ✅ Full-text search <50ms
- ✅ Version history recoverable
- ✅ 90%+ test coverage
- ✅ Performance benchmarks met

