using System;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Core.Knowledge.Adapters.Exceptions;

namespace Phoenix.Core.Knowledge.Adapters.Factories
{
    /// <summary>
    /// Factory interface for creating adapter instances with dependency injection support.
    /// Enables loose coupling and simplifies testing through dependency injection.
    /// </summary>
    public interface IAdapterFactory
    {
        /// <summary>
        /// Create a knowledge store adapter instance.
        /// </summary>
        /// <param name="type">Type of store: 'sqlite', 'memory', etc.</param>
        /// <param name="connectionString">Connection string or configuration</param>
        /// <returns>IKnowledgeStore instance</returns>
        IKnowledgeStore CreateKnowledgeStore(string type, string connectionString);

        /// <summary>
        /// Create a vector search adapter instance.
        /// </summary>
        /// <param name="type">Type of search: 'embedding', 'faiss', etc.</param>
        /// <param name="modelPath">Path to the embedding model</param>
        /// <returns>IVectorSearchAdapter instance</returns>
        IVectorSearchAdapter CreateVectorSearchAdapter(string type, string modelPath);

        /// <summary>
        /// Create a full-text search adapter instance.
        /// </summary>
        /// <param name="type">Type of search: 'sqlite-fts5', 'elasticsearch', etc.</param>
        /// <param name="config">Configuration string</param>
        /// <returns>IFullTextSearchAdapter instance</returns>
        IFullTextSearchAdapter CreateFullTextSearchAdapter(string type, string config);

        /// <summary>
        /// Create a versioning adapter instance.
        /// </summary>
        /// <param name="storeType">Store type for versioning</param>
        /// <returns>IVersioningAdapter instance</returns>
        IVersioningAdapter CreateVersioningAdapter(string storeType);

        /// <summary>
        /// Create a CosmosDB synchronization adapter instance.
        /// </summary>
        /// <param name="connectionString">CosmosDB connection string</param>
        /// <returns>ICosmosDbAdapter instance</returns>
        ICosmosDbAdapter CreateCosmosDbAdapter(string connectionString);

        /// <summary>
        /// Register all adapters in dependency injection container.
        /// </summary>
        void RegisterAll(IServiceCollection services);
    }

    /// <summary>
    /// Default implementation of adapter factory using service provider.
    /// </summary>
    public class AdapterFactory : IAdapterFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AdapterFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IKnowledgeStore CreateKnowledgeStore(string type, string connectionString)
        {
            try
            {
                return type?.ToLowerInvariant() switch
                {
                    "sqlite" => new SqliteKnowledgeStore(
                        connectionString,
                        _serviceProvider.GetService<Microsoft.Extensions.Logging.ILogger<SqliteKnowledgeStore>>()
                    ),
                    "memory" => throw new NotImplementedException("Memory store not yet implemented"),
                    _ => throw new ArgumentException($"Unknown knowledge store type: {type}")
                };
            }
            catch (Exception ex)
            {
                throw new AdapterInitializationException(
                    adapterType: $"KnowledgeStore:{type}",
                    message: $"Failed to create {type} knowledge store adapter",
                    innerException: ex
                );
            }
        }

        public IVectorSearchAdapter CreateVectorSearchAdapter(string type, string modelPath)
        {
            try
            {
                return type?.ToLowerInvariant() switch
                {
                    "embedding" => new EmbeddingVectorSearchAdapter(
                        modelPath,
                        _serviceProvider.GetService<Microsoft.Extensions.Logging.ILogger<EmbeddingVectorSearchAdapter>>()
                    ),
                    "faiss" => throw new NotImplementedException("FAISS adapter not yet implemented"),
                    _ => throw new ArgumentException($"Unknown vector search type: {type}")
                };
            }
            catch (Exception ex)
            {
                throw new AdapterInitializationException(
                    adapterType: $"VectorSearch:{type}",
                    message: $"Failed to create {type} vector search adapter",
                    innerException: ex
                );
            }
        }

        public IFullTextSearchAdapter CreateFullTextSearchAdapter(string type, string config)
        {
            try
            {
                return type?.ToLowerInvariant() switch
                {
                    "sqlite-fts5" => new FullTextSearchAdapter(
                        config,
                        _serviceProvider.GetService<Microsoft.Extensions.Logging.ILogger<FullTextSearchAdapter>>()
                    ),
                    "elasticsearch" => throw new NotImplementedException("Elasticsearch not yet implemented"),
                    _ => throw new ArgumentException($"Unknown full-text search type: {type}")
                };
            }
            catch (Exception ex)
            {
                throw new AdapterInitializationException(
                    adapterType: $"FullTextSearch:{type}",
                    message: $"Failed to create {type} full-text search adapter",
                    innerException: ex
                );
            }
        }

        public IVersioningAdapter CreateVersioningAdapter(string storeType)
        {
            try
            {
                // Implementation would depend on your versioning store
                throw new NotImplementedException("Versioning adapter factory not yet implemented");
            }
            catch (Exception ex)
            {
                throw new AdapterInitializationException(
                    adapterType: $"Versioning:{storeType}",
                    message: $"Failed to create versioning adapter",
                    innerException: ex
                );
            }
        }

        public ICosmosDbAdapter CreateCosmosDbAdapter(string connectionString)
        {
            try
            {
                return new CosmosDbAdapter(
                    connectionString,
                    _serviceProvider.GetService<Microsoft.Extensions.Logging.ILogger<CosmosDbAdapter>>()
                );
            }
            catch (Exception ex)
            {
                throw new AdapterInitializationException(
                    adapterType: "CosmosDb",
                    message: "Failed to create CosmosDB adapter",
                    innerException: ex
                );
            }
        }

        public void RegisterAll(IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            // Register factory itself
            services.AddSingleton<IAdapterFactory>(this);
        }
    }
}
