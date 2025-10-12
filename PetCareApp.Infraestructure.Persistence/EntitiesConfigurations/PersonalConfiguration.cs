using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class PersonalConfiguration : IEntityTypeConfiguration<Personal>
    {
        public void Configure(EntityTypeBuilder<Personal> builder)
        {
            #region base configuration
            builder.ToTable("Personal");
            builder.HasKey(e => e.Id);
            #endregion

            #region properties configuration
            builder.HasIndex(e => e.Cedula, "UQ__Personal__B4ADFE38CD5A32A7").IsUnique();
            builder.Property(e => e.Apellido).HasMaxLength(50);
            builder.Property(e => e.Cargo).HasMaxLength(50);
            builder.Property(e => e.Cedula).HasMaxLength(20);
            builder.Property(e => e.Nombre).HasMaxLength(50);
            #endregion

            #region relations configuration
            #endregion

        }
    }
}
