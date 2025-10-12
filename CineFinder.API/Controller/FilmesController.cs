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
    public class FilmesController : ControllerBase
    {
        private readonly IFilmeService _filmeService;

        public FilmesController(IFilmeService filmeService)
        {
            _filmeService = filmeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var filme = await _filmeService.GetByIdAsync(id);
                return Ok(filme);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/detalhado")]
        public async Task<IActionResult> GetDetalhado(Guid id)
        {
            try
            {
                var filme = await _filmeService.GetDetalhadoAsync(id);
                return Ok(filme);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Search([FromQuery] string titulo)
        {
            try
            {
                var filmes = await _filmeService.SearchAsync(titulo);
                return Ok(filmes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("genero/{generoId}")]
        public async Task<IActionResult> GetByGenero(Guid generoId)
        {
            try
            {
                var filmes = await _filmeService.GetByGeneroAsync(generoId);
                return Ok(filmes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("top-rated")]
        public async Task<IActionResult> GetTopRated([FromQuery] int top = 10)
        {
            try
            {
                var filmes = await _filmeService.GetTopRatedAsync(top);
                return Ok(filmes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFilmeDto dto)
        {
            try
            {
                var filme = await _filmeService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = filme.Id }, filme);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}