# Arquitetura - Project Phoenix Desktop

## Visão Geral

O **Project Phoenix Desktop** é um agente autônomo multi-funcional baseado em arquitetura de **Portos e Adaptadores (Hexagonal Architecture)** com separação clara entre domínio, aplicação e infraestrutura.

## Estrutura em Camadas

```
┌─────────────────────────────────────────┐
│   UI Layer (WPF / Console)              │
├─────────────────────────────────────────┤
│   Application Services Layer            │
├─────────────────────────────────────────┤
│   Domain Layer (Core Agent Logic)       │
│   ├── Agents                            │
│   ├── Tools                             │
│   ├── Knowledge Base                    │
│   └── Agent Loop Orchestrator           │
├─────────────────────────────────────────┤
│   Ports (Interfaces)                    │
│   ├── ISpeechToText                     │
│   ├── ITextToSpeech                     │
│   ├── IUIAutomationEngine               │
│   ├── ILLMProvider (Gemini)             │
│   └── IKnowledgeStore                   │
├─────────────────────────────────────────┤
│   Adapters (Implementations)            │
│   ├── WindowsSpeechAdapter              │
│   ├── UIAutomationAdapter               │
│   ├── GeminiAdapter                     │
│   └── SQLiteKnowledgeAdapter            │
└─────────────────────────────────────────┘
```

## Projetos

### 1. **Phoenix.Core** (Núcleo)
Contém toda a lógica de domínio do agente.

**Estrutura:**
- `Agent/` - Interfaces e classes base do agente
- `Tools/` - Contrato de ferramentas (Tools)
- `Knowledge/` - Abstração de banco de conhecimento
- `Speech/` - Interfaces para TTS/STT
- `UIAutomation/` - Abstração para automação de UI
- `LLM/` - Provider para LLM (Gemini)
- `DI/` - Configuração de Dependency Injection

### 2. **Phoenix.UI.Wpf** (Interface Gráfica)
UI moderna em WPF para interação com o agente.

**Funcionalidades:**
- Painel de controle do agente
- Visualização em tempo real do Agent Loop
- Editor de Knowledge Base
- Monitor de ferramentas executadas

### 3. **Phoenix.CLI** (Console)
Interface de linha de comando para testes e automação.

### 4. **Projetos de Teste**
- `Phoenix.Core.Tests` - Testes unitários
- `Phoenix.Integration.Tests` - Testes de integração

## Agent Loop (Ciclo de Vida do Agente)

O agente executa continuamente este ciclo:

```
1. PERCEPÇÃO (Perceive)
   ├─ Capturar entrada do usuário
   ├─ Ler estado da tela (UI Automation)
   └─ Recuperar contexto do Knowledge Base
   ↓
2. PLANEJAMENTO (Plan)
   ├─ Enviar contexto para Gemini
   ├─ Gemini retorna plano de ação
   └─ Decompor em ferramentas executáveis
   ↓
3. EXECUÇÃO (Execute)
   ├─ Executar ferramentas em sequência
   ├─ Capturar resultados de cada ferramenta
   └─ Atualizar estado do contexto
   ↓
4. AVALIAÇÃO (Evaluate)
   ├─ Verificar sucesso das ações
   ├─ Calcular score de sucesso
   └─ Identificar erros ou desvios
   ↓
5. APRENDIZADO (Learn)
   ├─ Persistir insights no Knowledge Base
   ├─ Atualizar histórico de execução
   └─ Gerar feedback para próximas iterações
```

## Padrões de Design Utilizados

### 1. Hexagonal Architecture (Ports & Adapters)
- **Ports**: Interfaces que definem contratos (`ISpeechToText`, `IUIAutomationEngine`)
- **Adapters**: Implementações concretas (`WindowsSpeechAdapter`, `UIAutomationAdapter`)
- **Benefício**: Fácil trocar de tecnologia sem mudar lógica de domínio

### 2. Dependency Injection
- Todas as dependências injetadas via DI
- Configuração centralizada em `ServiceConfiguration.cs`
- Permite swap de implementações em testes

### 3. Repository Pattern
- `IKnowledgeStore` abstrai a persistência
- Suporta múltiplos backends (SQLite, CosmosDB, arquivo local)

### 4. Strategy Pattern
- `ITool` define contrato para ferramentas
- Novo Tool = nova implementação de `ITool`
- Registry centralizado: `ToolRegistry`

### 5. Observer Pattern
- Eventos de mudança de estado do agente
- UI e logs se subscribem para reação em tempo real

## Fluxo de Dados

```
User Input
    ↓
┌───────────────────────┐
│  Agent Loop           │
│  Orchestrator         │
├───────────────────────┤
│ Perceive              │ ← UIAutomationEngine (lerUI)
│ Plan                  │ ← GeminiProvider (raciocina)
│ Execute               │ ← ToolRegistry (executa ações)
│ Evaluate              │ (analisa resultado)
│ Learn                 │ → KnowledgeBase (persiste)
└───────────────────────┘
    ↓
UI Update / Response
```

## Interfaces Críticas (Ports)

### IAgent
```csharp
public interface IAgent
{
    Task StartAsync();
    Task StopAsync();
    Task<AgentResponse> ProcessInputAsync(string input);
    IReadOnlyList<string> GetAvailableTools();
}
```

### ITool
```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolSchema Schema { get; }
    Task<ToolResult> ExecuteAsync(ToolInput input);
}
```

### IKnowledgeStore
```csharp
public interface IKnowledgeStore
{
    Task<KnowledgeItem> GetAsync(string id);
    Task SaveAsync(KnowledgeItem item);
    Task<List<KnowledgeItem>> SearchAsync(string query);
}
```

## Decisões Arquiteturais

| Decisão | Motivo | Alternativa |
|---------|--------|-------------|
| .NET 8.0 | Moderno, performance, cross-platform | Java, Python |
| WPF | Native Windows, integração com Windows APIs | MAUI, Electron |
| Hexagonal Arch | Testabilidade, flexibilidade | Monolítico |
| Gemini API | Raciocínio avançado, custo baixo | OpenAI, Anthropic |
| SQLite | Leve, sem servidor, offline | PostgreSQL, CosmosDB |

## Escalabilidade Futura

1. **Múltiplos Agentes**: Modificar `AgentLoopOrchestrator` para gerenciar N agentes
2. **Persistência em nuvem**: Implementar `IKnowledgeStore` com CosmosDB
3. **LLM plugável**: Interface `ILLMProvider` já preparada
4. **API REST**: Adicionar layer de API para acesso remoto
5. **Plugins**: Sistema de extensão via reflection + `ITool` dinâmicos
