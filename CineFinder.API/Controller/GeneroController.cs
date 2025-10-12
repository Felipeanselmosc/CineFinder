using CineFinder.Application.DTOs.Genero;
using CineFinder.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CineFinder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenerosController : ControllerBase
    {
        private readonly IGeneroService _generoService;

        public GenerosController(IGeneroService generoService)
        {
            _generoService = generoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var generos = await _generoService.GetAllAsync();
                return Ok(generos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var genero = await _generoService.GetByIdAsync(id);
                return Ok(genero);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("populares")]
        public async Task<IActionResult> GetPopulares()
        {
            try
            {
                var generos = await _generoService.GetPopularesAsync();
                return Ok(generos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGeneroDto dto)
        {
            try
            {
                var genero = await _generoService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = genero.Id }, genero);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}