using CineFinder.Application.DTOs.Avaliacao;
using CineFinder.Application.Services;
using CineFinder.Domain.Entities;
using CineFinder.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CineFinder.Tests.Unit.Services;

public class AvaliacaoServiceTests
{
    private readonly Mock<IAvaliacaoRepository> _avaliacaoRepositoryMock = new();
    private readonly Mock<IFilmeRepository> _filmeRepositoryMock = new();
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock = new();
    private readonly AvaliacaoService _sut;

    public AvaliacaoServiceTests()
    {
        _sut = new AvaliacaoService(_avaliacaoRepositoryMock.Object, _filmeRepositoryMock.Object, _usuarioRepositoryMock.Object);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public async Task CreateAsync_NotaForaDaFaixa_LancaException(int nota)
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var dto = new CreateAvaliacaoDto { FilmeId = Guid.NewGuid(), Nota = nota };

        // Act
        var acao = async () => await _sut.CreateAsync(usuarioId, dto);

        // Assert
        await acao.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task CreateAsync_UsuarioJaAvaliou_LancaException()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var filmeId = Guid.NewGuid();
        var dto = new CreateAvaliacaoDto { FilmeId = filmeId, Nota = 4 };
        _avaliacaoRepositoryMock.Setup(r => r.UsuarioJaAvaliouAsync(usuarioId, filmeId)).ReturnsAsync(true);

        // Act
        var acao = async () => await _sut.CreateAsync(usuarioId, dto);

        // Assert
        await acao.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task GetByIdAsync_AvaliacaoNaoEncontrada_LancaKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _avaliacaoRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Avaliacao?)null);

        // Act
        var acao = async () => await _sut.GetByIdAsync(id);

        // Assert
        await acao.Should().ThrowAsync<KeyNotFoundException>();
    }
}
