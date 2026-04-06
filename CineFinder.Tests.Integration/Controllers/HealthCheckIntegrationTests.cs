using System.Net;
using FluentAssertions;
using Xunit;

namespace CineFinder.Tests.Integration.Controllers;

[Collection("Integration Tests")]
public class HealthCheckIntegrationTests : IClassFixture<CineFinderWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HealthCheckIntegrationTests(CineFinderWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Health_Endpoint_Responde()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.ServiceUnavailable);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("status");
    }

    [Fact]
    public async Task HealthLive_Endpoint_RetornaOk()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/health/live");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task HealthReady_Endpoint_Responde()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/health/ready");

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.ServiceUnavailable);
    }
}
