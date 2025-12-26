# ⚡️ PROJECT PHOENIX – DOUTRINA OFICIAL (VERSÃO DESKTOP WINDOWS) ⚡️

Comandante Supremo: General David Dutra  
Aplicativo: Project Phoenix Desktop (Windows, .NET 8+, C#, WPF)  
Função: Agente Autônomo Multi-Função, Multi-Agente e Auto-Evolutivo para Plataforma Desktop

---

## 1. VISÃO GERAL - VERSÃO WINDOWS

Project Phoenix Desktop não é "só um app".
É um SISTEMA MULTI-AGENTE rodando nativamente no Windows, projetado para:

- Automatizar tarefas no desktop (UI Automation, Win32 API, WinAppDriver).
- Responder dúvidas técnicas (programação, IA, engenharia de prompt, linguagens).
- Interagir por voz (fala e escuta usando APIs nativas do Windows).
- Processar imagens de screenshots para análise visual.
- Auto-melhorar código C#, arquitetura e experiência com apoio de IAs avançadas (Gemini Agent Mode, etc.), sempre sob comando do General.
- Executar com máximo desempenho em máquinas Windows (8GB RAM+), com cache local e processamento multi-thread otimizado.

---

## 2. PAPÉIS DOS AGENTES INTERNOS (ADAPTADO PARA WINDOWS)

### 2.1 PhoenixOrchestrator (Orquestrador)

- Função: "Cérebro tático".
- Tecnologia: Implementado como serviço Windows ou aplicativo WPF.
- Recebe requisições de alto nível do General (via CLI, UI, ou voz).
- Decide quais agentes internos devem atuar.

### 2.2 PhoenixUIAutomationAgent

- Interface com o sistema via **UI Automation** (UIA) e **Win32 API**.
- Tarefas:
  - Capturar screenshots e analisar a tela.
  - Identificar elementos UI (botões, campos, janelas).
  - Executar cliques, digitação, atalhos de teclado.
  - Navegar entre aplicações desktop.
- Toda ação é registrada em log para auditoria.

### 2.3 PhoenixKnowledgeAgent

- Banco local (SQLite com Entity Framework) com conhecimento sobre:
  - Programação
  - IA
  - Engenharia de Prompt
  - Linguagens de programação
- Módulo de sincronização online (quando autorizado) para atualizar conhe cimento.

### 2.4 PhoenixVoiceAgent

- Interface de VOZ do Phoenix (Windows Speech Synthesis e Recognition).
- Falar respostas (Text-to-Speech).
- Ouvir comandos (Speech-to-Text).
- Sempre com permissão explícita.

### 2.5 PhoenixOptimizationAgent

- "Engenheiro residente" de auto-melhoria.
- Observa logs, métricas de performance, uso de CPU/memória.
- Propõe refatorações de código C# e arquitetura.
- Integra com Gemini Agent Mode para aplicação de melhorias.

### 2.6 PhoenixVisionAgent

- Processamento de screenshots e imagens.
- Função: ajudar o agente a entender visualmente o que está na tela.

---

## 3. ARQUITETURA TÉCNICA (WINDOWS .NET)

- **Framework**: .NET 8+ (LTS)
- **Linguagem**: C# (11+)
- **UI**: WPF (XAML) com MVVM
- **Banco Local**: SQLite + Entity Framework Core
- **Automação UI**: 
  - UIAutomation (UIA) para apps modernas
  - Win32 P/Invoke para compat ibilidade
  - WinAppDriver (opcional, para UWP)
- **Voz**: Windows Speech (System.Speech) + APIs modernas
- **Injeção de Dependência**: Microsoft.Extensions.DependencyInjection
- **Logs**: Serilog (com console + arquivo)

---

## 4. PERFIL DO GENERAL (PERSONALIZAÇÃO)

O app mantém um "Perfil do General" local para personalizar:

- Modo de interação (CLI, UI WPF, voz, mista).
- Tema visual (light/dark).
- Padrões de uso e preferências de agentes.

Tudo armazenado localmente (AppData).

---

## 5. FLUXOS TÍPICOS

### 5.1 Automação de tarefa Windows

1. General dá um comando (texto, voz, ou clique na UI).
2. Orquestrador roteia para PhoenixUIAutomationAgent.
3. Agent captura a tela, identifica elementos, executa ações.
4. Logs são registrados.

### 5.2 Pergunta técnica

1. General faz uma pergunta (voz ou texto).
2. PhoenixKnowledgeAgent busca no banco local.
3. VoiceAgent pode ler a resposta em voz alta.

### 5.3 Auto-melhoria

1. General ordena um "ciclo de auto-melhoria".
2. OptimizationAgent coleta métricas.
3. Gemini Agent Mode planeja e aplica refatorações no código C#.
4. Resultado é validado (build, testes).

---

## 6. REGRAS PARA QUALQUER IA QUE EDITAR ESTE PROJETO

1. Sempre respeitar a hierarquia de agentes.
2. Nunca criar comportamentos de malware (sem auto-modificação de exe, sem bypass UAC).
3. Para mudanças grandes: propor plano antes, executar em ondas pequenas.
4. Explicar ao General em linguagem simples: o que foi feito, por que, como usar.

---

## 7. MENSAGEM AO ENGENHEIRO DE PROMPT (GENERAL DAVID DUTRA)

Você não precisa ser programador.

Sua função é definir MISSÕES claras:
- Automatizar.
- Ensinar.
- Conversar.
- Melhorar-se.

Qualquer IA que ler este AGENTS.md deve:
- Tratar você como comandante.
- Respeitar esta doutrina.
- Entregar código compilável, limpo e explicável (C# .NET).

> **Estado atual: Phoenix Desktop (Windows) em preparação.**  
> **Pronto para novas ordens do General.**
