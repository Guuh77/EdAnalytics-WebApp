using EdAnalytics.Application.Interfaces;
using EdAnalytics.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EdAnalytics.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IAnalyticsService _analyticsService;

        public CreateModel(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [BindProperty]
        public CursoViewModel Curso { get; set; } = new CursoViewModel();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _analyticsService.CreateCursoAsync(Curso);
            return RedirectToPage("./Index");
        }
    }
}