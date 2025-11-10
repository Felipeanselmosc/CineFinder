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
        Task<(IEnumerable<Genero> generos, int totalCount)> SearchWithFiltersAsync(
            string? nome = null,
            bool? ativo = null,
            string? orderBy = null,
            bool orderDescending = false,
            int pageNumber = 1,
            int pageSize = 10);
    }
}