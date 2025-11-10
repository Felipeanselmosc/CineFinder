using CineFinder.Application.DTOs.Filme;
using CineFinder.Application.Interfaces;
using CineFinder.Application.Models;
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
                throw new KeyNotFoundException("Filme não encontrado");

            return MapToDto(filme);
        }

        public async Task<FilmeDetalhadoDto> GetDetalhadoAsync(Guid id)
        {
            var filme = await _filmeRepository.GetWithAvaliacoesAsync(id);
            if (filme == null)
                throw new KeyNotFoundException("Filme não encontrado");

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
                Generos = filmeComGeneros?.FilmeGeneros?.Select(fg => new GeneroSimplificadoDto
                {
                    Id = fg.Genero.Id,
                    Nome = fg.Genero.Nome
                }).ToList() ?? new List<GeneroSimplificadoDto>(),
                Avaliacoes = filme.Avaliacoes?.Select(a => new CineFinder.Application.DTOs.Avaliacao.AvaliacaoSimplificadaDto
                {
                    Id = a.Id,
                    Nota = a.Nota,
                    Comentario = a.Comentario,
                    DataAvaliacao = a.DataAvaliacao,
                    NomeUsuario = a.Usuario?.Nome ?? "Usuário Anônimo"
                }).ToList() ?? new List<CineFinder.Application.DTOs.Avaliacao.AvaliacaoSimplificadaDto>(),
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

        public async Task<IEnumerable<FilmeDto>> GetAllAsync()
        {
            var filmes = await _filmeRepository.GetAllAsync();
            return filmes.Select(MapToDto);
        }

        public async Task<PagedResult<FilmeDto>> SearchAsync(FilmeSearchParameters parameters)
        {
            var (filmes, totalCount) = await _filmeRepository.SearchWithFiltersAsync(
                titulo: parameters.Titulo,
                generoId: parameters.GeneroId,
                anoInicio: parameters.AnoInicio,
                anoFim: parameters.AnoFim,
                notaMinimaMedia: parameters.NotaMinimaMedia,
                duracaoMinima: parameters.DuracaoMinima,
                duracaoMaxima: parameters.DuracaoMaxima,
                diretor: parameters.Diretor,
                generosIds: parameters.GenerosIds,
                orderBy: parameters.OrderBy,
                orderDescending: parameters.OrderDescending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize
            );

            var filmesDto = filmes.Select(MapToDto).ToList();

            return new PagedResult<FilmeDto>(
                filmesDto,
                totalCount,
                parameters.PageNumber,
                parameters.PageSize
            );
        }

        public async Task<FilmeDto> CreateAsync(CreateFilmeDto dto)
        {
            var filmeExistente = await _filmeRepository.GetByTmdbIdAsync(dto.TmdbId);
            if (filmeExistente != null)
                throw new InvalidOperationException("Filme já cadastrado");

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

            // Adicionar gêneros se fornecidos
            if (dto.GeneroIds != null && dto.GeneroIds.Any())
            {
                foreach (var generoId in dto.GeneroIds)
                {
                    var genero = await _generoRepository.GetByIdAsync(generoId);
                    if (genero != null)
                    {
                        filme.FilmeGeneros.Add(new FilmeGenero
                        {
                            FilmeId = filme.Id,
                            GeneroId = genero.Id
                        });
                    }
                }
            }

            await _filmeRepository.AddAsync(filme);
            
            // Recarregar com gêneros para retornar DTO completo
            var filmeCompleto = await _filmeRepository.GetWithGenerosAsync(filme.Id);
            return MapToDto(filmeCompleto);
        }

        public async Task<FilmeDto> UpdateAsync(UpdateFilmeDto dto)
        {
            var filme = await _filmeRepository.GetWithGenerosAsync(dto.Id);
            if (filme == null)
                throw new KeyNotFoundException("Filme não encontrado");

            // Verificar se TMDB ID já existe em outro filme
            var filmeComMesmoTmdbId = await _filmeRepository.GetByTmdbIdAsync(dto.TmdbId);
            if (filmeComMesmoTmdbId != null && filmeComMesmoTmdbId.Id != dto.Id)
                throw new InvalidOperationException("Já existe outro filme com este TMDB ID");

            // Atualizar propriedades
            filme.TmdbId = dto.TmdbId;
            filme.Titulo = dto.Titulo;
            filme.Descricao = dto.Descricao;
            filme.DataLancamento = dto.DataLancamento;
            filme.PosterUrl = dto.PosterUrl;
            filme.TrailerUrl = dto.TrailerUrl;
            filme.Diretor = dto.Diretor;
            filme.Duracao = dto.Duracao;

            // Atualizar gêneros
            if (dto.GenerosIds != null)
            {
                // Remover gêneros existentes
                filme.FilmeGeneros.Clear();

                // Adicionar novos gêneros
                foreach (var generoId in dto.GenerosIds)
                {
                    var genero = await _generoRepository.GetByIdAsync(generoId);
                    if (genero != null)
                    {
                        filme.FilmeGeneros.Add(new FilmeGenero
                        {
                            FilmeId = filme.Id,
                            GeneroId = genero.Id
                        });
                    }
                }
            }

            await _filmeRepository.UpdateAsync(filme);
            
            // Recarregar com gêneros para retornar DTO completo
            var filmeAtualizado = await _filmeRepository.GetWithGenerosAsync(filme.Id);
            return MapToDto(filmeAtualizado);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filme = await _filmeRepository.GetByIdAsync(id);
            if (filme == null)
                throw new KeyNotFoundException("Filme não encontrado");

            await _filmeRepository.DeleteAsync(id);
        }

        private FilmeDto MapToDto(Filme filme)
        {
            if (filme == null)
                return null;

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
                }).ToList() ?? new List<GeneroSimplificadoDto>()
            };
        }
    }
}