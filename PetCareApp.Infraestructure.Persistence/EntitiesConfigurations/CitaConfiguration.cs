using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class CitaConfiguration : IEntityTypeConfiguration<Cita>
    {
        public void Configure(EntityTypeBuilder<Cita> builder)
        {
            #region base configurations
            builder.ToTable("Citas");
            builder.HasKey(e => e.Id).HasName("PK__Citas__3214EC07F879E4C2");
            #endregion

            #region properties configurations
            builder.Property(e => e.FechaHora).HasColumnType("datetime");
            builder.Property(e => e.Observaciones).HasMaxLength(255);
            builder.Ignore(e => e.RecetaId);
            #endregion

            #region relations

            // Relación con Dueño (N:1)
            builder.HasOne(d => d.Dueño)
                .WithMany(p => p.Cita)
                .HasForeignKey(d => d.DueñoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Citas_Duenios");

            // Relación con Mascota (N:1)
            builder.HasOne(d => d.Mascota)
                .WithMany(p => p.Citas) 
                .HasForeignKey(d => d.MascotaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Citas_Mascota");

            // Relación con Estado (N:1)
            builder.HasOne(d => d.Estado)
                .WithMany(p => p.Cita)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Citas_Estado");

            // Relación con MotivoCita (N:1)
            builder.HasOne(d => d.Motivo)
                .WithMany(p => p.Cita)
                .HasForeignKey(d => d.MotivoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Citas_MotivoCita");

            // Relación con Personal/Veterinario (N:1)
            builder.HasOne(d => d.Veterinario)
                .WithMany(p => p.Cita)
                .HasForeignKey(d => d.VeterinarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Citas_Personal");

            // Una Cita tiene UNA Receta (opcional), y la FK está en la tabla Recetas
            builder.HasOne(c => c.Recetas)
                .WithOne(r => r.Cita)
                .HasForeignKey<Receta>(r => r.CitaId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}