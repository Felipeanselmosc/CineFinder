using CineFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineFinder.Infrastructure.Data.Configurations
{
    public class GeneroConfiguration : IEntityTypeConfiguration<Genero>
    {
        public void Configure(EntityTypeBuilder<Genero> builder)
        {
            builder.ToTable("Generos");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.TmdbGeneroId)
                .IsRequired();

            builder.HasIndex(g => g.TmdbGeneroId)
                .IsUnique();

            builder.Property(g => g.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(g => g.Descricao)
                .HasMaxLength(500);
        }
    }
}