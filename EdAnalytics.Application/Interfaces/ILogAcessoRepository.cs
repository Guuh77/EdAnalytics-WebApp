using EdAnalytics.Domain;

namespace EdAnalytics.Application.Interfaces
{
    public interface ILogAcessoRepository
    {
        Task RegistrarLogAsync(LogAcesso log);
        Task<List<LogAcesso>> GetLogsPorCursoAsync(int cursoId);
        Task<List<LogAcesso>> GetLogsRecentesAsync(int quantidade);
    }
}
