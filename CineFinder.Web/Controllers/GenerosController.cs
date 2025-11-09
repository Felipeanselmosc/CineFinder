using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.ViewModels;
using CineFinder.Application.DTOs.Genero;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            try
            {
                var generosDto = await _generoService.GetAllAsync();
                var generos = generosDto.Select(g => new GeneroViewModel
                {
                    Id = g.Id,
                    Nome = g.Nome,
                    Descricao = g.Descricao
                }).ToList();

                return View(generos);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar gêneros: {ex.Message}";
                return View(new System.Collections.Generic.List<GeneroViewModel>());
            }
        }

        // GET: Generos/Detalhes/5
        public async Task<IActionResult> Detalhes(Guid id)
        {
            try
            {
                var generoDto = await _generoService.GetByIdAsync(id);

                var genero = new GeneroViewModel
                {
                    Id = generoDto.Id,
                    Nome = generoDto.Nome,
                    Descricao = generoDto.Descricao
                };

                // Obter filmes do gênero
                var filmesDto = await _filmeService.GetByGeneroAsync(id);
                ViewBag.Filmes = filmesDto.Select(f => new FilmeViewModel
                {
                    Id = f.Id,
                    Titulo = f.Titulo,
                    Descricao = f.Descricao,
                    PosterUrl = f.PosterUrl,
                    Diretor = f.Diretor,
                    DataLancamento = f.DataLancamento,
                    Generos = f.Generos?.Select(g => g.Nome).ToList() ?? new System.Collections.Generic.List<string>()
                }).ToList();

                return View(genero);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar gênero: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
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
                try
                {
                    var generoDto = new CreateGeneroDto
                    {
                        Nome = model.Nome,
                        Descricao = model.Descricao,
                        TmdbGeneroId = model.TmdbGeneroId
                    };

                    var resultado = await _generoService.CreateAsync(generoDto);
                    if (resultado != null)
                    {
                        TempData["SuccessMessage"] = "Gênero criado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", "Erro ao criar o gênero.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao criar o gênero: {ex.Message}");
                }
            }

            return View(model);
        }

        // GET: Generos/Editar/5
        public async Task<IActionResult> Editar(Guid id)
        {
            try
            {
                var generoDto = await _generoService.GetByIdAsync(id);

                var model = new GeneroCreateViewModel
                {
                    Nome = generoDto.Nome,
                    Descricao = generoDto.Descricao,
                    TmdbGeneroId = generoDto.TmdbGeneroId
                };

                ViewBag.Id = id;
                return View(model);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar gênero: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Generos/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, GeneroCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var generoDto = new UpdateGeneroDto
                    {
                        Id = id,
                        Nome = model.Nome,
                        Descricao = model.Descricao,
                        TmdbGeneroId = model.TmdbGeneroId
                    };

                    var resultado = await _generoService.UpdateAsync(generoDto);
                    if (resultado != null)
                    {
                        TempData["SuccessMessage"] = "Gênero atualizado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", "Erro ao atualizar o gênero.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao atualizar o gênero: {ex.Message}");
                }
            }

            ViewBag.Id = id;
            return View(model);
        }

        // GET: Generos/Excluir/5
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
                var generoDto = await _generoService.GetByIdAsync(id);

                var genero = new GeneroViewModel
                {
                    Id = generoDto.Id,
                    Nome = generoDto.Nome,
                    Descricao = generoDto.Descricao
                };

                return View(genero);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar gênero: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Generos/Excluir/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirConfirmado(Guid id)
        {
            try
            {
                await _generoService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Gênero excluído com sucesso!";
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "Gênero não encontrado.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao excluir o gênero: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
