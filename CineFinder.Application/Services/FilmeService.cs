using CineFinder.Application.DTOs.Filme;
using CineFinder.Application.Interfaces;
using CineFinder.Domain.Entities;
using CineFinder.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineFinder.Application.Services
{
    public class FilmeService : IFilmeService
    {
        private readonly IFilmeRepository _filmeRepository;
        private readonly IGeneroRepository _generoRepository;
        private readonly IAvaliacaoRepository _avaliacaoRepository;

        public FilmeService(
            IFilmeRepository filmeRepository,
            IGeneroRepository generoRepository,
            IAvaliacaoRepository avaliacaoRepository)
        {
            _filmeRepository = filmeRepository;
            _generoRepository = generoRepository;
            _avaliacaoRepository = avaliacaoRepository;
        }

        public async Task<FilmeDto> GetByIdAsync(Guid id)
        {
            var filme = await _filmeRepository.GetWithGenerosAsync(id);
            if (filme == null)
                throw new Exception("Filme não encontrado");

            return MapToDto(filme);
        }

        public async Task<FilmeDetalhadoDto> GetDetalhadoAsync(Guid id)
        {
            var filme = await _filmeRepository.GetWithAvaliacoesAsync(id);
            if (filme == null)
                throw new Exception("Filme não encontrado");

            var filmeComGeneros = await _filmeRepository.GetWithGenerosAsync(id);

            return new FilmeDetalhadoDto
            {
                Id = filme.Id,
                TmdbId = filme.TmdbId,
                Titulo = filme.Titulo,
                Descricao = filme.Descricao,
                DataLancamento = filme.DataLancamento,
                PosterUrl = filme.PosterUrl,
                TrailerUrl = filme.TrailerUrl,
                Diretor = filme.Diretor,
                Duracao = filme.Duracao,
                NotaMedia = filme.NotaMedia,
                Generos = filmeComGeneros.FilmeGeneros?.Select(fg => new GeneroSimplificadoDto
                {
                    Id = fg.Genero.Id,
                    Nome = fg.Genero.Nome
                }).ToList(),
                Avaliacoes = filme.Avaliacoes?.Select(a => new AvaliacaoDto
                {
                    Id = a.Id,
                    Nota = a.Nota,
                    Comentario = a.Comentario,
                    DataAvaliacao = a.DataAvaliacao,
                    Usuario = new DTOs.Usuario.UsuarioDto
                    {
                        Id = a.Usuario.Id,
                        Nome = a.Usuario.Nome
                    }
                }).ToList(),
                TotalAvaliacoes = filme.Avaliacoes?.Count ?? 0
            };
        }

        public async Task<IEnumerable<FilmeDto>> SearchAsync(string titulo)
        {
            var filmes = await _filmeRepository.SearchByTituloAsync(titulo);
            return filmes.Select(MapToDto);
        }

        public async Task<IEnumerable<FilmeDto>> GetByGeneroAsync(Guid generoId)
        {
            var filmes = await _filmeRepository.GetByGeneroAsync(generoId);
            return filmes.Select(MapToDto);
        }

        public async Task<IEnumerable<FilmeDto>> GetTopRatedAsync(int top = 10)
        {
            var filmes = await _filmeRepository.GetTopRatedAsync(top);
            return filmes.Select(MapToDto);
        }

        public async Task<FilmeDto> CreateAsync(CreateFilmeDto dto)
        {
            var filmeExistente = await _filmeRepository.GetByTmdbIdAsync(dto.TmdbId);
            if (filmeExistente != null)
                throw new Exception("Filme já cadastrado");

            var filme = new Filme
            {
                TmdbId = dto.TmdbId,
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                DataLancamento = dto.DataLancamento,
                PosterUrl = dto.PosterUrl,
                TrailerUrl = dto.TrailerUrl,
                Diretor = dto.Diretor,
                Duracao = dto.Duracao
            };

            await _filmeRepository.AddAsync(filme);
            return MapToDto(filme);
        }

        private FilmeDto MapToDto(Filme filme)
        {
            return new FilmeDto
            {
                Id = filme.Id,
                TmdbId = filme.TmdbId,
                Titulo = filme.Titulo,
                Descricao = filme.Descricao,
                DataLancamento = filme.DataLancamento,
                PosterUrl = filme.PosterUrl,
                TrailerUrl = filme.TrailerUrl,
                Diretor = filme.Diretor,
                Duracao = filme.Duracao,
                NotaMedia = filme.NotaMedia,
                Generos = filme.FilmeGeneros?.Select(fg => new GeneroSimplificadoDto
                {
                    Id = fg.Genero.Id,
                    Nome = fg.Genero.Nome
                }).ToList()
            };
        }
    }
}