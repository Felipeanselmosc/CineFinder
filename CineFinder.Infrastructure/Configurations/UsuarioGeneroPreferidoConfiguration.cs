using CineFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineFinder.Infrastructure.Data.Configurations
{
    public class UsuarioGeneroPreferidoConfiguration : IEntityTypeConfiguration<UsuarioGeneroPreferido>
    {
        public void Configure(EntityTypeBuilder<UsuarioGeneroPreferido> builder)
        {
            builder.ToTable("UsuarioGeneroPreferidos");

            builder.HasKey(ugp => new { ugp.UsuarioId, ugp.GeneroId });

            builder.Property(ugp => ugp.NivelPreferencia)
                .HasDefaultValue(1);

            builder.HasOne(ugp => ugp.Usuario)
                .WithMany(u => u.GenerosPreferidos)
                .HasForeignKey(ugp => ugp.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ugp => ugp.Genero)
                .WithMany(g => g.UsuariosPreferidos)
                .HasForeignKey(ugp => ugp.GeneroId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}