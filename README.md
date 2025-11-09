# EdAnalytics .NET (Sprint 2)

## 1. Visão Geral do Projeto

Este projeto, **EdAnalytics**, é uma plataforma de análise de dados comportamentais para ambientes de Ensino a Distância (EAD). O seu principal objetivo é resolver o paradoxo crítico do EAD moderno: o crescimento exponencial no número de matrículas acompanhado por taxas de conclusão alarmantemente baixas.

Esta aplicação web permite que administradores visualizem e gerenciem os cursos da plataforma, fornecendo a base para futuros dashboards de análise.

## 2. Arquitetura e Tecnologias

O projeto foi desenvolvido em **.NET** com **C#**, utilizando os princípios da **Clean Architecture** para garantir um código desacoplado, manutenível e testável.

### Camadas da Aplicação:

* **Domain:** Contém as entidades de negócio (ex: `Curso`).
* **Application:** Contém a lógica de negócio, ViewModels, DTOs e as interfaces (contratos) dos repositórios e serviços.
* **Infrastructure:** Implementa os contratos da Aplicação, principalmente o acesso a dados com **Entity Framework Core** (Padrão Repositório).
* **Presentation (EdAnalytics.WebApp):** Interface web construída com **ASP.NET Core Razor Pages** para exibir os dados e permitir o gerenciamento.

## 3. Funcionalidades Implementadas (Sprint 2)

* **CRUD Completo:** A aplicação agora suporta **Create, Read, Update e Delete** (CRUD) para a entidade de Cursos.
* **Validação de Dados:** Os formulários de criação e edição possuem validações robustas (tanto no lado do cliente quanto no servidor) usando `ViewModels` e `Data Annotations`.
* **Banco de Dados Permanente:** A aplicação foi migrada de um banco em memória para um banco de dados **Oracle** real, com persistência de dados e `Migrations` do EF Core.
* **Arquitetura de Serviços:** Toda a lógica de CRUD é orquestrada pela camada de `Application` (`AnalyticsService`), mantendo a camada de apresentação (Razor Pages) limpa e desacoplada.

## 4. Como Executar o Projeto

1.  **Clone o repositório.**
2.  **Abra a solução** (`EdAnalytics.WebApp.sln`) com o Visual Studio 2022.
3.  **Configure a Conexão:** Antes de executar, abra o arquivo `Program.cs` e insira suas credenciais do banco de dados Oracle na variável `connectionString`.
4.  **Execute o projeto:** Pressione **F5** para iniciar. O Visual Studio restaurará os pacotes NuGet e executará a aplicação. O EF Core garantirá que a tabela `Cursos` exista no banco Oracle.
