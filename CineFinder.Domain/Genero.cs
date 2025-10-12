using System;
using System.Collections.Generic;

namespace CineFinder.Domain.Entities
{
    public class Genero
    {
        public Guid Id { get; set; }
        public int TmdbGeneroId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; }

        public virtual ICollection<FilmeGenero> FilmeGeneros { get; set; }
        public virtual ICollection<UsuarioGeneroPreferido> UsuariosPreferidos { get; set; }

        public Genero()
        {
            Id = Guid.NewGuid();
            DataCriacao = DateTime.Now;
            FilmeGeneros = new List<FilmeGenero>();
            UsuariosPreferidos = new List<UsuarioGeneroPreferido>();
        }
    }
}