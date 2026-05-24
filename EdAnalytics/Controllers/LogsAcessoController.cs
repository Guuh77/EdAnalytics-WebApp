using EdAnalytics.Application.DTOs;
using EdAnalytics.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdAnalytics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LogsAcessoController : ControllerBase
    {
        private readonly ILogAcessoService _logService;

        public LogsAcessoController(ILogAcessoService logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// Retorna os logs de acesso mais recentes (MongoDB).
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<LogAcessoDto>), 200)]
        public async Task<IActionResult> GetRecentes([FromQuery] int quantidade = 20)
        {
            var logs = await _logService.GetLogsRecentesAsync(quantidade);
            return Ok(logs);
        }

        /// <summary>
        /// Retorna os logs de acesso de um curso específico (MongoDB).
        /// </summary>
        [HttpGet("curso/{cursoId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<LogAcessoDto>), 200)]
        public async Task<IActionResult> GetByCurso(int cursoId)
        {
            var logs = await _logService.GetLogsPorCursoAsync(cursoId);
            return Ok(logs);
        }

        /// <summary>
        /// Registra manualmente um log de acesso (MongoDB).
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Registrar([FromBody] LogAcessoDto logDto)
        {
            await _logService.RegistrarAcessoAsync(
                logDto.CursoId, logDto.CursoTitulo, logDto.Acao, logDto.UsuarioId);
            return StatusCode(201, new { message = "Log registrado com sucesso." });
        }
    }
}
