using System;
using System.ComponentModel.DataAnnotations;

namespace CineFinder.Application.DTOs.Lista
{
    public class UpdateListaDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [StringLength(500)]
        public string Descricao { get; set; }

        public bool IsPublica { get; set; }
    }
}