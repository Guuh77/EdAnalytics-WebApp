using EdAnalytics.Application.DTOs;

namespace EdAnalytics.Application.Interfaces
{
    public interface IAnalyticsService
    {
        Task<List<CursoDto>> GetCursosMaisVistosAsync();
    }
}