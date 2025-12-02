using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class MascotaPruebasMedicaConfiguration : IEntityTypeConfiguration<MascotaPruebasMedica>
    {

        public void Configure(EntityTypeBuilder<MascotaPruebasMedica> builder)
        {
            #region base configuration
            builder.ToTable("Mascota_PruebasMedicas");
            builder.HasKey(e => new { e.MascotaId, e.PruebaMedicaId });
            #endregion

            #region properties configuration

            builder.Property(e => e.Fecha)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime");

            builder.Property(e => e.Resultado).HasMaxLength(255);
            #endregion

            #region relations configuration
            builder.HasOne(d => d.Mascota).WithMany(p => p.MascotaPruebasMedicas)
                .HasForeignKey(d => d.MascotaId)
                .HasConstraintName("FK_MP_Mascota");

            builder.HasOne(d => d.PruebaMedica).WithMany(p => p.MascotaPruebasMedicas)
                .HasForeignKey(d => d.PruebaMedicaId)
                .HasConstraintName("FK_MP_PruebasMedicas");
            #endregion


        }
    }
}
