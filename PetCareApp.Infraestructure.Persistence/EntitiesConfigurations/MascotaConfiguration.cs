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
                builder.HasKey(e => e.Id).HasName("PK__Mascota__3214EC070A4B687D");
            #endregion

            #region properties configuration
            builder.Property(e => e.Nombre).HasMaxLength(50);
            builder.Property(e => e.Peso).HasColumnType("decimal(5, 2)");
            #endregion

            #region relations configuration
                builder.HasOne(d => d.Dueño).WithMany(p => p.Mascota)
                .HasForeignKey(d => d.DueñoId)
                .HasConstraintName("FK_Mascota_Dueños");

                builder.HasOne(d => d.TipoMascota).WithMany(p => p.Mascota)
                    .HasForeignKey(d => d.TipoMascotaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mascota_TipoMascota");
            #endregion
        }
    }
}
