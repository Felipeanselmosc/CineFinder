using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CineFinder.Application.DTOs.Usuario
{
    /// <summary>
    /// DTO básico de Usuário
    /// </summary>
    public class UsuarioDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataCriacao { get; set; }
    }

    /// <summary>
    /// DTO detalhado de Usuário
    /// </summary>
    public class UsuarioDetalhadoDto : UsuarioDto
    {
        public int TotalAvaliacoes { get; set; }
        public int TotalListas { get; set; }
        public List<GeneroPreferidoDto> GenerosPreferidos { get; set; } = new();
    }

    /// <summary>
    /// DTO para criar um novo Usuário
    /// </summary>
    public class CreateUsuarioDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres")]
        public string Senha { get; set; }
    }

    /// <summary>
    /// DTO para atualizar um Usuário existente
    /// </summary>
    public class UpdateUsuarioDto
    {
        [Required(ErrorMessage = "O ID é obrigatório")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
        public string Email { get; set; }
    }

    /// <summary>
    /// DTO para login
    /// </summary>
    public class LoginDto
    {
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        public string Senha { get; set; }
    }

    /// <summary>
    /// DTO de resposta do login
    /// </summary>
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public UsuarioDto Usuario { get; set; }
    }

    /// <summary>
    /// DTO de gênero preferido (usado em UsuarioDetalhadoDto)
    /// </summary>
    public class GeneroPreferidoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
    }
}