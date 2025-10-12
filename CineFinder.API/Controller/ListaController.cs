using CineFinder.Application.DTOs.Lista;
using CineFinder.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CineFinder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListasController : ControllerBase
    {
        private readonly IListaService _listaService;

        public ListasController(IListaService listaService)
        {
            _listaService = listaService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var lista = await _listaService.GetByIdAsync(id);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/detalhada")]
        public async Task<IActionResult> GetDetalhada(Guid id)
        {
            try
            {
                var lista = await _listaService.GetDetalhadaAsync(id);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(Guid usuarioId)
        {
            try
            {
                var listas = await _listaService.GetByUsuarioAsync(usuarioId);
                return Ok(listas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("publicas")]
        public async Task<IActionResult> GetPublicas()
        {
            try
            {
                var listas = await _listaService.GetPublicasAsync();
                return Ok(listas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] Guid usuarioId, [FromBody] CreateListaDto dto)
        {
            try
            {
                var lista = await _listaService.CreateAsync(usuarioId, dto);
                return CreatedAtAction(nameof(GetById), new { id = lista.Id }, lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromQuery] Guid usuarioId, [FromBody] UpdateListaDto dto)
        {
            try
            {
                var lista = await _listaService.UpdateAsync(id, usuarioId, dto);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid usuarioId)
        {
            try
            {
                await _listaService.DeleteAsync(id, usuarioId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/filmes")]
        public async Task<IActionResult> AdicionarFilme(Guid id, [FromQuery] Guid usuarioId, [FromBody] AdicionarFilmeListaDto dto)
        {
            try
            {
                await _listaService.AdicionarFilmeAsync(id, usuarioId, dto);
                return Ok(new { message = "Filme adicionado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}/filmes/{filmeId}")]
        public async Task<IActionResult> RemoverFilme(Guid id, Guid filmeId, [FromQuery] Guid usuarioId)
        {
            try
            {
                await _listaService.RemoverFilmeAsync(id, usuarioId, filmeId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}