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
    public class CursosController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly ILogAcessoService _logService;

        public CursosController(IAnalyticsService analyticsService, ILogAcessoService logService)
        {
            _analyticsService = analyticsService;
            _logService = logService;
        }

        /// <summary>
        /// Lista cursos com paginação, filtros e ordenação.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedResult<CursoHateoasDto>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
        {
            var result = await _analyticsService.GetCursosPagedAsync(parameters);

            var hateoasItems = result.Items.Select(c =>
            {
                var dto = new CursoHateoasDto
                {
                    Id = c.Id,
                    Titulo = c.Titulo,
                    Area = c.Area,
                    Visualizacoes = c.Visualizacoes
                };
                dto.Links = GenerateCursoLinks(c.Id);
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
        /// Busca um curso por ID.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CursoHateoasDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var curso = await _analyticsService.GetCursoParaEdicaoAsync(id);
            if (curso == null)
                return NotFound(new { message = $"Curso com ID {id} não encontrado." });

            var dto = new CursoHateoasDto
            {
                Id = curso.Id,
                Titulo = curso.Titulo,
                Area = curso.Area,
                Visualizacoes = curso.Visualizacoes
            };
            dto.Links = GenerateCursoLinks(id);

            await _logService.RegistrarAcessoAsync(id, curso.Titulo, "Visualizacao");

            return Ok(dto);
        }

        /// <summary>
        /// Cria um novo curso.
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(CursoHateoasDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Create([FromBody] CursoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _analyticsService.CreateCursoAsync(model);

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        /// <summary>
        /// Atualiza um curso existente.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Update(int id, [FromBody] CursoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.Id = id;
            var existing = await _analyticsService.GetCursoParaEdicaoAsync(id);
            if (existing == null)
                return NotFound(new { message = $"Curso com ID {id} não encontrado." });

            await _analyticsService.UpdateCursoAsync(model);
            return NoContent();
        }

        /// <summary>
        /// Deleta um curso.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete(int id)
        {
            await _analyticsService.DeleteCursoAsync(id);
            return NoContent();
        }

        private List<LinkDto> GenerateCursoLinks(int id)
        {
            return new List<LinkDto>
            {
                new LinkDto(Url.Action(nameof(GetById), new { id })!, "self", "GET"),
                new LinkDto(Url.Action(nameof(Update), new { id })!, "update", "PUT"),
                new LinkDto(Url.Action(nameof(Delete), new { id })!, "delete", "DELETE"),
                new LinkDto(Url.Action("GetByCurso", "Aulas", new { cursoId = id })!, "aulas", "GET")
            };
        }
    }
}
