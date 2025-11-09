using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.Application.DTOs.Lista;
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
    /// Controller REST para gerenciamento de Listas
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ListasController : ControllerBase
    {
        private readonly IListaService _listaService;
        private readonly ILogger<ListasController> _logger;

        public ListasController(
            IListaService listaService,
            ILogger<ListasController> logger)
        {
            _listaService = listaService;
            _logger = logger;
        }

        /// <summary>
        /// Busca listas com paginação e filtros
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(PagedResult<ResourceDto<ListaDto>>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PagedResult<ResourceDto<ListaDto>>>> Search(
            [FromQuery] ListaSearchParameters parameters)
        {
            try
            {
                _logger.LogInformation("Buscando listas com parâmetros: {@Parameters}", parameters);

                var pagedResult = await _listaService.SearchAsync(parameters);

                var listasComLinks = pagedResult.Items.Select(lista =>
                {
                    var resource = new ResourceDto<ListaDto>(lista);
                    resource.Links = LinkGenerator.GenerateListaLinks(lista.Id, Url);
                    return resource;
                }).ToList();

                var result = new PagedResult<ResourceDto<ListaDto>>(
                    listasComLinks,
                    pagedResult.TotalCount,
                    pagedResult.PageNumber,
                    pagedResult.PageSize
                );

                var paginationLinks = LinkGenerator.GeneratePaginationLinks(
                    Url, "Search", "Listas",
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
                _logger.LogError(ex, "Erro ao buscar listas");
                return BadRequest(new { message = "Erro ao buscar listas", error = ex.Message });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResourceDto<ListaDto>>), 200)]
        public async Task<ActionResult<IEnumerable<ResourceDto<ListaDto>>>> GetAll()
        {
            try
            {
                var listas = await _listaService.GetAllAsync();

                var listasComLinks = listas.Select(lista =>
                {
                    var resource = new ResourceDto<ListaDto>(lista);
                    resource.Links = LinkGenerator.GenerateListaLinks(lista.Id, Url);
                    return resource;
                });

                return Ok(listasComLinks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter listas");
                return BadRequest(new { message = "Erro ao obter listas", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResourceDto<ListaDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResourceDto<ListaDto>>> GetById(Guid id)
        {
            try
            {
                var lista = await _listaService.GetByIdAsync(id);

                var resource = new ResourceDto<ListaDto>(lista);
                resource.Links = LinkGenerator.GenerateListaLinks(lista.Id, Url);

                return Ok(resource);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Lista com ID {id} não encontrada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lista {ListaId}", id);
                return BadRequest(new { message = "Erro ao obter lista", error = ex.Message });
            }
        }

        [HttpGet("usuario/{usuarioId}")]
        [ProducesResponseType(typeof(IEnumerable<ResourceDto<ListaDto>>), 200)]
        public async Task<ActionResult<IEnumerable<ResourceDto<ListaDto>>>> GetByUsuario(Guid usuarioId)
        {
            try
            {
                var listas = await _listaService.GetByUsuarioAsync(usuarioId);

                var listasComLinks = listas.Select(lista =>
                {
                    var resource = new ResourceDto<ListaDto>(lista);
                    resource.Links = LinkGenerator.GenerateListaLinks(lista.Id, Url);
                    return resource;
                });

                return Ok(listasComLinks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter listas do usuário {UsuarioId}", usuarioId);
                return BadRequest(new { message = "Erro ao obter listas", error = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResourceDto<ListaDto>), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResourceDto<ListaDto>>> Create([FromBody] CreateListaDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var lista = await _listaService.CreateAsync(dto);

                var resource = new ResourceDto<ListaDto>(lista);
                resource.Links = LinkGenerator.GenerateListaLinks(lista.Id, Url);

                _logger.LogInformation("Lista criada com sucesso: {ListaId} - {Nome}", lista.Id, lista.Nome);

                return CreatedAtAction(nameof(GetById), new { id = lista.Id }, resource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar lista");
                return BadRequest(new { message = "Erro ao criar lista", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResourceDto<ListaDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResourceDto<ListaDto>>> Update(
            Guid id,
            [FromBody] UpdateListaDto dto)
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

                var lista = await _listaService.UpdateAsync(dto);

                var resource = new ResourceDto<ListaDto>(lista);
                resource.Links = LinkGenerator.GenerateListaLinks(lista.Id, Url);

                _logger.LogInformation("Lista atualizada com sucesso: {ListaId} - {Nome}", lista.Id, lista.Nome);

                return Ok(resource);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Lista com ID {id} não encontrada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar lista {ListaId}", id);
                return BadRequest(new { message = "Erro ao atualizar lista", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _listaService.DeleteAsync(id);

                _logger.LogInformation("Lista deletada com sucesso: {ListaId}", id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Lista com ID {id} não encontrada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar lista {ListaId}", id);
                return BadRequest(new { message = "Erro ao deletar lista", error = ex.Message });
            }
        }

        [HttpPost("{listaId}/filmes/{filmeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddFilme(Guid listaId, Guid filmeId)
        {
            try
            {
                await _listaService.AddFilmeAsync(listaId, filmeId);

                _logger.LogInformation("Filme {FilmeId} adicionado à lista {ListaId}", filmeId, listaId);

                return Ok(new { message = "Filme adicionado à lista com sucesso" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar filme {FilmeId} à lista {ListaId}", filmeId, listaId);
                return BadRequest(new { message = "Erro ao adicionar filme à lista", error = ex.Message });
            }
        }

        [HttpDelete("{listaId}/filmes/{filmeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveFilme(Guid listaId, Guid filmeId)
        {
            try
            {
                await _listaService.RemoveFilmeAsync(listaId, filmeId);

                _logger.LogInformation("Filme {FilmeId} removido da lista {ListaId}", filmeId, listaId);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover filme {FilmeId} da lista {ListaId}", filmeId, listaId);
                return BadRequest(new { message = "Erro ao remover filme da lista", error = ex.Message });
            }
        }
    }
}