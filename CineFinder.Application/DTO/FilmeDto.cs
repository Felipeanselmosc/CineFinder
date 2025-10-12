using System;
using System.Collections.Generic;

namespace CineFinder.Application.DTOs.Filme
{
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
        public List<GeneroSimplificadoDto> Generos { get; set; }
    }

    public class FilmeDetalhadoDto : FilmeDto
    {
        public List<AvaliacaoDto> Avaliacoes { get; set; }
        public int TotalAvaliacoes { get; set; }
    }

    public class CreateFilmeDto
    {
        public int TmdbId { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataLancamento { get; set; }
        public string PosterUrl { get; set; }
        public string TrailerUrl { get; set; }
        public string Diretor { get; set; }
        public int? Duracao { get; set; }
        public List<Guid> GeneroIds { get; set; }
    }

    public class GeneroSimplificadoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
    }
}