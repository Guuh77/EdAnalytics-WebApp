using Microsoft.AspNetCore.Mvc.RazorPages;
using EdAnalytics.Application.Interfaces;
using EdAnalytics.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EdAnalytics.Pages.Aulas
{
    public class DetailsModel : PageModel
    {
        private readonly IAulaService _aulaService;

        public DetailsModel(IAulaService aulaService)
        {
            _aulaService = aulaService;
        }

        public AulaDto? Aula { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Aula = await _aulaService.GetAulaDetalhesAsync(id);
            if (Aula == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
