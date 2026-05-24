using EdAnalytics.Domain;

namespace EdAnalytics.Application.Interfaces
{
    public interface IAulaRepository
    {
        Task<List<Aula>> GetAulasPorCursoAsync(int cursoId);
        Task<Aula?> GetByIdAsync(int id);
        Task<(List<Aula> Items, int TotalCount)> GetPagedAsync(int cursoId, int page, int pageSize);
        Task AddAsync(Aula aula);
        Task UpdateAsync(Aula aula);
        Task DeleteAsync(int id);
    }
}
