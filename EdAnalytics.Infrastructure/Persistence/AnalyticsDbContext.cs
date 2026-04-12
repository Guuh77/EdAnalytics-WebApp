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
        public DbSet<Aula> Aulas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Aula>()
                .Property(a => a.Conteudo)
                .HasColumnType("CLOB");
        }
    }
}