using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;
namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class TipoMascotaConfiguration : IEntityTypeConfiguration<TipoMascota>
    {
        public void Configure(EntityTypeBuilder<TipoMascota> builder)
        {
            #region base configuration
            builder.ToTable("TipoMascota");
            builder.HasKey(e => e.Id).HasName("PK__TipoMasc__3214EC07201D4BE4");
            #endregion

            #region properties configuration
            builder.Property(e => e.Tipo).HasMaxLength(30);
            #endregion

            #region relations configuration
            #endregion
        }
    }
}
