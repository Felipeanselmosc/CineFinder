using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CineFinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFilmeService _filmeService;
        private readonly IGeneroService _generoService;
        private readonly IListaService _listaService;

        public HomeController(
            IFilmeService filmeService,
            IGeneroService generoService,
            IListaService listaService)
        {
            _filmeService = filmeService;
            _generoService = generoService;
            _listaService = listaService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Buscar filmes em destaque (top rated)
                var filmesDestaque = await _filmeService.GetTopRatedAsync(8);

                // Buscar gêneros populares
                var generos = await _generoService.GetPopularesAsync();

                // Buscar listas públicas
                var listasPublicas = await _listaService.GetPublicasAsync();

                var viewModel = new HomeViewModel
                {
                    FilmesDestaque = filmesDestaque.Select(f => new FilmeViewModel
                    {
                        Id = f.Id,
                        Titulo = f.Titulo,
                        Descricao = f.Descricao,
                        PosterUrl = f.PosterUrl,
                        NotaMedia = f.NotaMedia,
                        Duracao = f.Duracao,
                        Diretor = f.Diretor,
                        Generos = f.Generos?.Select(g => g.Nome).ToList() ?? new()
                    }).ToList(),

                    GenerosPopulares = generos.Select(g => new GeneroViewModel
                    {
                        Id = g.Id,
                        Nome = g.Nome,
                        Descricao = g.Descricao
                    }).ToList(),

                    ListasPublicas = listasPublicas.Take(6).Select(l => new ListaViewModel
                    {
                        Id = l.Id,
                        Nome = l.Nome,
                        Descricao = l.Descricao,
                        NomeUsuario = l.Usuario.Nome,
                        TotalFilmes = l.TotalFilmes,
                        DataCriacao = l.DataCriacao
                    }).ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar página inicial: {ex.Message}";
                return View(new HomeViewModel());
            }
        }

        public IActionResult Sobre()
        {
            return View();
        }

        public IActionResult Contato()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }

    // ViewModel específico para a Home
    public class HomeViewModel
    {
        public List<FilmeViewModel> FilmesDestaque { get; set; } = new();
        public List<GeneroViewModel> GenerosPopulares { get; set; } = new();
        public List<ListaViewModel> ListasPublicas { get; set; } = new();
    }
}