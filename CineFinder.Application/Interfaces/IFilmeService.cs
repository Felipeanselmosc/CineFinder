using CineFinder.Application.DTOs.Filme;
using CineFinder.Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineFinder.Application.Interfaces
{
    public interface IFilmeService
    {
        Task<FilmeDto> GetByIdAsync(Guid id);
        Task<FilmeDetalhadoDto> GetDetalhadoAsync(Guid id);
        Task<IEnumerable<FilmeDto>> SearchAsync(string titulo);
        Task<IEnumerable<FilmeDto>> GetByGeneroAsync(Guid generoId);
        Task<IEnumerable<FilmeDto>> GetTopRatedAsync(int top = 10);
        Task<IEnumerable<FilmeDto>> GetAllAsync();
        Task<FilmeDto> CreateAsync(CreateFilmeDto dto);
        Task<FilmeDto> UpdateAsync(UpdateFilmeDto dto);
        Task DeleteAsync(Guid id);

        // Novo método com paginação e filtros
        Task<PagedResult<FilmeDto>> SearchAsync(FilmeSearchParameters parameters);
    }
}