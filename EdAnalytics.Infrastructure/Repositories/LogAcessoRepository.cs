using EdAnalytics.Application.Interfaces;
using EdAnalytics.Domain;
using EdAnalytics.Infrastructure.Persistence;
using MongoDB.Driver;

namespace EdAnalytics.Infrastructure.Repositories
{
    public class LogAcessoRepository : ILogAcessoRepository
    {
        private readonly IMongoCollection<LogAcesso> _logs;

        public LogAcessoRepository(MongoDbContext context)
        {
            _logs = context.LogsAcesso;
        }

        public async Task RegistrarLogAsync(LogAcesso log)
        {
            await _logs.InsertOneAsync(log);
        }

        public async Task<List<LogAcesso>> GetLogsPorCursoAsync(int cursoId)
        {
            return await _logs.Find(l => l.CursoId == cursoId)
                .SortByDescending(l => l.DataHora)
                .ToListAsync();
        }

        public async Task<List<LogAcesso>> GetLogsRecentesAsync(int quantidade)
        {
            return await _logs.Find(_ => true)
                .SortByDescending(l => l.DataHora)
                .Limit(quantidade)
                .ToListAsync();
        }
    }
}
