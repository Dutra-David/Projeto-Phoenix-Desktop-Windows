# Project Phoenix Desktop - Agente Autonô mo Multi-Funcional

[![Build Status](https://img.shields.io/badge/build-success-brightgreen)](.github/workflows/build.yml)
[![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)
[![Code Quality](https://img.shields.io/badge/code--quality-A-brightgreen)](#)

## Vision

**Project Phoenix Desktop** is an autonomous, multi-functional Windows desktop agent powered by:

- 🧠 **Gemini AI**: Advanced reasoning with LLM integration
- 🎤 **Voice I/O**: Bidirectional speech (TTS/STT) natively in Portuguese/English
- 🖥️ **UI Automation**: Control any Windows application
- 💾 **Knowledge Persistence**: Local SQLite knowledge base with learning capabilities
- 🏗️ **Modular Design**: Pluggable architecture (Ports & Adapters pattern)
- ⚡ **Performance**: .NET 8 modern stack with full async/await support

## Architecture Overview

### Agent Loop (Lifecycle)

```
PERCEPTION → PLANNING → EXECUTION → EVALUATION → LEARNING
    ↓           ↓          ↓           ↓            ↓
  Input     Gemini    Tools Run   Score Result  Update KB
```

### Tech Stack

| Component | Technology | Version |
|-----------|-----------|----------|
| Runtime | .NET | 8.0+ |
| LLM | Google Gemini API | Latest |
| UI | WPF (Desktop) + CLI | Built-in |
| Database | SQLite | Latest |
| Async | Task/async-await | Native |
| Testing | xUnit + Moq | Latest |
| DI | Microsoft.Extensions.DI | 8.0+ |

## Quick Start

### Prerequisites
- .NET 8.0 SDK
- Windows 10/11
- Visual Studio 2022+ or VS Code
- Gemini API Key (free tier available)

### Clone & Build

```bash
# Clone
git clone https://github.com/Dutra-David/Projeto-Phoenix-Desktop-Windows.git
cd Projeto-Phoenix-Desktop-Windows

# Restore & Build
dotnet restore
dotnet build -c Release

# Test
dotnet test
```

### Run

```bash
# WPF Desktop UI
cd src/Phoenix.UI.Wpf && dotnet run

# CLI
cd src/Phoenix.CLI && dotnet run -- --input "Open Notepad"
```

## Documentation

All documentation is versioned in the repo:

- **[ARCHITECTURE.md](ARCHITECTURE.md)** - Complete design & patterns  
- **[AGENTS.md](AGENTS.md)** - Agent specifications & types
- **[BUILD.md](BUILD.md)** - Build pipeline & reproducible builds

## Project Structure

```
src/
  ├── Phoenix.Core/           # Domain logic
  │   ├── Agent/             # IAgent, AgentLoopOrchestrator
  │   ├── Tools/             # ITool, ToolRegistry
  │   ├── Knowledge/         # IKnowledgeStore, KnowledgeBase
  │   ├── Speech/            # ISpeechToText, ITextToSpeech
  │   ├── UIAutomation/      # IUIAutomationEngine
  │   └── LLM/               # ILLMProvider (Gemini)
  ├── Phoenix.UI.Wpf/        # WPF desktop application
  └── Phoenix.CLI/           # Command-line interface
tests/
  ├── Phoenix.Core.Tests/
  └── Phoenix.Integration.Tests/
```

## Configuration

### Environment Variables

```bash
# Required: Get from https://makersuite.google.com/app/apikey
export GEMINI_API_KEY="your-api-key"

# Optional
export PHOENIX_KB_PATH="./data/knowledge.db"
export PHOENIX_LOG_LEVEL="Information"
```

## Design Patterns

✅ **Hexagonal Architecture** (Ports & Adapters)  
✅ **Dependency Injection** (Microsoft.Extensions)  
✅ **Repository Pattern** (IKnowledgeStore)  
✅ **Strategy Pattern** (ITool)  
✅ **Observer Pattern** (StateChanged events)  
✅ **Async/Await** (All I/O is non-blocking)

## Code Quality

- 100% Dependency Injection
- Fully testable (all dependencies mockable)
- C# 12 latest features
- Null-safe (#nullable enable)
- StyleCop enforcement
- Async-first design

## Example Usage

```csharp
var agent = serviceProvider.GetRequiredService<IAgent>();
await agent.StartAsync();

var response = await agent.ProcessInputAsync("Open Notepad and write 'Hello'");
if (response.Success)
{
    Console.WriteLine($"Tools executed: {string.Join(", ", response.ExecutedTools)}");
    Console.WriteLine($"Score: {response.Data}");
}

await agent.StopAsync();
```

## Contributing

1. Fork the repository
2. Create feature branch: `git checkout -b feature/MyFeature`
3. Commit: `git commit -m 'Add MyFeature'`
4. Push: `git push origin feature/MyFeature`
5. Open Pull Request

## Future Roadmap

- [ ] Multi-agent orchestration
- [ ] Cloud persistence (CosmosDB)
- [ ] REST API layer
- [ ] Plugin system
- [ ] LLM provider abstraction (OpenAI, Claude, etc)
- [ ] Advanced vector search
- [ ] GitHub Actions CI/CD
- [ ] Docker containerization

## License

MIT License - See [LICENSE](LICENSE)

## Support

- 📖 **Docs**: See [ARCHITECTURE.md](ARCHITECTURE.md)
- 🐛 **Issues**: [GitHub Issues](https://github.com/Dutra-David/Projeto-Phoenix-Desktop-Windows/issues)
- 💬 **Discussions**: [GitHub Discussions](https://github.com/Dutra-David/Projeto-Phoenix-Desktop-Windows/discussions)

---

**Built with passion by [Dutra-David](https://github.com/Dutra-David)** 💜

*Project Phoenix: Autonomous, Intelligent, Scalable.*
