using CineFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineFinder.Infrastructure.Data.Configurations
{
    public class ListaConfiguration : IEntityTypeConfiguration<Lista>
    {
        public void Configure(EntityTypeBuilder<Lista> builder)
        {
            builder.ToTable("Listas");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Nome)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(l => l.Descricao)
                .HasMaxLength(1000);

            builder.Property(l => l.IsPublica)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(l => l.DataCriacao)
                .IsRequired();

        }
    }
}