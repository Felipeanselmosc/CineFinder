using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.ViewModels;
using CineFinder.Application.DTOs;

namespace CineFinder.Web.Controllers
{
    public class ListasController : Controller
    {
        private readonly IListaService _listaService;
        private readonly IFilmeService _filmeService;
        private readonly IUsuarioService _usuarioService;

        public ListasController(
            IListaService listaService,
            IFilmeService filmeService,
            IUsuarioService usuarioService)
        {
            _listaService = listaService;
            _filmeService = filmeService;
            _usuarioService = usuarioService;
        }

        // GET: Listas
        public async Task<IActionResult> Index()
        {
            var listasDto = await _listaService.ObterTodasAsync();
            var listas = listasDto.Select(l => new ListaViewModel
            {
                Id = l.Id,
                Nome = l.Nome,
                Descricao = l.Descricao,
                IsPublica = l.IsPublica,
                DataCriacao = l.DataCriacao,
                NomeUsuario = l.NomeUsuario,
                TotalFilmes = l.TotalFilmes
            }).ToList();

            return View(listas);
        }

        // GET: Listas/Detalhes/5
        public async Task<IActionResult> Detalhes(Guid id)
        {
            var listaDto = await _listaService.ObterPorIdAsync(id);
            if (listaDto == null)
            {
                return NotFound();
            }

            var lista = new ListaViewModel
            {
                Id = listaDto.Id,
                Nome = listaDto.Nome,
                Descricao = listaDto.Descricao,
                IsPublica = listaDto.IsPublica,
                DataCriacao = listaDto.DataCriacao,
                NomeUsuario = listaDto.NomeUsuario,
                TotalFilmes = listaDto.TotalFilmes,
                Filmes = listaDto.Filmes.Select(f => new FilmeViewModel
                {
                    Id = f.Id,
                    Titulo = f.Titulo,
                    Descricao = f.Descricao,
                    PosterUrl = f.PosterUrl,
                    Diretor = f.Diretor,
                    DataLancamento = f.DataLancamento
                }).ToList()
            };

            return View(lista);
        }

        // GET: Listas/Criar
        public IActionResult Criar()
        {
            return View();
        }

        // POST: Listas/Criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(ListaCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Obter usuário logado. Por enquanto, usando um ID fixo para demonstração
                var usuarioId = Guid.NewGuid(); // Substituir pela lógica de autenticação real

                var listaDto = new ListaDto
                {
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    IsPublica = model.IsPublica,
                    UsuarioId = usuarioId
                };

                var resultado = await _listaService.AdicionarAsync(listaDto);
                if (resultado != null)
                {
                    TempData["Sucesso"] = "Lista criada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Erro ao criar a lista.");
            }

            return View(model);
        }

        // GET: Listas/Editar/5
        public async Task<IActionResult> Editar(Guid id)
        {
            var listaDto = await _listaService.ObterPorIdAsync(id);
            if (listaDto == null)
            {
                return NotFound();
            }

            var model = new ListaEditViewModel
            {
                Id = listaDto.Id,
                Nome = listaDto.Nome,
                Descricao = listaDto.Descricao,
                IsPublica = listaDto.IsPublica
            };

            return View(model);
        }

        // POST: Listas/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, ListaEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var listaDto = new ListaDto
                {
                    Id = id,
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    IsPublica = model.IsPublica
                };

                var resultado = await _listaService.AtualizarAsync(listaDto);
                if (resultado)
                {
                    TempData["Sucesso"] = "Lista atualizada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Erro ao atualizar a lista.");
            }

            return View(model);
        }

        // GET: Listas/Excluir/5
        public async Task<IActionResult> Excluir(Guid id)
        {
            var listaDto = await _listaService.ObterPorIdAsync(id);
            if (listaDto == null)
            {
                return NotFound();
            }

            var lista = new ListaViewModel
            {
                Id = listaDto.Id,
                Nome = listaDto.Nome,
                Descricao = listaDto.Descricao,
                IsPublica = listaDto.IsPublica,
                DataCriacao = listaDto.DataCriacao,
                NomeUsuario = listaDto.NomeUsuario
            };

            return View(lista);
        }

        // POST: Listas/Excluir/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirConfirmado(Guid id)
        {
            var resultado = await _listaService.RemoverAsync(id);
            if (resultado)
            {
                TempData["Sucesso"] = "Lista excluída com sucesso!";
            }
            else
            {
                TempData["Erro"] = "Erro ao excluir a lista.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Listas/AdicionarFilme/5
        public async Task<IActionResult> AdicionarFilme(Guid id)
        {
            var listaDto = await _listaService.ObterPorIdAsync(id);
            if (listaDto == null)
            {
                return NotFound();
            }

            var filmesDto = await _filmeService.ObterTodosAsync();

            ViewBag.ListaId = id;
            ViewBag.ListaNome = listaDto.Nome;
            ViewBag.Filmes = filmesDto.Select(f => new FilmeViewModel
            {
                Id = f.Id,
                Titulo = f.Titulo,
                PosterUrl = f.PosterUrl
            }).ToList();

            return View();
        }

        // POST: Listas/AdicionarFilme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarFilme(Guid listaId, Guid filmeId)
        {
            var resultado = await _listaService.AdicionarFilmeAsync(listaId, filmeId);
            if (resultado)
            {
                TempData["Sucesso"] = "Filme adicionado à lista com sucesso!";
            }
            else
            {
                TempData["Erro"] = "Erro ao adicionar o filme à lista.";
            }

            return RedirectToAction(nameof(Detalhes), new { id = listaId });
        }

        // POST: Listas/RemoverFilme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverFilme(Guid listaId, Guid filmeId)
        {
            var resultado = await _listaService.RemoverFilmeAsync(listaId, filmeId);
            if (resultado)
            {
                TempData["Sucesso"] = "Filme removido da lista com sucesso!";
            }
            else
            {
                TempData["Erro"] = "Erro ao remover o filme da lista.";
            }

            return RedirectToAction(nameof(Detalhes), new { id = listaId });
        }

        // GET: Listas/MinhasListas
        public async Task<IActionResult> MinhasListas(Guid usuarioId)
        {
            var listasDto = await _listaService.ObterPorUsuarioIdAsync(usuarioId);
            var listas = listasDto.Select(l => new ListaViewModel
            {
                Id = l.Id,
                Nome = l.Nome,
                Descricao = l.Descricao,
                IsPublica = l.IsPublica,
                DataCriacao = l.DataCriacao,
                TotalFilmes = l.TotalFilmes
            }).ToList();

            var usuarioDto = await _usuarioService.ObterPorIdAsync(usuarioId);
            ViewBag.NomeUsuario = usuarioDto?.Nome ?? "Usuário";

            return View(listas);
        }
    }
}