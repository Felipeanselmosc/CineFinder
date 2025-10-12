using System;

namespace CineFinder.Domain.Entities
{
    public class ListaFilme
    {
        public Guid ListaId { get; set; }
        public virtual Lista Lista { get; set; }

        public Guid FilmeId { get; set; }
        public virtual Filme Filme { get; set; }

        public DateTime DataAdicao { get; set; }
        public int Ordem { get; set; }

        public ListaFilme()
        {
            DataAdicao = DateTime.UtcNow;
        }
    }
}