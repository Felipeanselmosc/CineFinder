using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.Application.DTOs.Filme;
using CineFinder.Application.Models;
using CineFinder.API.Models;
using CineFinder.API.Helpers;

namespace CineFinder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FilmesController : ControllerBase
    {
        private readonly IFilmeService _filmeService;
        private readonly ILogger<FilmesController> _logger;

        public FilmesController(IFilmeService filmeService, ILogger<FilmesController> logger)
        {
            _filmeService = filmeService;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<ResourceDto<FilmeDto>>>> Search([FromQuery] FilmeSearchParameters parameters)
        {
            try
            {
                var pagedResult = await _filmeService.SearchAsync(parameters);
                var filmesComLinks = pagedResult.Items.Select(filme => { var r = new ResourceDto<FilmeDto>(filme); r.Links = HateoasLinkGenerator.GenerateFilmeLinks(filme.Id, Url); return r; }).ToList();
                var result = new PagedResult<ResourceDto<FilmeDto>>(filmesComLinks, pagedResult.TotalCount, pagedResult.PageNumber, pagedResult.PageSize);
                var pl = HateoasLinkGenerator.GeneratePaginationLinks(Url, "Search", "Filmes", result.PageNumber, result.PageSize, result.TotalPages, parameters);
                result.FirstPage = pl.GetValueOrDefault("firstPage"); result.PreviousPage = pl.GetValueOrDefault("previousPage");
                result.NextPage = pl.GetValueOrDefault("nextPage"); result.LastPage = pl.GetValueOrDefault("lastPage");
                return Ok(result);
            }
            catch (Exception ex) { _logger.LogError(ex, "Erro ao buscar filmes"); return BadRequest(new { message = "Erro ao buscar filmes", error = ex.Message }); }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceDto<FilmeDto>>>> GetAll()
        {
            try
            {
                var filmes = await _filmeService.GetAllAsync();
                var result = filmes.Select(f => { var r = new ResourceDto<FilmeDto>(f); r.Links = HateoasLinkGenerator.GenerateFilmeLinks(f.Id, Url); return r; });
                return Ok(result);
            }
            catch (Exception ex) { _logger.LogError(ex, "Erro ao obter filmes"); return BadRequest(new { message = "Erro ao obter filmes", error = ex.Message }); }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResourceDto<FilmeDetalhadoDto>>> GetById(Guid id)
        {
            try
            {
                var filme = await _filmeService.GetDetalhadoAsync(id);
                var resource = new ResourceDto<FilmeDetalhadoDto>(filme);
                resource.Links = HateoasLinkGenerator.GenerateFilmeLinks(filme.Id, Url);
                return Ok(resource);
            }
            catch (KeyNotFoundException) { return NotFound(new { message = $"Filme com ID {id} nao encontrado" }); }
            catch (Exception ex) { _logger.LogError(ex, "Erro ao obter filme {FilmeId}", id); return BadRequest(new { message = "Erro ao obter filme", error = ex.Message }); }
        }

        [HttpPost]
        public async Task<ActionResult<ResourceDto<FilmeDto>>> Create([FromBody] CreateFilmeDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var filme = await _filmeService.CreateAsync(dto);
                var resource = new ResourceDto<FilmeDto>(filme);
                resource.Links = HateoasLinkGenerator.GenerateFilmeLinks(filme.Id, Url);
                return CreatedAtAction(nameof(GetById), new { id = filme.Id }, resource);
            }
            catch (Exception ex) { _logger.LogError(ex, "Erro ao criar filme"); return BadRequest(new { message = "Erro ao criar filme", error = ex.Message }); }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResourceDto<FilmeDto>>> Update(Guid id, [FromBody] UpdateFilmeDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (id != dto.Id) return BadRequest(new { message = "ID nao corresponde" });
                var filme = await _filmeService.UpdateAsync(dto);
                var resource = new ResourceDto<FilmeDto>(filme);
                resource.Links = HateoasLinkGenerator.GenerateFilmeLinks(filme.Id, Url);
                return Ok(resource);
            }
            catch (KeyNotFoundException) { return NotFound(new { message = $"Filme com ID {id} nao encontrado" }); }
            catch (Exception ex) { _logger.LogError(ex, "Erro ao atualizar filme {FilmeId}", id); return BadRequest(new { message = "Erro ao atualizar filme", error = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _filmeService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException) { return NotFound(new { message = $"Filme com ID {id} nao encontrado" }); }
            catch (Exception ex) { _logger.LogError(ex, "Erro ao deletar filme {FilmeId}", id); return BadRequest(new { message = "Erro ao deletar filme", error = ex.Message }); }
        }

        [HttpGet("genero/{generoId}")]
        public async Task<ActionResult<IEnumerable<ResourceDto<FilmeDto>>>> GetByGenero(Guid generoId)
        {
            try
            {
                var filmes = await _filmeService.GetByGeneroAsync(generoId);
                var result = filmes.Select(f => { var r = new ResourceDto<FilmeDto>(f); r.Links = HateoasLinkGenerator.GenerateFilmeLinks(f.Id, Url); return r; });
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao obter filmes", error = ex.Message }); }
        }

        [HttpGet("top-rated")]
        public async Task<ActionResult<IEnumerable<ResourceDto<FilmeDto>>>> GetTopRated([FromQuery] int top = 10)
        {
            try
            {
                var filmes = await _filmeService.GetTopRatedAsync(top);
                var result = filmes.Select(f => { var r = new ResourceDto<FilmeDto>(f); r.Links = HateoasLinkGenerator.GenerateFilmeLinks(f.Id, Url); return r; });
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao obter filmes", error = ex.Message }); }
        }
    }
}
