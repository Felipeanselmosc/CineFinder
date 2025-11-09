using System;
using System.ComponentModel.DataAnnotations;

namespace CineFinder.Application.DTOs.Genero
{
    public class UpdateGeneroDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public int TmdbGeneroId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Nome { get; set; }

        [StringLength(200)]
        public string Descricao { get; set; }
    }
}