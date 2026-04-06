using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.Application.DTOs.Usuario;
using CineFinder.Application.Models;
using CineFinder.API.Models;
using CineFinder.API.Helpers;

namespace CineFinder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(IUsuarioService usuarioService, ILogger<UsuariosController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<ResourceDto<UsuarioDto>>>> Search([FromQuery] UsuarioSearchParameters parameters)
        {
            try
            {
                var pagedResult = await _usuarioService.SearchAsync(parameters);
                var itens = pagedResult.Items.Select(u => { var r = new ResourceDto<UsuarioDto>(u); r.Links = HateoasLinkGenerator.GenerateUsuarioLinks(u.Id, Url); return r; }).ToList();
                var result = new PagedResult<ResourceDto<UsuarioDto>>(itens, pagedResult.TotalCount, pagedResult.PageNumber, pagedResult.PageSize);
                var pl = HateoasLinkGenerator.GeneratePaginationLinks(Url, "Search", "Usuarios", result.PageNumber, result.PageSize, result.TotalPages, parameters);
                result.FirstPage = pl.GetValueOrDefault("firstPage"); result.PreviousPage = pl.GetValueOrDefault("previousPage");
                result.NextPage = pl.GetValueOrDefault("nextPage"); result.LastPage = pl.GetValueOrDefault("lastPage");
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao buscar usuarios", error = ex.Message }); }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceDto<UsuarioDto>>>> GetAll()
        {
            try
            {
                var usuarios = await _usuarioService.GetAllAsync();
                var result = usuarios.Select(u => { var r = new ResourceDto<UsuarioDto>(u); r.Links = HateoasLinkGenerator.GenerateUsuarioLinks(u.Id, Url); return r; });
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao obter usuarios", error = ex.Message }); }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResourceDto<UsuarioDto>>> GetById(Guid id)
        {
            try
            {
                var usuario = await _usuarioService.GetByIdAsync(id);
                var resource = new ResourceDto<UsuarioDto>(usuario);
                resource.Links = HateoasLinkGenerator.GenerateUsuarioLinks(usuario.Id, Url);
                return Ok(resource);
            }
            catch (KeyNotFoundException) { return NotFound(new { message = $"Usuario com ID {id} nao encontrado" }); }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao obter usuario", error = ex.Message }); }
        }

        [HttpPost]
        public async Task<ActionResult<ResourceDto<UsuarioDto>>> Create([FromBody] CreateUsuarioDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var usuario = await _usuarioService.CreateAsync(dto);
                var resource = new ResourceDto<UsuarioDto>(usuario);
                resource.Links = HateoasLinkGenerator.GenerateUsuarioLinks(usuario.Id, Url);
                return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, resource);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao criar usuario", error = ex.Message }); }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResourceDto<UsuarioDto>>> Update(Guid id, [FromBody] UpdateUsuarioDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var usuario = await _usuarioService.UpdateAsync(id, dto);
                var resource = new ResourceDto<UsuarioDto>(usuario);
                resource.Links = HateoasLinkGenerator.GenerateUsuarioLinks(usuario.Id, Url);
                return Ok(resource);
            }
            catch (KeyNotFoundException) { return NotFound(new { message = $"Usuario com ID {id} nao encontrado" }); }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao atualizar usuario", error = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _usuarioService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao deletar usuario", error = ex.Message }); }
        }
    }
}
