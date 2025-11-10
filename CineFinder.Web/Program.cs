using CineFinder.Application.Interfaces;
using CineFinder.Application.Services;
using CineFinder.Domain.Interfaces;
using CineFinder.Infrastructure.Data.Context;
using CineFinder.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<CineFinderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();
builder.Services.AddScoped<IListaRepository, ListaRepository>();
builder.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();

// Services
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IFilmeService, FilmeService>();
builder.Services.AddScoped<IGeneroService, GeneroService>();
builder.Services.AddScoped<IListaService, ListaService>();
builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();

var app = builder.Build();

// Pipeline HTTP
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

// Rotas personalizadas
app.MapControllerRoute(
    name: "filmes-busca",
    pattern: "Filmes/Buscar/{termo?}",
    defaults: new { controller = "Filmes", action = "Buscar" });

app.MapControllerRoute(
    name: "filmes-genero",
    pattern: "Filmes/Genero/{generoId}",
    defaults: new { controller = "Filmes", action = "PorGenero" });

app.MapControllerRoute(
    name: "filme-detalhes",
    pattern: "Filme/{id}/Detalhes",
    defaults: new { controller = "Filmes", action = "Detalhes" });

app.MapControllerRoute(
    name: "usuario-listas",
    pattern: "Usuario/{usuarioId}/Listas",
    defaults: new { controller = "Listas", action = "MinhasListas" });

app.MapControllerRoute(
    name: "filme-avaliacoes",
    pattern: "Filme/{filmeId}/Avaliacoes",
    defaults: new { controller = "Avaliacoes", action = "PorFilme" });

app.MapControllerRoute(
    name: "generos-populares",
    pattern: "Generos/Populares",
    defaults: new { controller = "Generos", action = "Populares" });

// Rota padrão MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Controllers de API
app.MapControllers();

app.Run();
