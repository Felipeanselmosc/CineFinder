using CineFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineFinder.Domain.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<Usuario?> GetByUsernameAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<(IEnumerable<Usuario> usuarios, int totalCount)> SearchWithFiltersAsync(
            string? nome = null,
            string? email = null,
            DateTime? dataCriacaoInicio = null,
            DateTime? dataCriacaoFim = null,
            string? orderBy = null,
            bool orderDescending = false,
            int pageNumber = 1,
            int pageSize = 10);
    }
}