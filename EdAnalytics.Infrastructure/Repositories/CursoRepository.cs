using EdAnalytics.Application.Interfaces;
using EdAnalytics.Domain;
using EdAnalytics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EdAnalytics.Infrastructure.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly AnalyticsDbContext _context;

        public CursoRepository(AnalyticsDbContext context)
        {
            _context = context;
        }

        public async Task<List<Curso>> GetCursosMaisAcessadosAsync()
        {
            return await _context.Cursos
                .OrderByDescending(c => c.Visualizacoes)
                .ToListAsync();
        }
    }
}