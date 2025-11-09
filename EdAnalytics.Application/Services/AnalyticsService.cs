using EdAnalytics.Application.DTOs;
using EdAnalytics.Application.Interfaces;
using EdAnalytics.Application.ViewModels;
using EdAnalytics.Domain;

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

            return cursos.Select(curso => new CursoDto
            {
                Id = curso.Id,
                Titulo = curso.Titulo,
                Area = curso.Area,
                Visualizacoes = curso.Visualizacoes
            }).ToList();
        }
        public async Task<List<CursoDto>> GetAllCursosAsync()
        {
            var cursos = await _cursoRepository.GetAllAsync();
            return cursos.Select(curso => new CursoDto
            {
                Id = curso.Id,
                Titulo = curso.Titulo,
                Area = curso.Area,
                Visualizacoes = curso.Visualizacoes
            }).ToList();
        }

        public async Task<CursoViewModel?> GetCursoParaEdicaoAsync(int id)
        {
            var curso = await _cursoRepository.GetByIdAsync(id);

            if (curso == null) return null;
            return new CursoViewModel
            {
                Id = curso.Id,
                Titulo = curso.Titulo,
                Area = curso.Area,
                Visualizacoes = curso.Visualizacoes
            };
        }
        public async Task CreateCursoAsync(CursoViewModel model)
        {
            var curso = new Curso
            {
                Titulo = model.Titulo,
                Area = model.Area,
                Visualizacoes = model.Visualizacoes
            };

            await _cursoRepository.AddAsync(curso);
        }

        public async Task UpdateCursoAsync(CursoViewModel model)
        {
            var curso = await _cursoRepository.GetByIdAsync(model.Id);

            if (curso != null)
            {
                curso.Titulo = model.Titulo;
                curso.Area = model.Area;
                curso.Visualizacoes = model.Visualizacoes;

                await _cursoRepository.UpdateAsync(curso);
            }
        }

        public async Task DeleteCursoAsync(int id)
        {
            await _cursoRepository.DeleteAsync(id);
        }
    }
}