using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CineFinder.Application.DTOs.Filme
{
    /// <summary>
    /// DTO básico de Filme
    /// </summary>
    public class FilmeDto
    {
        public Guid Id { get; set; }
        public int TmdbId { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataLancamento { get; set; }
        public string PosterUrl { get; set; }
        public string TrailerUrl { get; set; }
        public string Diretor { get; set; }
        public int? Duracao { get; set; }
        public decimal? NotaMedia { get; set; }
        public List<GeneroSimplificadoDto> Generos { get; set; } = new();
    }

    /// <summary>
    /// DTO detalhado de Filme com avaliações
    /// </summary>
    public class FilmeDetalhadoDto : FilmeDto
    {
        public List<AvaliacaoSimplificadaDto> Avaliacoes { get; set; } = new();
        public int TotalAvaliacoes { get; set; }
    }

    /// <summary>
    /// DTO para criar um novo Filme
    /// </summary>
    public class CreateFilmeDto
    {
        [Required(ErrorMessage = "O ID do TMDB é obrigatório")]
        public int TmdbId { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "O título deve ter entre 1 e 200 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(2000, ErrorMessage = "A descrição deve ter no máximo 2000 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A data de lançamento é obrigatória")]
        public DateTime? DataLancamento { get; set; }

        [Url(ErrorMessage = "URL do poster inválida")]
        [StringLength(500)]
        public string PosterUrl { get; set; }

        [Url(ErrorMessage = "URL do trailer inválida")]
        [StringLength(500)]
        public string TrailerUrl { get; set; }

        [StringLength(100, ErrorMessage = "O nome do diretor deve ter no máximo 100 caracteres")]
        public string Diretor { get; set; }

        [Range(1, 500, ErrorMessage = "A duração deve estar entre 1 e 500 minutos")]
        public int? Duracao { get; set; }

        public List<Guid> GeneroIds { get; set; } = new();
    }

    /// <summary>
    /// DTO para atualizar um Filme existente
    /// </summary>
    public class UpdateFilmeDto
    {
        [Required(ErrorMessage = "O ID é obrigatório")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O ID do TMDB é obrigatório")]
        public int TmdbId { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "O título deve ter entre 1 e 200 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(2000, ErrorMessage = "A descrição deve ter no máximo 2000 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A data de lançamento é obrigatória")]
        public DateTime? DataLancamento { get; set; }

        [Url(ErrorMessage = "URL do poster inválida")]
        [StringLength(500)]
        public string PosterUrl { get; set; }

        [Url(ErrorMessage = "URL do trailer inválida")]
        [StringLength(500)]
        public string TrailerUrl { get; set; }

        [StringLength(100, ErrorMessage = "O nome do diretor deve ter no máximo 100 caracteres")]
        public string Diretor { get; set; }

        [Range(1, 500, ErrorMessage = "A duração deve estar entre 1 e 500 minutos")]
        public int? Duracao { get; set; }

        public List<Guid> GenerosIds { get; set; } = new();
    }

    /// <summary>
    /// DTO simplificado de Gênero (usado dentro de FilmeDto)
    /// </summary>
    public class GeneroSimplificadoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
    }

    /// <summary>
    /// DTO simplificado de Avaliação (usado dentro de FilmeDetalhadoDto)
    /// </summary>
    public class AvaliacaoSimplificadaDto
    {
        public Guid Id { get; set; }
        public int Nota { get; set; }
        public string Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public string NomeUsuario { get; set; }
    }
}