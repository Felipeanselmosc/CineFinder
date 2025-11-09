using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.Application.DTOs.Avaliacao;
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
    /// Controller REST para gerenciamento de Avaliações
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AvaliacoesController : ControllerBase
    {
        private readonly IAvaliacaoService _avaliacaoService;
        private readonly ILogger<AvaliacoesController> _logger;

        public AvaliacoesController(
            IAvaliacaoService avaliacaoService,
            ILogger<AvaliacoesController> logger)
        {
            _avaliacaoService = avaliacaoService;
            _logger = logger;
        }

        /// <summary>
        /// Busca avaliações com paginação e filtros
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(PagedResult<ResourceDto<AvaliacaoDto>>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PagedResult<ResourceDto<AvaliacaoDto>>>> Search(
            [FromQuery] AvaliacaoSearchParameters parameters)
        {
            try
            {
                _logger.LogInformation("Buscando avaliações com parâmetros: {@Parameters}", parameters);

                var pagedResult = await _avaliacaoService.SearchAsync(parameters);

                var avaliacoesComLinks = pagedResult.Items.Select(avaliacao =>
                {
                    var resource = new ResourceDto<AvaliacaoDto>(avaliacao);
                    resource.Links = LinkGenerator.GenerateAvaliacaoLinks(avaliacao.Id, Url);
                    return resource;
                }).ToList();

                var result = new PagedResult<ResourceDto<AvaliacaoDto>>(
                    avaliacoesComLinks,
                    pagedResult.TotalCount,
                    pagedResult.PageNumber,
                    pagedResult.PageSize
                );

                var paginationLinks = LinkGenerator.GeneratePaginationLinks(
                    Url, "Search", "Avaliacoes",
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
                _logger.LogError(ex, "Erro ao buscar avaliações");
                return BadRequest(new { message = "Erro ao buscar avaliações", error = ex.Message });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResourceDto<AvaliacaoDto>>), 200)]
        public async Task<ActionResult<IEnumerable<ResourceDto<AvaliacaoDto>>>> GetAll()
        {
            try
            {
                var avaliacoes = await _avaliacaoService.GetAllAsync();

                var avaliacoesComLinks = avaliacoes.Select(avaliacao =>
                {
                    var resource = new ResourceDto<AvaliacaoDto>(avaliacao);
                    resource.Links = LinkGenerator.GenerateAvaliacaoLinks(avaliacao.Id, Url);
                    return resource;
                });

                return Ok(avaliacoesComLinks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter avaliações");
                return BadRequest(new { message = "Erro ao obter avaliações", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResourceDto<AvaliacaoDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResourceDto<AvaliacaoDto>>> GetById(Guid id)
        {
            try
            {
                var avaliacao = await _avaliacaoService.GetByIdAsync(id);

                var resource = new ResourceDto<AvaliacaoDto>(avaliacao);
                resource.Links = LinkGenerator.GenerateAvaliacaoLinks(avaliacao.Id, Url);

                return Ok(resource);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Avaliação com ID {id} não encontrada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter avaliação {AvaliacaoId}", id);
                return BadRequest(new { message = "Erro ao obter avaliação", error = ex.Message });
            }
        }

        [HttpGet("filme/{filmeId}")]
        [ProducesResponseType(typeof(IEnumerable<ResourceDto<AvaliacaoDto>>), 200)]
        public async Task<ActionResult<IEnumerable<ResourceDto<AvaliacaoDto>>>> GetByFilme(Guid filmeId)
        {
            try
            {
                var avaliacoes = await _avaliacaoService.GetByFilmeAsync(filmeId);

                var avaliacoesComLinks = avaliacoes.Select(avaliacao =>
                {
                    var resource = new ResourceDto<AvaliacaoDto>(avaliacao);
                    resource.Links = LinkGenerator.GenerateAvaliacaoLinks(avaliacao.Id, Url);
                    return resource;
                });

                return Ok(avaliacoesComLinks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter avaliações do filme {FilmeId}", filmeId);
                return BadRequest(new { message = "Erro ao obter avaliações", error = ex.Message });
            }
        }

        [HttpGet("usuario/{usuarioId}")]
        [ProducesResponseType(typeof(IEnumerable<ResourceDto<AvaliacaoDto>>), 200)]
        public async Task<ActionResult<IEnumerable<ResourceDto<AvaliacaoDto>>>> GetByUsuario(Guid usuarioId)
        {
            try
            {
                var avaliacoes = await _avaliacaoService.GetByUsuarioAsync(usuarioId);

                var avaliacoesComLinks = avaliacoes.Select(avaliacao =>
                {
                    var resource = new ResourceDto<AvaliacaoDto>(avaliacao);
                    resource.Links = LinkGenerator.GenerateAvaliacaoLinks(avaliacao.Id, Url);
                    return resource;
                });

                return Ok(avaliacoesComLinks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter avaliações do usuário {UsuarioId}", usuarioId);
                return BadRequest(new { message = "Erro ao obter avaliações", error = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResourceDto<AvaliacaoDto>), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResourceDto<AvaliacaoDto>>> Create([FromBody] CreateAvaliacaoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var avaliacao = await _avaliacaoService.CreateAsync(dto);

                var resource = new ResourceDto<AvaliacaoDto>(avaliacao);
                resource.Links = LinkGenerator.GenerateAvaliacaoLinks(avaliacao.Id, Url);

                _logger.LogInformation("Avaliação criada com sucesso: {AvaliacaoId}", avaliacao.Id);

                return CreatedAtAction(nameof(GetById), new { id = avaliacao.Id }, resource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar avaliação");
                return BadRequest(new { message = "Erro ao criar avaliação", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResourceDto<AvaliacaoDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResourceDto<AvaliacaoDto>>> Update(
            Guid id,
            [FromBody] UpdateAvaliacaoDto dto)
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

                var avaliacao = await _avaliacaoService.UpdateAsync(dto);

                var resource = new ResourceDto<AvaliacaoDto>(avaliacao);
                resource.Links = LinkGenerator.GenerateAvaliacaoLinks(avaliacao.Id, Url);

                _logger.LogInformation("Avaliação atualizada com sucesso: {AvaliacaoId}", avaliacao.Id);

                return Ok(resource);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Avaliação com ID {id} não encontrada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar avaliação {AvaliacaoId}", id);
                return BadRequest(new { message = "Erro ao atualizar avaliação", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _avaliacaoService.DeleteAsync(id);

                _logger.LogInformation("Avaliação deletada com sucesso: {AvaliacaoId}", id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Avaliação com ID {id} não encontrada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar avaliação {AvaliacaoId}", id);
                return BadRequest(new { message = "Erro ao deletar avaliação", error = ex.Message });
            }
        }
    }
}