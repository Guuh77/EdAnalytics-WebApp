using System.ComponentModel.DataAnnotations;
using EdAnalytics.Application.DTOs;
using EdAnalytics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EdAnalytics.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IAuthService _authService;

        public RegisterModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "O e-mail é obrigatório")]
            [EmailAddress(ErrorMessage = "E-mail inválido")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "A senha é obrigatória")]
            [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "A confirmação de senha é obrigatória")]
            [Compare("Password", ErrorMessage = "As senhas não conferem")]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Required(ErrorMessage = "O perfil de acesso é obrigatório")]
            public string Role { get; set; } = "Aluno";
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

            var registerDto = new RegisterDto
            {
                Email = Input.Email,
                Password = Input.Password,
                ConfirmPassword = Input.ConfirmPassword,
                Role = Input.Role
            };

            var success = await _authService.RegisterAsync(registerDto);

            if (!success)
            {
                ErrorMessage = "Este e-mail já está sendo utilizado.";
                return Page();
            }

            TempData["SuccessMessage"] = "Cadastro realizado com sucesso! Faça login abaixo.";
            return RedirectToPage("/Account/Login");
        }
    }
}
