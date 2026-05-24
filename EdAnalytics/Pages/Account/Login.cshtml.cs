using System.ComponentModel.DataAnnotations;
using EdAnalytics.Application.DTOs;
using EdAnalytics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EdAnalytics.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ErrorMessage { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "O e-mail é obrigatório")]
            [EmailAddress(ErrorMessage = "E-mail inválido")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "A senha é obrigatória")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var loginDto = new LoginDto
            {
                Email = Input.Email,
                Password = Input.Password
            };

            var result = await _authService.AuthenticateAsync(loginDto);

            if (result == null)
            {
                ErrorMessage = "Credenciais inválidas. Verifique seu e-mail e senha.";
                return Page();
            }

            // Gravar o token JWT em um cookie seguro
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = result.Expiration
            };

            Response.Cookies.Append("jwt", result.Token, cookieOptions);

            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToPage("/Index");
        }

        public IActionResult OnGetLogout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToPage("/Index");
        }
    }
}
