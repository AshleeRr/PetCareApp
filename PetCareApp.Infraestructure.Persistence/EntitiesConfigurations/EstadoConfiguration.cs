using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class EstadoConfiguration : IEntityTypeConfiguration<Estado>
    {
        public void Configure(EntityTypeBuilder<Estado> builder)
        {
            #region base configurations
            builder.ToTable("Estado");
            builder.HasKey(e => e.Id).HasName("PK__Estado__3214EC07E4AF41C7");
            #endregion

            #region properties configurations
            builder.Property(e => e.Nombre)
                .HasMaxLength(30)
                .HasColumnName("Estado");
            #endregion

            #region relationship configurations
            #endregion
        }
    }
}
