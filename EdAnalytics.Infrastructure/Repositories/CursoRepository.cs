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
        public async Task<List<Curso>> GetAllAsync()
        {
            return await _context.Cursos.ToListAsync();
        }

        public async Task<Curso?> GetByIdAsync(int id)
        {
            return await _context.Cursos.FindAsync(id);
        }
        public async Task<Curso> AddAsync(Curso curso)
        {
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();
            return curso;
        }

        public async Task UpdateAsync(Curso curso)
        {
            _context.Entry(curso).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso != null)
            {
                _context.Cursos.Remove(curso);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<(List<Curso> Items, int TotalCount)> GetPagedAsync(
            int page, int pageSize, string? search = null, string? area = null,
            string? orderBy = null, bool descending = false)
        {
            var query = _context.Cursos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c => c.Titulo.Contains(search));

            if (!string.IsNullOrWhiteSpace(area))
                query = query.Where(c => c.Area == area);

            query = orderBy?.ToLower() switch
            {
                "titulo" => descending ? query.OrderByDescending(c => c.Titulo) : query.OrderBy(c => c.Titulo),
                "area" => descending ? query.OrderByDescending(c => c.Area) : query.OrderBy(c => c.Area),
                "visualizacoes" => descending ? query.OrderByDescending(c => c.Visualizacoes) : query.OrderBy(c => c.Visualizacoes),
                _ => descending ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id)
            };

            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, totalCount);
        }
    }
}