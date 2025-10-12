using CineFinder.Application.DTOs.Filme;
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
        Task<FilmeDto> CreateAsync(CreateFilmeDto dto);
    }
}