using CineFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineFinder.Domain.Interfaces
{
    public interface IAvaliacaoRepository : IRepository<Avaliacao>
    {
        Task<IEnumerable<Avaliacao>> GetByFilmeIdAsync(Guid filmeId);
        Task<IEnumerable<Avaliacao>> GetByUsuarioIdAsync(Guid usuarioId);
        Task<decimal> GetNotaMediaFilmeAsync(Guid filmeId);
        Task<bool> UsuarioJaAvaliouAsync(Guid usuarioId, Guid filmeId);
    }
}