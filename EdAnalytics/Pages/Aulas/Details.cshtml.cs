using Microsoft.AspNetCore.Mvc.RazorPages;
using EdAnalytics.Application.Interfaces;
using EdAnalytics.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EdAnalytics.Pages.Aulas
{
    public class DetailsModel : PageModel
    {
        private readonly IAulaService _aulaService;
        private readonly ILogAcessoService _logAcessoService;

        public DetailsModel(IAulaService aulaService, ILogAcessoService logAcessoService)
        {
            _aulaService = aulaService;
            _logAcessoService = logAcessoService;
        }

        public AulaDto? Aula { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Aula = await _aulaService.GetAulaDetalhesAsync(id);
            if (Aula == null)
            {
                return NotFound();
            }

            // 1. Gravar progresso no cookie de forma dinâmica e interativa
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

            if (!watchedAulas.Contains(id))
            {
                watchedAulas.Add(id);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Expires = DateTime.UtcNow.AddDays(30),
                    SameSite = SameSiteMode.Strict
                };
                Response.Cookies.Append("edanalytics_progress", JsonSerializer.Serialize(watchedAulas), cookieOptions);
            }

            // 2. Registrar log analítico no MongoDB (Observabilidade em tempo real!)
            try
            {
                var userToken = Request.Cookies["jwt"];
                string userEmail = "anonimo@edanalytics.com";
                if (!string.IsNullOrEmpty(userToken))
                {
                    var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(userToken);
                    userEmail = jwtToken.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email || c.Type == "email")?.Value ?? userEmail;
                }

                await _logAcessoService.RegistrarAcessoAsync(Aula.CursoId, Aula.Titulo, "Visualizacao_Aula", userEmail);
            }
            catch { }

            return Page();
        }
    }
}
