using System;
using System.Collections.Generic;

namespace CineFinder.Application.DTOs.Usuario
{
    public class UsuarioDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataCriacao { get; set; }
    }

    public class CreateUsuarioDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class UpdateUsuarioDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class LoginResponseDto
    {
        public string Token { get; set; }
        public UsuarioDto Usuario { get; set; }
    }
}