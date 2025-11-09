using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.ViewModels;
using CineFinder.Application.DTOs.Filme;
using CineFinder.Application.DTOs.Genero;
using CineFinder.Application.DTOs.Avaliacao;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            try
            {
                var filmesDto = await _filmeService.GetAllAsync();
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
                    Generos = f.Generos?.Select(g => g.Nome).ToList() ?? new System.Collections.Generic.List<string>()
                }).ToList();

                return View(filmes);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar filmes: {ex.Message}";
                return View(new System.Collections.Generic.List<FilmeViewModel>());
            }
        }

        // GET: Filmes/Detalhes/5
        public async Task<IActionResult> Detalhes(Guid id)
        {
            try
            {
                var filmeDto = await _filmeService.GetDetalhadoAsync(id);
                
                var avaliacoesDto = await _avaliacaoService.GetByFilmeAsync(id);

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
                    TotalAvaliacoes = filmeDto.TotalAvaliacoes,
                    Generos = filmeDto.Generos?.Select(g => g.Nome).ToList() ?? new System.Collections.Generic.List<string>()
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
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar filme: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Filmes/Criar
        public async Task<IActionResult> Criar()
        {
            var generosDto = await _generoService.GetAllAsync();
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
                try
                {
                    var filmeDto = new CreateFilmeDto
                    {
                        Titulo = model.Titulo,
                        Descricao = model.Descricao,
                        DataLancamento = model.DataLancamento,
                        PosterUrl = model.PosterUrl,
                        TrailerUrl = model.TrailerUrl,
                        Diretor = model.Diretor,
                        Duracao = model.Duracao,
                        TmdbId = model.TmdbId,
                        GeneroIds = model.GenerosIds ?? new System.Collections.Generic.List<Guid>()
                    };

                    var resultado = await _filmeService.CreateAsync(filmeDto);
                    if (resultado != null)
                    {
                        TempData["SuccessMessage"] = "Filme criado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", "Erro ao criar o filme.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao criar o filme: {ex.Message}");
                }
            }

            var generosDto = await _generoService.GetAllAsync();
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
            try
            {
                var filmeDto = await _filmeService.GetByIdAsync(id);

                var model = new FilmeEditViewModel
                {
                    Id = filmeDto.Id,
                    Titulo = filmeDto.Titulo,
                    Descricao = filmeDto.Descricao,
                    DataLancamento = filmeDto.DataLancamento,
                    PosterUrl = filmeDto.PosterUrl,
                    Diretor = filmeDto.Diretor,
                    Duracao = filmeDto.Duracao,
                    TmdbId = filmeDto.TmdbId,
                    GenerosIds = filmeDto.Generos?.Select(g => g.Id).ToList() ?? new System.Collections.Generic.List<Guid>()
                };

                var generosDto = await _generoService.GetAllAsync();
                ViewBag.Generos = generosDto.Select(g => new GeneroViewModel
                {
                    Id = g.Id,
                    Nome = g.Nome
                }).ToList();

                return View(model);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar filme: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
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
                try
                {
                    var filmeDto = new UpdateFilmeDto
                    {
                        Id = model.Id,
                        Titulo = model.Titulo,
                        Descricao = model.Descricao,
                        DataLancamento = model.DataLancamento,
                        PosterUrl = model.PosterUrl,
                        TrailerUrl = model.TrailerUrl,
                        Diretor = model.Diretor,
                        Duracao = model.Duracao,
                        TmdbId = model.TmdbId,
                        GenerosIds = model.GenerosIds ?? new System.Collections.Generic.List<Guid>()
                    };

                    var resultado = await _filmeService.UpdateAsync(filmeDto);
                    if (resultado != null)
                    {
                        TempData["SuccessMessage"] = "Filme atualizado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", "Erro ao atualizar o filme.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao atualizar o filme: {ex.Message}");
                }
            }

            var generosDto = await _generoService.GetAllAsync();
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
            try
            {
                var filmeDto = await _filmeService.GetByIdAsync(id);

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
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar filme: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Filmes/Excluir/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirConfirmado(Guid id)
        {
            await _filmeService.DeleteAsync(id);
            // Removido - DeleteAsync não retorna bool
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
            var generosDto = await _generoService.GetAllAsync();
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
                var filmesDto = await _filmeService.SearchAsync(termo ?? "");
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
            try
            {
                var generoDto = await _generoService.GetByIdAsync(generoId);

                var filmesDto = await _filmeService.GetByGeneroAsync(generoId);
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
                    Generos = f.Generos?.Select(g => g.Nome).ToList() ?? new System.Collections.Generic.List<string>()
                }).ToList();

                ViewBag.NomeGenero = generoDto.Nome;
                return View(filmes);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar filmes: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}