using EdAnalytics.Application.Interfaces;
using EdAnalytics.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EdAnalytics.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IAnalyticsService _analyticsService;

        public DeleteModel(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [BindProperty]
        public CursoViewModel Curso { get; set; } = new CursoViewModel();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var cursoParaDeletar = await _analyticsService.GetCursoParaEdicaoAsync(id.Value);

            if (cursoParaDeletar == null) return NotFound();

            Curso = cursoParaDeletar;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();
            await _analyticsService.DeleteCursoAsync(id.Value);

            return RedirectToPage("./Index");
        }
    }
}