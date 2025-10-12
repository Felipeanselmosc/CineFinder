using CineFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineFinder.Domain.Interfaces
{
    public interface IFilmeRepository : IRepository<Filme>
    {
        Task<IEnumerable<Filme>> GetByGeneroAsync(Guid generoId);
        Task<Filme?> GetByTituloAsync(string titulo);
        Task<Filme?> GetWithGenerosAsync(Guid filmeId);
        Task<Filme?> GetWithAvaliacoesAsync(Guid filmeId);
        Task<IEnumerable<Filme>> SearchByTituloAsync(string titulo);
        Task<IEnumerable<Filme>> GetTopRatedAsync(int top);
        Task<Filme?> GetByTmdbIdAsync(int tmdbId);
    }
}