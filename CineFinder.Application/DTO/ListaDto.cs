using System;
using System.Collections.Generic;
using CineFinder.Application.DTOs.Filme;
using CineFinder.Application.DTOs.Usuario;

namespace CineFinder.Application.DTOs.Lista
{
    public class ListaDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool IsPublica { get; set; }
        public DateTime DataCriacao { get; set; }
        public UsuarioDto Usuario { get; set; }
        public int TotalFilmes { get; set; }
    }

    public class ListaDetalhadaDto : ListaDto
    {
        public List<FilmeDto> Filmes { get; set; }
    }

    public class CreateListaDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool IsPublica { get; set; }
    }

    public class UpdateListaDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool IsPublica { get; set; }
    }

    public class AdicionarFilmeListaDto
    {
        public Guid FilmeId { get; set; }
    }
}