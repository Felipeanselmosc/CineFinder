using Microsoft.AspNetCore.Mvc;
using CineFinder.Application.Interfaces;
using CineFinder.ViewModels;
using CineFinder.Application.DTOs;

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
            var usuariosDto = await _usuarioService.ObterTodosAsync();
            var usuarios = usuariosDto.Select(u => new UsuarioViewModel
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                DataCriacao = u.DataCriacao,
                TotalListas = u.TotalListas,
                TotalAvaliacoes = u.TotalAvaliacoes
            }).ToList();

            return View(usuarios);
        }

        // GET: Usuarios/Detalhes/5
        public async Task<IActionResult> Detalhes(Guid id)
        {
            var usuarioDto = await _usuarioService.ObterPorIdAsync(id);
            if (usuarioDto == null)
            {
                return NotFound();
            }

            var usuario = new UsuarioViewModel
            {
                Id = usuarioDto.Id,
                Nome = usuarioDto.Nome,
                Email = usuarioDto.Email,
                DataCriacao = usuarioDto.DataCriacao,
                TotalListas = usuarioDto.TotalListas,
                TotalAvaliacoes = usuarioDto.TotalAvaliacoes
            };

            // Obter listas do usuário
            var listasDto = await _listaService.ObterPorUsuarioIdAsync(id);
            ViewBag.Listas = listasDto.Select(l => new ListaViewModel
            {
                Id = l.Id,
                Nome = l.Nome,
                Descricao = l.Descricao,
                IsPublica = l.IsPublica,
                DataCriacao = l.DataCriacao,
                TotalFilmes = l.TotalFilmes
            }).ToList();

            // Obter avaliações do usuário
            var avaliacoesDto = await _avaliacaoService.ObterPorUsuarioIdAsync(id);
            ViewBag.Avaliacoes = avaliacoesDto.Select(a => new AvaliacaoViewModel
            {
                Id = a.Id,
                Nota = a.Nota,
                Comentario = a.Comentario,
                DataAvaliacao = a.DataAvaliacao,
                TituloFilme = a.TituloFilme,
                FilmeId = a.FilmeId
            }).ToList();

            return View(usuario);
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
                var usuarioDto = new UsuarioDto
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    Senha = model.Senha
                };

                var resultado = await _usuarioService.AdicionarAsync(usuarioDto);
                if (resultado != null)
                {
                    TempData["Sucesso"] = "Usuário criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Erro ao criar o usuário. O e-mail pode já estar em uso.");
            }

            return View(model);
        }

        // GET: Usuarios/Editar/5
        public async Task<IActionResult> Editar(Guid id)
        {
            var usuarioDto = await _usuarioService.ObterPorIdAsync(id);
            if (usuarioDto == null)
            {
                return NotFound();
            }

            var model = new UsuarioEditViewModel
            {
                Id = usuarioDto.Id,
                Nome = usuarioDto.Nome,
                Email = usuarioDto.Email
            };

            return View(model);
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
                var usuarioDto = new UsuarioDto
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    Email = model.Email
                };

                var resultado = await _usuarioService.AtualizarAsync(usuarioDto);
                if (resultado)
                {
                    TempData["Sucesso"] = "Usuário atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Erro ao atualizar o usuário.");
            }

            return View(model);
        }

        // GET: Usuarios/Excluir/5
        public async Task<IActionResult> Excluir(Guid id)
        {
            var usuarioDto = await _usuarioService.ObterPorIdAsync(id);
            if (usuarioDto == null)
            {
                return NotFound();
            }

            var usuario = new UsuarioViewModel
            {
                Id = usuarioDto.Id,
                Nome = usuarioDto.Nome,
                Email = usuarioDto.Email,
                DataCriacao = usuarioDto.DataCriacao
            };

            return View(usuario);
        }

        // POST: Usuarios/Excluir/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirConfirmado(Guid id)
        {
            var resultado = await _usuarioService.RemoverAsync(id);
            if (resultado)
            {
                TempData["Sucesso"] = "Usuário excluído com sucesso!";
            }
            else
            {
                TempData["Erro"] = "Erro ao excluir o usuário.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}