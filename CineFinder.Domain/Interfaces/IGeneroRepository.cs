using CineFinder.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineFinder.Domain.Interfaces
{
    public interface IGeneroRepository : IRepository<Genero>
    {
        Task<Genero?> GetByNomeAsync(string nome);
        Task<IEnumerable<Genero>> GetGenerosPopularesAsync();
        Task<Genero?> GetByTmdbIdAsync(int tmdbId);
    }
}