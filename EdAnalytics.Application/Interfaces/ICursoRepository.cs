using EdAnalytics.Domain;

namespace EdAnalytics.Application.Interfaces
{
    public interface ICursoRepository
    {
        Task<List<Curso>> GetCursosMaisAcessadosAsync();
    }
}