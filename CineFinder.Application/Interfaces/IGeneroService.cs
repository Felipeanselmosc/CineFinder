using CineFinder.Application.DTOs.Genero;
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
    }
}