using EdAnalytics.Application.Interfaces;
using EdAnalytics.Domain;
using EdAnalytics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EdAnalytics.Infrastructure.Repositories
{
    public class AulaRepository : IAulaRepository
    {
        private readonly AnalyticsDbContext _context;

        public AulaRepository(AnalyticsDbContext context)
        {
            _context = context;
        }

        public async Task<List<Aula>> GetAulasPorCursoAsync(int cursoId)
        {
            return await _context.Aulas
                .Where(a => a.CursoId == cursoId)
                .ToListAsync();
        }

        public async Task<Aula?> GetByIdAsync(int id)
        {
            return await _context.Aulas.FindAsync(id);
        }

        public async Task AddAsync(Aula aula)
        {
            await _context.Aulas.AddAsync(aula);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Aula aula)
        {
            _context.Aulas.Update(aula);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var aula = await GetByIdAsync(id);
            if (aula != null)
            {
                _context.Aulas.Remove(aula);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<(List<Aula> Items, int TotalCount)> GetPagedAsync(int cursoId, int page, int pageSize)
        {
            var query = _context.Aulas.Where(a => a.CursoId == cursoId);
            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(a => a.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
