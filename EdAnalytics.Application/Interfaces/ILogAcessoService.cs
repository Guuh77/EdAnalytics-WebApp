using EdAnalytics.Application.DTOs;

namespace EdAnalytics.Application.Interfaces
{
    public interface ILogAcessoService
    {
        Task RegistrarAcessoAsync(int cursoId, string cursoTitulo, string acao, string? usuarioId = null);
        Task<List<LogAcessoDto>> GetLogsPorCursoAsync(int cursoId);
        Task<List<LogAcessoDto>> GetLogsRecentesAsync(int quantidade = 20);
    }
}
