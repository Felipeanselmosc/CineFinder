using CineFinder.Application.DTOs.Usuario;
using CineFinder.Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineFinder.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDto> GetByIdAsync(Guid id);
        Task<IEnumerable<UsuarioDto>> GetAllAsync();
        Task<UsuarioDto> CreateAsync(CreateUsuarioDto dto);
        Task<UsuarioDto> UpdateAsync(Guid id, UpdateUsuarioDto dto);
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<PagedResult<UsuarioDto>> SearchAsync(UsuarioSearchParameters parameters);
    }
}