# Build Pipeline - Project Phoenix Desktop

## Visao Geral

Pipeline de build reproducivel e automatizado para .NET 8.0.

## Requisitos de Build

- .NET 8.0 SDK ou superior
- Visual Studio 2022 v17.0+ (opcional, recomendado)
- Windows 10/11 (para compilacao WPF)

## Local Build

### 1. Restaurar dependencias

```bash
dotnet restore
```

### 2. Build Debug

```bash
dotnet build -c Debug
```

### 3. Build Release

```bash
dotnet build -c Release
```

### 4. Executar Testes

```bash
# Unit tests
dotnet test --filter "Category=Unit"

# Integration tests
dotnet test --filter "Category=Integration"

# Todos os testes
dotnet test
```

## Build por Projeto

### Phoenix.Core

```bash
cd src/Phoenix.Core
dotnet build -c Release
```

### Phoenix.UI.Wpf

```bash
cd src/Phoenix.UI.Wpf
dotnet build -c Release
```

### Phoenix.CLI

```bash
cd src/Phoenix.CLI
dotnet build -c Release
```

## Publicacao (Release)

### Self-Contained Executable

```bash
# Windows x64
dotnet publish -c Release -r win-x64 --self-contained

# Windows ARM64
dotnet publish -c Release -r win-arm64 --self-contained
```

### Framework-Dependent

```bash
dotnet publish -c Release
```

## Analise de Codigo

### Code Style (StyleCop)

```bash
dotnet format --verify-no-changes
```

Ou corrigir automaticamente:

```bash
dotnet format
```

### Analise Estatica

```bash
# Usar Roslyn Analyzers (ja configurado em .csproj)
dotnet build /p:EnforceCodeStyleInBuild=true
```

## CI/CD (GitHub Actions)

Ver `.github/workflows/build.yml` para pipeline automatizado.

### Triggers
- Push para `main` branch
- Pull Requests
- Release tags

## Troubleshooting

### Erro: "Unable to find framework .NET 8.0"

```bash
dotnet --list-sdks  # Verificar SDKs instalados
dotnet --list-runtimes  # Verificar runtimes
```

Instalar .NET 8.0: https://dotnet.microsoft.com/download

### Erro ao compilar WPF

Garantir que esta em um Windows machine. WPF nao e suportado em Linux/Mac.

### Erro de dependencias

```bash
dotnet nuget locals all --clear
dotnet restore
```

## Performance

### Parallel Build

```bash
dotnet build -m
```

### Incremental Build

O .NET ja faz rebuild incremental automaticamente.

## Versionamento

A versao e definida em `src/Phoenix.Core/Phoenix.Core.csproj`:

```xml
<Version>1.0.0</Version>
```

Para atualizar a versao:

```bash
# Editar manualmente em .csproj OU usar
dotnet build --property:Version=1.1.0
```

## Output

- Binarios: `**/bin/Release/net8.0/`
- Publicados: `**/bin/Release/net8.0/publish/`
- Packages: `*.nupkg` (se publicado no NuGet)
