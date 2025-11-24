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
            builder.HasKey(e => e.Id);
            #endregion

            #region properties configuration
            builder.Property(e => e.Tipo)
                .IsRequired()
                .HasMaxLength(30);
            #endregion

            #region relations configuration
            builder.HasMany(t => t.Mascotas)
                .WithOne(m => m.TipoMascota)
                .HasForeignKey(m => m.TipoMascotaId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
