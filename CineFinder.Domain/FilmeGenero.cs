using System;

namespace CineFinder.Domain.Entities
{
    public class FilmeGenero
    {
        public Guid FilmeId { get; set; }
        public virtual Filme Filme { get; set; }

        public Guid GeneroId { get; set; }
        public virtual Genero Genero { get; set; }
    }
}