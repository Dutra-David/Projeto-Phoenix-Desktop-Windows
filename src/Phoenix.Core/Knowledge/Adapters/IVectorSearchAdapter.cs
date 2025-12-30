using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Phoenix.Core.Knowledge.Models;

namespace Phoenix.Core.Knowledge.Adapters
{
    /// <summary>
    /// Interface for vector search capabilities using embeddings.
    /// </summary>
    public interface IVectorSearchAdapter
    {
        /// <summary>
        /// Initialize vector search with embedding model.
        /// </summary>
        Task InitializeAsync(string modelPath);

        /// <summary>
        /// Generate embedding vector for text.
        /// </summary>
        Task<float[]> GenerateEmbeddingAsync(string text);

        /// <summary>
        /// Search knowledge items by semantic similarity.
        /// </summary>
        Task<IEnumerable<(KnowledgeItem Item, float Similarity)>> SearchBySemanticAsync(
            string query, int topK = 10);

        /// <summary>
        /// Index knowledge item with embeddings.
        /// </summary>
        Task IndexItemAsync(KnowledgeItem item);

        /// <summary>
        /// Calculate cosine similarity between vectors.
        /// </summary>
        float CalculateSimilarity(float[] vector1, float[] vector2);
    }
}
