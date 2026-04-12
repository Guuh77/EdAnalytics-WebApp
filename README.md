# EdAnalytics .NET (Sprints 2 e 3)

## 1. Visão Geral do Projeto

Este projeto, **EdAnalytics**, é uma plataforma premium de análise de dados educacionais focada em métricas profundas e observabilidade para ambientes de Ensino a Distância (EAD). O seu principal objetivo é resolver o paradoxo crítico do EAD moderno: o crescimento exponencial no número de matrículas acompanhado por taxas de evasão e de conclusão alarmantemente baixas.

Esta aplicação web permite que administradores e professores visualizem e gerenciem os **Cursos** e suas respectivas **Aulas** de maneira interativa, compondo a base de dados robusta para os sistemas de Analytics e BI.

## 2. Padrões de Interface (UX/UI)
Na evolução deste sistema, abandonamos layouts genéricos em prol de um design altamente capacitado focado em engajamento:
* **Tema Dark e Vinho:** Uso absoluto de **Dark Mode Nativo** com detalhes luxuosos em **Vinho (`#8e1b34`)**, favorecendo leitura profunda que não cansa aos olhos do estudante.
* **Glassmorphism e Micro-interações:** Navegação superior (Navbar) translúcida que mescla com o plano de fundo. Botões responsivos que flutuam (`translateY`) em interações de hover, gerando dinamismo vivo na tela.
* **Tipografia Moderna:** Incorporação de Google Fonts (`Outfit`) substituindo tipos serifados monótonos.
* **Apresentação em Landing Page:** Página "Home" principal criada do zero detalhando claramente a missão do ecossistema e levando o usuário à tela administrativa (Painel de Cursos).

## 3. Arquitetura e Tecnologias (Visão Back-end)

O projeto foi desenvolvido em **.NET 8** com **C#**, utilizando estritamente os princípios da **Clean Architecture** para garantir um código desacoplado, manutenível e altamente testável em larga escala.

### Camadas da Aplicação:

* **Domain:** Contém as entidades de negócio persistentes (ex: `Curso` e `Aula` interligadas num esquema *1:N*).
* **Application:** Contém a lógica de negócio, Transferências de Dados (DTOs/ViewModels) e as interfaces/contratos dos repositórios nativos e serviços (ex: `AulasService`).
* **Infrastructure:** Implementa os contratos da Aplicação de ponta a ponta, sendo protagonista no acesso a grandes volumes de dados através do **Entity Framework Core** num fluxo de **Padrão Repositório**.
* **Presentation (EdAnalytics.WebApp):** Interface web construída com **ASP.NET Core Razor Pages**, encarregada de exibir e orquestrar as rotas diretas dos Painéis.

## 4. Estrutura de Funcionalidades
A infraestrutura cresceu significativamente desde a Sprint 2, garantindo agora um esquema massivo relacional.

### O Poder do Módulo de Conteúdo (Database Seeding) 🚀
Foi programado e configurado o processo de **Data Seeding Inteligente**. Ao iniciar a aplicação (Seeding via EF Core), o banco de dados Oracle é populado de forma automatizada com Cursos fictícios dinâmicos (Ex: _Gestão Ágil_ e _Filosofia Corinthiana_).
- Suporte a grandes cargas de Texto: Substituiu-se limitações convencionais pelo processamento do tipo de dado **CLOB**, capaz de absorver textos gigantescos sem sobrecarregar a memória do ORM. O sistema já cria 5 Cursos com exatas 3 Aulas ultra detalhadas, cada um compondo cenários acadêmicos humorísticos e educativos incríveis.

## 5. Monitoramento, Observabilidade e Testes (Trilha Sprint 3)

Nesta sprint conclusiva, elevamos a qualidade e disponibilidade de falhas arquiteturais implantando novos módulos críticos da vida real:

* **Health Checks Dinâmicos:** A aplicação foi munida com múltiplos endpoints rastreáveis acessíveis em `/health`. Eles verificam o estado nativo da aplicação local (`Self`) e efetuam ping simultâneo atestando a conexão forte com o Banco Oracle externo, devolvendo payloads no formato JSON prontas pro consumo.
* **Logging Estruturado e Eficiente:** Implantamos o ecossistema **Serilog** substituindo inteiramente o ineficaz Logger nativo do ASP.NET. Ele intercepta as requisições complexas, detalha a correlação em tela no Console, formata pacotes claros e, vitalmente, preserva arquivos contínuos localmente na raiz do projeto diariamente (`/logs`).
* **Application Tracing e Métricas:** Todo o tráfego HTTP e atividades densas que trafegamos com o Entity Framework são empacotadas via rastreio de **OpenTelemetry**, exportando métricas precisas (ex: tempo em milissegundos nas renderizações Razor) para a linha de frente do desenvolvimento (Distributed Tracing).
* **Testes Automatizados Profundos (Padrão AAA):** Implantou-se e reforçou-se o framework de garantias divididos em duas facetas brutais:
  - `Unitários`: Utilizando exclusividade da junção **Moq + xUnit**, as checagens isolam e garantem as resoluções numéricas dos nossos Services de Negócios sem acessar bancos reais.
  - `Integração`: Acionam todos os endpoints REST de CRUD sem simulação abstrata, usando o poderoso sistema `WebApplicationFactory`. Ele carrega injetores puros em memória a fim de varrer toda a jornada de experiência sem macular o Oracle Oficial de produção.

## 6. Como Executar o Projeto Passo a Passo

1.  **Clone o repositório** para a pasta principal de arquivos desejada.
2.  Invoque o terminal do diretório e lance a execução, a injeção em massa dos Cursos automáticos será gerada a tempo de runtime:
```bash
cd EdAnalytics
dotnet run
```
3.  **Para acompanhar Painel Front-end:** Assim que iniciar, vá ao navegador via HTTPS: `https://localhost:<porta>/`(Normalmente a 5187).
4.  **Para acompanhar Integridade e Observabilidade:** Acesse `https://localhost:<porta>/health` para testar ao vivo as métricas.
5.  **Para validar as Qualidades de Código do xUnit:** Em novo terminal (Na Raiz), garanta a solidez com o comando:
```bash
dotnet test
```
