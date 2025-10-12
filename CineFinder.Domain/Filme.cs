using System;
using System.Collections.Generic;

namespace CineFinder.Domain.Entities
{
    public class Filme
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
        public DateTime DataCriacao { get; set; }

  
        public virtual ICollection<FilmeGenero> FilmeGeneros { get; set; }
        public virtual ICollection<Avaliacao> Avaliacoes { get; set; }
        public virtual ICollection<ListaFilme> ListaFilmes { get; set; }

        public Filme()
        {
            Id = Guid.NewGuid();
            DataCriacao = DateTime.UtcNow;
            FilmeGeneros = new List<FilmeGenero>();
            Avaliacoes = new List<Avaliacao>();
            ListaFilmes = new List<ListaFilme>();
        }
    }
}