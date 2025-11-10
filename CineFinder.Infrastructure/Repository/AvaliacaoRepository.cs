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
    public class AvaliacaoRepository : Repository<Avaliacao>, IAvaliacaoRepository
    {
        public AvaliacaoRepository(CineFinderDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Avaliacao>> GetByFilmeIdAsync(Guid filmeId)
        {
            return await _context.Avaliacoes
                .Include(a => a.Usuario)
                .Where(a => a.FilmeId == filmeId)
                .OrderByDescending(a => a.DataAvaliacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Avaliacao>> GetByUsuarioIdAsync(Guid usuarioId)
        {
            return await _context.Avaliacoes
                .Include(a => a.Filme)
                .ThenInclude(f => f.FilmeGeneros)
                .ThenInclude(fg => fg.Genero)
                .Where(a => a.UsuarioId == usuarioId)
                .OrderByDescending(a => a.DataAvaliacao)
                .ToListAsync();
        }

        public async Task<decimal> GetNotaMediaFilmeAsync(Guid filmeId)
        {
            var avaliacoes = await _context.Avaliacoes
                .Where(a => a.FilmeId == filmeId)
                .ToListAsync();

            if (!avaliacoes.Any())
                return 0;

            return (decimal)avaliacoes.Average(a => a.Nota);
        }

        public async Task<bool> UsuarioJaAvaliouAsync(Guid usuarioId, Guid filmeId)
        {
            return await _context.Avaliacoes
                .AnyAsync(a => a.UsuarioId == usuarioId && a.FilmeId == filmeId);
        }

        public async Task<(IEnumerable<Avaliacao> avaliacoes, int totalCount)> SearchWithFiltersAsync(
            Guid? filmeId = null,
            Guid? usuarioId = null,
            int? notaMinima = null,
            int? notaMaxima = null,
            DateTime? dataAvaliacaoInicio = null,
            DateTime? dataAvaliacaoFim = null,
            bool? temComentario = null,
            string? orderBy = null,
            bool orderDescending = false,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _context.Avaliacoes
                .Include(a => a.Usuario)
                .Include(a => a.Filme)
                .AsQueryable();

            // Aplicar filtros
            if (filmeId.HasValue)
            {
                query = query.Where(a => a.FilmeId == filmeId.Value);
            }

            if (usuarioId.HasValue)
            {
                query = query.Where(a => a.UsuarioId == usuarioId.Value);
            }

            if (notaMinima.HasValue)
            {
                query = query.Where(a => a.Nota >= notaMinima.Value);
            }

            if (notaMaxima.HasValue)
            {
                query = query.Where(a => a.Nota <= notaMaxima.Value);
            }

            if (dataAvaliacaoInicio.HasValue)
            {
                query = query.Where(a => a.DataAvaliacao >= dataAvaliacaoInicio.Value);
            }

            if (dataAvaliacaoFim.HasValue)
            {
                query = query.Where(a => a.DataAvaliacao <= dataAvaliacaoFim.Value);
            }

            if (temComentario.HasValue)
            {
                if (temComentario.Value)
                {
                    query = query.Where(a => !string.IsNullOrEmpty(a.Comentario));
                }
                else
                {
                    query = query.Where(a => string.IsNullOrEmpty(a.Comentario));
                }
            }

            // Contar total antes da paginação
            var totalCount = await query.CountAsync();

            // Aplicar ordenação
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                switch (orderBy.ToLower())
                {
                    case "nota":
                        query = orderDescending ? query.OrderByDescending(a => a.Nota) : query.OrderBy(a => a.Nota);
                        break;
                    case "data":
                        query = orderDescending ? query.OrderByDescending(a => a.DataAvaliacao) : query.OrderBy(a => a.DataAvaliacao);
                        break;
                    default:
                        query = query.OrderByDescending(a => a.DataAvaliacao);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(a => a.DataAvaliacao);
            }

            // Aplicar paginação
            var avaliacoes = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (avaliacoes, totalCount);
        }
    }
}