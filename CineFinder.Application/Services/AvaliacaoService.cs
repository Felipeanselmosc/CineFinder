using CineFinder.Application.DTOs.Avaliacao;
using CineFinder.Application.DTOs.Filme;
using CineFinder.Application.DTOs.Usuario;
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
    public class AvaliacaoService : IAvaliacaoService
    {
        private readonly IAvaliacaoRepository _avaliacaoRepository;
        private readonly IFilmeRepository _filmeRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public AvaliacaoService(
            IAvaliacaoRepository avaliacaoRepository,
            IFilmeRepository filmeRepository,
            IUsuarioRepository usuarioRepository)
        {
            _avaliacaoRepository = avaliacaoRepository;
            _filmeRepository = filmeRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<AvaliacaoDto> CreateAsync(Guid usuarioId, CreateAvaliacaoDto dto)
        {
            if (dto.Nota < 1 || dto.Nota > 5)
                throw new Exception("Nota deve estar entre 1 e 5");

            if (await _avaliacaoRepository.UsuarioJaAvaliouAsync(usuarioId, dto.FilmeId))
                throw new Exception("Você já avaliou este filme");

            var filme = await _filmeRepository.GetByIdAsync(dto.FilmeId);
            if (filme == null)
                throw new Exception("Filme não encontrado");

            var avaliacao = new Avaliacao
            {
                UsuarioId = usuarioId,
                FilmeId = dto.FilmeId,
                Nota = dto.Nota,
                Comentario = dto.Comentario
            };

            await _avaliacaoRepository.AddAsync(avaliacao);

            await AtualizarNotaMediaFilme(dto.FilmeId);

            return await MapToDtoCompleto(avaliacao);
        }

        public async Task<AvaliacaoDto> UpdateAsync(Guid id, Guid usuarioId, UpdateAvaliacaoDto dto)
        {
            var avaliacao = await _avaliacaoRepository.GetByIdAsync(id);
            if (avaliacao == null)
                throw new Exception("Avaliação não encontrada");

            if (avaliacao.UsuarioId != usuarioId)
                throw new Exception("Você não tem permissão para editar esta avaliação");

            if (dto.Nota < 1 || dto.Nota > 5)
                throw new Exception("Nota deve estar entre 1 e 5");

            avaliacao.Nota = dto.Nota;
            avaliacao.Comentario = dto.Comentario;

            await _avaliacaoRepository.UpdateAsync(avaliacao);
            await AtualizarNotaMediaFilme(avaliacao.FilmeId);

            return await MapToDtoCompleto(avaliacao);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid usuarioId)
        {
            var avaliacao = await _avaliacaoRepository.GetByIdAsync(id);
            if (avaliacao == null)
                throw new Exception("Avaliação não encontrada");

            if (avaliacao.UsuarioId != usuarioId)
                throw new Exception("Você não tem permissão para deletar esta avaliação");

            var filmeId = avaliacao.FilmeId;
            await _avaliacaoRepository.DeleteAsync(id);
            await AtualizarNotaMediaFilme(filmeId);

            return true;
        }

        public async Task<IEnumerable<AvaliacaoDto>> GetByUsuarioAsync(Guid usuarioId)
        {
            var avaliacoes = await _avaliacaoRepository.GetByUsuarioIdAsync(usuarioId);
            var result = new List<AvaliacaoDto>();

            foreach (var avaliacao in avaliacoes)
            {
                result.Add(await MapToDtoCompleto(avaliacao));
            }

            return result;
        }

        public async Task<IEnumerable<AvaliacaoSimplificadaDto>> GetByFilmeAsync(Guid filmeId)
        {
            var avaliacoes = await _avaliacaoRepository.GetByFilmeIdAsync(filmeId);
            return avaliacoes.Select(a => new CineFinder.Application.DTOs.Avaliacao.AvaliacaoSimplificadaDto
            {
                Id = a.Id,
                Nota = a.Nota,
                Comentario = a.Comentario,
                DataAvaliacao = a.DataAvaliacao,
                NomeUsuario = a.Usuario?.Nome ?? "Usuário"
            });
        }

        public async Task<AvaliacaoDto> GetByIdAsync(Guid id)
        {
            var avaliacao = await _avaliacaoRepository.GetByIdAsync(id);
            if (avaliacao == null)
                throw new KeyNotFoundException("Avaliação não encontrada");

            return await MapToDtoCompleto(avaliacao);
        }

        public async Task<IEnumerable<AvaliacaoDto>> GetAllAsync()
        {
            var avaliacoes = await _avaliacaoRepository.GetAllAsync();
            var result = new List<AvaliacaoDto>();

            foreach (var avaliacao in avaliacoes)
            {
                result.Add(await MapToDtoCompleto(avaliacao));
            }

            return result;
        }

        public async Task<PagedResult<AvaliacaoDto>> SearchAsync(AvaliacaoSearchParameters parameters)
        {
            var (avaliacoes, totalCount) = await _avaliacaoRepository.SearchWithFiltersAsync(
                filmeId: parameters.FilmeId,
                usuarioId: parameters.UsuarioId,
                notaMinima: parameters.NotaMinima,
                notaMaxima: parameters.NotaMaxima,
                dataAvaliacaoInicio: parameters.DataAvaliacaoInicio,
                dataAvaliacaoFim: parameters.DataAvaliacaoFim,
                temComentario: parameters.TemComentario,
                orderBy: parameters.OrderBy,
                orderDescending: parameters.OrderDescending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize
            );

            var avaliacoesDto = new List<AvaliacaoDto>();
            foreach (var avaliacao in avaliacoes)
            {
                avaliacoesDto.Add(await MapToDtoCompleto(avaliacao));
            }

            return new PagedResult<AvaliacaoDto>(
                avaliacoesDto,
                totalCount,
                parameters.PageNumber,
                parameters.PageSize
            );
        }

        private async Task AtualizarNotaMediaFilme(Guid filmeId)
        {
            var notaMedia = await _avaliacaoRepository.GetNotaMediaFilmeAsync(filmeId);
            var filme = await _filmeRepository.GetByIdAsync(filmeId);

            if (filme != null)
            {
                filme.NotaMedia = notaMedia;
                await _filmeRepository.UpdateAsync(filme);
            }
        }

        private async Task<AvaliacaoDto> MapToDtoCompleto(Avaliacao avaliacao)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(avaliacao.UsuarioId);
            var filme = await _filmeRepository.GetByIdAsync(avaliacao.FilmeId);

            return new AvaliacaoDto
            {
                Id = avaliacao.Id,
                Nota = avaliacao.Nota,
                Comentario = avaliacao.Comentario,
                DataAvaliacao = avaliacao.DataAvaliacao,
                Usuario = new UsuarioAvaliacaoDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                },
                Filme = new FilmeAvaliacaoDto
                {
                    Id = filme.Id,
                    Titulo = filme.Titulo,
                    PosterUrl = filme.PosterUrl,
                    NotaMedia = filme.NotaMedia
                }
            };
        }
    }
}