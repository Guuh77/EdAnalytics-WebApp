using EdAnalytics.Application.DTOs;
using EdAnalytics.Application.Interfaces;

namespace EdAnalytics.Application.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly ICursoRepository _cursoRepository;
        public AnalyticsService(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<List<CursoDto>> GetCursosMaisVistosAsync()
        {
            var cursos = await _cursoRepository.GetCursosMaisAcessadosAsync();
            var cursosDto = cursos.Select(curso => new CursoDto
            {
                Id = curso.Id,
                Titulo = curso.Titulo,
                Area = curso.Area,
                Visualizacoes = curso.Visualizacoes
            }).ToList();
            return cursosDto;
        }
    }
}