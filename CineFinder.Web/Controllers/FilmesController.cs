using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.ViewModels;
using CineFinder.Application.DTOs;

namespace CineFinder.Web.Controllers
{
    public class FilmesController : Controller
    {
        private readonly IFilmeService _filmeService;
        private readonly IGeneroService _generoService;
        private readonly IAvaliacaoService _avaliacaoService;

        public FilmesController(
            IFilmeService filmeService,
            IGeneroService generoService,
            IAvaliacaoService avaliacaoService)
        {
            _filmeService = filmeService;
            _generoService = generoService;
            _avaliacaoService = avaliacaoService;
        }

        // GET: Filmes
        public async Task<IActionResult> Index()
        {
            var filmesDto = await _filmeService.ObterTodosAsync();
            var filmes = filmesDto.Select(f => new FilmeViewModel
            {
                Id = f.Id,
                Titulo = f.Titulo,
                Descricao = f.Descricao,
                DataLancamento = f.DataLancamento,
                PosterUrl = f.PosterUrl,
                Diretor = f.Diretor,
                Duracao = f.Duracao,
                NotaMedia = f.NotaMedia,
                TotalAvaliacoes = f.TotalAvaliacoes
            }).ToList();

            return View(filmes);
        }

        // GET: Filmes/Detalhes/5
        public async Task<IActionResult> Detalhes(Guid id)
        {
            var filmeDto = await _filmeService.ObterPorIdAsync(id);
            if (filmeDto == null)
            {
                return NotFound();
            }

            var avaliacoesDto = await _avaliacaoService.ObterPorFilmeIdAsync(id);

            var filme = new FilmeViewModel
            {
                Id = filmeDto.Id,
                Titulo = filmeDto.Titulo,
                Descricao = filmeDto.Descricao,
                DataLancamento = filmeDto.DataLancamento,
                PosterUrl = filmeDto.PosterUrl,
                Diretor = filmeDto.Diretor,
                Duracao = filmeDto.Duracao,
                NotaMedia = filmeDto.NotaMedia,
                TotalAvaliacoes = filmeDto.TotalAvaliacoes
            };

            ViewBag.Avaliacoes = avaliacoesDto.Select(a => new AvaliacaoViewModel
            {
                Id = a.Id,
                Nota = a.Nota,
                Comentario = a.Comentario,
                DataAvaliacao = a.DataAvaliacao,
                NomeUsuario = a.NomeUsuario
            }).ToList();

            return View(filme);
        }

        // GET: Filmes/Criar
        public async Task<IActionResult> Criar()
        {
            var generosDto = await _generoService.ObterTodosAsync();
            ViewBag.Generos = generosDto.Select(g => new GeneroViewModel
            {
                Id = g.Id,
                Nome = g.Nome
            }).ToList();

            return View();
        }

        // POST: Filmes/Criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(FilmeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var filmeDto = new FilmeDto
                {
                    Titulo = model.Titulo,
                    Descricao = model.Descricao,
                    DataLancamento = model.DataLancamento,
                    PosterUrl = model.PosterUrl,
                    Diretor = model.Diretor,
                    Duracao = model.Duracao,
                    TmdbId = model.TmdbId
                };

                var resultado = await _filmeService.AdicionarAsync(filmeDto);
                if (resultado != null)
                {
                    TempData["Sucesso"] = "Filme criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Erro ao criar o filme.");
            }

            var generosDto = await _generoService.ObterTodosAsync();
            ViewBag.Generos = generosDto.Select(g => new GeneroViewModel
            {
                Id = g.Id,
                Nome = g.Nome
            }).ToList();

            return View(model);
        }

        // GET: Filmes/Editar/5
        public async Task<IActionResult> Editar(Guid id)
        {
            var filmeDto = await _filmeService.ObterPorIdAsync(id);
            if (filmeDto == null)
            {
                return NotFound();
            }

            var model = new FilmeEditViewModel
            {
                Id = filmeDto.Id,
                Titulo = filmeDto.Titulo,
                Descricao = filmeDto.Descricao,
                DataLancamento = filmeDto.DataLancamento,
                PosterUrl = filmeDto.PosterUrl,
                Diretor = filmeDto.Diretor,
                Duracao = filmeDto.Duracao,
                TmdbId = filmeDto.TmdbId
            };

            var generosDto = await _generoService.ObterTodosAsync();
            ViewBag.Generos = generosDto.Select(g => new GeneroViewModel
            {
                Id = g.Id,
                Nome = g.Nome
            }).ToList();

            return View(model);
        }

        // POST: Filmes/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, FilmeEditViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var filmeDto = new FilmeDto
                {
                    Id = model.Id,
                    Titulo = model.Titulo,
                    Descricao = model.Descricao,
                    DataLancamento = model.DataLancamento,
                    PosterUrl = model.PosterUrl,
                    Diretor = model.Diretor,
                    Duracao = model.Duracao,
                    TmdbId = model.TmdbId
                };

                var resultado = await _filmeService.AtualizarAsync(filmeDto);
                if (resultado)
                {
                    TempData["Sucesso"] = "Filme atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Erro ao atualizar o filme.");
            }

            var generosDto = await _generoService.ObterTodosAsync();
            ViewBag.Generos = generosDto.Select(g => new GeneroViewModel
            {
                Id = g.Id,
                Nome = g.Nome
            }).ToList();

            return View(model);
        }

        // GET: Filmes/Excluir/5
        public async Task<IActionResult> Excluir(Guid id)
        {
            var filmeDto = await _filmeService.ObterPorIdAsync(id);
            if (filmeDto == null)
            {
                return NotFound();
            }

            var filme = new FilmeViewModel
            {
                Id = filmeDto.Id,
                Titulo = filmeDto.Titulo,
                Descricao = filmeDto.Descricao,
                DataLancamento = filmeDto.DataLancamento,
                PosterUrl = filmeDto.PosterUrl,
                Diretor = filmeDto.Diretor,
                Duracao = filmeDto.Duracao
            };

            return View(filme);
        }

        // POST: Filmes/Excluir/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirConfirmado(Guid id)
        {
            var resultado = await _filmeService.RemoverAsync(id);
            if (resultado)
            {
                TempData["Sucesso"] = "Filme exclu�do com sucesso!";
            }
            else
            {
                TempData["Erro"] = "Erro ao excluir o filme.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Filmes/Buscar
        public async Task<IActionResult> Buscar(string termo, Guid? generoId, int? ano)
        {
            var generosDto = await _generoService.ObterTodosAsync();
            var model = new FilmeBuscaViewModel
            {
                Termo = termo,
                GeneroId = generoId,
                Ano = ano,
                Generos = generosDto.Select(g => new GeneroViewModel
                {
                    Id = g.Id,
                    Nome = g.Nome
                }).ToList()
            };

            if (!string.IsNullOrEmpty(termo) || generoId.HasValue || ano.HasValue)
            {
                var filmesDto = await _filmeService.BuscarFilmesAsync(termo, generoId, ano);
                model.Resultados = filmesDto.Select(f => new FilmeViewModel
                {
                    Id = f.Id,
                    Titulo = f.Titulo,
                    Descricao = f.Descricao,
                    DataLancamento = f.DataLancamento,
                    PosterUrl = f.PosterUrl,
                    Diretor = f.Diretor,
                    Duracao = f.Duracao,
                    NotaMedia = f.NotaMedia
                }).ToList();
            }

            return View(model);
        }

        // GET: Filmes/PorGenero/5
        public async Task<IActionResult> PorGenero(Guid generoId)
        {
            var generoDto = await _generoService.ObterPorIdAsync(generoId);
            if (generoDto == null)
            {
                return NotFound();
            }

            var filmesDto = await _filmeService.ObterPorGeneroAsync(generoId);
            var filmes = filmesDto.Select(f => new FilmeViewModel
            {
                Id = f.Id,
                Titulo = f.Titulo,
                Descricao = f.Descricao,
                DataLancamento = f.DataLancamento,
                PosterUrl = f.PosterUrl,
                Diretor = f.Diretor,
                Duracao = f.Duracao,
                NotaMedia = f.NotaMedia
            }).ToList();

            ViewBag.NomeGenero = generoDto.Nome;
            return View(filmes);
        }
    }
}