using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class DueñoConfiguration : IEntityTypeConfiguration<Dueño>
    {
        public void Configure(EntityTypeBuilder<Dueño> builder)
        {
            #region base configurations
            builder.ToTable("Dueños");
            builder.HasKey(e => e.Id).HasName("PK__Dueños__3214EC075F9F3976");
            #endregion

            #region properties configurations
            builder.HasIndex(e => e.Cedula, "UQ__Dueños__B4ADFE3868223F11").IsUnique();

            builder.Property(e => e.Apellido).HasMaxLength(50);
            builder.Property(e => e.Cedula).HasMaxLength(20);
            builder.Property(e => e.Direccion).HasMaxLength(100);
            builder.Property(e => e.Nombre).HasMaxLength(50);

            builder.Property(e => e.Email)
                .HasMaxLength(100)
                .IsRequired(false);
            #endregion

            #region relationship configurations
            #endregion





        }
    }
}
