using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Phoenix.Core.Knowledge.Models;

namespace Phoenix.Core.Knowledge.Adapters
{
    /// <summary>
    /// Interface for FTS5 full-text search using SQLite FTS5 module.
    /// Provides advanced text search with stemming and tokenization.
    /// </summary>
    public interface IFullTextSearchAdapter
    {
        /// <summary>
        /// Initialize FTS5 index.
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// Index item for full-text search.
        /// </summary>
        Task IndexItemAsync(KnowledgeItem item);

        /// <summary>
        /// Search with FTS5 query syntax.
        /// </summary>
        Task<IEnumerable<KnowledgeItem>> SearchAsync(string query);

        /// <summary>
        /// Advanced search with filters.
        /// </summary>
        Task<IEnumerable<KnowledgeItem>> SearchAdvancedAsync(string query, string domain = null, string category = null);

        /// <summary>
        /// Get search statistics.
        /// </summary>
        Task<(int IndexedCount, DateTime LastIndexed)> GetStatsAsync();
    }
}
