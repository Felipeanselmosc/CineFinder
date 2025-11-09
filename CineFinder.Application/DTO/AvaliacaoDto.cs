using System;
using System.ComponentModel.DataAnnotations;

namespace CineFinder.Application.DTOs.Avaliacao
{
    /// <summary>
    /// DTO básico de Avaliação
    /// </summary>
    public class AvaliacaoDto
    {
        public Guid Id { get; set; }
        public int Nota { get; set; }
        public string Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public UsuarioAvaliacaoDto Usuario { get; set; }
        public FilmeAvaliacaoDto Filme { get; set; }
    }

    /// <summary>
    /// DTO simplificado de Avaliação
    /// </summary>
    public class AvaliacaoSimplificadaDto
    {
        public Guid Id { get; set; }
        public int Nota { get; set; }
        public string Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public string NomeUsuario { get; set; }
    }

    /// <summary>
    /// DTO para criar uma nova Avaliação
    /// </summary>
    public class CreateAvaliacaoDto
    {
        [Required(ErrorMessage = "O ID do filme é obrigatório")]
        public Guid FilmeId { get; set; }

        [Required(ErrorMessage = "A nota é obrigatória")]
        [Range(1, 10, ErrorMessage = "A nota deve estar entre 1 e 10")]
        public int Nota { get; set; }

        [StringLength(1000, ErrorMessage = "O comentário deve ter no máximo 1000 caracteres")]
        public string Comentario { get; set; }
    }

    /// <summary>
    /// DTO para atualizar uma Avaliação existente
    /// </summary>
    public class UpdateAvaliacaoDto
    {
        [Required(ErrorMessage = "O ID é obrigatório")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "A nota é obrigatória")]
        [Range(1, 10, ErrorMessage = "A nota deve estar entre 1 e 10")]
        public int Nota { get; set; }

        [StringLength(1000, ErrorMessage = "O comentário deve ter no máximo 1000 caracteres")]
        public string Comentario { get; set; }
    }

    /// <summary>
    /// DTO simplificado de Usuário (usado dentro de AvaliacaoDto)
    /// </summary>
    public class UsuarioAvaliacaoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }

    /// <summary>
    /// DTO simplificado de Filme (usado dentro de AvaliacaoDto)
    /// </summary>
    public class FilmeAvaliacaoDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string PosterUrl { get; set; }
        public decimal? NotaMedia { get; set; }
    }
}