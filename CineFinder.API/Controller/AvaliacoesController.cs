using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.Application.DTOs.Avaliacao;
using CineFinder.Application.Models;
using CineFinder.API.Models;
using CineFinder.API.Helpers;

namespace CineFinder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AvaliacoesController : ControllerBase
    {
        private readonly IAvaliacaoService _avaliacaoService;
        private readonly ILogger<AvaliacoesController> _logger;
        private static readonly Guid _usuarioIdFake = Guid.Parse("00000000-0000-0000-0000-000000000001");

        public AvaliacoesController(IAvaliacaoService avaliacaoService, ILogger<AvaliacoesController> logger)
        {
            _avaliacaoService = avaliacaoService;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<ResourceDto<AvaliacaoDto>>>> Search([FromQuery] AvaliacaoSearchParameters parameters)
        {
            try
            {
                var pagedResult = await _avaliacaoService.SearchAsync(parameters);
                var itens = pagedResult.Items.Select(a => { var r = new ResourceDto<AvaliacaoDto>(a); r.Links = HateoasLinkGenerator.GenerateAvaliacaoLinks(a.Id, Url); return r; }).ToList();
                var result = new PagedResult<ResourceDto<AvaliacaoDto>>(itens, pagedResult.TotalCount, pagedResult.PageNumber, pagedResult.PageSize);
                var pl = HateoasLinkGenerator.GeneratePaginationLinks(Url, "Search", "Avaliacoes", result.PageNumber, result.PageSize, result.TotalPages, parameters);
                result.FirstPage = pl.GetValueOrDefault("firstPage"); result.PreviousPage = pl.GetValueOrDefault("previousPage");
                result.NextPage = pl.GetValueOrDefault("nextPage"); result.LastPage = pl.GetValueOrDefault("lastPage");
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao buscar avaliacoes", error = ex.Message }); }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceDto<AvaliacaoDto>>>> GetAll()
        {
            try
            {
                var avaliacoes = await _avaliacaoService.GetAllAsync();
                var result = avaliacoes.Select(a => { var r = new ResourceDto<AvaliacaoDto>(a); r.Links = HateoasLinkGenerator.GenerateAvaliacaoLinks(a.Id, Url); return r; });
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao obter avaliacoes", error = ex.Message }); }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResourceDto<AvaliacaoDto>>> GetById(Guid id)
        {
            try
            {
                var avaliacao = await _avaliacaoService.GetByIdAsync(id);
                var resource = new ResourceDto<AvaliacaoDto>(avaliacao);
                resource.Links = HateoasLinkGenerator.GenerateAvaliacaoLinks(avaliacao.Id, Url);
                return Ok(resource);
            }
            catch (KeyNotFoundException) { return NotFound(new { message = $"Avaliacao com ID {id} nao encontrada" }); }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao obter avaliacao", error = ex.Message }); }
        }

        [HttpGet("filme/{filmeId}")]
        public async Task<ActionResult<IEnumerable<ResourceDto<AvaliacaoSimplificadaDto>>>> GetByFilme(Guid filmeId)
        {
            try
            {
                var avaliacoes = await _avaliacaoService.GetByFilmeAsync(filmeId);
                var result = avaliacoes.Select(a => { var r = new ResourceDto<AvaliacaoSimplificadaDto>(a); r.Links = HateoasLinkGenerator.GenerateAvaliacaoLinks(a.Id, Url); return r; });
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao obter avaliacoes", error = ex.Message }); }
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<ResourceDto<AvaliacaoDto>>>> GetByUsuario(Guid usuarioId)
        {
            try
            {
                var avaliacoes = await _avaliacaoService.GetByUsuarioAsync(usuarioId);
                var result = avaliacoes.Select(a => { var r = new ResourceDto<AvaliacaoDto>(a); r.Links = HateoasLinkGenerator.GenerateAvaliacaoLinks(a.Id, Url); return r; });
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao obter avaliacoes", error = ex.Message }); }
        }

        [HttpPost]
        public async Task<ActionResult<ResourceDto<AvaliacaoDto>>> Create([FromBody] CreateAvaliacaoDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var avaliacao = await _avaliacaoService.CreateAsync(_usuarioIdFake, dto);
                var resource = new ResourceDto<AvaliacaoDto>(avaliacao);
                resource.Links = HateoasLinkGenerator.GenerateAvaliacaoLinks(avaliacao.Id, Url);
                return CreatedAtAction(nameof(GetById), new { id = avaliacao.Id }, resource);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao criar avaliacao", error = ex.Message }); }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResourceDto<AvaliacaoDto>>> Update(Guid id, [FromBody] UpdateAvaliacaoDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (id != dto.Id) return BadRequest(new { message = "ID nao corresponde" });
                var avaliacao = await _avaliacaoService.UpdateAsync(id, _usuarioIdFake, dto);
                var resource = new ResourceDto<AvaliacaoDto>(avaliacao);
                resource.Links = HateoasLinkGenerator.GenerateAvaliacaoLinks(avaliacao.Id, Url);
                return Ok(resource);
            }
            catch (KeyNotFoundException) { return NotFound(new { message = $"Avaliacao com ID {id} nao encontrada" }); }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao atualizar avaliacao", error = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _avaliacaoService.DeleteAsync(id, _usuarioIdFake);
                return NoContent();
            }
            catch (KeyNotFoundException) { return NotFound(new { message = $"Avaliacao com ID {id} nao encontrada" }); }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao deletar avaliacao", error = ex.Message }); }
        }
    }
}
