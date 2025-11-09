using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.ViewModels;
using CineFinder.Application.DTOs.Filme;
using CineFinder.Application.DTOs.Avaliacao;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CineFinder.Web.Controllers
{
    public class AvaliacoesController : Controller
    {
        private readonly IAvaliacaoService _avaliacaoService;
        private readonly IFilmeService _filmeService;
        private readonly IUsuarioService _usuarioService;

        public AvaliacoesController(
            IAvaliacaoService avaliacaoService,
            IFilmeService filmeService,
            IUsuarioService usuarioService)
        {
            _avaliacaoService = avaliacaoService;
            _filmeService = filmeService;
            _usuarioService = usuarioService;
        }

        // GET: Avaliacoes
        public async Task<IActionResult> Index()
        {
            try
            {
                // Nota: IAvaliacaoService não tem GetAllAsync
                // Por enquanto, retornamos uma lista vazia ou você pode implementar GetAllAsync no service
                var avaliacoes = new System.Collections.Generic.List<AvaliacaoViewModel>();
                TempData["InfoMessage"] = "Funcionalidade de listar todas as avaliações ainda não implementada. Use a busca na API ou visualize por filme.";
                return View(avaliacoes);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar avaliações: {ex.Message}";
                return View(new System.Collections.Generic.List<AvaliacaoViewModel>());
            }
        }

        // GET: Avaliacoes/Detalhes/5
        public async Task<IActionResult> Detalhes(Guid id)
        {
            try
            {
                // Nota: IAvaliacaoService não tem GetByIdAsync
                // Por enquanto, vamos tentar buscar através do filme ou usuário
                TempData["InfoMessage"] = "Funcionalidade de detalhes de avaliação ainda não implementada completamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar avaliação: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Avaliacoes/Criar
        public async Task<IActionResult> Criar(Guid? filmeId)
        {
            var model = new AvaliacaoCreateViewModel();

            if (filmeId.HasValue)
            {
                try
                {
                    var filmeDto = await _filmeService.GetByIdAsync(filmeId.Value);
                    if (filmeDto != null)
                    {
                        model.FilmeId = filmeDto.Id;
                        model.TituloFilme = filmeDto.Titulo;
                    }
                }
                catch (KeyNotFoundException)
                {
                    TempData["ErrorMessage"] = "Filme não encontrado.";
                    return RedirectToAction("Index", "Filmes");
                }
            }

            try
            {
                var filmesDto = await _filmeService.GetAllAsync();
                ViewBag.Filmes = filmesDto.Select(f => new
                {
                    f.Id,
                    f.Titulo
                }).ToList();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar filmes: {ex.Message}";
            }

            return View(model);
        }

        // POST: Avaliacoes/Criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(AvaliacaoCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Obter usuário logado. Por enquanto, usando um ID fixo para demonstração
                    var usuarioId = Guid.NewGuid(); // Substituir pela lógica de autenticação real

                    var avaliacaoDto = new CreateAvaliacaoDto
                    {
                        Nota = model.Nota,
                        Comentario = model.Comentario,
                        FilmeId = model.FilmeId
                    };

                    var resultado = await _avaliacaoService.CreateAsync(usuarioId, avaliacaoDto);
                    if (resultado != null)
                    {
                        TempData["SuccessMessage"] = "Avaliação criada com sucesso!";
                        return RedirectToAction("Detalhes", "Filmes", new { id = model.FilmeId });
                    }

                    ModelState.AddModelError("", "Erro ao criar a avaliação.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao criar a avaliação: {ex.Message}");
                }
            }

            try
            {
                var filmesDto = await _filmeService.GetAllAsync();
                ViewBag.Filmes = filmesDto.Select(f => new
                {
                    f.Id,
                    f.Titulo
                }).ToList();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar filmes: {ex.Message}";
            }

            return View(model);
        }

        // GET: Avaliacoes/Editar/5
        public async Task<IActionResult> Editar(Guid id)
        {
            try
            {
                // Nota: IAvaliacaoService não tem GetByIdAsync
                // Por enquanto, vamos redirecionar
                TempData["InfoMessage"] = "Funcionalidade de editar avaliação ainda não implementada completamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar avaliação: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Avaliacoes/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, AvaliacaoEditViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Obter usuário logado
                    var usuarioId = Guid.NewGuid(); // Substituir pela lógica de autenticação real

                    var avaliacaoDto = new UpdateAvaliacaoDto
                    {
                        Id = model.Id,
                        Nota = model.Nota,
                        Comentario = model.Comentario
                    };

                    var resultado = await _avaliacaoService.UpdateAsync(id, usuarioId, avaliacaoDto);
                    if (resultado != null)
                    {
                        TempData["SuccessMessage"] = "Avaliação atualizada com sucesso!";
                        return RedirectToAction("Detalhes", "Filmes", new { id = model.FilmeId });
                    }

                    ModelState.AddModelError("", "Erro ao atualizar a avaliação.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao atualizar a avaliação: {ex.Message}");
                }
            }

            return View(model);
        }

        // GET: Avaliacoes/Excluir/5
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
                // Nota: IAvaliacaoService não tem GetByIdAsync
                // Por enquanto, vamos redirecionar
                TempData["InfoMessage"] = "Funcionalidade de excluir avaliação ainda não implementada completamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar avaliação: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Avaliacoes/Excluir/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirConfirmado(Guid id)
        {
            try
            {
                // TODO: Obter usuário logado e filmeId antes de deletar
                var usuarioId = Guid.NewGuid(); // Substituir pela lógica de autenticação real
                var filmeId = Guid.Empty; // Obter do contexto se possível

                var resultado = await _avaliacaoService.DeleteAsync(id, usuarioId);
                if (resultado)
                {
                    TempData["SuccessMessage"] = "Avaliação excluída com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Erro ao excluir a avaliação.";
                }

                if (filmeId != Guid.Empty)
                {
                    return RedirectToAction("Detalhes", "Filmes", new { id = filmeId });
                }
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "Avaliação não encontrada.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao excluir a avaliação: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
