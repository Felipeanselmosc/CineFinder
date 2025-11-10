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
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(CineFinderDbContext context) : base(context)
        {
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario?> GetByUsernameAsync(string username)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Nome == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Email == email);
        }

        public async Task<(IEnumerable<Usuario> usuarios, int totalCount)> SearchWithFiltersAsync(
            string? nome = null,
            string? email = null,
            DateTime? dataCriacaoInicio = null,
            DateTime? dataCriacaoFim = null,
            string? orderBy = null,
            bool orderDescending = false,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _context.Usuarios.AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(u => u.Nome.Contains(nome));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(u => u.Email.Contains(email));
            }

            if (dataCriacaoInicio.HasValue)
            {
                query = query.Where(u => u.DataCriacao >= dataCriacaoInicio.Value);
            }

            if (dataCriacaoFim.HasValue)
            {
                query = query.Where(u => u.DataCriacao <= dataCriacaoFim.Value);
            }

            // Contar total antes da paginação
            var totalCount = await query.CountAsync();

            // Aplicar ordenação
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                switch (orderBy.ToLower())
                {
                    case "nome":
                        query = orderDescending ? query.OrderByDescending(u => u.Nome) : query.OrderBy(u => u.Nome);
                        break;
                    case "email":
                        query = orderDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email);
                        break;
                    case "data":
                        query = orderDescending ? query.OrderByDescending(u => u.DataCriacao) : query.OrderBy(u => u.DataCriacao);
                        break;
                    default:
                        query = query.OrderBy(u => u.Nome);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(u => u.Nome);
            }

            // Aplicar paginação
            var usuarios = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (usuarios, totalCount);
        }
    }
}