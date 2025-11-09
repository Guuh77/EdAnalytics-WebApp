using EdAnalytics.Domain;

namespace EdAnalytics.Application.Interfaces
{
    public interface ICursoRepository
    {
        // metodos de Leitura (Read)
        Task<List<Curso>> GetCursosMaisAcessadosAsync();
        Task<List<Curso>> GetAllAsync();
        Task<Curso?> GetByIdAsync(int id);

        // metodos de Escrita (Create, Update, Delete)
        Task<Curso> AddAsync(Curso curso);
        Task UpdateAsync(Curso curso);
        Task DeleteAsync(int id);
    }
}