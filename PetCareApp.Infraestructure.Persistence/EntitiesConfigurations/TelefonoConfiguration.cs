using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class TelefonoConfiguration : IEntityTypeConfiguration<Telefono>
    {
        public void Configure(EntityTypeBuilder<Telefono> builder)
        {
            #region base configuration
            builder.ToTable("Telefonos");
            builder.HasKey(e => e.Id).HasName("PK__Telefono__3214EC07F9F7016C");
            #endregion

            #region properties configuration
            builder.Property(e => e.Contacto).HasMaxLength(20);
            builder.Property(e => e.Tipo).HasMaxLength(20);
            #endregion

            #region relations configuration
            builder.HasOne(d => d.Dueño).WithMany(p => p.Telefonos)
                .HasForeignKey(d => d.DueñoId)
                .HasConstraintName("FK_Telefonos_Duenios");
            #endregion
        }
    }
}
