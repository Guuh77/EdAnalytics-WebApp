using EdAnalytics.Application.Interfaces;
using EdAnalytics.Application.Services;
using EdAnalytics.Domain;
using EdAnalytics.Infrastructure.Persistence;
using EdAnalytics.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AnalyticsDbContext>(options =>
    options.UseInMemoryDatabase("EdAnalyticsDb"));
builder.Services.AddScoped<ICursoRepository, CursoRepository>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddRazorPages();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AnalyticsDbContext>();

    context.Database.EnsureCreated();

    if (!context.Cursos.Any())
    {
        context.Cursos.AddRange(
            new Curso { Id = 1, Titulo = "C# para Iniciantes", Area = "Back-End", Visualizacoes = 1520 },
            new Curso { Id = 2, Titulo = "React: Do Zero ao Avançado", Area = "Front-End", Visualizacoes = 2890 },
            new Curso { Id = 3, Titulo = "Docker e Kubernetes", Area = "DevOps", Visualizacoes = 950 },
            new Curso { Id = 4, Titulo = "Introdução a UI/UX Design", Area = "Design", Visualizacoes = 1800 },
            new Curso { Id = 5, Titulo = "ASP.NET Core Web API", Area = "Back-End", Visualizacoes = 2150 }
        );
        context.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();