using CineFinder.Domain.Entities;
using System.Threading.Tasks;

namespace CineFinder.Domain.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<Usuario?> GetByUsernameAsync(string username);
        Task<bool> EmailExistsAsync(string email);
    }
}