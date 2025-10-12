using System;
using System.Collections.Generic;

namespace CineFinder.Domain.Entities
{
    public class Lista
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool IsPublica { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public Guid UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<ListaFilme> ListaFilmes { get; set; }

        public Lista()
        {
            Id = Guid.NewGuid();
            DataCriacao = DateTime.UtcNow;
            IsPublica = false;
            ListaFilmes = new List<ListaFilme>();
        }
    }
}