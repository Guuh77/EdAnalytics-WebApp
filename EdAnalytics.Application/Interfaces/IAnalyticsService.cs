using EdAnalytics.Application.DTOs;
using EdAnalytics.Application.ViewModels;

namespace EdAnalytics.Application.Interfaces
{
    public interface IAnalyticsService
    {
        // Métodos de Leitura (Read)
        Task<List<CursoDto>> GetCursosMaisVistosAsync();
        Task<List<CursoDto>> GetAllCursosAsync();
        Task<CursoViewModel?> GetCursoParaEdicaoAsync(int id);

        // Métodos de Escrita (Create, Update, Delete)
        Task CreateCursoAsync(CursoViewModel model);
        Task UpdateCursoAsync(CursoViewModel model);
        Task DeleteCursoAsync(int id);
    }
}