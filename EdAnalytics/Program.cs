using EdAnalytics.Application.Interfaces;
using EdAnalytics.Application.Services;
using EdAnalytics.Infrastructure.Persistence;
using EdAnalytics.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration));

string connectionString = "User Id=rm561055;Password=150206;Data Source=oracle.fiap.com.br/orcl";

// OpenTelemetry Tracing e Metrics
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("EdAnalytics.WebApp"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter());

// Configurar HealthChecks
builder.Services.AddHealthChecks()
    .AddCheck("Self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy())
    .AddOracle(connectionString, name: "OracleDb");

builder.Services.AddDbContext<AnalyticsDbContext>(options =>
    options.UseOracle(connectionString)
);
builder.Services.AddScoped<ICursoRepository, CursoRepository>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IAulaRepository, AulaRepository>();
builder.Services.AddScoped<IAulaService, AulaService>();
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        });
        await context.Response.WriteAsync(result);
    }
});

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AnalyticsDbContext>();
        context.Database.EnsureCreated();
        AnalyticsDataSeeder.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro no Database Seeder.");
    }
}

app.Run();

public partial class Program { }