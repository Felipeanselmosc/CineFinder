using CineFinder.Application.DTOs.Genero;
using CineFinder.Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineFinder.Application.Interfaces
{
    public interface IGeneroService
    {
        Task<GeneroDto> GetByIdAsync(Guid id);
        Task<IEnumerable<GeneroDto>> GetAllAsync();
        Task<IEnumerable<GeneroDto>> GetPopularesAsync();
        Task<GeneroDto> CreateAsync(CreateGeneroDto dto);
        Task<GeneroDto> UpdateAsync(UpdateGeneroDto dto);
        Task DeleteAsync(Guid id);

        // Novo método com paginação e filtros
        Task<PagedResult<GeneroDto>> SearchAsync(GeneroSearchParameters parameters);
    }
}