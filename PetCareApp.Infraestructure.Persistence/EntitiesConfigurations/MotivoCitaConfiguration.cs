using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class MotivoCitaConfiguration : IEntityTypeConfiguration<MotivoCita>
    {
        public void Configure(EntityTypeBuilder<MotivoCita> builder)
        {
            #region base configuration
            builder.ToTable("MotivoCita");
            builder.HasKey(e => e.Id).HasName("PK__MotivoCi__3214EC07083B5AB8");
            #endregion

            #region properties configuration
            builder.Property(e => e.Motivo).HasColumnName("MotivoCita")
               .HasMaxLength(100)
               .IsRequired();
            #endregion

            #region relations configuration
            #endregion
        }
    }
}
