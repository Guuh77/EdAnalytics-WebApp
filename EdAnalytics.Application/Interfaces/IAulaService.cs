using EdAnalytics.Application.DTOs;
using EdAnalytics.Application.ViewModels;

namespace EdAnalytics.Application.Interfaces
{
    public interface IAulaService
    {
        Task<List<AulaDto>> GetAulasPorCursoAsync(int cursoId);
        Task<AulaViewModel?> GetAulaParaEdicaoAsync(int id);
        Task<AulaDto?> GetAulaDetalhesAsync(int id);
        Task<PagedResult<AulaDto>> GetAulasPagedAsync(int cursoId, int page, int pageSize);
        Task CreateAulaAsync(AulaViewModel model);
        Task UpdateAulaAsync(AulaViewModel model);
        Task DeleteAulaAsync(int id);
    }
}
