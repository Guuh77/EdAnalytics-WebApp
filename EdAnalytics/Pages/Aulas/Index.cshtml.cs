using Microsoft.AspNetCore.Mvc.RazorPages;
using EdAnalytics.Application.Interfaces;
using EdAnalytics.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

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

        public async Task OnGetAsync()
        {
            Aulas = await _aulaService.GetAulasPorCursoAsync(CursoId);
        }
    }
}
