using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CineFinder.ViewModels
{
    // ============= FILME VIEW MODELS =============

    public class FilmeViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Data de Lançamento")]
        [DataType(DataType.Date)]
        public DateTime? DataLancamento { get; set; }

        [Display(Name = "Poster")]
        public string PosterUrl { get; set; }

        [Display(Name = "Diretor")]
        public string Diretor { get; set; }

        [Display(Name = "Duração (min)")]
        public int? Duracao { get; set; }

        [Display(Name = "Nota Média")]
        [DisplayFormat(DataFormatString = "{0:N1}")]
        public decimal? NotaMedia { get; set; }

        public List<string> Generos { get; set; } = new();

        [Display(Name = "Total de Avaliações")]
        public int TotalAvaliacoes { get; set; }
    }

    public class FilmeCreateViewModel
    {
        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres")]
        [Display(Name = "Descrição")]
        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; }

        [Display(Name = "Data de Lançamento")]
        [DataType(DataType.Date)]
        public DateTime? DataLancamento { get; set; }

        [Display(Name = "URL do Poster")]
        [Url(ErrorMessage = "URL inválida")]
        public string PosterUrl { get; set; }

        [Display(Name = "URL do Trailer")]
        [Url(ErrorMessage = "URL inválida")]
        public string TrailerUrl { get; set; }

        [Required(ErrorMessage = "O diretor é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome do diretor deve ter no máximo 100 caracteres")]
        [Display(Name = "Diretor")]
        public string Diretor { get; set; }

        [Range(1, 500, ErrorMessage = "A duração deve estar entre 1 e 500 minutos")]
        [Display(Name = "Duração (minutos)")]
        public int? Duracao { get; set; }

        [Required(ErrorMessage = "O TMDB ID é obrigatório")]
        [Display(Name = "TMDB ID")]
        public int TmdbId { get; set; }

        [Display(Name = "Gêneros")]
        public List<Guid> GenerosIds { get; set; } = new();
    }

    public class FilmeEditViewModel : FilmeCreateViewModel
    {
        public Guid Id { get; set; }
    }

    public class FilmeBuscaViewModel
    {
        [Display(Name = "Buscar por título")]
        public string Termo { get; set; }

        [Display(Name = "Gênero")]
        public Guid? GeneroId { get; set; }

        [Display(Name = "Ano")]
        public int? Ano { get; set; }

        public List<FilmeViewModel> Resultados { get; set; } = new();
        public List<GeneroViewModel> Generos { get; set; } = new();
    }

    // ============= USUÁRIO VIEW MODELS =============

    public class UsuarioViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Data de Cadastro")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataCriacao { get; set; }

        [Display(Name = "Total de Listas")]
        public int TotalListas { get; set; }

        [Display(Name = "Total de Avaliações")]
        public int TotalAvaliacoes { get; set; }
    }

    public class UsuarioCreateViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        [Display(Name = "Nome Completo")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "A confirmação de senha é obrigatória")]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
        [Display(Name = "Confirmar Senha")]
        public string ConfirmarSenha { get; set; }
    }

    public class UsuarioEditViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        [Display(Name = "Nome Completo")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Display(Name = "Lembrar-me")]
        public bool LembrarMe { get; set; }
    }

    // ============= LISTA VIEW MODELS =============

    public class ListaViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Pública")]
        public bool IsPublica { get; set; }

        [Display(Name = "Data de Criação")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataCriacao { get; set; }

        [Display(Name = "Criador")]
        public string NomeUsuario { get; set; }

        [Display(Name = "Total de Filmes")]
        public int TotalFilmes { get; set; }

        public List<FilmeViewModel> Filmes { get; set; } = new();
    }

    public class ListaCreateViewModel
    {
        [Required(ErrorMessage = "O nome da lista é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome da Lista")]
        public string Nome { get; set; }

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        [Display(Name = "Descrição")]
        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; }

        [Display(Name = "Lista Pública")]
        public bool IsPublica { get; set; }
    }

    public class ListaEditViewModel : ListaCreateViewModel
    {
        public Guid Id { get; set; }
    }

    // ============= AVALIAÇÃO VIEW MODELS =============

    public class AvaliacaoViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Nota")]
        public int Nota { get; set; }

        [Display(Name = "Comentário")]
        public string Comentario { get; set; }

        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DataAvaliacao { get; set; }

        [Display(Name = "Usuário")]
        public string NomeUsuario { get; set; }

        [Display(Name = "Filme")]
        public string TituloFilme { get; set; }

        public Guid FilmeId { get; set; }
        public Guid UsuarioId { get; set; }
    }

    public class AvaliacaoCreateViewModel
    {
        [Required(ErrorMessage = "A nota é obrigatória")]
        [Range(1, 5, ErrorMessage = "A nota deve estar entre 1 e 5")]
        [Display(Name = "Nota (1-5)")]
        public int Nota { get; set; }

        [StringLength(1000, ErrorMessage = "O comentário deve ter no máximo 1000 caracteres")]
        [Display(Name = "Comentário")]
        [DataType(DataType.MultilineText)]
        public string Comentario { get; set; }

        [Required]
        public Guid FilmeId { get; set; }

        [Display(Name = "Filme")]
        public string TituloFilme { get; set; }
    }

    public class AvaliacaoEditViewModel : AvaliacaoCreateViewModel
    {
        public Guid Id { get; set; }
    }

    // ============= GÊNERO VIEW MODELS =============

    public class GeneroViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Total de Filmes")]
        public int TotalFilmes { get; set; }
    }

    public class GeneroCreateViewModel
    {
        [Required(ErrorMessage = "O nome do gênero é obrigatório")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O TMDB Gênero ID é obrigatório")]
        [Display(Name = "TMDB Gênero ID")]
        public int TmdbGeneroId { get; set; }
    }
}