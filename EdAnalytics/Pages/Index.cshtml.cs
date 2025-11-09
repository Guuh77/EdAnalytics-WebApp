using EdAnalytics.Application.DTOs;
using EdAnalytics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EdAnalytics.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAnalyticsService _analyticsService;

        public IndexModel(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        public List<CursoDto> Cursos { get; set; } = new List<CursoDto>();

        public async Task OnGetAsync()
        {
            Cursos = await _analyticsService.GetAllCursosAsync();
        }
    }
}