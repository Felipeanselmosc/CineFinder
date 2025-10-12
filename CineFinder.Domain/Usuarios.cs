using System;
using System.Collections;
using System.Collections.Generic;

namespace CineFinder.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public virtual ICollection<Avaliacao> Avaliacoes { get; set; }
        public virtual ICollection<Lista> Listas { get; set; }
        public virtual ICollection<UsuarioGeneroPreferido> GenerosPreferidos { get; set; }

        public Usuario()
        {
            Id = Guid.NewGuid();
            DataCriacao = DateTime.UtcNow;
            Avaliacoes = new List<Avaliacao>();
            Listas = new List<Lista>();
            GenerosPreferidos = new List<UsuarioGeneroPreferido>();
        }
    }
}