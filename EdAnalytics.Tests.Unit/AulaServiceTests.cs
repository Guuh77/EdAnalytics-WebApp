using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using EdAnalytics.Application.Services;
using EdAnalytics.Application.Interfaces;
using EdAnalytics.Application.ViewModels;
using EdAnalytics.Domain;
using System.Linq;

namespace EdAnalytics.Tests.Unit
{
    public class AulaServiceTests
    {
        private readonly Mock<IAulaRepository> _aulaRepositoryMock;
        private readonly AulaService _aulaService;

        public AulaServiceTests()
        {
            _aulaRepositoryMock = new Mock<IAulaRepository>();
            _aulaService = new AulaService(_aulaRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAulasPorCursoAsync_ShouldReturnAllAulasForCourse()
        {
            // Arrange
            int cursoId = 1;
            var aulas = new List<Aula>
            {
                new Aula { Id = 1, Titulo = "Introdução", Conteudo = "Conteudo 1", CursoId = cursoId },
                new Aula { Id = 2, Titulo = "Avançado", Conteudo = "Conteudo 2", CursoId = cursoId }
            };
            _aulaRepositoryMock.Setup(repo => repo.GetAulasPorCursoAsync(cursoId))
                .ReturnsAsync(aulas);

            // Act
            var result = await _aulaService.GetAulasPorCursoAsync(cursoId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Introdução", result[0].Titulo);
            Assert.Equal("Avançado", result[1].Titulo);
            _aulaRepositoryMock.Verify(repo => repo.GetAulasPorCursoAsync(cursoId), Times.Once);
        }

        [Fact]
        public async Task GetAulasPagedAsync_ShouldReturnPagedAulas()
        {
            // Arrange
            int cursoId = 1;
            int page = 1;
            int pageSize = 2;
            var aulas = new List<Aula>
            {
                new Aula { Id = 1, Titulo = "Aula 1", Conteudo = "Conteudo 1", CursoId = cursoId },
                new Aula { Id = 2, Titulo = "Aula 2", Conteudo = "Conteudo 2", CursoId = cursoId }
            };
            _aulaRepositoryMock.Setup(repo => repo.GetPagedAsync(cursoId, page, pageSize))
                .ReturnsAsync((aulas, 5)); // 5 total items

            // Act
            var result = await _aulaService.GetAulasPagedAsync(cursoId, page, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(5, result.TotalCount);
            Assert.Equal(page, result.Page);
            Assert.Equal(pageSize, result.PageSize);
            Assert.Equal(3, result.TotalPages); // 5 / 2 = 2.5 => 3 pages
            Assert.True(result.HasNext);
            Assert.False(result.HasPrevious);
            _aulaRepositoryMock.Verify(repo => repo.GetPagedAsync(cursoId, page, pageSize), Times.Once);
        }

        [Fact]
        public async Task CreateAulaAsync_ShouldCallAddAsyncOnRepository()
        {
            // Arrange
            var viewModel = new AulaViewModel
            {
                Titulo = "Nova Aula",
                Conteudo = "Conteudo Novo",
                CursoId = 1
            };

            // Act
            await _aulaService.CreateAulaAsync(viewModel);

            // Assert
            _aulaRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Aula>(a => 
                a.Titulo == viewModel.Titulo && 
                a.Conteudo == viewModel.Conteudo && 
                a.CursoId == viewModel.CursoId)), Times.Once);
        }
    }
}
