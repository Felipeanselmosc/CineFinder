using CineFinder.Application.DTOs.Genero;
using CineFinder.Application.Interfaces;
using CineFinder.Application.Models;
using CineFinder.Domain.Entities;
using CineFinder.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<GeneroDto> UpdateAsync(UpdateGeneroDto dto)
        {
            var genero = await _generoRepository.GetByIdAsync(dto.Id);
            if (genero == null)
                throw new KeyNotFoundException("Gênero não encontrado");

            genero.Nome = dto.Nome;
            genero.Descricao = dto.Descricao;
            genero.TmdbGeneroId = dto.TmdbGeneroId;

            await _generoRepository.UpdateAsync(genero);
            return MapToDto(genero);
        }

        public async Task DeleteAsync(Guid id)
        {
            var genero = await _generoRepository.GetByIdAsync(id);
            if (genero == null)
                throw new KeyNotFoundException("Gênero não encontrado");

            await _generoRepository.DeleteAsync(id);
        }

        public async Task<PagedResult<GeneroDto>> SearchAsync(GeneroSearchParameters parameters)
        {
            var (generos, totalCount) = await _generoRepository.SearchWithFiltersAsync(
                nome: parameters.Nome,
                ativo: parameters.Ativo,
                orderBy: parameters.OrderBy,
                orderDescending: parameters.OrderDescending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize
            );

            var generosDto = generos.Select(MapToDto).ToList();

            return new PagedResult<GeneroDto>(
                generosDto,
                totalCount,
                parameters.PageNumber,
                parameters.PageSize
            );
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