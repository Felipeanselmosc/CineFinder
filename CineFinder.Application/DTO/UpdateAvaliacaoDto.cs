using System;
using System.ComponentModel.DataAnnotations;

namespace CineFinder.Application.DTOs.Avaliacao
{
    public class UpdateAvaliacaoDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Range(1, 10)]
        public int Nota { get; set; }

        [StringLength(1000)]
        public string Comentario { get; set; }
    }
}