using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.Application.DTOs.Genero;
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
    /// Controller REST para gerenciamento de Gêneros
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GenerosController : ControllerBase
    {
        private readonly IGeneroService _generoService;
        private readonly ILogger<GenerosController> _logger;

        public GenerosController(
            IGeneroService generoService,
            ILogger<GenerosController> logger)
        {
            _generoService = generoService;
            _logger = logger;
        }

        /// <summary>
        /// Busca gêneros com paginação e filtros
        /// </summary>
        /// <param name="parameters">Parâmetros de busca</param>
        /// <returns>Lista paginada de gêneros com links HATEOAS</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(PagedResult<ResourceDto<GeneroDto>>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PagedResult<ResourceDto<GeneroDto>>>> Search(
            [FromQuery] GeneroSearchParameters parameters)
        {
            try
            {
                _logger.LogInformation("Buscando gêneros com parâmetros: {@Parameters}", parameters);

                var pagedResult = await _generoService.SearchAsync(parameters);

                var generosComLinks = pagedResult.Items.Select(genero =>
                {
                    var resource = new ResourceDto<GeneroDto>(genero);
                    resource.Links = LinkGenerator.GenerateGeneroLinks(genero.Id, Url);
                    return resource;
                }).ToList();

                var result = new PagedResult<ResourceDto<GeneroDto>>(
                    generosComLinks,
                    pagedResult.TotalCount,
                    pagedResult.PageNumber,
                    pagedResult.PageSize
                );

                var paginationLinks = LinkGenerator.GeneratePaginationLinks(
                    Url, "Search", "Generos",
                    result.PageNumber,
                    result.PageSize,
                    result.TotalPages,
                    parameters
                );

                result.FirstPage = paginationLinks.GetValueOrDefault("firstPage");
                result.PreviousPage = paginationLinks.GetValueOrDefault("previousPage");
                result.NextPage = paginationLinks.GetValueOrDefault("nextPage");
                result.LastPage = paginationLinks.GetValueOrDefault("lastPage");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar gêneros");
                return BadRequest(new { message = "Erro ao buscar gêneros", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém todos os gêneros
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResourceDto<GeneroDto>>), 200)]
        public async Task<ActionResult<IEnumerable<ResourceDto<GeneroDto>>>> GetAll()
        {
            try
            {
                var generos = await _generoService.GetAllAsync();

                var generosComLinks = generos.Select(genero =>
                {
                    var resource = new ResourceDto<GeneroDto>(genero);
                    resource.Links = LinkGenerator.GenerateGeneroLinks(genero.Id, Url);
                    return resource;
                });

                return Ok(generosComLinks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter gêneros");
                return BadRequest(new { message = "Erro ao obter gêneros", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém um gênero por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResourceDto<GeneroDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResourceDto<GeneroDto>>> GetById(Guid id)
        {
            try
            {
                var genero = await _generoService.GetByIdAsync(id);

                var resource = new ResourceDto<GeneroDto>(genero);
                resource.Links = LinkGenerator.GenerateGeneroLinks(genero.Id, Url);

                return Ok(resource);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Gênero com ID {id} não encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter gênero {GeneroId}", id);
                return BadRequest(new { message = "Erro ao obter gênero", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém os gêneros mais populares
        /// </summary>
        [HttpGet("populares")]
        [ProducesResponseType(typeof(IEnumerable<ResourceDto<GeneroDto>>), 200)]
        public async Task<ActionResult<IEnumerable<ResourceDto<GeneroDto>>>> GetPopulares()
        {
            try
            {
                var generos = await _generoService.GetPopularesAsync();

                var generosComLinks = generos.Select(genero =>
                {
                    var resource = new ResourceDto<GeneroDto>(genero);
                    resource.Links = LinkGenerator.GenerateGeneroLinks(genero.Id, Url);
                    return resource;
                });

                return Ok(generosComLinks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter gêneros populares");
                return BadRequest(new { message = "Erro ao obter gêneros populares", error = ex.Message });
            }
        }

        /// <summary>
        /// Cria um novo gênero
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ResourceDto<GeneroDto>), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResourceDto<GeneroDto>>> Create([FromBody] CreateGeneroDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var genero = await _generoService.CreateAsync(dto);

                var resource = new ResourceDto<GeneroDto>(genero);
                resource.Links = LinkGenerator.GenerateGeneroLinks(genero.Id, Url);

                _logger.LogInformation("Gênero criado com sucesso: {GeneroId} - {Nome}", genero.Id, genero.Nome);

                return CreatedAtAction(nameof(GetById), new { id = genero.Id }, resource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar gênero");
                return BadRequest(new { message = "Erro ao criar gênero", error = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um gênero existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResourceDto<GeneroDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResourceDto<GeneroDto>>> Update(
            Guid id,
            [FromBody] UpdateGeneroDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    return BadRequest(new { message = "ID não corresponde" });
                }

                var genero = await _generoService.UpdateAsync(dto);

                var resource = new ResourceDto<GeneroDto>(genero);
                resource.Links = LinkGenerator.GenerateGeneroLinks(genero.Id, Url);

                _logger.LogInformation("Gênero atualizado com sucesso: {GeneroId} - {Nome}", genero.Id, genero.Nome);

                return Ok(resource);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Gênero com ID {id} não encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar gênero {GeneroId}", id);
                return BadRequest(new { message = "Erro ao atualizar gênero", error = ex.Message });
            }
        }

        /// <summary>
        /// Deleta um gênero
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _generoService.DeleteAsync(id);

                _logger.LogInformation("Gênero deletado com sucesso: {GeneroId}", id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Gênero com ID {id} não encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar gênero {GeneroId}", id);
                return BadRequest(new { message = "Erro ao deletar gênero", error = ex.Message });
            }
        }
    }
}