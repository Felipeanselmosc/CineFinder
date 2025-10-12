using CineFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CineFinder.Infrastructure.Data.Context
{
    public class CineFinderDbContext : DbContext
    {
        public CineFinderDbContext(DbContextOptions<CineFinderDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Lista> Listas { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<FilmeGenero> FilmeGeneros { get; set; }
        public DbSet<ListaFilme> ListaFilmes { get; set; }
        public DbSet<UsuarioGeneroPreferido> UsuarioGeneroPreferidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CineFinderDbContext).Assembly);
        }
    }
}