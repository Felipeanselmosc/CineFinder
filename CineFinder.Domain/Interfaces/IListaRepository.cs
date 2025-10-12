using CineFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineFinder.Domain.Interfaces
{
    public interface IListaRepository : IRepository<Lista>
    {
        Task<IEnumerable<Lista>> GetByUsuarioIdAsync(Guid usuarioId);
        Task<Lista?> GetWithFilmesAsync(Guid listaId);
        Task<IEnumerable<Lista>> GetListasPublicasAsync();
        Task AdicionarFilmeAsync(Guid listaId, Guid filmeId);
        Task RemoverFilmeAsync(Guid listaId, Guid filmeId);
    }
}