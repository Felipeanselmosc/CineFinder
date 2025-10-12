
using CineFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineFinder.Infrastructure.Data.Configurations
{
    public class AvaliacaoConfiguration : IEntityTypeConfiguration<Avaliacao>
    {
        public void Configure(EntityTypeBuilder<Avaliacao> builder)
        {
            builder.ToTable("Avaliacoes");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Nota)
                .IsRequired();

            builder.Property(a => a.Comentario)
                .HasMaxLength(2000);

            builder.Property(a => a.DataAvaliacao)
                .IsRequired();

            builder.HasIndex(a => new { a.UsuarioId, a.FilmeId })
                .IsUnique();
        }
    }
}