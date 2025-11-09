using System;
using System.ComponentModel.DataAnnotations;

namespace CineFinder.Application.DTOs.Usuario
{
    public class UpdateUsuarioDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
    }
}