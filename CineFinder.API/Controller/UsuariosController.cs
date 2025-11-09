using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.Application.DTOs.Usuario;
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
    /// Controller REST para gerenciamento de Usuários
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(
            IUsuarioService usuarioService,
            ILogger<UsuariosController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        /// <summary>
        /// Busca usuários com paginação e filtros
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(PagedResult<ResourceDto<UsuarioDto>>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PagedResult<ResourceDto<UsuarioDto>>>> Search(
            [FromQuery] UsuarioSearchParameters parameters)
        {
            try
            {
                _logger.LogInformation("Buscando usuários com parâmetros: {@Parameters}", parameters);

                var pagedResult = await _usuarioService.SearchAsync(parameters);

                var usuariosComLinks = pagedResult.Items.Select(usuario =>
                {
                    var resource = new ResourceDto<UsuarioDto>(usuario);
                    resource.Links = LinkGenerator.GenerateUsuarioLinks(usuario.Id, Url);
                    return resource;
                }).ToList();

                var result = new PagedResult<ResourceDto<UsuarioDto>>(
                    usuariosComLinks,
                    pagedResult.TotalCount,
                    pagedResult.PageNumber,
                    pagedResult.PageSize
                );

                var paginationLinks = LinkGenerator.GeneratePaginationLinks(
                    Url, "Search", "Usuarios",
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
                _logger.LogError(ex, "Erro ao buscar usuários");
                return BadRequest(new { message = "Erro ao buscar usuários", error = ex.Message });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResourceDto<UsuarioDto>>), 200)]
        public async Task<ActionResult<IEnumerable<ResourceDto<UsuarioDto>>>> GetAll()
        {
            try
            {
                var usuarios = await _usuarioService.GetAllAsync();

                var usuariosComLinks = usuarios.Select(usuario =>
                {
                    var resource = new ResourceDto<UsuarioDto>(usuario);
                    resource.Links = LinkGenerator.GenerateUsuarioLinks(usuario.Id, Url);
                    return resource;
                });

                return Ok(usuariosComLinks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter usuários");
                return BadRequest(new { message = "Erro ao obter usuários", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResourceDto<UsuarioDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResourceDto<UsuarioDto>>> GetById(Guid id)
        {
            try
            {
                var usuario = await _usuarioService.GetByIdAsync(id);

                var resource = new ResourceDto<UsuarioDto>(usuario);
                resource.Links = LinkGenerator.GenerateUsuarioLinks(usuario.Id, Url);

                return Ok(resource);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter usuário {UsuarioId}", id);
                return BadRequest(new { message = "Erro ao obter usuário", error = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResourceDto<UsuarioDto>), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResourceDto<UsuarioDto>>> Create([FromBody] CreateUsuarioDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var usuario = await _usuarioService.CreateAsync(dto);

                var resource = new ResourceDto<UsuarioDto>(usuario);
                resource.Links = LinkGenerator.GenerateUsuarioLinks(usuario.Id, Url);

                _logger.LogInformation("Usuário criado com sucesso: {UsuarioId} - {Nome}", usuario.Id, usuario.Nome);

                return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, resource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar usuário");
                return BadRequest(new { message = "Erro ao criar usuário", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResourceDto<UsuarioDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResourceDto<UsuarioDto>>> Update(
            Guid id,
            [FromBody] UpdateUsuarioDto dto)
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

                var usuario = await _usuarioService.UpdateAsync(dto);

                var resource = new ResourceDto<UsuarioDto>(usuario);
                resource.Links = LinkGenerator.GenerateUsuarioLinks(usuario.Id, Url);

                _logger.LogInformation("Usuário atualizado com sucesso: {UsuarioId} - {Nome}", usuario.Id, usuario.Nome);

                return Ok(resource);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar usuário {UsuarioId}", id);
                return BadRequest(new { message = "Erro ao atualizar usuário", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _usuarioService.DeleteAsync(id);

                _logger.LogInformation("Usuário deletado com sucesso: {UsuarioId}", id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar usuário {UsuarioId}", id);
                return BadRequest(new { message = "Erro ao deletar usuário", error = ex.Message });
            }
        }
    }
}