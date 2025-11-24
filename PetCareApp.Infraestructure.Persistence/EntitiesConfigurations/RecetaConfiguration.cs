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

            // Relación con Veterinario (Usuario)
            builder.HasOne(d => d.Veterinario)
                .WithMany() 
                .HasForeignKey(d => d.VeterinarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recetas_Usuarios");

            // Relación 1:1 con Cita
            builder.HasOne(e => e.Cita)
                .WithOne(c => c.Recetas) 
                .HasForeignKey<Receta>(e => e.CitaId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Recetas_Citas");

            // Relación con RecetaMedicamentos
            builder.HasMany(r => r.RecetaMedicamentos)
                .WithOne(rm => rm.Receta)
                .HasForeignKey(rm => rm.RecetaId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}