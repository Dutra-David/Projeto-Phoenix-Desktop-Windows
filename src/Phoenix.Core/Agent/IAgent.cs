namespace Phoenix.Core.Agent
{
    /// <summary>
    /// Define o contrato para um agente autônomo dentro do Phoenix.
    /// Todo agente deve implementar percepção, planejamento, ação e aprendizado.
    /// </summary>
    public interface IAgent
    {
        string Id { get; }
        string Name { get; }
        AgentState State { get; }

        /// <summary>
        /// Inicia o ciclo de vida do agente.
        /// </summary>
        Task StartAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Para o ciclo de vida do agente gracefully.
        /// </summary>
        Task StopAsync();

        /// <summary>
        /// Processa uma entrada do usuário ou evento externo.
        /// </summary>
        Task<AgentResponse> ProcessInputAsync(string input, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retorna capacidades/ferramentas disponíveis.
        /// </summary>
        IReadOnlyList<string> GetAvailableTools();

        /// <summary>
        /// Evento disparado quando o agente muda de estado.
        /// </summary>
        event EventHandler<AgentStateChangedEventArgs>? StateChanged;
    }

    public enum AgentState
    {
        Idle,
        Perceiving,
        Planning,
        Executing,
        Learning,
        Paused,
        Error
    }

    public class AgentStateChangedEventArgs : EventArgs
    {
        public AgentState OldState { get; set; }
        public AgentState NewState { get; set; }
        public string? Reason { get; set; }
    }

    public class AgentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
        public List<string> ExecutedTools { get; set; } = new();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
