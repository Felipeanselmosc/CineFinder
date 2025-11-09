using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.ViewModels;
using CineFinder.Application.DTOs;

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
            var avaliacoesDto = await _avaliacaoService.ObterTodasAsync();
            var avaliacoes = avaliacoesDto.Select(a => new AvaliacaoViewModel
            {
                Id = a.Id,
                Nota = a.Nota,
                Comentario = a.Comentario,
                DataAvaliacao = a.DataAvaliacao,
                NomeUsuario = a.NomeUsuario,
                TituloFilme = a.TituloFilme,
                FilmeId = a.FilmeId,
                UsuarioId = a.UsuarioId
            }).ToList();

            return View(avaliacoes);
        }

        // GET: Avaliacoes/Detalhes/5
        public async Task<IActionResult> Detalhes(Guid id)
        {
            var avaliacaoDto = await _avaliacaoService.ObterPorIdAsync(id);
            if (avaliacaoDto == null)
            {
                return NotFound();
            }

            var avaliacao = new AvaliacaoViewModel
            {
                Id = avaliacaoDto.Id,
                Nota = avaliacaoDto.Nota,
                Comentario = avaliacaoDto.Comentario,
                DataAvaliacao = avaliacaoDto.DataAvaliacao,
                NomeUsuario = avaliacaoDto.NomeUsuario,
                TituloFilme = avaliacaoDto.TituloFilme,
                FilmeId = avaliacaoDto.FilmeId,
                UsuarioId = avaliacaoDto.UsuarioId
            };

            return View(avaliacao);
        }

        // GET: Avaliacoes/Criar
        public async Task<IActionResult> Criar(Guid? filmeId)
        {
            var model = new AvaliacaoCreateViewModel();

            if (filmeId.HasValue)
            {
                var filmeDto = await _filmeService.ObterPorIdAsync(filmeId.Value);
                if (filmeDto != null)
                {
                    model.FilmeId = filmeDto.Id;
                    model.TituloFilme = filmeDto.Titulo;
                }
            }

            var filmesDto = await _filmeService.ObterTodosAsync();
            ViewBag.Filmes = filmesDto.Select(f => new
            {
                f.Id,
                f.Titulo
            }).ToList();

            return View(model);
        }

        // POST: Avaliacoes/Criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(AvaliacaoCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Obter usuário logado. Por enquanto, usando um ID fixo para demonstração
                var usuarioId = Guid.NewGuid(); // Substituir pela lógica de autenticação real

                var avaliacaoDto = new AvaliacaoDto
                {
                    Nota = model.Nota,
                    Comentario = model.Comentario,
                    FilmeId = model.FilmeId,
                    UsuarioId = usuarioId
                };

                var resultado = await _avaliacaoService.AdicionarAsync(avaliacaoDto);
                if (resultado != null)
                {
                    TempData["Sucesso"] = "Avaliação criada com sucesso!";
                    return RedirectToAction("Detalhes", "Filmes", new { id = model.FilmeId });
                }

                ModelState.AddModelError("", "Erro ao criar a avaliação.");
            }

            var filmesDto = await _filmeService.ObterTodosAsync();
            ViewBag.Filmes = filmesDto.Select(f => new
            {
                f.Id,
                f.Titulo
            }).ToList();

            return View(model);
        }

        // GET: Avaliacoes/Editar/5
        public async Task<IActionResult> Editar(Guid id)
        {
            var avaliacaoDto = await _avaliacaoService.ObterPorIdAsync(id);
            if (avaliacaoDto == null)
            {
                return NotFound();
            }

            var model = new AvaliacaoEditViewModel
            {
                Id = avaliacaoDto.Id,
                Nota = avaliacaoDto.Nota,
                Comentario = avaliacaoDto.Comentario,
                FilmeId = avaliacaoDto.FilmeId,
                TituloFilme = avaliacaoDto.TituloFilme
            };

            return View(model);
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
                var avaliacaoDto = new AvaliacaoDto
                {
                    Id = model.Id,
                    Nota = model.Nota,
                    Comentario = model.Comentario,
                    FilmeId = model.FilmeId
                };

                var resultado = await _avaliacaoService.AtualizarAsync(avaliacaoDto);
                if (resultado)
                {
                    TempData["Sucesso"] = "Avaliação atualizada com sucesso!";
                    return RedirectToAction("Detalhes", "Filmes", new { id = model.FilmeId });
                }

                ModelState.AddModelError("", "Erro ao atualizar a avaliação.");
            }

            return View(model);
        }

        // GET: Avaliacoes/Excluir/5
        public async Task<IActionResult> Excluir(Guid id)
        {
            var avaliacaoDto = await _avaliacaoService.ObterPorIdAsync(id);
            if (avaliacaoDto == null)
            {
                return NotFound();
            }

            var avaliacao = new AvaliacaoViewModel
            {
                Id = avaliacaoDto.Id,
                Nota = avaliacaoDto.Nota,
                Comentario = avaliacaoDto.Comentario,
                DataAvaliacao = avaliacaoDto.DataAvaliacao,
                TituloFilme = avaliacaoDto.TituloFilme,
                NomeUsuario = avaliacaoDto.NomeUsuario
            };

            return View(avaliacao);
        }

        // POST: Avaliacoes/Excluir/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirConfirmado(Guid id)
        {
            var avaliacaoDto = await _avaliacaoService.ObterPorIdAsync(id);
            var filmeId = avaliacaoDto?.FilmeId;

            var resultado = await _avaliacaoService.RemoverAsync(id);
            if (resultado)
            {
                TempData["Sucesso"] = "Avaliação excluída com sucesso!";
            }
            else
            {
                TempData["Erro"] = "Erro ao excluir a avaliação.";
            }

            if (filmeId.HasValue)
            {
                return RedirectToAction("Detalhes", "Filmes", new { id = filmeId });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}