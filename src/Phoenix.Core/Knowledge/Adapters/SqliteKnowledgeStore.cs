using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using Phoenix.Core.Knowledge.Models;
using Phoenix.Core.Knowledge.Interfaces;
using Microsoft.Extensions.Logging;

namespace Phoenix.Core.Knowledge.Adapters
{
    /// <summary>
    /// SQLite implementation of IKnowledgeStore interface.
    /// Provides persistent storage and retrieval of knowledge items with advanced search capabilities.
    /// </summary>
    public class SqliteKnowledgeStore : IKnowledgeStore
    {
        private readonly SQLiteAsyncConnection _connection;
        private readonly ILogger<SqliteKnowledgeStore> _logger;
        private readonly string _databasePath;
        private const int MaxConnectionPoolSize = 10;
        private const int QueryTimeoutMs = 5000;

        public SqliteKnowledgeStore(string databasePath, ILogger<SqliteKnowledgeStore> logger)
        {
            _databasePath = databasePath ?? throw new ArgumentNullException(nameof(databasePath));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connection = new SQLiteAsyncConnection(_databasePath);
        }

        /// <summary>
        /// Initialize the database schema and create necessary tables.
        /// </summary>
        public async Task InitializeAsync()
        {
            try
            {
                _logger.LogInformation("Initializing SQLite knowledge store database");
                
                await _connection.CreateTableAsync<KnowledgeItem>();
                await _connection.ExecuteAsync(
                    "CREATE INDEX IF NOT EXISTS idx_category ON KnowledgeItem(Category)"
                );
                await _connection.ExecuteAsync(
                    "CREATE INDEX IF NOT EXISTS idx_domain ON KnowledgeItem(Domain)"
                );
                await _connection.ExecuteAsync(
                    "CREATE INDEX IF NOT EXISTS idx_created ON KnowledgeItem(CreatedAt)"
                );
                
                _logger.LogInformation("SQLite knowledge store initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing SQLite knowledge store");
                throw;
            }
        }

        /// <summary>
        /// Add a new knowledge item to the store.
        /// </summary>
        public async Task<KnowledgeItem> AddAsync(KnowledgeItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            try
            {
                item.Id = Guid.NewGuid().ToString();
                item.CreatedAt = DateTime.UtcNow;
                item.UpdatedAt = DateTime.UtcNow;
                item.Version = 1;

                await _connection.InsertAsync(item);
                _logger.LogInformation($"Knowledge item added: {item.Id}");
                
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding knowledge item");
                throw;
            }
        }

        /// <summary>
        /// Get a knowledge item by ID.
        /// </summary>
        public async Task<KnowledgeItem> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("ID cannot be null or empty", nameof(id));

            try
            {
                var item = await _connection.FindAsync<KnowledgeItem>(id);
                if (item == null)
                    _logger.LogWarning($"Knowledge item not found: {id}");
                
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving knowledge item: {id}");
                throw;
            }
        }

        /// <summary>
        /// Get all knowledge items.
        /// </summary>
        public async Task<IEnumerable<KnowledgeItem>> GetAllAsync()
        {
            try
            {
                var items = await _connection.Table<KnowledgeItem>().ToListAsync();
                _logger.LogInformation($"Retrieved {items.Count} knowledge items");
                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all knowledge items");
                throw;
            }
        }

        /// <summary>
        /// Get knowledge items by domain.
        /// </summary>
        public async Task<IEnumerable<KnowledgeItem>> GetByDomainAsync(string domain)
        {
            if (string.IsNullOrEmpty(domain))
                throw new ArgumentException("Domain cannot be null or empty", nameof(domain));

            try
            {
                var items = await _connection.Table<KnowledgeItem>()
                    .Where(k => k.Domain == domain)
                    .ToListAsync();
                
                _logger.LogInformation($"Retrieved {items.Count} items for domain: {domain}");
                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving items for domain: {domain}");
                throw;
            }
        }

        /// <summary>
        /// Get knowledge items by category.
        /// </summary>
        public async Task<IEnumerable<KnowledgeItem>> GetByCategoryAsync(string category)
        {
            if (string.IsNullOrEmpty(category))
                throw new ArgumentException("Category cannot be null or empty", nameof(category));

            try
            {
                var items = await _connection.Table<KnowledgeItem>()
                    .Where(k => k.Category == category)
                    .ToListAsync();
                
                _logger.LogInformation($"Retrieved {items.Count} items for category: {category}");
                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving items for category: {category}");
                throw;
            }
        }

        /// <summary>
        /// Search knowledge items by keyword in title, description, and tags.
        /// </summary>
        public async Task<IEnumerable<KnowledgeItem>> SearchAsync(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                throw new ArgumentException("Keyword cannot be null or empty", nameof(keyword));

            try
            {
                var searchTerm = $"%{keyword}%";
                var items = await _connection.QueryAsync<KnowledgeItem>(
                    "SELECT * FROM KnowledgeItem WHERE Title LIKE ? OR Description LIKE ? OR Tags LIKE ?",
                    searchTerm, searchTerm, searchTerm
                );
                
                _logger.LogInformation($"Found {items.Count} items matching keyword: {keyword}");
                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching for keyword: {keyword}");
                throw;
            }
        }

        /// <summary>
        /// Update an existing knowledge item.
        /// </summary>
        public async Task<KnowledgeItem> UpdateAsync(KnowledgeItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (string.IsNullOrEmpty(item.Id))
                throw new ArgumentException("Item ID cannot be null or empty", nameof(item));

            try
            {
                var existing = await _connection.FindAsync<KnowledgeItem>(item.Id);
                if (existing == null)
                    throw new InvalidOperationException($"Knowledge item not found: {item.Id}");

                item.UpdatedAt = DateTime.UtcNow;
                item.Version = existing.Version + 1;

                await _connection.UpdateAsync(item);
                _logger.LogInformation($"Knowledge item updated: {item.Id} (version {item.Version})");
                
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating knowledge item");
                throw;
            }
        }

        /// <summary>
        /// Delete a knowledge item by ID.
        /// </summary>
        public async Task<bool> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("ID cannot be null or empty", nameof(id));

            try
            {
                var result = await _connection.DeleteAsync<KnowledgeItem>(id);
                if (result > 0)
                {
                    _logger.LogInformation($"Knowledge item deleted: {id}");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"Knowledge item not found for deletion: {id}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting knowledge item: {id}");
                throw;
            }
        }

        /// <summary>
        /// Get paginated results with optional filtering by domain.
        /// </summary>
        public async Task<(IEnumerable<KnowledgeItem> Items, int Total)> GetPagedAsync(int pageNumber, int pageSize, string domain = null)
        {
            if (pageNumber < 1 || pageSize < 1)
                throw new ArgumentException("Page number and size must be greater than 0");

            try
            {
                IQueryable<KnowledgeItem> query = _connection.Table<KnowledgeItem>();

                if (!string.IsNullOrEmpty(domain))
                    query = query.Where(k => k.Domain == domain);

                var total = await query.CountAsync();
                var items = await query
                    .OrderByDescending(k => k.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                _logger.LogInformation($"Retrieved page {pageNumber} with {items.Count} items (total: {total})");
                return (items, total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged results");
                throw;
            }
        }

        /// <summary>
        /// Get most recent knowledge items.
        /// </summary>
        public async Task<IEnumerable<KnowledgeItem>> GetRecentAsync(int limit = 10)
        {
            if (limit < 1)
                throw new ArgumentException("Limit must be greater than 0", nameof(limit));

            try
            {
                var items = await _connection.Table<KnowledgeItem>()
                    .OrderByDescending(k => k.CreatedAt)
                    .Take(limit)
                    .ToListAsync();
                
                _logger.LogInformation($"Retrieved {items.Count} recent knowledge items");
                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving recent knowledge items");
                throw;
            }
        }

        /// <summary>
        /// Count total knowledge items.
        /// </summary>
        public async Task<int> GetCountAsync()
        {
            try
            {
                var count = await _connection.Table<KnowledgeItem>().CountAsync();
                _logger.LogInformation($"Total knowledge items count: {count}");
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting knowledge items");
                throw;
            }
        }

        /// <summary>
        /// Dispose resources.
        /// </summary>
        public async Task DisposeAsync()
        {
            try
            {
                if (_connection != null)
                {
                    await _connection.CloseAsync();
                    _logger.LogInformation("SQLite knowledge store connection closed");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disposing SQLite connection");
            }
        }

        /// <summary>
        /// Clear all knowledge items from the database.
        /// </summary>
        public async Task ClearAsync()
        {
            try
            {
                await _connection.DeleteAllAsync<KnowledgeItem>();
                _logger.LogInformation("All knowledge items cleared from database");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing knowledge items");
                throw;
            }
        }
    }
}
