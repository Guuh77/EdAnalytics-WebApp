using EdAnalytics.Application.Interfaces;
using EdAnalytics.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EdAnalytics.Pages
{
    public class EditModel : PageModel
    {
        private readonly IAnalyticsService _analyticsService;

        public EditModel(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [BindProperty]
        public CursoViewModel Curso { get; set; } = new CursoViewModel();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var cursoParaEdicao = await _analyticsService.GetCursoParaEdicaoAsync(id.Value);

            if (cursoParaEdicao == null) return NotFound();

            Curso = cursoParaEdicao;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _analyticsService.UpdateCursoAsync(Curso);

            return RedirectToPage("./Index");
        }
    }
}