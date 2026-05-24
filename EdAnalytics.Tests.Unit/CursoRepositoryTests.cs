using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using EdAnalytics.Infrastructure.Persistence;
using EdAnalytics.Infrastructure.Repositories;
using EdAnalytics.Domain;

namespace EdAnalytics.Tests.Unit
{
    public class CursoRepositoryTests
    {
        private DbContextOptions<AnalyticsDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<AnalyticsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task AddAsync_ShouldAddCursoToDatabase()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new AnalyticsDbContext(options);
            var repository = new CursoRepository(context);
            var curso = new Curso { Titulo = "Curso Teste", Area = "Tecnologia", Visualizacoes = 10 };

            // Act
            var result = await repository.AddAsync(curso);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            var stored = await context.Cursos.FindAsync(result.Id);
            Assert.NotNull(stored);
            Assert.Equal("Curso Teste", stored.Titulo);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldReturnPagedCursos()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new AnalyticsDbContext(options);
            context.Cursos.AddRange(new List<Curso>
            {
                new Curso { Titulo = "C# Avançado", Area = "Tecnologia", Visualizacoes = 50 },
                new Curso { Titulo = "Java Básico", Area = "Tecnologia", Visualizacoes = 20 },
                new Curso { Titulo = "Design UX", Area = "Design", Visualizacoes = 15 },
                new Curso { Titulo = "Python para Dados", Area = "Dados", Visualizacoes = 100 }
            });
            await context.SaveChangesAsync();

            var repository = new CursoRepository(context);

            // Act: filter by area and paginate
            var (items, totalCount) = await repository.GetPagedAsync(1, 2, search: null, area: "Tecnologia", orderBy: "visualizacoes", descending: true);

            // Assert
            Assert.Equal(2, totalCount);
            Assert.Equal(2, items.Count);
            Assert.Equal("C# Avançado", items[0].Titulo); // 50 visualizações
            Assert.Equal("Java Básico", items[1].Titulo);  // 20 visualizações
        }
    }
}
