using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.ViewModels;
using CineFinder.Application.DTOs.Filme;
using CineFinder.Application.DTOs.Lista;
using CineFinder.Application.DTOs.Usuario;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            try
            {
                // Usar GetPublicasAsync já que não temos GetAllAsync
                var listasDto = await _listaService.GetPublicasAsync();
                var listas = listasDto.Select(l => new ListaViewModel
                {
                    Id = l.Id,
                    Nome = l.Nome,
                    Descricao = l.Descricao,
                    IsPublica = l.IsPublica,
                    DataCriacao = l.DataCriacao,
                    NomeUsuario = l.Usuario?.Nome ?? "Usuário",
                    TotalFilmes = l.TotalFilmes
                }).ToList();

                return View(listas);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar listas: {ex.Message}";
                return View(new System.Collections.Generic.List<ListaViewModel>());
            }
        }

        // GET: Listas/Detalhes/5
        public async Task<IActionResult> Detalhes(Guid id)
        {
            try
            {
                var listaDto = await _listaService.GetDetalhadaAsync(id);

                var lista = new ListaViewModel
                {
                    Id = listaDto.Id,
                    Nome = listaDto.Nome,
                    Descricao = listaDto.Descricao,
                    IsPublica = listaDto.IsPublica,
                    DataCriacao = listaDto.DataCriacao,
                    NomeUsuario = listaDto.Usuario?.Nome ?? "Usuário",
                    TotalFilmes = listaDto.TotalFilmes,
                    Filmes = listaDto.Filmes?.Select(f => new FilmeViewModel
                    {
                        Id = f.Id,
                        Titulo = f.Titulo,
                        Descricao = f.Descricao ?? "",
                        PosterUrl = f.PosterUrl,
                        DataLancamento = f.DataLancamento,
                        NotaMedia = f.NotaMedia,
                        Duracao = f.Duracao
                    }).ToList() ?? new System.Collections.Generic.List<FilmeViewModel>()
                };

                return View(lista);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar lista: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
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
                try
                {
                    // TODO: Obter usuário logado. Por enquanto, usando um ID fixo para demonstração
                    var usuarioId = Guid.NewGuid(); // Substituir pela lógica de autenticação real

                    var listaDto = new CreateListaDto
                    {
                        Nome = model.Nome,
                        Descricao = model.Descricao,
                        IsPublica = model.IsPublica,
                        UsuarioId = usuarioId
                    };

                    var resultado = await _listaService.CreateAsync(usuarioId, listaDto);
                    if (resultado != null)
                    {
                        TempData["SuccessMessage"] = "Lista criada com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", "Erro ao criar a lista.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao criar a lista: {ex.Message}");
                }
            }

            return View(model);
        }

        // GET: Listas/Editar/5
        public async Task<IActionResult> Editar(Guid id)
        {
            try
            {
                var listaDto = await _listaService.GetByIdAsync(id);

                var model = new ListaEditViewModel
                {
                    Id = listaDto.Id,
                    Nome = listaDto.Nome,
                    Descricao = listaDto.Descricao,
                    IsPublica = listaDto.IsPublica
                };

                return View(model);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar lista: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Listas/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, ListaEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Obter usuário logado
                    var usuarioId = Guid.NewGuid(); // Substituir pela lógica de autenticação real

                    var listaDto = new UpdateListaDto
                    {
                        Id = id,
                        Nome = model.Nome,
                        Descricao = model.Descricao,
                        IsPublica = model.IsPublica
                    };

                    var resultado = await _listaService.UpdateAsync(id, usuarioId, listaDto);
                    if (resultado != null)
                    {
                        TempData["SuccessMessage"] = "Lista atualizada com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", "Erro ao atualizar a lista.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao atualizar a lista: {ex.Message}");
                }
            }

            return View(model);
        }

        // GET: Listas/Excluir/5
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
                var listaDto = await _listaService.GetByIdAsync(id);

                var lista = new ListaViewModel
                {
                    Id = listaDto.Id,
                    Nome = listaDto.Nome,
                    Descricao = listaDto.Descricao,
                    IsPublica = listaDto.IsPublica,
                    DataCriacao = listaDto.DataCriacao,
                    NomeUsuario = listaDto.Usuario?.Nome ?? "Usuário"
                };

                return View(lista);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar lista: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Listas/Excluir/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirConfirmado(Guid id)
        {
            try
            {
                // TODO: Obter usuário logado
                var usuarioId = Guid.NewGuid(); // Substituir pela lógica de autenticação real

                var resultado = await _listaService.DeleteAsync(id, usuarioId);
                if (resultado)
                {
                    TempData["SuccessMessage"] = "Lista excluída com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Erro ao excluir a lista.";
                }
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "Lista não encontrada.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao excluir a lista: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Listas/AdicionarFilme/5
        public async Task<IActionResult> AdicionarFilme(Guid id)
        {
            try
            {
                var listaDto = await _listaService.GetByIdAsync(id);

                var filmesDto = await _filmeService.GetAllAsync();

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
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar lista: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Listas/AdicionarFilme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarFilme(Guid listaId, Guid filmeId)
        {
            try
            {
                // TODO: Obter usuário logado
                var usuarioId = Guid.NewGuid(); // Substituir pela lógica de autenticação real

                var dto = new AdicionarFilmeListaDto { FilmeId = filmeId };
                await _listaService.AdicionarFilmeAsync(listaId, usuarioId, dto);
                
                TempData["SuccessMessage"] = "Filme adicionado à lista com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao adicionar o filme à lista: {ex.Message}";
            }

            return RedirectToAction(nameof(Detalhes), new { id = listaId });
        }

        // POST: Listas/RemoverFilme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverFilme(Guid listaId, Guid filmeId)
        {
            try
            {
                // TODO: Obter usuário logado
                var usuarioId = Guid.NewGuid(); // Substituir pela lógica de autenticação real

                await _listaService.RemoverFilmeAsync(listaId, usuarioId, filmeId);
                TempData["SuccessMessage"] = "Filme removido da lista com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao remover o filme da lista: {ex.Message}";
            }

            return RedirectToAction(nameof(Detalhes), new { id = listaId });
        }

        // GET: Listas/MinhasListas
        public async Task<IActionResult> MinhasListas(Guid usuarioId)
        {
            try
            {
                var listasDto = await _listaService.GetByUsuarioAsync(usuarioId);
                var listas = listasDto.Select(l => new ListaViewModel
                {
                    Id = l.Id,
                    Nome = l.Nome,
                    Descricao = l.Descricao,
                    IsPublica = l.IsPublica,
                    DataCriacao = l.DataCriacao,
                    TotalFilmes = l.TotalFilmes,
                    NomeUsuario = l.Usuario?.Nome ?? "Usuário"
                }).ToList();

                var usuarioDto = await _usuarioService.GetByIdAsync(usuarioId);
                ViewBag.NomeUsuario = usuarioDto?.Nome ?? "Usuário";

                return View(listas);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar listas: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
