using CineFinder.Domain.Entities;
using CineFinder.Domain.Interfaces;
using CineFinder.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CineFinder.Infrastructure.Repositories
{
    public class GeneroRepository : Repository<Genero>, IGeneroRepository
    {
        public GeneroRepository(CineFinderDbContext context) : base(context)
        {
        }

        public async Task<Genero?> GetByNomeAsync(string nome)
        {
            return await _context.Generos
                .FirstOrDefaultAsync(g => g.Nome.ToLower() == nome.ToLower());
        }

        public async Task<Genero?> GetByTmdbIdAsync(int tmdbId)
        {
            return await Task.FromResult<Genero?>(null);
        }

        public async Task<Genero?> GetWithFilmesAsync(Guid generoId)
        {
            return await _context.Generos
                .Include(g => g.FilmeGeneros)
                .ThenInclude(fg => fg.Filme)
                .FirstOrDefaultAsync(g => g.Id == generoId);
        }

        public async Task<IEnumerable<Genero>> GetGenerosPopularesAsync()
        {
            return await _context.Generos
                .Include(g => g.FilmeGeneros)
                .OrderByDescending(g => g.FilmeGeneros.Count)
                .Take(10)
                .ToListAsync();
        }
    }
}