-- Project Phoenix Desktop - Knowledge Base Schema
-- SQLite Database with 5 Knowledge Domains
-- Version: 1.0.0

-- ============================================
-- CORE TABLES
-- ============================================

-- Main knowledge items table
CREATE TABLE IF NOT EXISTS KnowledgeItems (
    Id TEXT PRIMARY KEY,
    Domain TEXT NOT NULL,
    Title TEXT NOT NULL,
    Content TEXT NOT NULL,
    Tags TEXT,
    Priority INTEGER DEFAULT 1,
    IsActive INTEGER DEFAULT 1,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    ViewCount INTEGER DEFAULT 0,
    Relevance REAL DEFAULT 0.5
);

-- Domain-specific storage (Psicologia)
CREATE TABLE IF NOT EXISTS PsicologiaHumana (
    Id TEXT PRIMARY KEY,
    Categoria TEXT NOT NULL,
    Conceito TEXT NOT NULL,
    Descricao TEXT NOT NULL,
    Aplicacoes TEXT,
    Referencias TEXT,
    Nivel TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Domain-specific storage (Mecanica Automotiva)
CREATE TABLE IF NOT EXISTS MecanicaAutomotiva (
    Id TEXT PRIMARY KEY,
    Tipo TEXT NOT NULL,
    Sistema TEXT NOT NULL,
    Descricao TEXT NOT NULL,
    Diagnostico TEXT,
    Manutencao TEXT,
    Ferramentas TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Domain-specific storage (Programacao)
CREATE TABLE IF NOT EXISTS Programacao (
    Id TEXT PRIMARY KEY,
    Linguagem TEXT NOT NULL,
    Topico TEXT NOT NULL,
    Descricao TEXT NOT NULL,
    CodigoExemplo TEXT,
    MelhoresPraticas TEXT,
    Complexidade TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Domain-specific storage (Engenharia de Prompts)
CREATE TABLE IF NOT EXISTS EngenhariadePrompts (
    Id TEXT PRIMARY KEY,
    Tecnica TEXT NOT NULL,
    Descricao TEXT NOT NULL,
    PromptExemplo TEXT NOT NULL,
    ResultadoEsperado TEXT,
    CasosdeUso TEXT,
    Eficacia REAL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Domain-specific storage (Engenharia de Software)
CREATE TABLE IF NOT EXISTS EngenhariaSoftware (
    Id TEXT PRIMARY KEY,
    Padrao TEXT NOT NULL,
    Descricao TEXT NOT NULL,
    Implementacao TEXT,
    Vantagens TEXT,
    Desvantagens TEXT,
    CasosdoUso TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Indexes for performance
CREATE INDEX IF NOT EXISTS idx_knowledge_domain ON KnowledgeItems(Domain);
CREATE INDEX IF NOT EXISTS idx_knowledge_active ON KnowledgeItems(IsActive);
CREATE INDEX IF NOT EXISTS idx_knowledge_tags ON KnowledgeItems(Tags);
CREATE INDEX IF NOT EXISTS idx_psicologia_categoria ON PsicologiaHumana(Categoria);
CREATE INDEX IF NOT EXISTS idx_mecanica_sistema ON MecanicaAutomotiva(Sistema);
CREATE INDEX IF NOT EXISTS idx_programacao_linguagem ON Programacao(Linguagem);
CREATE INDEX IF NOT EXISTS idx_prompt_tecnica ON EngenhariadePrompts(Tecnica);
CREATE INDEX IF NOT EXISTS idx_software_padrao ON EngenhariaSoftware(Padrao);

-- ============================================
-- INITIAL DATA - PSICOLOGIA HUMANA
-- ============================================

INSERT INTO PsicologiaHumana (Id, Categoria, Conceito, Descricao, Aplicacoes, Referencias, Nivel) VALUES
('PSI_001', 'Cognitivismo', 'Teoria Cognitiva', 'Abordagem que estuda como a mente processa informacoes', 'Terapia comportamental, Educacao, Coaching', 'Aaron Beck, Albert Ellis', 'Intermediario'),
('PSI_002', 'Emocoes', 'Inteligencia Emocional', 'Capacidade de reconhecer e gerenciar emocoes proprias e alheias', 'Lideranca, Relacionamentos, Autoconhecimento', 'Daniel Goleman', 'Avancado'),
('PSI_003', 'Comportamento', 'Condicionar Operante', 'Modificacao do comportamento atraves de consequencias', 'Treinamento, Motivacao, Habitos', 'B.F. Skinner', 'Intermediario'),
('PSI_004', 'Aprendizagem', 'Zona Proximal de Desenvolvimento', 'Distancia entre o que se pode fazer sozinho e com ajuda', 'Ensino, Desenvolvimento, Coaching', 'Lev Vygotsky', 'Intermediario'),
('PSI_005', 'Personalidade', 'Modelo Big Five', 'Cinco dimensoes principais da personalidade humana', 'Recrutamento, Desenvolvimento Pessoal, Relacionamentos', 'McCrae e Costa', 'Basico');

-- ============================================
-- INITIAL DATA - MECANICA AUTOMOTIVA
-- ============================================

INSERT INTO MecanicaAutomotiva (Id, Tipo, Sistema, Descricao, Diagnostico, Manutencao, Ferramentas) VALUES
('AUTO_001', 'Manutencao', 'Motor', 'Peca responsavel pela conversao de combustivel em energia', 'Verificar óleo, Ouvir barulhos anormais', 'Trocar oleo a cada 10000km, Verificar filtros', 'Chave inglesa, Jarreteira'),
('AUTO_002', 'Diagnostico', 'Transmissao', 'Sistema que transmite potencia do motor as rodas', 'Verificar vazamentos, Testar engates', 'Trocar oleo de transmissao, Verificar correntes', 'Scanner OBD, Chave de fenda'),
('AUTO_003', 'Manutencao', 'Freios', 'Sistema de reducao de velocidade e parada', 'Testar resposta dos pedais, Medir espessura das pastilhas', 'Trocar pastilhas, Sangrar sistema', 'Chave Phillips, Cilindro de vácuo'),
('AUTO_004', 'Diagnostico', 'Suspensao', 'Sistema que absorve impactos e mante estabilidade', 'Verificar desgaste dos pneus, Testar molas', 'Alinhar rodas, Trocar amortecedores', 'Alinhador de rodas, Molas helicoidais'),
('AUTO_005', 'Manutencao', 'Eletrica', 'Sistema de geracao e distribuicao de energia', 'Testar bateria, Verificar alternador', 'Carregar bateria, Trocar correia de distribuicao', 'Multimetro, Carregador');

-- ============================================
-- INITIAL DATA - PROGRAMACAO
-- ============================================

INSERT INTO Programacao (Id, Linguagem, Topico, Descricao, CodigoExemplo, MelhoresPraticas, Complexidade) VALUES
('PROG_001', 'C#', 'LINQ', 'Language Integrated Query para consultas em colecoes', 'var pares = numeros.Where(n => n % 2 == 0).ToList();', 'Prefer ReadOnly, Use method syntax', 'Intermediario'),
('PROG_002', 'C#', 'Async/Await', 'Programacao assincrona para operacoes I/O', 'await Task.Delay(1000);', 'Sempre retornar Task, Evitar .Result', 'Avancado'),
('PROG_003', 'JavaScript', 'Promises', 'Manipular operacoes assinconas em JS', 'return fetch(url).then(r => r.json());', 'Sempre adicionar .catch(), Use async/await', 'Intermediario'),
('PROG_004', 'Python', 'List Comprehension', 'Sintaxe concisa para criar listas', '[x*2 for x in range(10)]', 'Mantenha legivelidade, Nao abuse', 'Basico'),
('PROG_005', 'C#', 'Dependency Injection', 'Padrao de injecao de dependencias', 'services.AddSingleton<IService, Service>();', 'Use interfaces, Registre no DI container', 'Avancado');

-- ============================================
-- INITIAL DATA - ENGENHARIA DE PROMPTS
-- ============================================

INSERT INTO EngenhariadePrompts (Id, Tecnica, Descricao, PromptExemplo, ResultadoEsperado, CasosdeUso, Eficacia) VALUES
('PROMPT_001', 'Chain of Thought', 'Pedir ao modelo que pense passo a passo', 'Resuelve este problema paso a paso: 123 + 456', 'Raciocinio detalhado e acurado', 'Problemas complexos, Matematica', 0.85),
('PROMPT_002', 'Few-Shot Learning', 'Fornecer exemplos para o modelo aprender padroes', 'Exemplo: gato -> animal. Cachorro -> ?', 'Resposta: animal', 'Classificacao, Pattern matching', 0.78),
('PROMPT_003', 'Role Playing', 'Atribuir um papel ao modelo para gerar conteudo', 'Voce eh um especialista em Python. Como otimizar...', 'Resposta especializada e contextual', 'Tutoriais, Explicacoes tecnicas', 0.82),
('PROMPT_004', 'Prompt Chaining', 'Usar saida de um prompt como entrada de outro', 'Prompt 1: Resuma. Prompt 2: Traduza o resumo', 'Resultados mais refinados e precisos', 'Processamento multi-etapa', 0.88),
('PROMPT_005', 'Negative Prompting', 'Especificar o que NAO fazer', 'Explique conceito de IA, NAO mencione ChatGPT', 'Resposta focada, sem mencoes irrelevantes', 'Controle de conteudo', 0.72);

-- ============================================
-- INITIAL DATA - ENGENHARIA DE SOFTWARE
-- ============================================

INSERT INTO EngenhariaSoftware (Id, Padrao, Descricao, Implementacao, Vantagens, Desvantagens, CasosdoUso) VALUES
('SOFT_001', 'MVC', 'Model-View-Controller para separacao de responsabilidades', 'Separar dados (Model), Interface (View), Logica (Controller)', 'Separacao clara, Testable, Mantenivel', 'Complexidade inicial, Overhead', 'Aplicacoes web, Desktop apps'),
('SOFT_002', 'Repository Pattern', 'Abstrai acesso a dados, permitindo trocar implementacao', 'Interface IRepository<T>, Implementacoes concretas', 'Desacoplamento, Testavel, Flexivel', 'Mais codigo, Maior complexidade', 'Aplicacoes com multiplos datastores'),
('SOFT_003', 'Singleton', 'Garante uma unica instancia de uma classe', 'private constructor + static instance', 'Controle de recurso, Simplicidade', 'Dificil testar, Problemas com threading', 'Logger, Database connection'),
('SOFT_004', 'Factory Pattern', 'Cria objetos sem especificar exatamente suas classes', 'Factory method que retorna tipo abstrato', 'Desacoplamento, Flexibilidade', 'Complexidade adicional', 'Criacao de famílias de objetos'),
('SOFT_005', 'SOLID Principles', 'Cinco principios para design de software robusto', 'Single, Open/Closed, Liskov, Interface Segregation, Dependency Inversion', 'Codigo limpo, Mantenivel, Escalavel', 'Curva de aprendizado', 'Qualquer projeto serio');

-- ============================================
-- TRIGGERS - Auto-update UpdatedAt
-- ============================================

CREATE TRIGGER IF NOT EXISTS update_knowledge_timestamp
AFTER UPDATE ON KnowledgeItems
BEGIN
  UPDATE KnowledgeItems SET UpdatedAt = CURRENT_TIMESTAMP WHERE Id = NEW.Id;
END;

-- ============================================
-- VIEWS - Useful Queries
-- ============================================

CREATE VIEW IF NOT EXISTS ActiveKnowledge AS
SELECT * FROM KnowledgeItems WHERE IsActive = 1 ORDER BY UpdatedAt DESC;

CREATE VIEW IF NOT EXISTS PopularKnowledge AS
SELECT * FROM KnowledgeItems WHERE ViewCount > 5 ORDER BY ViewCount DESC;

CREATE VIEW IF NOT EXISTS KnowledgeByDomain AS
SELECT Domain, COUNT(*) as TotalItems, AVG(Relevance) as AvgRelevance
FROM KnowledgeItems
GROUP BY Domain;
