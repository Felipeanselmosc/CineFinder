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
    public class ListaRepository : Repository<Lista>, IListaRepository
    {
        public ListaRepository(CineFinderDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Lista>> GetByUsuarioIdAsync(Guid usuarioId)
        {
            return await _context.Listas
                .Include(l => l.Usuario)
                .Where(l => l.UsuarioId == usuarioId)
                .OrderByDescending(l => l.DataCriacao)
                .ToListAsync();
        }

        public async Task<Lista?> GetWithFilmesAsync(Guid listaId)
        {
            return await _context.Listas
                .Include(l => l.Usuario)
                .Include(l => l.ListaFilmes)
                .ThenInclude(lf => lf.Filme)
                .ThenInclude(f => f.FilmeGeneros)
                .ThenInclude(fg => fg.Genero)
                .FirstOrDefaultAsync(l => l.Id == listaId);
        }

        public async Task<IEnumerable<Lista>> GetListasPublicasAsync()
        {
            return await _context.Listas
                .Include(l => l.Usuario)
                .Where(l => l.IsPublica)
                .OrderByDescending(l => l.DataCriacao)
                .ToListAsync();
        }

        public async Task AdicionarFilmeAsync(Guid listaId, Guid filmeId)
        {
            var lista = await _context.Listas
                .Include(l => l.ListaFilmes)
                .FirstOrDefaultAsync(l => l.Id == listaId);

            if (lista == null)
                throw new Exception("Lista não encontrada");

            if (lista.ListaFilmes.Any(lf => lf.FilmeId == filmeId))
                throw new Exception("Filme já está na lista");

            var ultimaOrdem = lista.ListaFilmes.Any()
                ? lista.ListaFilmes.Max(lf => lf.Ordem)
                : 0;

            var listaFilme = new ListaFilme
            {
                ListaId = listaId,
                FilmeId = filmeId,
                Ordem = ultimaOrdem + 1
            };

            _context.ListaFilmes.Add(listaFilme);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverFilmeAsync(Guid listaId, Guid filmeId)
        {
            var listaFilme = await _context.ListaFilmes
                .FirstOrDefaultAsync(lf => lf.ListaId == listaId && lf.FilmeId == filmeId);

            if (listaFilme != null)
            {
                _context.ListaFilmes.Remove(listaFilme);
                await _context.SaveChangesAsync();
            }
        }
    }
}