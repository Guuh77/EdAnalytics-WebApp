using EdAnalytics.Application.Interfaces;
using EdAnalytics.Application.Services;
using EdAnalytics.Infrastructure.Persistence;
using EdAnalytics.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connectionString = "User Id=rm561055;Password=150206;Data Source=oracle.fiap.com.br/orcl";
builder.Services.AddDbContext<AnalyticsDbContext>(options =>
    options.UseOracle(connectionString)
);
builder.Services.AddScoped<ICursoRepository, CursoRepository>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddRazorPages();

var app = builder.Build();
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