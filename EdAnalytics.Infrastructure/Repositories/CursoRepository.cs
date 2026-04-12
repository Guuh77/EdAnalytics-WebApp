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
    }
}