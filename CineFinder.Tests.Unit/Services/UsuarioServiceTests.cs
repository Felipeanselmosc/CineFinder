using CineFinder.Application.DTOs.Usuario;
using CineFinder.Application.Services;
using CineFinder.Domain.Entities;
using CineFinder.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CineFinder.Tests.Unit.Services;

public class UsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock = new();
    private readonly UsuarioService _sut;

    public UsuarioServiceTests()
    {
        _sut = new UsuarioService(_usuarioRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_EmailJaCadastrado_LancaException()
    {
        // Arrange
        var dto = new CreateUsuarioDto { Email = "existente@email.com", Nome = "X", Senha = "Y" };
        _usuarioRepositoryMock.Setup(r => r.EmailExistsAsync(dto.Email)).ReturnsAsync(true);

        // Act
        var acao = async () => await _sut.CreateAsync(dto);

        // Assert
        await acao.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task GetByIdAsync_UsuarioNaoEncontrado_LancaException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Usuario?)null);

        // Act
        var acao = async () => await _sut.GetByIdAsync(id);

        // Assert
        await acao.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task DeleteAsync_IdValido_RetornaTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        _usuarioRepositoryMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        // Act
        var resultado = await _sut.DeleteAsync(id);

        // Assert
        resultado.Should().BeTrue();
    }
}
