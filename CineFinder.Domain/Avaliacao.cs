using System;

namespace CineFinder.Domain.Entities
{
    public class Avaliacao
    {
        public Guid Id { get; set; }
        public int Nota { get; set; } 
        public string Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; }

        public Guid UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        public Guid FilmeId { get; set; }
        public virtual Filme Filme { get; set; }

        public Avaliacao()
        {
            Id = Guid.NewGuid();
            DataAvaliacao = DateTime.UtcNow;
        }
    }
}