using CineFinder.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CineFinder.Tests.Integration;

public class CineFinderWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CineFinderDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<CineFinderDbContext>(options =>
            {
                options.UseInMemoryDatabase($"CineFinderTestDb_{Guid.NewGuid()}");
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CineFinderDbContext>();
            db.Database.EnsureCreated();
        });

        builder.UseEnvironment("Development");
    }
}

[CollectionDefinition("Integration Tests")]
public class IntegrationTestCollection : ICollectionFixture<CineFinderWebApplicationFactory>
{
}
