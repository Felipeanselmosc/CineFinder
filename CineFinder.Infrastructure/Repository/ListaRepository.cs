using CineFinder.Application.Models;
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
    public class ListaRepository : Repository<Lista>, IListaRepository
    {
        public ListaRepository(CineFinderDbContext context) : base(context)
        {
        }

        // ===== MÉTODO NOVO: GetAllAsync =====
        public async Task<IEnumerable<Lista>> GetAllAsync()
        {
            return await _context.Listas
                .Include(l => l.Usuario)
                .OrderByDescending(l => l.DataCriacao)
                .ToListAsync();
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

        // ===== MÉTODO NOVO: GetPublicasAsync (alias para GetListasPublicasAsync) =====
        public async Task<IEnumerable<Lista>> GetPublicasAsync()
        {
            return await GetListasPublicasAsync();
        }

        // Método original mantido
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

        // ===== MÉTODO NOVO: SearchAsync (wrapper simplificado) =====
        public async Task<(IEnumerable<Lista> listas, int totalCount)> SearchAsync(ListaSearchParameters parameters)
        {
            // garante que não é nulo
            parameters ??= new ListaSearchParameters();

            // Chama o método SearchWithFiltersAsync mapeando os parâmetros
            return await SearchWithFiltersAsync(
                usuarioId: parameters.UsuarioId,
                // isPublica REMOVIDO porque o model não tem essa propriedade
                nome: parameters.Nome,
                dataCriacaoInicio: parameters.DataCriacaoInicio,
                dataCriacaoFim: parameters.DataCriacaoFim,
                orderBy: parameters.OrderBy,
                orderDescending: parameters.OrderDescending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize
            );
        }

        // Método original mantido para retrocompatibilidade
        public async Task<(IEnumerable<Lista> listas, int totalCount)> SearchWithFiltersAsync(
            Guid? usuarioId = null,
            bool? isPublica = null,
            string? nome = null,
            DateTime? dataCriacaoInicio = null,
            DateTime? dataCriacaoFim = null,
            string? orderBy = null,
            bool orderDescending = false,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _context.Listas
                .Include(l => l.Usuario)
                .AsQueryable();

            // Aplicar filtros
            if (usuarioId.HasValue)
            {
                query = query.Where(l => l.UsuarioId == usuarioId.Value);
            }

            if (isPublica.HasValue)
            {
                query = query.Where(l => l.IsPublica == isPublica.Value);
            }

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(l => l.Nome.Contains(nome));
            }

            if (dataCriacaoInicio.HasValue)
            {
                query = query.Where(l => l.DataCriacao >= dataCriacaoInicio.Value);
            }

            if (dataCriacaoFim.HasValue)
            {
                query = query.Where(l => l.DataCriacao <= dataCriacaoFim.Value);
            }

            // Contar total antes da paginação
            var totalCount = await query.CountAsync();

            // Aplicar ordenação
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                switch (orderBy.ToLower())
                {
                    case "nome":
                        query = orderDescending ? query.OrderByDescending(l => l.Nome) : query.OrderBy(l => l.Nome);
                        break;
                    case "data":
                        query = orderDescending ? query.OrderByDescending(l => l.DataCriacao) : query.OrderBy(l => l.DataCriacao);
                        break;
                    default:
                        query = query.OrderByDescending(l => l.DataCriacao);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(l => l.DataCriacao);
            }

            // Aplicar paginação
            var listas = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (listas, totalCount);
        }
    }
}
