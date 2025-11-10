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
        Task AdicionarFilmeAsync(Guid listaId, Guid filmeId);
        Task RemoverFilmeAsync(Guid listaId, Guid filmeId);

        // Métodos adicionados
        Task<IEnumerable<Lista>> GetAllAsync();
        Task<IEnumerable<Lista>> GetPublicasAsync();
    }
}