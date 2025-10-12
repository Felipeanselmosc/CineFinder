using System;

namespace CineFinder.Application.DTOs.Genero
{
    public class GeneroDto
    {
        public Guid Id { get; set; }
        public int TmdbGeneroId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }

    public class CreateGeneroDto
    {
        public int TmdbGeneroId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}