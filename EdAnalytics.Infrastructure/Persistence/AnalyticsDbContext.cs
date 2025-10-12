using EdAnalytics.Domain;
using Microsoft.EntityFrameworkCore;

namespace EdAnalytics.Infrastructure.Persistence
{
    public class AnalyticsDbContext : DbContext
    {
        public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Curso> Cursos { get; set; }
    }
}