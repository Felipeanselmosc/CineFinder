using CineFinder.Domain.Entities;
using CineFinder.Domain.Interfaces;
using CineFinder.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<(IEnumerable<Genero> generos, int totalCount)> SearchWithFiltersAsync(
            string? nome = null,
            bool? ativo = null,
            string? orderBy = null,
            bool orderDescending = false,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _context.Generos
                .Include(g => g.FilmeGeneros)
                .AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(g => g.Nome.Contains(nome));
            }

            // Contar total antes da paginação
            var totalCount = await query.CountAsync();

            // Aplicar ordenação
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                switch (orderBy.ToLower())
                {
                    case "nome":
                        query = orderDescending ? query.OrderByDescending(g => g.Nome) : query.OrderBy(g => g.Nome);
                        break;
                    case "filmes":
                        query = orderDescending 
                            ? query.OrderByDescending(g => g.FilmeGeneros.Count) 
                            : query.OrderBy(g => g.FilmeGeneros.Count);
                        break;
                    default:
                        query = query.OrderBy(g => g.Nome);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(g => g.Nome);
            }

            // Aplicar paginação
            var generos = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (generos, totalCount);
        }
    }
}