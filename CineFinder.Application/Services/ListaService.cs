using CineFinder.Application.DTOs.Filme;
using CineFinder.Application.DTOs.Lista;
using CineFinder.Application.DTOs.Usuario;
using CineFinder.Application.Interfaces;
using CineFinder.Domain.Entities;
using CineFinder.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineFinder.Application.Services
{
    public class ListaService : IListaService
    {
        private readonly IListaRepository _listaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IFilmeRepository _filmeRepository;

        public ListaService(
            IListaRepository listaRepository,
            IUsuarioRepository usuarioRepository,
            IFilmeRepository filmeRepository)
        {
            _listaRepository = listaRepository;
            _usuarioRepository = usuarioRepository;
            _filmeRepository = filmeRepository;
        }

        public async Task<ListaDto> GetByIdAsync(Guid id)
        {
            var lista = await _listaRepository.GetByIdAsync(id);
            if (lista == null)
                return null;

            var usuario = await _usuarioRepository.GetByIdAsync(lista.UsuarioId);
            var listaComFilmes = await _listaRepository.GetWithFilmesAsync(lista.Id);

            return new ListaDto
            {
                Id = lista.Id,
                Nome = lista.Nome,
                Descricao = lista.Descricao,
                IsPublica = lista.IsPublica,
                DataCriacao = lista.DataCriacao,
                Usuario = new UsuarioListaDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                },
                TotalFilmes = listaComFilmes?.ListaFilmes?.Count ?? 0
            };
        }

        public async Task<ListaDetalhadaDto> GetDetalhadaAsync(Guid id)
        {
            var lista = await _listaRepository.GetWithFilmesAsync(id);
            if (lista == null)
                throw new Exception("Lista não encontrada");

            var usuario = await _usuarioRepository.GetByIdAsync(lista.UsuarioId);

            return new ListaDetalhadaDto
            {
                Id = lista.Id,
                Nome = lista.Nome,
                Descricao = lista.Descricao,
                IsPublica = lista.IsPublica,
                DataCriacao = lista.DataCriacao,
                Usuario = new UsuarioListaDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                },
                TotalFilmes = lista.ListaFilmes?.Count ?? 0,
                Filmes = lista.ListaFilmes?
                    .OrderBy(lf => lf.Ordem)
                    .Select(lf => new FilmeListaDto
                    {
                        Id = lf.Filme.Id,
                        Titulo = lf.Filme.Titulo,
                        Descricao = lf.Filme.Descricao,
                        PosterUrl = lf.Filme.PosterUrl,
                        DataLancamento = lf.Filme.DataLancamento,
                        NotaMedia = lf.Filme.NotaMedia,
                        Duracao = lf.Filme.Duracao
                    }).ToList() ?? new List<FilmeListaDto>()
            };
        }

        public async Task<IEnumerable<ListaDto>> GetAllAsync()
        {
            var listas = await _listaRepository.GetAllAsync();
            var result = new List<ListaDto>();

            foreach (var lista in listas)
            {
                var usuario = await _usuarioRepository.GetByIdAsync(lista.UsuarioId);
                var listaComFilmes = await _listaRepository.GetWithFilmesAsync(lista.Id);

                result.Add(new ListaDto
                {
                    Id = lista.Id,
                    Nome = lista.Nome,
                    Descricao = lista.Descricao,
                    IsPublica = lista.IsPublica,
                    DataCriacao = lista.DataCriacao,
                    Usuario = new UsuarioListaDto
                    {
                        Id = usuario.Id,
                        Nome = usuario.Nome,
                        Email = usuario.Email
                    },
                    TotalFilmes = listaComFilmes?.ListaFilmes?.Count ?? 0
                });
            }

            return result;
        }

        public async Task<IEnumerable<ListaDto>> GetByUsuarioAsync(Guid usuarioId)
        {
            var listas = await _listaRepository.GetByUsuarioIdAsync(usuarioId);
            var result = new List<ListaDto>();

            foreach (var lista in listas)
            {
                var usuario = await _usuarioRepository.GetByIdAsync(lista.UsuarioId);
                var listaComFilmes = await _listaRepository.GetWithFilmesAsync(lista.Id);

                result.Add(new ListaDto
                {
                    Id = lista.Id,
                    Nome = lista.Nome,
                    Descricao = lista.Descricao,
                    IsPublica = lista.IsPublica,
                    DataCriacao = lista.DataCriacao,
                    Usuario = new UsuarioListaDto
                    {
                        Id = usuario.Id,
                        Nome = usuario.Nome,
                        Email = usuario.Email
                    },
                    TotalFilmes = listaComFilmes?.ListaFilmes?.Count ?? 0
                });
            }

            return result;
        }

        public async Task<IEnumerable<ListaDto>> GetPublicasAsync()
        {
            var listas = await _listaRepository.GetPublicasAsync();
            var result = new List<ListaDto>();

            foreach (var lista in listas)
            {
                var usuario = await _usuarioRepository.GetByIdAsync(lista.UsuarioId);
                var listaComFilmes = await _listaRepository.GetWithFilmesAsync(lista.Id);

                result.Add(new ListaDto
                {
                    Id = lista.Id,
                    Nome = lista.Nome,
                    Descricao = lista.Descricao,
                    IsPublica = lista.IsPublica,
                    DataCriacao = lista.DataCriacao,
                    Usuario = new UsuarioListaDto
                    {
                        Id = usuario.Id,
                        Nome = usuario.Nome,
                        Email = usuario.Email
                    },
                    TotalFilmes = listaComFilmes?.ListaFilmes?.Count ?? 0
                });
            }

            return result;
        }

        public async Task<ListaDto> CreateAsync(Guid usuarioId, CreateListaDto dto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
                throw new Exception("Usuário não encontrado");

            var lista = new Lista
            {
                Id = Guid.NewGuid(),
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                IsPublica = dto.IsPublica,
                DataCriacao = DateTime.UtcNow,
                UsuarioId = usuarioId
            };

            await _listaRepository.AddAsync(lista);

            return new ListaDto
            {
                Id = lista.Id,
                Nome = lista.Nome,
                Descricao = lista.Descricao,
                IsPublica = lista.IsPublica,
                DataCriacao = lista.DataCriacao,
                Usuario = new UsuarioListaDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                },
                TotalFilmes = 0
            };
        }

        public async Task<ListaDto> UpdateAsync(Guid id, Guid usuarioId, UpdateListaDto dto)
        {
            var lista = await _listaRepository.GetByIdAsync(id);
            if (lista == null)
                throw new Exception("Lista não encontrada");

            if (lista.UsuarioId != usuarioId)
                throw new Exception("Você não tem permissão para editar esta lista");

            lista.Nome = dto.Nome;
            lista.Descricao = dto.Descricao;
            lista.IsPublica = dto.IsPublica;

            await _listaRepository.UpdateAsync(lista);

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid usuarioId)
        {
            var lista = await _listaRepository.GetByIdAsync(id);
            if (lista == null)
                return false;

            if (lista.UsuarioId != usuarioId)
                throw new Exception("Você não tem permissão para excluir esta lista");

            // CORREÇÃO: o repositório espera Guid, então passamos lista.Id
            await _listaRepository.DeleteAsync(lista.Id);

            return true;
        }

        public async Task AdicionarFilmeAsync(Guid listaId, Guid usuarioId, AdicionarFilmeListaDto dto)
        {
            var lista = await _listaRepository.GetWithFilmesAsync(listaId);
            if (lista == null)
                throw new Exception("Lista não encontrada");

            if (lista.UsuarioId != usuarioId)
                throw new Exception("Você não tem permissão para alterar esta lista");

            var filme = await _filmeRepository.GetByIdAsync(dto.FilmeId);
            if (filme == null)
                throw new Exception("Filme não encontrado");

            var jaExiste = lista.ListaFilmes.Any(lf => lf.FilmeId == dto.FilmeId);
            if (jaExiste)
                throw new Exception("Este filme já está na lista");

            var ordem = lista.ListaFilmes.Any()
                ? lista.ListaFilmes.Max(lf => lf.Ordem) + 1
                : 1;

            var listaFilme = new ListaFilme
            {
                ListaId = listaId,
                FilmeId = dto.FilmeId,
                Ordem = ordem
            };

            lista.ListaFilmes.Add(listaFilme);
            await _listaRepository.UpdateAsync(lista);
        }

        public async Task RemoverFilmeAsync(Guid listaId, Guid usuarioId, Guid filmeId)
        {
            var lista = await _listaRepository.GetWithFilmesAsync(listaId);
            if (lista == null)
                throw new Exception("Lista não encontrada");

            if (lista.UsuarioId != usuarioId)
                throw new Exception("Você não tem permissão para alterar esta lista");

            var listaFilme = lista.ListaFilmes.FirstOrDefault(lf => lf.FilmeId == filmeId);
            if (listaFilme == null)
                throw new Exception("Filme não encontrado na lista");

            lista.ListaFilmes.Remove(listaFilme);

            // Reorganizar ordens
            var ordem = 1;
            foreach (var lf in lista.ListaFilmes.OrderBy(lf => lf.Ordem))
            {
                lf.Ordem = ordem++;
            }

            await _listaRepository.UpdateAsync(lista);
        }
    }
}
