using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.Application.DTOs.Filme;
using CineFinder.Application.Models;
using CineFinder.API.Models;
using CineFinder.API.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineFinder.API.Controllers
{
    /// <summary>
    /// Controller REST para gerenciamento de Filmes
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FilmesController : ControllerBase
    {
        private readonly IFilmeService _filmeService;
        private readonly ILogger<FilmesController> _logger;

        public FilmesController(
            IFilmeService filmeService,
            ILogger<FilmesController> logger)
        {
            _filmeService = filmeService;
            _logger = logger;
        }

        /// <summary>
        /// Busca filmes com paginação, filtros e ordenação
        /// </summary>
        /// <param name="parameters">Parâmetros de busca</param>
        /// <returns>Lista paginada de filmes com links HATEOAS</returns>
        /// <response code="200">Retorna a lista paginada de filmes</response>
        /// <response code="400">Se houver erro na busca</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(PagedResult<ResourceDto<FilmeDto>>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PagedResult<ResourceDto<FilmeDto>>>> Search(
            [FromQuery] FilmeSearchParameters parameters)
        {
            try
            {
                _logger.LogInformation("Buscando filmes com parâmetros: {@Parameters}", parameters);

                var pagedResult = await _filmeService.SearchAsync(parameters);

                // Adicionar HATEOAS links em cada filme
                var filmesComLinks = pagedResult.Items.Select(filme =>
                {
                    var resource = new ResourceDto<FilmeDto>(filme);
                    resource.Links = LinkGenerator.GenerateFilmeLinks(filme.Id, Url);
                    return resource;
                }).ToList();

                var result = new PagedResult<ResourceDto<FilmeDto>>(
                    filmesComLinks,
                    pagedResult.TotalCount,
                    pagedResult.PageNumber,
                    pagedResult.PageSize
                );

                // Adicionar links de paginação
                var paginationLinks = LinkGenerator.GeneratePaginationLinks(
                    Url, "Search", "Filmes",
                    result.PageNumber,
                    result.PageSize,
                    result.TotalPages,
                    parameters
                );

                result.FirstPage = paginationLinks.GetValueOrDefault("firstPage");
                result.PreviousPage = paginationLinks.GetValueOrDefault("previousPage");
                result.NextPage = paginationLinks.GetValueOrDefault("nextPage");
                result.LastPage = paginationLinks.GetValueOrDefault("lastPage");

                _logger.LogInformation("Busca realizada com sucesso. Total: {Total}, Página: {Page}",
                    result.TotalCount, result.PageNumber);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar filmes");
                return BadRequest(new { message = "Erro ao buscar filmes", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém todos os filmes
        /// </summary>
        /// <returns>Lista de filmes com links HATEOAS</returns>
        /// <response code="200">Retorna a lista de filmes</response>
        /// <response code="400">Se houver erro ao obter filmes</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResourceDto<FilmeDto>>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ResourceDto<FilmeDto>>>> GetAll()
        {
            try
            {
                var filmes = await _filmeService.GetAllAsync();

                var filmesComLinks = filmes.Select(filme =>
                {
                    var resource = new ResourceDto<FilmeDto>(filme);
                    resource.Links = LinkGenerator.GenerateFilmeLinks(filme.Id, Url);
                    return resource;
                });

                return Ok(filmesComLinks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter filmes");
                return BadRequest(new { message = "Erro ao obter filmes", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém um filme específico por ID
        /// </summary>
        /// <param name="id">ID do filme</param>
        /// <returns>Filme com links HATEOAS</returns>
        /// <response code="200">Retorna o filme</response>
        /// <response code="404">Se o filme não for encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResourceDto<FilmeDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResourceDto<FilmeDto>>> GetById(Guid id)
        {
            try
            {
                var filme = await _filmeService.GetDetalhadoAsync(id);

                var resource = new ResourceDto<FilmeDetalhadoDto>(filme);
                resource.Links = LinkGenerator.GenerateFilmeLinks(filme.Id, Url);

                return Ok(resource);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Filme com ID {id} não encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter filme {FilmeId}", id);
                return BadRequest(new { message = "Erro ao obter filme", error = ex.Message });
            }
        }

        /// <summary>
        /// Cria um novo filme
        /// </summary>
        /// <param name="dto">Dados do filme</param>
        /// <returns>Filme criado com links HATEOAS</returns>
        /// <response code="201">Filme criado com sucesso</response>
        /// <response code="400">Se os dados forem inválidos</response>
        [HttpPost]
        [ProducesResponseType(typeof(ResourceDto<FilmeDto>), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResourceDto<FilmeDto>>> Create([FromBody] CreateFilmeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var filme = await _filmeService.CreateAsync(dto);

                var resource = new ResourceDto<FilmeDto>(filme);
                resource.Links = LinkGenerator.GenerateFilmeLinks(filme.Id, Url);

                _logger.LogInformation("Filme criado com sucesso: {FilmeId} - {Titulo}", filme.Id, filme.Titulo);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = filme.Id },
                    resource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar filme");
                return BadRequest(new { message = "Erro ao criar filme", error = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um filme existente
        /// </summary>
        /// <param name="id">ID do filme</param>
        /// <param name="dto">Dados atualizados</param>
        /// <returns>Filme atualizado com links HATEOAS</returns>
        /// <response code="200">Filme atualizado com sucesso</response>
        /// <response code="400">Se os dados forem inválidos</response>
        /// <response code="404">Se o filme não for encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResourceDto<FilmeDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResourceDto<FilmeDto>>> Update(
            Guid id,
            [FromBody] UpdateFilmeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    return BadRequest(new { message = "ID da URL não corresponde ao ID do corpo da requisição" });
                }

                var filme = await _filmeService.UpdateAsync(dto);

                var resource = new ResourceDto<FilmeDto>(filme);
                resource.Links = LinkGenerator.GenerateFilmeLinks(filme.Id, Url);

                _logger.LogInformation("Filme atualizado com sucesso: {FilmeId} - {Titulo}", filme.Id, filme.Titulo);

                return Ok(resource);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Filme com ID {id} não encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar filme {FilmeId}", id);
                return BadRequest(new { message = "Erro ao atualizar filme", error = ex.Message });
            }
        }

        /// <summary>
        /// Deleta um filme
        /// </summary>
        /// <param name="id">ID do filme</param>
        /// <returns>No content</returns>
        /// <response code="204">Filme deletado com sucesso</response>
        /// <response code="404">Se o filme não for encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _filmeService.DeleteAsync(id);

                _logger.LogInformation("Filme deletado com sucesso: {FilmeId}", id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Filme com ID {id} não encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar filme {FilmeId}", id);
                return BadRequest(new { message = "Erro ao deletar filme", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém filmes por gênero
        /// </summary>
        /// <param name="generoId">ID do gênero</param>
        /// <returns>Lista de filmes do gênero com links HATEOAS</returns>
        /// <response code="200">Retorna a lista de filmes</response>
        [HttpGet("genero/{generoId}")]
        [ProducesResponseType(typeof(IEnumerable<ResourceDto<FilmeDto>>), 200)]
        public async Task<ActionResult<IEnumerable<ResourceDto<FilmeDto>>>> GetByGenero(Guid generoId)
        {
            try
            {
                var filmes = await _filmeService.GetByGeneroAsync(generoId);

                var filmesComLinks = filmes.Select(filme =>
                {
                    var resource = new ResourceDto<FilmeDto>(filme);
                    resource.Links = LinkGenerator.GenerateFilmeLinks(filme.Id, Url);
                    return resource;
                });

                return Ok(filmesComLinks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter filmes por gênero {GeneroId}", generoId);
                return BadRequest(new { message = "Erro ao obter filmes", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém os filmes mais bem avaliados
        /// </summary>
        /// <param name="top">Quantidade de filmes</param>
        /// <returns>Lista dos filmes mais bem avaliados com links HATEOAS</returns>
        /// <response code="200">Retorna a lista de filmes</response>
        [HttpGet("top-rated")]
        [ProducesResponseType(typeof(IEnumerable<ResourceDto<FilmeDto>>), 200)]
        public async Task<ActionResult<IEnumerable<ResourceDto<FilmeDto>>>> GetTopRated(
            [FromQuery] int top = 10)
        {
            try
            {
                var filmes = await _filmeService.GetTopRatedAsync(top);

                var filmesComLinks = filmes.Select(filme =>
                {
                    var resource = new ResourceDto<FilmeDto>(filme);
                    resource.Links = LinkGenerator.GenerateFilmeLinks(filme.Id, Url);
                    return resource;
                });

                return Ok(filmesComLinks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter filmes mais bem avaliados");
                return BadRequest(new { message = "Erro ao obter filmes", error = ex.Message });
            }
        }
    }
}