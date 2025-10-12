using CineFinder.Application.DTOs.Usuario;
using System;
using System.Threading.Tasks;

namespace CineFinder.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDto> GetByIdAsync(Guid id);
        Task<UsuarioDto> CreateAsync(CreateUsuarioDto dto);
        Task<UsuarioDto> UpdateAsync(Guid id, UpdateUsuarioDto dto);
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}