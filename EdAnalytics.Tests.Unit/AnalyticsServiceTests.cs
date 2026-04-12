using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using EdAnalytics.Application.Services;
using EdAnalytics.Application.Interfaces;
using EdAnalytics.Application.ViewModels;
using EdAnalytics.Domain;

namespace EdAnalytics.Tests.Unit
{
    public class AnalyticsServiceTests
    {
        private readonly Mock<ICursoRepository> _cursoRepositoryMock;
        private readonly AnalyticsService _analyticsService;

        public AnalyticsServiceTests()
        {
            _cursoRepositoryMock = new Mock<ICursoRepository>();
            _analyticsService = new AnalyticsService(_cursoRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllCursosAsync_WhenChamado_RetornaTodosOsCursosComoDto()
        {
            // Arrange
            var cursosNoBanco = new List<Curso>
            {
                new Curso { Id = 1, Titulo = "Curso A", Area = "Tecnologia", Visualizacoes = 100 },
                new Curso { Id = 2, Titulo = "Curso B", Area = "Humanas", Visualizacoes = 50 }
            };
            _cursoRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(cursosNoBanco);

            // Act
            var result = await _analyticsService.GetAllCursosAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Curso A", result[0].Titulo);
            _cursoRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCursoParaEdicaoAsync_QuandoIdValido_RetornaViewModel()
        {
            // Arrange
            int cursoId = 1;
            var curso = new Curso { Id = cursoId, Titulo = "Curso Teste", Area = "Area Teste", Visualizacoes = 10 };
            _cursoRepositoryMock.Setup(repo => repo.GetByIdAsync(cursoId)).ReturnsAsync(curso);

            // Act
            var result = await _analyticsService.GetCursoParaEdicaoAsync(cursoId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cursoId, result.Id);
            Assert.Equal("Curso Teste", result.Titulo);
        }

        [Fact]
        public async Task GetCursoParaEdicaoAsync_QuandoIdInvalido_RetornaNull()
        {
            // Arrange
            int cursoId = 99;
            _cursoRepositoryMock.Setup(repo => repo.GetByIdAsync(cursoId)).ReturnsAsync((Curso)null);

            // Act
            var result = await _analyticsService.GetCursoParaEdicaoAsync(cursoId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateCursoAsync_ComModeloValido_ChamaRepositorioAdd()
        {
            // Arrange
            var viewModel = new CursoViewModel { Titulo = "Novo Curso", Area = "Exatas", Visualizacoes = 0 };

            // Act
            await _analyticsService.CreateCursoAsync(viewModel);

            // Assert
            _cursoRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Curso>(c => c.Titulo == viewModel.Titulo && c.Area == viewModel.Area)), Times.Once);
        }
        
        [Fact]
        public async Task UpdateCursoAsync_ComCursoExistente_AtualizaPropriedades()
        {
            // Arrange
            var cursoNoBanco = new Curso { Id = 1, Titulo = "Antigo", Area = "Antiga", Visualizacoes = 0 };
            var viewModelAtualizado = new CursoViewModel { Id = 1, Titulo = "Novo", Area = "Nova", Visualizacoes = 5 };
            
            _cursoRepositoryMock.Setup(repo => repo.GetByIdAsync(viewModelAtualizado.Id)).ReturnsAsync(cursoNoBanco);

            // Act
            await _analyticsService.UpdateCursoAsync(viewModelAtualizado);

            // Assert
            Assert.Equal("Novo", cursoNoBanco.Titulo);
            _cursoRepositoryMock.Verify(repo => repo.UpdateAsync(cursoNoBanco), Times.Once);
        }

        [Fact]
        public async Task DeleteCursoAsync_QuandoChamado_ChamaRepositorioDelete()
        {
            // Arrange
            int idParaDeletar = 1;

            // Act
            await _analyticsService.DeleteCursoAsync(idParaDeletar);

            // Assert
            _cursoRepositoryMock.Verify(repo => repo.DeleteAsync(idParaDeletar), Times.Once);
        }
    }
}
