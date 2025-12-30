using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Phoenix.Core.Knowledge.Models;

namespace Phoenix.Core.Knowledge.Adapters
{
    /// <summary>
    /// Interface for versioning and change history management.
    /// Tracks changes and enables rollback to previous versions.
    /// </summary>
    public interface IVersioningAdapter
    {
        /// <summary>
        /// Create version snapshot.
        /// </summary>
        Task<string> CreateVersionAsync(KnowledgeItem item, string changeDescription);

        /// <summary>
        /// Get version history for an item.
        /// </summary>
        Task<IEnumerable<(int Version, DateTime CreatedAt, string ChangedBy, string Description)>> GetHistoryAsync(string itemId);

        /// <summary>
        /// Rollback to specific version.
        /// </summary>
        Task<KnowledgeItem> RollbackAsync(string itemId, int versionNumber);

        /// <summary>
        /// Get diff between versions.
        /// </summary>
        Task<string> GetDiffAsync(string itemId, int versionFrom, int versionTo);

        /// <summary>
        /// Get current version number.
        /// </summary>
        Task<int> GetCurrentVersionAsync(string itemId);
    }
}
