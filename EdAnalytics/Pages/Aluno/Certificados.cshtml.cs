using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using EdAnalytics.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EdAnalytics.Pages.Aluno
{
    public class CertificadosModel : PageModel
    {
        private readonly AnalyticsDbContext _context;

        public CertificadosModel(AnalyticsDbContext context)
        {
            _context = context;
        }

        public string StudentName { get; set; } = string.Empty;
        public List<CertificateItem> Certificates { get; set; } = new();

        public class CertificateItem
        {
            public string CourseTitle { get; set; } = string.Empty;
            public string IssueDate { get; set; } = string.Empty;
            public string Workload { get; set; } = string.Empty;
            public string VerificationCode { get; set; } = string.Empty;
            public string Grade { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = Request.Cookies["jwt"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Account/Login", new { ReturnUrl = "/Aluno/Certificados" });
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    Response.Cookies.Delete("jwt");
                    return RedirectToPage("/Account/Login", new { ReturnUrl = "/Aluno/Certificados" });
                }

                var email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email")?.Value ?? "";
                StudentName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "name")?.Value ?? email;

                if (StudentName.Contains("@"))
                {
                    StudentName = StudentName.Split('@')[0].Replace(".", " ");
                    StudentName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(StudentName);
                }

                // 1. Carregar aulas assistidas do cookie
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

                // 2. Carregar todos os cursos e aulas do banco relacional
                var dbCursos = await _context.Cursos.Include(c => c.Aulas).ToListAsync();

                foreach (var curso in dbCursos)
                {
                    if (curso.Aulas == null || !curso.Aulas.Any())
                    {
                        continue;
                    }

                    int totalAulas = curso.Aulas.Count;
                    int aulasAssistidas = curso.Aulas.Count(a => watchedAulas.Contains(a.Id));

                    // Certificado só é emitido se o progresso for de 100%
                    if (aulasAssistidas == totalAulas)
                    {
                        // Gerar hash estável baseado no curso e e-mail do aluno para a nota
                        double gradeValue = 9.0 + (Math.Abs(email.GetHashCode() + curso.Id) % 11) * 0.1;
                        if (gradeValue > 10.0) gradeValue = 10.0;

                        Certificates.Add(new CertificateItem
                        {
                            CourseTitle = curso.Titulo,
                            IssueDate = DateTime.Now.ToString("dd/mm/yyyy"),
                            Workload = $"{totalAulas * 2} horas",
                            VerificationCode = $"CERT-ED-{curso.Id:D4}-{Math.Abs(email.GetHashCode() % 100000):D5}",
                            Grade = gradeValue.ToString("F1")
                        });
                    }
                }

                return Page();
            }
            catch
            {
                Response.Cookies.Delete("jwt");
                return RedirectToPage("/Account/Login", new { ReturnUrl = "/Aluno/Certificados" });
            }
        }
    }
}
