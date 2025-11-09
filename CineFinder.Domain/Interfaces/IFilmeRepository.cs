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
        Task<(IEnumerable<Filme> filmes, int totalCount)> SearchWithFiltersAsync(
            string? titulo = null,
            Guid? generoId = null,
            int? anoInicio = null,
            int? anoFim = null,
            int? notaMinimaMedia = null,
            int? duracaoMinima = null,
            int? duracaoMaxima = null,
            string? diretor = null,
            List<Guid>? generosIds = null,
            string? orderBy = null,
            bool orderDescending = false,
            int pageNumber = 1,
            int pageSize = 10);
    }
}