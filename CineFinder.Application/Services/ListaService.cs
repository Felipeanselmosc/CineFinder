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
                throw new Exception("Lista não encontrada");

            return await MapToDto(lista);
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
                Usuario = new UsuarioDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome
                },
                TotalFilmes = lista.ListaFilmes?.Count ?? 0,
                Filmes = lista.ListaFilmes?
                    .OrderBy(lf => lf.Ordem)
                    .Select(lf => new FilmeDto
                    {
                        Id = lf.Filme.Id,
                        TmdbId = lf.Filme.TmdbId,
                        Titulo = lf.Filme.Titulo,
                        Descricao = lf.Filme.Descricao,
                        PosterUrl = lf.Filme.PosterUrl,
                        NotaMedia = lf.Filme.NotaMedia,
                        Generos = lf.Filme.FilmeGeneros?.Select(fg => new GeneroSimplificadoDto
                        {
                            Id = fg.Genero.Id,
                            Nome = fg.Genero.Nome
                        }).ToList()
                    }).ToList()
            };
        }

        public async Task<IEnumerable<ListaDto>> GetByUsuarioAsync(Guid usuarioId)
        {
            var listas = await _listaRepository.GetByUsuarioIdAsync(usuarioId);
            var result = new List<ListaDto>();

            foreach (var lista in listas)
            {
                result.Add(await MapToDto(lista));
            }

            return result;
        }

        public async Task<IEnumerable<ListaDto>> GetPublicasAsync()
        {
            var listas = await _listaRepository.GetListasPublicasAsync();
            var result = new List<ListaDto>();

            foreach (var lista in listas)
            {
                result.Add(await MapToDto(lista));
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
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                IsPublica = dto.IsPublica,
                UsuarioId = usuarioId
            };

            await _listaRepository.AddAsync(lista);
            return await MapToDto(lista);
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
            lista.DataAtualizacao = DateTime.UtcNow;

            await _listaRepository.UpdateAsync(lista);
            return await MapToDto(lista);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid usuarioId)
        {
            var lista = await _listaRepository.GetByIdAsync(id);
            if (lista == null)
                throw new Exception("Lista não encontrada");

            if (lista.UsuarioId != usuarioId)
                throw new Exception("Você não tem permissão para deletar esta lista");

            await _listaRepository.DeleteAsync(id);
            return true;
        }

        public async Task AdicionarFilmeAsync(Guid listaId, Guid usuarioId, AdicionarFilmeListaDto dto)
        {
            var lista = await _listaRepository.GetByIdAsync(listaId);
            if (lista == null)
                throw new Exception("Lista não encontrada");

            if (lista.UsuarioId != usuarioId)
                throw new Exception("Você não tem permissão para editar esta lista");

            var filme = await _filmeRepository.GetByIdAsync(dto.FilmeId);
            if (filme == null)
                throw new Exception("Filme não encontrado");

            await _listaRepository.AdicionarFilmeAsync(listaId, dto.FilmeId);
        }

        public async Task RemoverFilmeAsync(Guid listaId, Guid usuarioId, Guid filmeId)
        {
            var lista = await _listaRepository.GetByIdAsync(listaId);
            if (lista == null)
                throw new Exception("Lista não encontrada");

            if (lista.UsuarioId != usuarioId)
                throw new Exception("Você não tem permissão para editar esta lista");

            await _listaRepository.RemoverFilmeAsync(listaId, filmeId);
        }

        private async Task<ListaDto> MapToDto(Lista lista)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(lista.UsuarioId);
            var listaComFilmes = await _listaRepository.GetWithFilmesAsync(lista.Id);

            return new ListaDto
            {
                Id = lista.Id,
                Nome = lista.Nome,
                Descricao = lista.Descricao,
                IsPublica = lista.IsPublica,
                DataCriacao = lista.DataCriacao,
                Usuario = new UsuarioDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome
                },
                TotalFilmes = listaComFilmes?.ListaFilmes?.Count ?? 0
            };
        }
    }
}