using EdAnalytics.Application.DTOs;
using EdAnalytics.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdAnalytics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Autentica o usuário e retorna um token JWT.
        /// </summary>
        /// <remarks>
        /// Credenciais disponíveis para teste:
        /// 
        /// - **Admin**: admin@edanalytics.com / Admin@123
        /// - **Professor**: professor@edanalytics.com / Prof@123
        /// - **Aluno**: aluno@edanalytics.com / Aluno@123
        /// </remarks>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenResponseDto), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AuthenticateAsync(loginDto);

            if (result == null)
                return Unauthorized(new { message = "Credenciais inválidas." });

            return Ok(result);
        }

        /// <summary>
        /// Registra um novo usuário na plataforma.
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _authService.RegisterAsync(registerDto);

            if (!success)
                return BadRequest(new { message = "E-mail já cadastrado." });

            return Ok(new { message = "Usuário registrado com sucesso." });
        }
    }
}

