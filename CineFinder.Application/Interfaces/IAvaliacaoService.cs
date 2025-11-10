using CineFinder.Application.DTOs.Avaliacao;
using CineFinder.Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineFinder.Application.Interfaces
{
    public interface IAvaliacaoService
    {
        Task<AvaliacaoDto> GetByIdAsync(Guid id);
        Task<AvaliacaoDto> CreateAsync(Guid usuarioId, CreateAvaliacaoDto dto);
        Task<AvaliacaoDto> UpdateAsync(Guid id, Guid usuarioId, UpdateAvaliacaoDto dto);
        Task<bool> DeleteAsync(Guid id, Guid usuarioId);
        Task<IEnumerable<AvaliacaoDto>> GetAllAsync();
        Task<IEnumerable<AvaliacaoDto>> GetByUsuarioAsync(Guid usuarioId);
        Task<IEnumerable<AvaliacaoSimplificadaDto>> GetByFilmeAsync(Guid filmeId);
        Task<PagedResult<AvaliacaoDto>> SearchAsync(AvaliacaoSearchParameters parameters);
    }
}