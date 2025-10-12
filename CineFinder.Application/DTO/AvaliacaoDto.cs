using System;
using CineFinder.Application.DTOs.Filme;
using CineFinder.Application.DTOs.Usuario;

namespace CineFinder.Application.DTOs.Filme
{
    public class AvaliacaoDto
    {
        public Guid Id { get; set; }
        public int Nota { get; set; }
        public string Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public UsuarioDto Usuario { get; set; }
        public FilmeDto Filme { get; set; }
    }

    public class AvaliacaoSimplificadaDto
    {
        public Guid Id { get; set; }
        public int Nota { get; set; }
        public string Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public string NomeUsuario { get; set; }
    }

    public class CreateAvaliacaoDto
    {
        public Guid FilmeId { get; set; }
        public int Nota { get; set; }
        public string Comentario { get; set; }
    }

    public class UpdateAvaliacaoDto
    {
        public int Nota { get; set; }
        public string Comentario { get; set; }
    }
}