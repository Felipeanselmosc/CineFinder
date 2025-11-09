using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.ViewModels;
using CineFinder.Application.DTOs.Usuario;
using CineFinder.Application.DTOs.Lista;
using CineFinder.Application.DTOs.Avaliacao;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CineFinder.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IListaService _listaService;
        private readonly IAvaliacaoService _avaliacaoService;

        public UsuariosController(
            IUsuarioService usuarioService,
            IListaService listaService,
            IAvaliacaoService avaliacaoService)
        {
            _usuarioService = usuarioService;
            _listaService = listaService;
            _avaliacaoService = avaliacaoService;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            try
            {
                // Nota: IUsuarioService não tem GetAllAsync, então vamos usar uma abordagem alternativa
                // Por enquanto, retornamos uma lista vazia ou você pode implementar GetAllAsync no service
                var usuarios = new System.Collections.Generic.List<UsuarioViewModel>();
                TempData["InfoMessage"] = "Funcionalidade de listar todos os usuários ainda não implementada. Use a busca na API.";
                return View(usuarios);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar usuários: {ex.Message}";
                return View(new System.Collections.Generic.List<UsuarioViewModel>());
            }
        }

        // GET: Usuarios/Detalhes/5
        public async Task<IActionResult> Detalhes(Guid id)
        {
            try
            {
                var usuarioDto = await _usuarioService.GetByIdAsync(id);

                var usuario = new UsuarioViewModel
                {
                    Id = usuarioDto.Id,
                    Nome = usuarioDto.Nome,
                    Email = usuarioDto.Email,
                    DataCriacao = usuarioDto.DataCriacao
                };

                // Obter listas do usuário
                var listasDto = await _listaService.GetByUsuarioAsync(id);
                ViewBag.Listas = listasDto.Select(l => new ListaViewModel
                {
                    Id = l.Id,
                    Nome = l.Nome,
                    Descricao = l.Descricao,
                    IsPublica = l.IsPublica,
                    DataCriacao = l.DataCriacao,
                    TotalFilmes = l.TotalFilmes,
                    NomeUsuario = l.Usuario?.Nome ?? "Usuário"
                }).ToList();

                // Obter avaliações do usuário
                var avaliacoesDto = await _avaliacaoService.GetByUsuarioAsync(id);
                ViewBag.Avaliacoes = avaliacoesDto.Select(a => new AvaliacaoViewModel
                {
                    Id = a.Id,
                    Nota = a.Nota,
                    Comentario = a.Comentario,
                    DataAvaliacao = a.DataAvaliacao,
                    TituloFilme = a.Filme?.Titulo ?? "Filme",
                    FilmeId = a.Filme?.Id ?? Guid.Empty,
                    UsuarioId = a.Usuario?.Id ?? Guid.Empty,
                    NomeUsuario = a.Usuario?.Nome ?? "Usuário"
                }).ToList();

                return View(usuario);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar usuário: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Usuarios/Criar
        public IActionResult Criar()
        {
            return View();
        }

        // POST: Usuarios/Criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(UsuarioCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioDto = new CreateUsuarioDto
                    {
                        Nome = model.Nome,
                        Email = model.Email,
                        Senha = model.Senha
                    };

                    var resultado = await _usuarioService.CreateAsync(usuarioDto);
                    if (resultado != null)
                    {
                        TempData["SuccessMessage"] = "Usuário criado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", "Erro ao criar o usuário.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao criar o usuário: {ex.Message}");
                }
            }

            return View(model);
        }

        // GET: Usuarios/Editar/5
        public async Task<IActionResult> Editar(Guid id)
        {
            try
            {
                var usuarioDto = await _usuarioService.GetByIdAsync(id);

                var model = new UsuarioEditViewModel
                {
                    Id = usuarioDto.Id,
                    Nome = usuarioDto.Nome,
                    Email = usuarioDto.Email
                };

                return View(model);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar usuário: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Usuarios/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, UsuarioEditViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioDto = new UpdateUsuarioDto
                    {
                        Id = model.Id,
                        Nome = model.Nome,
                        Email = model.Email
                    };

                    var resultado = await _usuarioService.UpdateAsync(id, usuarioDto);
                    if (resultado != null)
                    {
                        TempData["SuccessMessage"] = "Usuário atualizado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", "Erro ao atualizar o usuário.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao atualizar o usuário: {ex.Message}");
                }
            }

            return View(model);
        }

        // GET: Usuarios/Excluir/5
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
                var usuarioDto = await _usuarioService.GetByIdAsync(id);

                var usuario = new UsuarioViewModel
                {
                    Id = usuarioDto.Id,
                    Nome = usuarioDto.Nome,
                    Email = usuarioDto.Email,
                    DataCriacao = usuarioDto.DataCriacao
                };

                return View(usuario);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar usuário: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Usuarios/Excluir/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirConfirmado(Guid id)
        {
            try
            {
                var resultado = await _usuarioService.DeleteAsync(id);
                if (resultado)
                {
                    TempData["SuccessMessage"] = "Usuário excluído com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Erro ao excluir o usuário.";
                }
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "Usuário não encontrado.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao excluir o usuário: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
