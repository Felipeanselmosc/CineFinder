using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.Application.DTOs.Genero;
using CineFinder.Application.Models;
using CineFinder.API.Models;
using CineFinder.API.Helpers;

namespace CineFinder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GenerosController : ControllerBase
    {
        private readonly IGeneroService _generoService;
        private readonly ILogger<GenerosController> _logger;

        public GenerosController(IGeneroService generoService, ILogger<GenerosController> logger)
        {
            _generoService = generoService;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<ResourceDto<GeneroDto>>>> Search([FromQuery] GeneroSearchParameters parameters)
        {
            try
            {
                var pagedResult = await _generoService.SearchAsync(parameters);
                var itens = pagedResult.Items.Select(g => { var r = new ResourceDto<GeneroDto>(g); r.Links = HateoasLinkGenerator.GenerateGeneroLinks(g.Id, Url); return r; }).ToList();
                var result = new PagedResult<ResourceDto<GeneroDto>>(itens, pagedResult.TotalCount, pagedResult.PageNumber, pagedResult.PageSize);
                var pl = HateoasLinkGenerator.GeneratePaginationLinks(Url, "Search", "Generos", result.PageNumber, result.PageSize, result.TotalPages, parameters);
                result.FirstPage = pl.GetValueOrDefault("firstPage"); result.PreviousPage = pl.GetValueOrDefault("previousPage");
                result.NextPage = pl.GetValueOrDefault("nextPage"); result.LastPage = pl.GetValueOrDefault("lastPage");
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao buscar generos", error = ex.Message }); }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceDto<GeneroDto>>>> GetAll()
        {
            try
            {
                var generos = await _generoService.GetAllAsync();
                var result = generos.Select(g => { var r = new ResourceDto<GeneroDto>(g); r.Links = HateoasLinkGenerator.GenerateGeneroLinks(g.Id, Url); return r; });
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao obter generos", error = ex.Message }); }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResourceDto<GeneroDto>>> GetById(Guid id)
        {
            try
            {
                var genero = await _generoService.GetByIdAsync(id);
                var resource = new ResourceDto<GeneroDto>(genero);
                resource.Links = HateoasLinkGenerator.GenerateGeneroLinks(genero.Id, Url);
                return Ok(resource);
            }
            catch (KeyNotFoundException) { return NotFound(new { message = $"Genero com ID {id} nao encontrado" }); }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao obter genero", error = ex.Message }); }
        }

        [HttpGet("populares")]
        public async Task<ActionResult<IEnumerable<ResourceDto<GeneroDto>>>> GetPopulares()
        {
            try
            {
                var generos = await _generoService.GetPopularesAsync();
                var result = generos.Select(g => { var r = new ResourceDto<GeneroDto>(g); r.Links = HateoasLinkGenerator.GenerateGeneroLinks(g.Id, Url); return r; });
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao obter generos populares", error = ex.Message }); }
        }

        [HttpPost]
        public async Task<ActionResult<ResourceDto<GeneroDto>>> Create([FromBody] CreateGeneroDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var genero = await _generoService.CreateAsync(dto);
                var resource = new ResourceDto<GeneroDto>(genero);
                resource.Links = HateoasLinkGenerator.GenerateGeneroLinks(genero.Id, Url);
                return CreatedAtAction(nameof(GetById), new { id = genero.Id }, resource);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao criar genero", error = ex.Message }); }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResourceDto<GeneroDto>>> Update(Guid id, [FromBody] UpdateGeneroDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (id != dto.Id) return BadRequest(new { message = "ID nao corresponde" });
                var genero = await _generoService.UpdateAsync(dto);
                var resource = new ResourceDto<GeneroDto>(genero);
                resource.Links = HateoasLinkGenerator.GenerateGeneroLinks(genero.Id, Url);
                return Ok(resource);
            }
            catch (KeyNotFoundException) { return NotFound(new { message = $"Genero com ID {id} nao encontrado" }); }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao atualizar genero", error = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _generoService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException) { return NotFound(new { message = $"Genero com ID {id} nao encontrado" }); }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao deletar genero", error = ex.Message }); }
        }
    }
}
