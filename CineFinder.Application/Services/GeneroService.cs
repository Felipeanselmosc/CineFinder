using CineFinder.Application.DTOs.Genero;
using CineFinder.Application.Interfaces;
using CineFinder.Domain.Entities;
using CineFinder.Domain.Interfaces;
namespace CineFinder.Application.Services
{
    public class GeneroService : IGeneroService
    {
        private readonly IGeneroRepository _generoRepository;

        public GeneroService(IGeneroRepository generoRepository)
        {
            _generoRepository = generoRepository;
        }

        public async Task<GeneroDto> GetByIdAsync(Guid id)
        {
            var genero = await _generoRepository.GetByIdAsync(id);
            if (genero == null)
                throw new Exception("Gênero não encontrado");

            return MapToDto(genero);
        }

        public async Task<IEnumerable<GeneroDto>> GetAllAsync()
        {
            var generos = await _generoRepository.GetAllAsync();
            return generos.Select(MapToDto);
        }

        public async Task<IEnumerable<GeneroDto>> GetPopularesAsync()
        {
            var generos = await _generoRepository.GetGenerosPopularesAsync();
            return generos.Select(MapToDto);
        }

        public async Task<GeneroDto> CreateAsync(CreateGeneroDto dto)
        {
            var generoExistente = await _generoRepository.GetByTmdbIdAsync(dto.TmdbGeneroId);
            if (generoExistente != null)
                throw new Exception("Gênero já cadastrado");

            var genero = new Genero
            {
                TmdbGeneroId = dto.TmdbGeneroId,
                Nome = dto.Nome,
                Descricao = dto.Descricao
            };

            await _generoRepository.AddAsync(genero);
            return MapToDto(genero);
        }

        private GeneroDto MapToDto(Genero genero)
        {
            return new GeneroDto
            {
                Id = genero.Id,
                TmdbGeneroId = genero.TmdbGeneroId,
                Nome = genero.Nome,
                Descricao = genero.Descricao
            };
        }
    }
}