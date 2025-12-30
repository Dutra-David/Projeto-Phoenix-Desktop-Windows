namespace Phoenix.Core.Knowledge
{
    /// <summary>
    /// Interface para persistencia e recuperacao de conhecimento
    /// Suporta multiplas implementacoes (SQLite, CosmosDB, arquivo local, etc)
    /// </summary>
    public interface IKnowledgeStore
    {
        // CRUD Operations

        /// <summary>
        /// Salva um novo item de conhecimento
        /// </summary>
        Task<KnowledgeItem> AddAsync(KnowledgeItem item, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtem um item de conhecimento por ID
        /// </summary>
        Task<KnowledgeItem?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Atualiza um item de conhecimento existente
        /// </summary>
        Task<bool> UpdateAsync(KnowledgeItem item, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove um item de conhecimento
        /// </summary>
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);

        // Search & Query

        /// <summary>
        /// Busca itens por dominio
        /// </summary>
        Task<List<KnowledgeItem>> GetByDomainAsync(string domain, CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca itens por tags
        /// </summary>
        Task<List<KnowledgeItem>> SearchByTagsAsync(string[] tags, CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca full-text em titulo e conteudo
        /// </summary>
        Task<List<KnowledgeItem>> SearchAsync(string query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retorna itens ativos ordenados por relevancia
        /// </summary>
        Task<List<KnowledgeItem>> GetActiveItemsAsync(int limit = 50, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retorna itens mais visualizados
        /// </summary>
        Task<List<KnowledgeItem>> GetPopularItemsAsync(int limit = 10, CancellationToken cancellationToken = default);

        // Statistics

        /// <summary>
        /// Retorna total de itens por dominio
        /// </summary>
        Task<Dictionary<string, int>> GetStatisticsByDomainAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retorna numero total de itens
        /// </summary>
        Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Marca um item como visualizado (incrementa ViewCount)
        /// </summary>
        Task<bool> MarkAsViewedAsync(string id, CancellationToken cancellationToken = default);

        // Bulk Operations

        /// <summary>
        /// Retorna todos os itens de um dominio
        /// </summary>
        Task<List<KnowledgeItem>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Importa multiplos itens em lote
        /// </summary>
        Task<int> BulkInsertAsync(List<KnowledgeItem> items, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sincroniza banco de dados (cria tabelas, indexes, etc)
        /// </summary>
        Task<bool> InitializeDatabaseAsync(CancellationToken cancellationToken = default);
    }
}
