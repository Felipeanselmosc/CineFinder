using CineFinder.Application.DTOs.Lista;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineFinder.Application.Interfaces
{
    public interface IListaService
    {
        Task<ListaDto> GetByIdAsync(Guid id);
        Task<ListaDetalhadaDto> GetDetalhadaAsync(Guid id);
        Task<IEnumerable<ListaDto>> GetByUsuarioAsync(Guid usuarioId);
        Task<IEnumerable<ListaDto>> GetPublicasAsync();
        Task<ListaDto> CreateAsync(Guid usuarioId, CreateListaDto dto);
        Task<ListaDto> UpdateAsync(Guid id, Guid usuarioId, UpdateListaDto dto);
        Task<bool> DeleteAsync(Guid id, Guid usuarioId);
        Task AdicionarFilmeAsync(Guid listaId, Guid usuarioId, AdicionarFilmeListaDto dto);
        Task RemoverFilmeAsync(Guid listaId, Guid usuarioId, Guid filmeId);
    }
}