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
    }
}