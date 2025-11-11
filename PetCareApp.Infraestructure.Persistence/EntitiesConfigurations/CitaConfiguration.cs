using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class CitaConfiguration : IEntityTypeConfiguration<Cita>
    {
        public void Configure(EntityTypeBuilder<Cita> builder) {

            #region base configurations
                builder.ToTable("Citas");
                builder.HasKey(e => e.Id).HasName("PK__Citas__3214EC07F879E4C2");
            #endregion

            #region properties configurations
            builder.Property(e => e.FechaHora).HasColumnType("datetime");
            #endregion region

            #region relations
            builder.HasOne(d => d.Dueño).WithMany(p => p.Cita)
            .HasForeignKey(d => d.DueñoId)
            .HasConstraintName("FK_Citas_Duenios");

            builder.HasOne(d => d.Mascota).WithMany(p => p.Cita)
            .HasForeignKey(d => d.DueñoId)
            .HasConstraintName("FK_Citas_Mascotas");

            builder.HasOne(d => d.Estado).WithMany(p => p.Cita)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Citas_Estado");

            builder.HasOne(d => d.Motivo).WithMany(p => p.Cita)
                .HasForeignKey(d => d.MotivoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Citas_MotivoCita");

            builder.HasOne(d => d.Veterinario).WithMany(p => p.Cita)
                .HasForeignKey(d => d.VeterinarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Citas_Personal");

            builder.HasOne(m => m.Mascota).WithMany(t => t.Cita)
                .HasForeignKey(m => m.MascotaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Citas_Mascota");
            #endregion


        }
    }
}
