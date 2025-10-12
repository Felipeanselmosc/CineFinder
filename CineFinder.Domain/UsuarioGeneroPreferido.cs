using System;

namespace CineFinder.Domain.Entities
{
    public class UsuarioGeneroPreferido
    {
        public Guid UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        public Guid GeneroId { get; set; }
        public virtual Genero Genero { get; set; }

        public int NivelPreferencia { get; set; }
    }
}