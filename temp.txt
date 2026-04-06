using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CineFinder.Infrastructure.Repositories;
using CineFinder.Application.Interfaces;
using CineFinder.Application.Services;
using CineFinder.Domain.Interfaces;
using CineFinder.Infrastructure.Data.Context;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços de API
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "CineFinder API",
        Version = "v1",
        Description = "API REST para gerenciamento de filmes, avaliações, listas e gêneros",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "CineFinder",
            Email = "contato@cinefinder.com"
        }
    });
});

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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CineFinder API v1");
        c.RoutePrefix = string.Empty; // Swagger na raiz
    });
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthorization();

// Mapear controllers
app.MapControllers();

app.Run();

