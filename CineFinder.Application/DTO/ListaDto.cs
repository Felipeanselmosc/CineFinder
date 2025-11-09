using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CineFinder.Application.DTOs.Lista
{
    /// <summary>
    /// DTO básico de Lista
    /// </summary>
    public class ListaDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool IsPublica { get; set; }
        public DateTime DataCriacao { get; set; }
        public UsuarioListaDto Usuario { get; set; }
        public int TotalFilmes { get; set; }
    }

    /// <summary>
    /// DTO detalhado de Lista com filmes
    /// </summary>
    public class ListaDetalhadaDto : ListaDto
    {
        public List<FilmeListaDto> Filmes { get; set; } = new();
    }

    /// <summary>
    /// DTO para criar uma nova Lista
    /// </summary>
    public class CreateListaDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "O nome deve ter entre 1 e 100 caracteres")]
        public string Nome { get; set; }

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string Descricao { get; set; }

        public bool IsPublica { get; set; } = true;

        [Required(ErrorMessage = "O ID do usuário é obrigatório")]
        public Guid UsuarioId { get; set; }
    }

    /// <summary>
    /// DTO para atualizar uma Lista existente
    /// </summary>
    public class UpdateListaDto
    {
        [Required(ErrorMessage = "O ID é obrigatório")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "O nome deve ter entre 1 e 100 caracteres")]
        public string Nome { get; set; }

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string Descricao { get; set; }

        public bool IsPublica { get; set; }
    }

    /// <summary>
    /// DTO para adicionar filme à lista
    /// </summary>
    public class AdicionarFilmeListaDto
    {
        [Required(ErrorMessage = "O ID do filme é obrigatório")]
        public Guid FilmeId { get; set; }
    }

    /// <summary>
    /// DTO simplificado de Usuário (usado dentro de ListaDto)
    /// </summary>
    public class UsuarioListaDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }

    /// <summary>
    /// DTO simplificado de Filme (usado dentro de ListaDetalhadaDto)
    /// </summary>
    public class FilmeListaDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string PosterUrl { get; set; }
        public DateTime? DataLancamento { get; set; }
        public decimal? NotaMedia { get; set; }
        public int? Duracao { get; set; }
    }
}