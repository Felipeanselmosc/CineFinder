using CineFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineFinder.Infrastructure.Data.Configurations
{
    public class ListaFilmeConfiguration : IEntityTypeConfiguration<ListaFilme>
    {
        public void Configure(EntityTypeBuilder<ListaFilme> builder)
        {
            builder.ToTable("ListaFilmes");

            builder.HasKey(lf => new { lf.ListaId, lf.FilmeId });

            builder.Property(lf => lf.DataAdicao)
                .IsRequired();

            builder.Property(lf => lf.Ordem)
                .HasDefaultValue(0);

            builder.HasOne(lf => lf.Lista)
                .WithMany(l => l.ListaFilmes)
                .HasForeignKey(lf => lf.ListaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(lf => lf.Filme)
                .WithMany(f => f.ListaFilmes)
                .HasForeignKey(lf => lf.FilmeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}