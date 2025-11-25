using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class MascotaConfiguration : IEntityTypeConfiguration<Mascota>
    {
        public void Configure(EntityTypeBuilder<Mascota> builder)
        {
            #region base configuration
            builder.ToTable("Mascota");
            builder.HasKey(e => e.Id);
            #endregion

            #region properties configuration
            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Peso)
                .HasColumnType("decimal(5, 2)");

            builder.Property(e => e.PhotoUrl)
                .HasMaxLength(225)
                .HasColumnType("varchar(225)");
            #endregion

            #region relations configuration
            // Relación con Dueño
            builder.HasOne(d => d.Dueño)
                .WithMany(p => p.Mascota)
                .HasForeignKey(d => d.DueñoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Mascota_Dueños");

            // Relación con TipoMascota
            builder.HasOne(d => d.TipoMascota)
                .WithMany(p => p.Mascotas)
                .HasForeignKey(d => d.TipoMascotaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Mascota_TipoMascota");

            // Relación con RazaMascota
            builder.HasOne(d => d.Raza)
                .WithMany(p => p.Mascota)
                .HasForeignKey(d => d.RazaId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Mascota_RazaMascota");
            #endregion
        }
    }
}
