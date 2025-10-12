using CineFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineFinder.Infrastructure.Data.Configurations
{
    public class FilmeConfiguration : IEntityTypeConfiguration<Filme>
    {
        public void Configure(EntityTypeBuilder<Filme> builder)
        {
            builder.ToTable("Filmes");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.TmdbId)
                .IsRequired();

            builder.HasIndex(f => f.TmdbId)
                .IsUnique();

            builder.Property(f => f.Titulo)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(f => f.Descricao)
                .HasMaxLength(2000);

            builder.Property(f => f.PosterUrl)
                .HasMaxLength(500);

            builder.Property(f => f.TrailerUrl)
                .HasMaxLength(500);

            builder.Property(f => f.Diretor)
                .HasMaxLength(200);

            builder.Property(f => f.NotaMedia)
                .HasPrecision(3, 2);

            builder.HasMany(f => f.Avaliacoes)
                .WithOne(a => a.Filme)
                .HasForeignKey(a => a.FilmeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}