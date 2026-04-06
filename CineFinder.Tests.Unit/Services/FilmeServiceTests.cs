using CineFinder.Application.DTOs.Filme;
using CineFinder.Application.Services;
using CineFinder.Domain.Entities;
using CineFinder.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CineFinder.Tests.Unit.Services;

public class FilmeServiceTests
{
    private readonly Mock<IFilmeRepository> _filmeRepositoryMock = new();
    private readonly Mock<IGeneroRepository> _generoRepositoryMock = new();
    private readonly Mock<IAvaliacaoRepository> _avaliacaoRepositoryMock = new();
    private readonly FilmeService _sut;

    public FilmeServiceTests()
    {
        _sut = new FilmeService(_filmeRepositoryMock.Object, _generoRepositoryMock.Object, _avaliacaoRepositoryMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_FilmeExistente_RetornaFilmeDto()
    {
        // Arrange
        var filmeId = Guid.NewGuid();
        var filme = new Filme { Id = filmeId, TmdbId = 1, Titulo = "Inception", FilmeGeneros = new List<FilmeGenero>(), Avaliacoes = new List<Avaliacao>() };
        _filmeRepositoryMock.Setup(r => r.GetWithGenerosAsync(filmeId)).ReturnsAsync(filme);

        // Act
        var resultado = await _sut.GetByIdAsync(filmeId);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Titulo.Should().Be("Inception");
    }

    [Fact]
    public async Task GetByIdAsync_FilmeNaoEncontrado_LancaKeyNotFoundException()
    {
        // Arrange
        var filmeId = Guid.NewGuid();
        _filmeRepositoryMock.Setup(r => r.GetWithGenerosAsync(filmeId)).ReturnsAsync((Filme?)null);

        // Act
        var acao = async () => await _sut.GetByIdAsync(filmeId);

        // Assert
        await acao.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_FilmeJaCadastrado_LancaInvalidOperationException()
    {
        // Arrange
        var dto = new CreateFilmeDto { TmdbId = 123, Titulo = "Duplicado" };
        var filmeExistente = new Filme { Id = Guid.NewGuid(), TmdbId = 123, Titulo = "Duplicado", FilmeGeneros = new List<FilmeGenero>() };
        _filmeRepositoryMock.Setup(r => r.GetByTmdbIdAsync(dto.TmdbId)).ReturnsAsync(filmeExistente);

        // Act
        var acao = async () => await _sut.CreateAsync(dto);

        // Assert
        await acao.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task DeleteAsync_FilmeNaoExistente_LancaKeyNotFoundException()
    {
        // Arrange
        var filmeId = Guid.NewGuid();
        _filmeRepositoryMock.Setup(r => r.GetByIdAsync(filmeId)).ReturnsAsync((Filme?)null);

        // Act
        var acao = async () => await _sut.DeleteAsync(filmeId);

        // Assert
        await acao.Should().ThrowAsync<KeyNotFoundException>();
    }
}
