using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EdAnalytics.Application.Interfaces;
using EdAnalytics.Application.ViewModels;

namespace EdAnalytics.Pages.Aulas
{
    public class CreateModel : PageModel
    {
        private readonly IAulaService _aulaService;

        public CreateModel(IAulaService aulaService)
        {
            _aulaService = aulaService;
        }

        [BindProperty]
        public AulaViewModel Aula { get; set; } = new AulaViewModel();

        public void OnGet(int cursoId)
        {
            Aula.CursoId = cursoId;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _aulaService.CreateAulaAsync(Aula);
            return RedirectToPage("./Index", new { cursoId = Aula.CursoId });
        }
    }
}
