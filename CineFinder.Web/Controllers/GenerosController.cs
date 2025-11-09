using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.ViewModels;
using CineFinder.Application.DTOs;

namespace CineFinder.Web.Controllers
{
    public class GenerosController : Controller
    {
        private readonly IGeneroService _generoService;
        private readonly IFilmeService _filmeService;

        public GenerosController(IGeneroService generoService, IFilmeService filmeService)
        {
            _generoService = generoService;
            _filmeService = filmeService;
        }

        // GET: Generos
        public async Task<IActionResult> Index()
        {
            var generosDto = await _generoService.ObterTodosAsync();
            var generos = generosDto.Select(g => new GeneroViewModel
            {
                Id = g.Id,
                Nome = g.Nome,
                Descricao = g.Descricao,
                TotalFilmes = g.TotalFilmes
            }).ToList();

            return View(generos);
        }

        // GET: Generos/Detalhes/5
        public async Task<IActionResult> Detalhes(Guid id)
        {
            var generoDto = await _generoService.ObterPorIdAsync(id);
            if (generoDto == null)
            {
                return NotFound();
            }

            var genero = new GeneroViewModel
            {
                Id = generoDto.Id,
                Nome = generoDto.Nome,
                Descricao = generoDto.Descricao,
                TotalFilmes = generoDto.TotalFilmes
            };

            // Obter filmes do g�nero
            var filmesDto = await _filmeService.ObterPorGeneroAsync(id);
            ViewBag.Filmes = filmesDto.Select(f => new FilmeViewModel
            {
                Id = f.Id,
                Titulo = f.Titulo,
                Descricao = f.Descricao,
                PosterUrl = f.PosterUrl,
                Diretor = f.Diretor,
                DataLancamento = f.DataLancamento
            }).ToList();

            return View(genero);
        }

        // GET: Generos/Criar
        public IActionResult Criar()
        {
            return View();
        }

        // POST: Generos/Criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(GeneroCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var generoDto = new GeneroDto
                {
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    TmdbGeneroId = model.TmdbGeneroId
                };

                var resultado = await _generoService.AdicionarAsync(generoDto);
                if (resultado != null)
                {
                    TempData["Sucesso"] = "G�nero criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Erro ao criar o g�nero.");
            }

            return View(model);
        }

        // GET: Generos/Editar/5
        public async Task<IActionResult> Editar(Guid id)
        {
            var generoDto = await _generoService.ObterPorIdAsync(id);
            if (generoDto == null)
            {
                return NotFound();
            }

            var model = new GeneroCreateViewModel
            {
                Nome = generoDto.Nome,
                Descricao = generoDto.Descricao,
                TmdbGeneroId = generoDto.TmdbGeneroId
            };

            ViewBag.Id = id;
            return View(model);
        }

        // POST: Generos/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, GeneroCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var generoDto = new GeneroDto
                {
                    Id = id,
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    TmdbGeneroId = model.TmdbGeneroId
                };

                var resultado = await _generoService.AtualizarAsync(generoDto);
                if (resultado)
                {
                    TempData["Sucesso"] = "G�nero atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Erro ao atualizar o g�nero.");
            }

            ViewBag.Id = id;
            return View(model);
        }

        // GET: Generos/Excluir/5
        public async Task<IActionResult> Excluir(Guid id)
        {
            var generoDto = await _generoService.ObterPorIdAsync(id);
            if (generoDto == null)
            {
                return NotFound();
            }

            var genero = new GeneroViewModel
            {
                Id = generoDto.Id,
                Nome = generoDto.Nome,
                Descricao = generoDto.Descricao,
                TotalFilmes = generoDto.TotalFilmes
            };

            return View(genero);
        }

        // POST: Generos/Excluir/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirConfirmado(Guid id)
        {
            var resultado = await _generoService.RemoverAsync(id);
            if (resultado)
            {
                TempData["Sucesso"] = "G�nero exclu�do com sucesso!";
            }
            else
            {
                TempData["Erro"] = "Erro ao excluir o g�nero. Pode haver filmes associados.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}