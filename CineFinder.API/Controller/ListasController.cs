using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.Application.DTOs.Lista;
using CineFinder.Application.Models;
using CineFinder.API.Models;
using CineFinder.API.Helpers;

namespace CineFinder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ListasController : ControllerBase
    {
        private readonly IListaService _listaService;
        private readonly ILogger<ListasController> _logger;
        private static readonly Guid _usuarioIdFake = Guid.Parse("00000000-0000-0000-0000-000000000001");

        public ListasController(IListaService listaService, ILogger<ListasController> logger)
        {
            _listaService = listaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceDto<ListaDto>>>> GetAll()
        {
            try
            {
                var listas = await _listaService.GetAllAsync();
                var result = listas.Select(l => { var r = new ResourceDto<ListaDto>(l); r.Links = HateoasLinkGenerator.GenerateListaLinks(l.Id, Url); return r; });
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao obter listas", error = ex.Message }); }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResourceDto<ListaDto>>> GetById(Guid id)
        {
            try
            {
                var lista = await _listaService.GetByIdAsync(id);
                if (lista == null) return NotFound(new { message = $"Lista com ID {id} nao encontrada" });
                var resource = new ResourceDto<ListaDto>(lista);
                resource.Links = HateoasLinkGenerator.GenerateListaLinks(lista.Id, Url);
                return Ok(resource);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao obter lista", error = ex.Message }); }
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<ResourceDto<ListaDto>>>> GetByUsuario(Guid usuarioId)
        {
            try
            {
                var listas = await _listaService.GetByUsuarioAsync(usuarioId);
                var result = listas.Select(l => { var r = new ResourceDto<ListaDto>(l); r.Links = HateoasLinkGenerator.GenerateListaLinks(l.Id, Url); return r; });
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao obter listas", error = ex.Message }); }
        }

        [HttpPost]
        public async Task<ActionResult<ResourceDto<ListaDto>>> Create([FromBody] CreateListaDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var lista = await _listaService.CreateAsync(_usuarioIdFake, dto);
                var resource = new ResourceDto<ListaDto>(lista);
                resource.Links = HateoasLinkGenerator.GenerateListaLinks(lista.Id, Url);
                return CreatedAtAction(nameof(GetById), new { id = lista.Id }, resource);
            }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao criar lista", error = ex.Message }); }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResourceDto<ListaDto>>> Update(Guid id, [FromBody] UpdateListaDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (id != dto.Id) return BadRequest(new { message = "ID nao corresponde" });
                var lista = await _listaService.UpdateAsync(id, _usuarioIdFake, dto);
                var resource = new ResourceDto<ListaDto>(lista);
                resource.Links = HateoasLinkGenerator.GenerateListaLinks(lista.Id, Url);
                return Ok(resource);
            }
            catch (KeyNotFoundException) { return NotFound(new { message = $"Lista com ID {id} nao encontrada" }); }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao atualizar lista", error = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _listaService.DeleteAsync(id, _usuarioIdFake);
                return NoContent();
            }
            catch (KeyNotFoundException) { return NotFound(new { message = $"Lista com ID {id} nao encontrada" }); }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao deletar lista", error = ex.Message }); }
        }

        [HttpPost("{listaId}/filmes/{filmeId}")]
        public async Task<IActionResult> AddFilme(Guid listaId, Guid filmeId)
        {
            try
            {
                var dto = new AdicionarFilmeListaDto { FilmeId = filmeId };
                await _listaService.AdicionarFilmeAsync(listaId, _usuarioIdFake, dto);
                return Ok(new { message = "Filme adicionado com sucesso" });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao adicionar filme", error = ex.Message }); }
        }

        [HttpDelete("{listaId}/filmes/{filmeId}")]
        public async Task<IActionResult> RemoveFilme(Guid listaId, Guid filmeId)
        {
            try
            {
                await _listaService.RemoverFilmeAsync(listaId, _usuarioIdFake, filmeId);
                return NoContent();
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (Exception ex) { return BadRequest(new { message = "Erro ao remover filme", error = ex.Message }); }
        }
    }
}
