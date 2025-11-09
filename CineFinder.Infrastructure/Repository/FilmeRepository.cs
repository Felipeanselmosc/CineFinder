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

        public async Task<(IEnumerable<Filme> filmes, int totalCount)> SearchWithFiltersAsync(
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
            int pageSize = 10)
        {
            var query = _context.Filmes
                .Include(f => f.FilmeGeneros)
                .ThenInclude(fg => fg.Genero)
                .AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrWhiteSpace(titulo))
            {
                query = query.Where(f => f.Titulo.Contains(titulo));
            }

            if (generoId.HasValue)
            {
                query = query.Where(f => f.FilmeGeneros.Any(fg => fg.GeneroId == generoId.Value));
            }

            if (generosIds != null && generosIds.Any())
            {
                query = query.Where(f => f.FilmeGeneros.Any(fg => generosIds.Contains(fg.GeneroId)));
            }

            if (anoInicio.HasValue)
            {
                query = query.Where(f => f.DataLancamento.HasValue && f.DataLancamento.Value.Year >= anoInicio.Value);
            }

            if (anoFim.HasValue)
            {
                query = query.Where(f => f.DataLancamento.HasValue && f.DataLancamento.Value.Year <= anoFim.Value);
            }

            if (notaMinimaMedia.HasValue)
            {
                query = query.Where(f => f.NotaMedia.HasValue && f.NotaMedia.Value >= notaMinimaMedia.Value);
            }

            if (duracaoMinima.HasValue)
            {
                query = query.Where(f => f.Duracao.HasValue && f.Duracao.Value >= duracaoMinima.Value);
            }

            if (duracaoMaxima.HasValue)
            {
                query = query.Where(f => f.Duracao.HasValue && f.Duracao.Value <= duracaoMaxima.Value);
            }

            if (!string.IsNullOrWhiteSpace(diretor))
            {
                query = query.Where(f => f.Diretor.Contains(diretor));
            }

            // Contar total antes da paginação
            var totalCount = await query.CountAsync();

            // Aplicar ordenação
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                switch (orderBy.ToLower())
                {
                    case "titulo":
                        query = orderDescending ? query.OrderByDescending(f => f.Titulo) : query.OrderBy(f => f.Titulo);
                        break;
                    case "data":
                    case "datalancamento":
                        query = orderDescending ? query.OrderByDescending(f => f.DataLancamento) : query.OrderBy(f => f.DataLancamento);
                        break;
                    case "nota":
                    case "notamedia":
                        query = orderDescending ? query.OrderByDescending(f => f.NotaMedia) : query.OrderBy(f => f.NotaMedia);
                        break;
                    case "duracao":
                        query = orderDescending ? query.OrderByDescending(f => f.Duracao) : query.OrderBy(f => f.Duracao);
                        break;
                    default:
                        query = query.OrderBy(f => f.Titulo);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(f => f.Titulo);
            }

            // Aplicar paginação
            var filmes = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (filmes, totalCount);
        }
    }
}