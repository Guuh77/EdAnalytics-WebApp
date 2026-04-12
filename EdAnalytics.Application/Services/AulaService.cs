using EdAnalytics.Application.DTOs;
using EdAnalytics.Application.Interfaces;
using EdAnalytics.Application.ViewModels;
using EdAnalytics.Domain;

namespace EdAnalytics.Application.Services
{
    public class AulaService : IAulaService
    {
        private readonly IAulaRepository _aulaRepository;

        public AulaService(IAulaRepository aulaRepository)
        {
            _aulaRepository = aulaRepository;
        }

        public async Task<List<AulaDto>> GetAulasPorCursoAsync(int cursoId)
        {
            var aulas = await _aulaRepository.GetAulasPorCursoAsync(cursoId);
            return aulas.Select(a => new AulaDto
            {
                Id = a.Id,
                Titulo = a.Titulo,
                Conteudo = a.Conteudo,
                CursoId = a.CursoId
            }).ToList();
        }

        public async Task<AulaViewModel?> GetAulaParaEdicaoAsync(int id)
        {
            var aula = await _aulaRepository.GetByIdAsync(id);
            if (aula == null) return null;

            return new AulaViewModel
            {
                Id = aula.Id,
                Titulo = aula.Titulo,
                Conteudo = aula.Conteudo,
                CursoId = aula.CursoId
            };
        }

        public async Task<AulaDto?> GetAulaDetalhesAsync(int id)
        {
            var aula = await _aulaRepository.GetByIdAsync(id);
            if (aula == null) return null;

            return new AulaDto
            {
                Id = aula.Id,
                Titulo = aula.Titulo,
                Conteudo = aula.Conteudo,
                CursoId = aula.CursoId
            };
        }

        public async Task CreateAulaAsync(AulaViewModel model)
        {
            var aula = new Aula
            {
                Titulo = model.Titulo,
                Conteudo = model.Conteudo,
                CursoId = model.CursoId
            };
            await _aulaRepository.AddAsync(aula);
        }

        public async Task UpdateAulaAsync(AulaViewModel model)
        {
            var aula = await _aulaRepository.GetByIdAsync(model.Id);
            if (aula != null)
            {
                aula.Titulo = model.Titulo;
                aula.Conteudo = model.Conteudo;
                
                await _aulaRepository.UpdateAsync(aula);
            }
        }

        public async Task DeleteAulaAsync(int id)
        {
            await _aulaRepository.DeleteAsync(id);
        }
    }
}
