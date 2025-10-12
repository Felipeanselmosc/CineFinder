using CineFinder.Domain.Entities;
using CineFinder.Domain.Interfaces;
using CineFinder.Infrastructure.Data;
using CineFinder.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineFinder.Infrastructure.Repositories
{
    public class FilmeRepository : Repository<Filme>, IFilmeRepository
    {
        public FilmeRepository(CineFinderDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Filme>> GetByGeneroAsync(Guid generoId)
        {
            return await _context.Filmes
                .Include(f => f.FilmeGeneros)
                .ThenInclude(fg => fg.Genero)
                .Where(f => f.FilmeGeneros.Any(fg => fg.GeneroId == generoId))
                .ToListAsync();
        }

        public async Task<Filme?> GetByTituloAsync(string titulo)
        {
            return await _context.Filmes
                .Include(f => f.FilmeGeneros)
                .ThenInclude(fg => fg.Genero)
                .FirstOrDefaultAsync(f => f.Titulo == titulo);
        }

        public async Task<Filme?> GetWithGenerosAsync(Guid filmeId)
        {
            return await _context.Filmes
                .Include(f => f.FilmeGeneros)
                .ThenInclude(fg => fg.Genero)
                .FirstOrDefaultAsync(f => f.Id == filmeId);
        }

        public async Task<Filme?> GetWithAvaliacoesAsync(Guid filmeId)
        {
            return await _context.Filmes
                .Include(f => f.Avaliacoes)
                .ThenInclude(a => a.Usuario)
                .Include(f => f.FilmeGeneros)
                .ThenInclude(fg => fg.Genero)
                .FirstOrDefaultAsync(f => f.Id == filmeId);
        }

        public async Task<IEnumerable<Filme>> SearchByTituloAsync(string titulo)
        {
            return await _context.Filmes
                .Include(f => f.FilmeGeneros)
                .ThenInclude(fg => fg.Genero)
                .Where(f => f.Titulo.Contains(titulo))
                .ToListAsync();
        }

        public async Task<IEnumerable<Filme>> GetTopRatedAsync(int top)
        {
            return await _context.Filmes
                .Include(f => f.FilmeGeneros)
                .ThenInclude(fg => fg.Genero)
                .OrderByDescending(f => f.NotaMedia)
                .Take(top)
                .ToListAsync();
        }

        public async Task<Filme?> GetByTmdbIdAsync(int tmdbId)
        {
            return await _context.Filmes
                .Include(f => f.FilmeGeneros)
                .ThenInclude(fg => fg.Genero)
                .FirstOrDefaultAsync(f => f.TmdbId == tmdbId);
        }
    }
}