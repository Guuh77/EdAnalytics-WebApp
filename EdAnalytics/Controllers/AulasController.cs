using EdAnalytics.Application.DTOs;
using EdAnalytics.Application.Interfaces;
using EdAnalytics.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdAnalytics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AulasController : ControllerBase
    {
        private readonly IAulaService _aulaService;

        public AulasController(IAulaService aulaService)
        {
            _aulaService = aulaService;
        }

        /// <summary>
        /// Lista aulas de um curso com paginação.
        /// </summary>
        [HttpGet("curso/{cursoId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedResult<AulaHateoasDto>), 200)]
        public async Task<IActionResult> GetByCurso(int cursoId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _aulaService.GetAulasPagedAsync(cursoId, page, pageSize);

            var hateoasItems = result.Items.Select(a =>
            {
                var dto = new AulaHateoasDto
                {
                    Id = a.Id,
                    Titulo = a.Titulo,
                    Conteudo = a.Conteudo,
                    CursoId = a.CursoId
                };
                dto.Links = GenerateAulaLinks(a.Id, a.CursoId);
                return dto;
            }).ToList();

            return Ok(new
            {
                items = hateoasItems,
                result.TotalCount,
                result.Page,
                result.PageSize,
                result.TotalPages,
                result.HasPrevious,
                result.HasNext
            });
        }

        /// <summary>
        /// Busca uma aula por ID.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AulaHateoasDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var aula = await _aulaService.GetAulaDetalhesAsync(id);
            if (aula == null)
                return NotFound(new { message = $"Aula com ID {id} não encontrada." });

            var dto = new AulaHateoasDto
            {
                Id = aula.Id,
                Titulo = aula.Titulo,
                Conteudo = aula.Conteudo,
                CursoId = aula.CursoId
            };
            dto.Links = GenerateAulaLinks(id, aula.CursoId);

            return Ok(dto);
        }

        /// <summary>
        /// Cria uma nova aula.
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(AulaHateoasDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Create([FromBody] AulaViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _aulaService.CreateAulaAsync(model);

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        /// <summary>
        /// Atualiza uma aula existente.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Update(int id, [FromBody] AulaViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.Id = id;
            var existing = await _aulaService.GetAulaDetalhesAsync(id);
            if (existing == null)
                return NotFound(new { message = $"Aula com ID {id} não encontrada." });

            await _aulaService.UpdateAulaAsync(model);
            return NoContent();
        }

        /// <summary>
        /// Deleta uma aula.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete(int id)
        {
            await _aulaService.DeleteAulaAsync(id);
            return NoContent();
        }

        private List<LinkDto> GenerateAulaLinks(int id, int cursoId)
        {
            return new List<LinkDto>
            {
                new LinkDto(Url.Action(nameof(GetById), new { id })!, "self", "GET"),
                new LinkDto(Url.Action(nameof(Update), new { id })!, "update", "PUT"),
                new LinkDto(Url.Action(nameof(Delete), new { id })!, "delete", "DELETE"),
                new LinkDto(Url.Action(nameof(GetByCurso), new { cursoId })!, "curso-aulas", "GET")
            };
        }
    }
}
