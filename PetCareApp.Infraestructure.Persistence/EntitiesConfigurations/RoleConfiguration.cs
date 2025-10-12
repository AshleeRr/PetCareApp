using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            #region base configuration
            builder.ToTable("Roles");
            builder.HasKey(e => e.Id).HasName("PK__Roles__3214EC076169ADF0");
            #endregion

            #region properties configuration
            builder.Property(e => e.Rol).HasMaxLength(50);
            #endregion

            #region relations configuration
            #endregion
        }
    }
}
