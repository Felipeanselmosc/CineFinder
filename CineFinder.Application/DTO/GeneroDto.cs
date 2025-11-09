using System;
using System.ComponentModel.DataAnnotations;

namespace CineFinder.Application.DTOs.Genero
{
    /// <summary>
    /// DTO básico de Gênero
    /// </summary>
    public class GeneroDto
    {
        public Guid Id { get; set; }
        public int TmdbGeneroId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }

    /// <summary>
    /// DTO para criar um novo Gênero
    /// </summary>
    public class CreateGeneroDto
    {
        [Required(ErrorMessage = "O ID do TMDB é obrigatório")]
        public int TmdbGeneroId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 50 caracteres")]
        public string Nome { get; set; }

        [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres")]
        public string Descricao { get; set; }
    }

    /// <summary>
    /// DTO para atualizar um Gênero existente
    /// </summary>
    public class UpdateGeneroDto
    {
        [Required(ErrorMessage = "O ID é obrigatório")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O ID do TMDB é obrigatório")]
        public int TmdbGeneroId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 50 caracteres")]
        public string Nome { get; set; }

        [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres")]
        public string Descricao { get; set; }
    }
}