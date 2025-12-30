using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Phoenix.Core.Knowledge.Models;

namespace Phoenix.Core.Knowledge.Adapters
{
    /// <summary>
    /// Interface for CosmosDB synchronization adapter.
    /// Handles bidirectional sync between local SQLite and Azure CosmosDB.
    /// </summary>
    public interface ICosmosDbAdapter
    {
        /// <summary>
        /// Initialize connection to CosmosDB.
        /// </summary>
        Task InitializeAsync(string connectionString, string databaseId, string containerId);

        /// <summary>
        /// Sync local item to cloud.
        /// </summary>
        Task SyncToCloudAsync(KnowledgeItem item);

        /// <summary>
        /// Sync all local items to cloud.
        /// </summary>
        Task SyncAllToCloudAsync(IEnumerable<KnowledgeItem> items);

        /// <summary>
        /// Sync from cloud to local.
        /// </summary>
        Task<IEnumerable<KnowledgeItem>> SyncFromCloudAsync();

        /// <summary>
        /// Check sync status.
        /// </summary>
        Task<bool> IsSyncedAsync(string itemId);

        /// <summary>
        /// Get sync statistics.
        /// </summary>
        Task<(int SyncedCount, int PendingCount, DateTime LastSync)> GetSyncStatsAsync();
    }
}
