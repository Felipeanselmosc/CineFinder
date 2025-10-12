using CineFinder.Application.DTOs.Filme;
using CineFinder.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CineFinder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacoesController : ControllerBase
    {
        private readonly IAvaliacaoService _avaliacaoService;

        public AvaliacoesController(IAvaliacaoService avaliacaoService)
        {
            _avaliacaoService = avaliacaoService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] Guid usuarioId, [FromBody] CreateAvaliacaoDto dto)
        {
            try
            {
                var avaliacao = await _avaliacaoService.CreateAsync(usuarioId, dto);
                return CreatedAtAction(nameof(GetByUsuario), new { usuarioId }, avaliacao);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromQuery] Guid usuarioId, [FromBody] UpdateAvaliacaoDto dto)
        {
            try
            {
                var avaliacao = await _avaliacaoService.UpdateAsync(id, usuarioId, dto);
                return Ok(avaliacao);
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
                await _avaliacaoService.DeleteAsync(id, usuarioId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(Guid usuarioId)
        {
            try
            {
                var avaliacoes = await _avaliacaoService.GetByUsuarioAsync(usuarioId);
                return Ok(avaliacoes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("filme/{filmeId}")]
        public async Task<IActionResult> GetByFilme(Guid filmeId)
        {
            try
            {
                var avaliacoes = await _avaliacaoService.GetByFilmeAsync(filmeId);
                return Ok(avaliacoes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}