using CineFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineFinder.Infrastructure.Data.Configurations
{
    public class FilmeGeneroConfiguration : IEntityTypeConfiguration<FilmeGenero>
    {
        public void Configure(EntityTypeBuilder<FilmeGenero> builder)
        {
            builder.ToTable("FilmeGeneros");

            builder.HasKey(fg => new { fg.FilmeId, fg.GeneroId });

            builder.HasOne(fg => fg.Filme)
                .WithMany(f => f.FilmeGeneros)
                .HasForeignKey(fg => fg.FilmeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fg => fg.Genero)
                .WithMany(g => g.FilmeGeneros)
                .HasForeignKey(fg => fg.GeneroId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}