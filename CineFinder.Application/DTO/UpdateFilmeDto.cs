using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CineFinder.Application.DTOs.Filme
{
    public class UpdateFilmeDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public int TmdbId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(2000)]
        public string Descricao { get; set; }

        [Required]
        public DateTime? DataLancamento { get; set; }

        [Url]
        [StringLength(500)]
        public string PosterUrl { get; set; }

        [Url]
        [StringLength(500)]
        public string TrailerUrl { get; set; }

        [StringLength(100)]
        public string Diretor { get; set; }

        [Range(1, 500)]
        public int? Duracao { get; set; }

        public List<Guid> GenerosIds { get; set; } = new();
    }
}