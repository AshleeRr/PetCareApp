using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class PruebasMedicaConfiguration : IEntityTypeConfiguration<PruebasMedica>
    {
        public void Configure(EntityTypeBuilder<PruebasMedica> builder)
        {
            #region base configuration
            builder.ToTable("PruebasMedicas");
            builder.HasKey(e => e.Id).HasName("PK__PruebasM__3214EC071D44E250");
            #endregion

            #region properties configuration
            builder.Property(e => e.NombrePrueba).HasMaxLength(100);
            #endregion

            #region relations configuration
            #endregion




        }
    }
}
