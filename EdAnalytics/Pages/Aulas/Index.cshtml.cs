using Microsoft.AspNetCore.Mvc.RazorPages;
using EdAnalytics.Application.Interfaces;
using EdAnalytics.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EdAnalytics.Pages.Aulas
{
    public class IndexModel : PageModel
    {
        private readonly IAulaService _aulaService;

        public IndexModel(IAulaService aulaService)
        {
            _aulaService = aulaService;
        }

        public List<AulaDto> Aulas { get; set; } = new List<AulaDto>();
        
        [BindProperty(SupportsGet = true)]
        public int CursoId { get; set; }

        public List<int> WatchedAulas { get; set; } = new();

        public async Task OnGetAsync()
        {
            Aulas = await _aulaService.GetAulasPorCursoAsync(CursoId);

            // Ler o progresso das aulas assistidas a partir do cookie
            var progressCookie = Request.Cookies["edanalytics_progress"];
            if (!string.IsNullOrEmpty(progressCookie))
            {
                try
                {
                    WatchedAulas = JsonSerializer.Deserialize<List<int>>(progressCookie) ?? new();
                }
                catch { }
            }
        }
    }
}
