using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using EdAnalytics.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EdAnalytics.Pages.Aluno
{
    public class PerfilModel : PageModel
    {
        private readonly AnalyticsDbContext _context;

        public PerfilModel(AnalyticsDbContext context)
        {
            _context = context;
        }

        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        // Dynamic Progress Analytics for Student Dashboard
        public int CompletedCoursesCount { get; set; }
        public int TotalStudyHours { get; set; }
        public int TotalCertificatesEarned { get; set; }
        public double CompletionRatePercentage { get; set; }

        public List<ProgressItem> RecentActivities { get; set; } = new();

        public class ProgressItem
        {
            public int CourseId { get; set; }
            public string CourseName { get; set; } = string.Empty;
            public int ProgressPercentage { get; set; }
            public string LastAccess { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = Request.Cookies["jwt"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Account/Login", new { ReturnUrl = "/Aluno/Perfil" });
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    Response.Cookies.Delete("jwt");
                    return RedirectToPage("/Account/Login", new { ReturnUrl = "/Aluno/Perfil" });
                }

                Email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email")?.Value ?? "estudante@edanalytics.com";
                Role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value ?? "Aluno";
                Name = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "name")?.Value ?? Email;

                if (Name.Contains("@"))
                {
                    Name = Name.Split('@')[0].Replace(".", " ");
                    Name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Name);
                }

                // 1. Carregar ID das aulas assistidas a partir do cookie
                var progressCookie = Request.Cookies["edanalytics_progress"];
                List<int> watchedAulas = new();
                if (!string.IsNullOrEmpty(progressCookie))
                {
                    try
                    {
                        watchedAulas = JsonSerializer.Deserialize<List<int>>(progressCookie) ?? new();
                    }
                    catch { }
                }

                // 2. Carregar todos os cursos com suas respectivas aulas do banco relacional
                var dbCursos = await _context.Cursos.Include(c => c.Aulas).ToListAsync();

                CompletedCoursesCount = 0;
                TotalStudyHours = watchedAulas.Count * 2; // Cada aula assistida soma 2 horas de estudo
                TotalCertificatesEarned = 0;

                int totalCursosComAulas = 0;
                double somaPercentuais = 0;

                foreach (var curso in dbCursos)
                {
                    if (curso.Aulas == null || !curso.Aulas.Any())
                    {
                        continue;
                    }

                    totalCursosComAulas++;
                    int totalAulas = curso.Aulas.Count;
                    int aulasAssistidas = curso.Aulas.Count(a => watchedAulas.Contains(a.Id));

                    int percentual = (int)Math.Round((double)aulasAssistidas / totalAulas * 100);
                    somaPercentuais += percentual;

                    // Adicionar ao painel se o aluno já tiver iniciado (assistido pelo menos uma aula)
                    if (aulasAssistidas > 0)
                    {
                        RecentActivities.Add(new ProgressItem
                        {
                            CourseId = curso.Id,
                            CourseName = curso.Titulo,
                            ProgressPercentage = percentual,
                            LastAccess = percentual == 100 ? "Concluído" : "Recentemente"
                        });
                    }

                    if (percentual == 100)
                    {
                        CompletedCoursesCount++;
                        TotalCertificatesEarned++;
                    }
                }

                CompletionRatePercentage = totalCursosComAulas > 0 
                    ? Math.Round(somaPercentuais / totalCursosComAulas, 1) 
                    : 0;

                return Page();
            }
            catch
            {
                Response.Cookies.Delete("jwt");
                return RedirectToPage("/Account/Login", new { ReturnUrl = "/Aluno/Perfil" });
            }
        }
    }
}
