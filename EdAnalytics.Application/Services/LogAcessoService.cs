using EdAnalytics.Application.DTOs;
using EdAnalytics.Application.Interfaces;
using EdAnalytics.Domain;

namespace EdAnalytics.Application.Services
{
    public class LogAcessoService : ILogAcessoService
    {
        private readonly ILogAcessoRepository _logRepository;

        public LogAcessoService(ILogAcessoRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task RegistrarAcessoAsync(int cursoId, string cursoTitulo, string acao, string? usuarioId = null)
        {
            var log = new LogAcesso
            {
                CursoId = cursoId,
                CursoTitulo = cursoTitulo,
                Acao = acao,
                DataHora = DateTime.UtcNow,
                UsuarioId = usuarioId,
                Detalhes = new Dictionary<string, string>
                {
                    { "Timestamp", DateTime.UtcNow.ToString("O") },
                    { "Fonte", "API" }
                }
            };

            await _logRepository.RegistrarLogAsync(log);
        }

        public async Task<List<LogAcessoDto>> GetLogsPorCursoAsync(int cursoId)
        {
            var logs = await _logRepository.GetLogsPorCursoAsync(cursoId);
            return logs.Select(MapToDto).ToList();
        }

        public async Task<List<LogAcessoDto>> GetLogsRecentesAsync(int quantidade = 20)
        {
            var logs = await _logRepository.GetLogsRecentesAsync(quantidade);
            return logs.Select(MapToDto).ToList();
        }

        private static LogAcessoDto MapToDto(LogAcesso log)
        {
            return new LogAcessoDto
            {
                Id = log.Id,
                CursoId = log.CursoId,
                CursoTitulo = log.CursoTitulo,
                Acao = log.Acao,
                DataHora = log.DataHora,
                UsuarioId = log.UsuarioId,
                Detalhes = log.Detalhes
            };
        }
    }
}
