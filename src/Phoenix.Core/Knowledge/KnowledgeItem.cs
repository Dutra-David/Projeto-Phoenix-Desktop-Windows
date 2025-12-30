namespace Phoenix.Core.Knowledge
{
    /// <summary>
    /// Representa um item de conhecimento no banco de dados Phoenix.
    /// Suporta 5 domínios: Psicologia, Mecanica, Programacao, Engenharia de Prompts, Engenharia de Software
    /// </summary>
    public class KnowledgeItem
    {
        /// <summary>
        /// Identificador único do item
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Domínio do conhecimento (Psychology, Mechanics, Programming, PromptEngineering, SoftwareEngineering)
        /// </summary>
        public string Domain { get; set; } = string.Empty;

        /// <summary>
        /// Título ou conceito principal
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Conteúdo detalhado do conhecimento
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Tags para categorização e busca (separadas por vírgula)
        /// </summary>
        public string Tags { get; set; } = string.Empty;

        /// <summary>
        /// Prioridade do item (1-5)
        /// </summary>
        public int Priority { get; set; } = 1;

        /// <summary>
        /// Indica se o item está ativo
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Data da última atualização
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Quantidade de visualizações
        /// </summary>
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// Score de relevância (0.0 - 1.0)
        /// </summary>
        public double Relevance { get; set; } = 0.5;

        // Domain-specific properties

        /// <summary>
        /// Categoria específica do domínio
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Descrição detalhada
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Aplicações práticas (para Psicologia)
        /// </summary>
        public string? Applications { get; set; }

        /// <summary>
        /// Referências bibliográficas
        /// </summary>
        public string? References { get; set; }

        /// <summary>
        /// Nível de dificuldade/complexidade
        /// </summary>
        public string? Level { get; set; }

        /// <summary>
        /// Exemplos de código (para Programação)
        /// </summary>
        public string? CodeExample { get; set; }

        /// <summary>
        /// Melhores práticas
        /// </summary>
        public string? BestPractices { get; set; }

        /// <summary>
        /// Técnica específica (para Engenharia de Prompts)
        /// </summary>
        public string? Technique { get; set; }

        /// <summary>
        /// Exemplo de prompt
        /// </summary>
        public string? PromptExample { get; set; }

        /// <summary>
        /// Resultado esperado
        /// </summary>
        public string? ExpectedResult { get; set; }

        /// <summary>
        /// Casos de uso
        /// </summary>
        public string? UseCases { get; set; }

        /// <summary>
        /// Eficácia da técnica (0.0 - 1.0)
        /// </summary>
        public double? Effectiveness { get; set; }

        /// <summary>
        /// Padrão de design (para Engenharia de Software)
        /// </summary>
        public string? Pattern { get; set; }

        /// <summary>
        /// Implementação do padrão
        /// </summary>
        public string? Implementation { get; set; }

        /// <summary>
        /// Vantagens
        /// </summary>
        public string? Advantages { get; set; }

        /// <summary>
        /// Desvantagens
        /// </summary>
        public string? Disadvantages { get; set; }

        /// <summary>
        /// Método para marcar visualização
        /// </summary>
        public void MarkViewed()
        {
            ViewCount++;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Método para atualizar relevância
        /// </summary>
        public void UpdateRelevance(double newRelevance)
        {
            if (newRelevance >= 0.0 && newRelevance <= 1.0)
            {
                Relevance = newRelevance;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Retorna uma representação em string do item
        /// </summary>
        public override string ToString()
        {
            return $"[{Domain}] {Title} (Relevance: {Relevance:P}, Views: {ViewCount})";
        }
    }
}
