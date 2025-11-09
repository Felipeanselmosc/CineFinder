using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CineFinder.Infrastructure.Repositories;
using CineFinder.Application.Interfaces;
using CineFinder.Application.Services;
using CineFinder.Domain.Interfaces;
using CineFinder.Infrastructure.Data.Context;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços MVC
builder.Services.AddControllersWithViews();

// Adicionar serviços de API (Controllers de API)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar DbContext
builder.Services.AddDbContext<CineFinderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Registrar Repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();
builder.Services.AddScoped<IListaRepository, ListaRepository>();
builder.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();

// Registrar Services
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IFilmeService, FilmeService>();
builder.Services.AddScoped<IGeneroService, GeneroService>();
builder.Services.AddScoped<IListaService, ListaService>();
builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();

var app = builder.Build();

// Configurar pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");
app.UseAuthorization();

// Rotas MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Rota personalizada para busca de filmes
app.MapControllerRoute(
    name: "filmes-busca",
    pattern: "Filmes/Buscar/{termo?}",
    defaults: new { controller = "Filmes", action = "Buscar" });

// Rota personalizada para filmes por gênero
app.MapControllerRoute(
    name: "filmes-genero",
    pattern: "Filmes/Genero/{generoId}",
    defaults: new { controller = "Filmes", action = "PorGenero" });

// Rota personalizada para listas de usuário
app.MapControllerRoute(
    name: "usuario-listas",
    pattern: "Usuario/{usuarioId}/Listas",
    defaults: new { controller = "Listas", action = "MinhasListas" });

// Controllers de API (mantém as rotas /api/...)
app.MapControllers();

app.Run();