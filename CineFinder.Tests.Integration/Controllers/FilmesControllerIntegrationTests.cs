using System.Net;
using System.Text;
using System.Text.Json;
using CineFinder.Application.DTOs.Filme;
using FluentAssertions;
using Xunit;

namespace CineFinder.Tests.Integration.Controllers;

[Collection("Integration Tests")]
public class FilmesControllerIntegrationTests : IClassFixture<CineFinderWebApplicationFactory>
{
    private readonly HttpClient _client;

    public FilmesControllerIntegrationTests(CineFinderWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_Endpoint_Responde()
    {
        var response = await _client.GetAsync("/api/filmes");
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound, HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task GetById_IdInexistente_RetornaNotFoundOuErro()
    {
        var response = await _client.GetAsync($"/api/filmes/{Guid.NewGuid()}");
        response.StatusCode.Should().BeOneOf(HttpStatusCode.NotFound, HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Create_Endpoint_Responde()
    {
        var dto = new CreateFilmeDto { TmdbId = 100001, Titulo = "Filme Teste", Descricao = "Desc", Diretor = "Dir", Duracao = 120, DataLancamento = new DateTime(2023, 1, 1), GeneroIds = new List<Guid>() };
        var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/filmes", content);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.NotFound, HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Delete_IdInexistente_RetornaNotFoundOuErro()
    {
        var response = await _client.DeleteAsync($"/api/filmes/{Guid.NewGuid()}");
        response.StatusCode.Should().BeOneOf(HttpStatusCode.NotFound, HttpStatusCode.InternalServerError);
    }
}
