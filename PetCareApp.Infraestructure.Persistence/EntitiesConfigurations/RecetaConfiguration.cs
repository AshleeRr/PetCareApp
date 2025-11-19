using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class RecetaConfiguration : IEntityTypeConfiguration<Receta>
    {
        public void Configure(EntityTypeBuilder<Receta> builder)
        {
            #region base configuration
            builder.ToTable("Recetas");
            builder.HasKey(e => e.Id).HasName("PK__Recetas__3214EC070CE8C4A1");
            #endregion

            #region properties configuration
            builder.Property(e => e.Fecha)
               .HasDefaultValueSql("(getdate())")
               .HasColumnType("datetime");
            builder.Property(e => e.Observaciones).HasMaxLength(255);
            #endregion

            #region relations configuration
            builder.HasOne(d => d.Veterinario).WithMany(p => p.Recetas)
                .HasForeignKey(d => d.VeterinarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recetas_Usuarios");

            builder.HasMany(r => r.RecetaMedicamentos)
                .WithOne(rm => rm.Receta)
                .HasForeignKey(rm => rm.RecetaId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
