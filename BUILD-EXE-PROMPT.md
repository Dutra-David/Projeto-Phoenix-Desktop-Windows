# ⚡️ PROJECT PHOENIX - BUILD .EXE WINDOWS (PROMPT PARA GEMINI AGENT MODE) ⚡️

## MISSÃO: Gerar .EXE funcional do Phoenix Desktop para testes locais

**General**: Use este prompt no Gemini Agent Mode (Visual Studio) ou via CLI para criar um .exe do Project Phoenix pronto para deploy na sua máquina local.

---

## ⚡️ ORDEM SUPREMA AO CAPITÃO GEMINI

```
⚔️ OPERAÇÃO: PHOENIX DESKTOP - BUILD V1.0
Modo: Visual Studio Gemini Agent Mode
Comandante: General David Dutra
Objetivo: Criar .EXE funcional com Phoenix Desktop v1.0

PASSO 1 - VALIDAÇÃO
- Leia o arquivo AGENTS.md deste repositório
- Confirme que você compreende a arquitetura de agentes
- Valide se o Visual Studio 2022 está instalado com:
  ✓ .NET 8 SDK
  ✓ Ferramentas de desenvolvimento C#
  ✓ WPF Designer

PASSO 2 - CLONE E ESTRUTURA
- Clone o repositório: https://github.com/Dutra-David/Projeto-Phoenix-Desktop-Windows.git
- Navegue até a pasta raiz
- Crie a seguinte estrutura de pastas (se não existir):
  
  Projeto-Phoenix-Desktop-Windows/
  ├── src/
  │   ├── Phoenix.Desktop/          (Projeto WPF principal)
  │   ├── Phoenix.Core/              (Núcleo de agentes - Class Library)
  │   ├── Phoenix.UIAutomation/      (Agente de automação UI - Class Library)
  │   └── Phoenix.Knowledge/         (Agente de conhecimento - Class Library)
  ├── tests/
  │   └── Phoenix.Tests/             (Testes unitários - MSTest)
  ├── build/
  │   └── (outputs será gerado aqui)
  ├── docs/
  ├── .gitignore
  ├── README.md
  ├── AGENTS.md
  └── build-config.json

PASSO 3 - CRIAR PROJETO .NET 8 WPF (SE NÃO EXISTIR)
- Nome: Phoenix.Desktop
- Template: WPF Application (.NET 8)
- Target Framework: .NET 8.0
- Language: C#
- Diretório: src/Phoenix.Desktop/

PASSO 4 - ESTRUTURA INICIAL DO CÓDIGO (MÍNIMO FUNCIONAL)

Arquivo: src/Phoenix.Desktop/App.xaml.cs
```csharp
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Phoenix.Desktop
{
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
            
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }
}
```

Arquivo: src/Phoenix.Desktop/App.xaml
```xaml
<Application x:Class="Phoenix.Desktop.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
    </Application.Resources>
</Application>
```

Arquivo: src/Phoenix.Desktop/MainWindow.xaml
```xaml
<Window x:Class="Phoenix.Desktop.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Project Phoenix Desktop v1.0" Height="600" Width="900"
        WindowStartupLocation="CenterScreen"
        Background="#1E1E1E">
    <Grid>
        <StackPanel Margin="20" VerticalAlignment="Top">
            <TextBlock Text="⚔️ Project Phoenix - Desktop Agent v1.0" 
                       FontSize="24" Foreground="#00D4FF" Margin="0,0,0,20" FontWeight="Bold"/>
            
            <TextBlock Text="Status: INITIALIZED" 
                       FontSize="14" Foreground="#00FF00" Margin="0,0,0,10"/>
            
            <TextBlock Text="Comandante: General David Dutra" 
                       FontSize="12" Foreground="#FFFFFF" Margin="0,0,0,10"/>
            
            <Border BorderBrush="#00D4FF" BorderThickness="1" Padding="15" Margin="0,20,0,0">
                <StackPanel>
                    <TextBlock Text="Agentes Internos Carregados:" FontSize="12" FontWeight="Bold" Foreground="#00D4FF" Margin="0,0,0,10"/>
                    <TextBlock Text="✓ PhoenixOrchestrator" FontSize="11" Foreground="#00FF00" Margin="10,0,0,5"/>
                    <TextBlock Text="✓ PhoenixUIAutomationAgent" FontSize="11" Foreground="#00FF00" Margin="10,0,0,5"/>
                    <TextBlock Text="✓ PhoenixKnowledgeAgent" FontSize="11" Foreground="#00FF00" Margin="10,0,0,5"/>
                    <TextBlock Text="✓ PhoenixVoiceAgent" FontSize="11" Foreground="#FFFF00" Margin="10,0,0,5"/>
                    <TextBlock Text="✓ PhoenixOptimizationAgent" FontSize="11" Foreground="#FFFF00" Margin="10,0,0,5"/>
                </StackPanel>
            </Border>
            
            <Button Content="Iniciar Operações" Width="200" Height="40" Margin="0,20,0,0" 
                    Background="#00D4FF" Foreground="#000000" FontWeight="Bold"/>
        </StackPanel>
    </Grid>
</Window>
```

Arquivo: src/Phoenix.Desktop/MainWindow.xaml.cs
```csharp
using System.Windows;

namespace Phoenix.Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
```

Arquivo: src/Phoenix.Desktop/MainViewModel.cs
```csharp
using System.Collections.ObjectModel;

namespace Phoenix.Desktop
{
    public class MainViewModel
    {
        public ObservableCollection<string> AgentStatus { get; } = new();

        public MainViewModel()
        {
            InitializeAgents();
        }

        private void InitializeAgents()
        {
            AgentStatus.Add("[INFO] PhoenixOrchestrator - READY");
            AgentStatus.Add("[INFO] PhoenixUIAutomationAgent - READY");
            AgentStatus.Add("[INFO] PhoenixKnowledgeAgent - READY");
            AgentStatus.Add("[INFO] PhoenixVoiceAgent - WAITING FOR INITIALIZATION");
            AgentStatus.Add("[INFO] PhoenixOptimizationAgent - STANDBY");
        }
    }
}
```

PASSO 5 - CONFIGURAR PROJETO CSPROJ

Arquivo: src/Phoenix.Desktop/Phoenix.Desktop.csproj
```xml
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0-windows</TargetFramework>
    <UseWPF>true</UseWPF>
    <Nullable>enable</Nullable>
    <AssemblyName>PhoenixDesktop</AssemblyName>
    <RootNamespace>Phoenix.Desktop</RootNamespace>
    <Version>1.0.0</Version>
    <Authors>General David Dutra</Authors>
    <Company>Project Phoenix</Company>
    <Product>Phoenix Desktop Agent</Product>
    <Description>Agente Autônomo Multi-Funcional para Windows</Description>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="8.0.0" />
    <PackageReference Include="Serilog" Version="3.1.1" />
    <PackageReference Include="Serilog.Sinks.Console" Version="5.0.1" />
  </ItemGroup>
</Project>
```

PASSO 6 - BUILD E COMPILAÇÃO

```bash
# Abra terminal na raiz do projeto
cd Projeto-Phoenix-Desktop-Windows

# Restaurar dependências
dotnet restore

# Compilar em modo Debug (rápido, para testes)
dotnet build src/Phoenix.Desktop/Phoenix.Desktop.csproj -c Debug

# OU compilar em modo Release (otimizado, para produção)
dotnet build src/Phoenix.Desktop/Phoenix.Desktop.csproj -c Release

# Publicar como .EXE self-contained
dotnet publish src/Phoenix.Desktop/Phoenix.Desktop.csproj -c Release -r win-x64 --self-contained=false -o build/release
```

PASSO 7 - LOCALIZAR O .EXE

Após a compilação:
- **Debug**: `src/Phoenix.Desktop/bin/Debug/net8.0-windows/PhoenixDesktop.exe`
- **Release Standalone**: `build/release/PhoenixDesktop.exe`

PASSO 8 - TESTAR LOCALMENTE

```bash
# Executar direto pela CLI
build/release/PhoenixDesktop.exe

# OU clicar duas vezes em:
C:\(seu-path)\Projeto-Phoenix-Desktop-Windows\build\release\PhoenixDesktop.exe
```

PASSO 9 - VALIDAÇÃO DO APP

O app deve:
✓ Abrir uma janela WPF com fundo escuro (#1E1E1E)
✓ Mostrar "Project Phoenix - Desktop Agent v1.0" no título
✓ Listar os 5 agentes como READY/STANDBY
✓ Responder a cliques no botão "Iniciar Operações" (sem erro)
✓ Fechar sem exceções

PASSO 10 - PREPARAR PARA DEPLOYMENT

```bash
# Copiar o .EXE para uma pasta de distribuição
mkdir C:\Phoenix-Deploy
copy build\release\PhoenixDesktop.exe C:\Phoenix-Deploy\

# (Opcional) Criar atalho na Desktop
# botão direito em PhoenixDesktop.exe → Enviar para → Desktop (criar atalho)
```

PASSO 11 - DOCUMENTAR RESULTADO

Crie um arquivo build-output.txt com:
- Data/hora do build
- Versão do .NET SDK usada
- Tamanho do .EXE
- Caminhos dos outputs
- Resultado dos testes

PASSO 12 - REPORT AO GENERAL

Ao terminar, reporte:
```
✓ Projeto clonado e estruturado
✓ Código inicial criado (WPF + MainViewModel)
✓ Dependências restauradas (DI, Serilog)
✓ Build compilado com sucesso
✓ .EXE gerado em: [caminho exato]
✓ Testes locais passaram
✓ Pronto para deploy na máquina local do General

Operação Phoenix Build v1.0 concluída com sucesso!
```

---

## ⚠️ NOTAS IMPORTANTES

- **Pré-requisito**: Visual Studio 2022 Community (ou superior) + .NET 8 SDK
- **Alternativa CLI**: Se não tiver VS2022, use apenas `.NET CLI` (dotnet)
- **Testes**: Todos os passos devem ser validados antes de cada build
- **Segurança**: Não remova/altere permissões sem avisar ao General

---

## 🎖️ PRÓXIMO PASSO

Após o .EXE funcionar:
> "Capitão, agora implante o PhoenixDesktop.exe na máquina local e me mostre um vídeo/screenshot da janela funcionando."
```
