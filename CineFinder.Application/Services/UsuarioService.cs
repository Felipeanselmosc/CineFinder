using CineFinder.Application.DTOs.Usuario;
using CineFinder.Application.Interfaces;
using CineFinder.Application.Models;
using CineFinder.Domain.Entities;
using CineFinder.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CineFinder.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioDto> GetByIdAsync(Guid id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                throw new Exception("Usuário não encontrado");

            return MapToDto(usuario);
        }

        public async Task<UsuarioDto> CreateAsync(CreateUsuarioDto dto)
        {
            if (await _usuarioRepository.EmailExistsAsync(dto.Email))
                throw new Exception("Email já cadastrado");

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = HashPassword(dto.Senha)
            };

            await _usuarioRepository.AddAsync(usuario);
            return MapToDto(usuario);
        }

        public async Task<UsuarioDto> UpdateAsync(Guid id, UpdateUsuarioDto dto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                throw new Exception("Usuário não encontrado");

            if (dto.Email != usuario.Email && await _usuarioRepository.EmailExistsAsync(dto.Email))
                throw new Exception("Email já cadastrado");

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email;
            usuario.DataAtualizacao = DateTime.UtcNow;

            await _usuarioRepository.UpdateAsync(usuario);
            return MapToDto(usuario);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(dto.Email);
            if (usuario == null)
                throw new Exception("Email ou senha inválidos");

            if (!VerifyPassword(dto.Senha, usuario.SenhaHash))
                throw new Exception("Email ou senha inválidos");

            var token = GenerateJwtToken(usuario);

            return new LoginResponseDto
            {
                Token = token,
                Usuario = MapToDto(usuario)
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _usuarioRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Select(MapToDto);
        }

        public async Task<PagedResult<UsuarioDto>> SearchAsync(UsuarioSearchParameters parameters)
        {
            var (usuarios, totalCount) = await _usuarioRepository.SearchWithFiltersAsync(
                nome: parameters.Nome,
                email: parameters.Email,
                dataCriacaoInicio: parameters.DataCadastroInicio,
                dataCriacaoFim: parameters.DataCadastroFim,
                orderBy: parameters.OrderBy,
                orderDescending: parameters.OrderDescending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize
            );

            var usuariosDto = usuarios.Select(MapToDto).ToList();

            return new PagedResult<UsuarioDto>(
                usuariosDto,
                totalCount,
                parameters.PageNumber,
                parameters.PageSize
            );
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hash = HashPassword(password);
            return hash == hashedPassword;
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            return "token-jwt-aqui";
        }

        private UsuarioDto MapToDto(Usuario usuario)
        {
            return new UsuarioDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                DataCriacao = usuario.DataCriacao
            };
        }
    }
}