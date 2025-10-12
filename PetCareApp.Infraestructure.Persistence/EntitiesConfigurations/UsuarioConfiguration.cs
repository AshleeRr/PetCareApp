using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            #region base configuration
            builder.ToTable("Usuarios");
            builder.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC07E64ECD58");
            #endregion

            #region properties configuration
            builder.HasIndex(e => e.Email, "UQ__Usuarios__A9D105347C3DFB5E").IsUnique();

            builder.HasIndex(e => e.UserName, "UQ__Usuarios__C9F284562DE21081").IsUnique();

            builder.Property(e => e.Email).HasMaxLength(100);
            builder.Property(e => e.PasswordHashed).HasMaxLength(255);
            builder.Property(e => e.PhotoUrl).HasMaxLength(255);
            builder.Property(e => e.UserName).HasMaxLength(50);
            #endregion

            #region relations configuration
            builder.HasOne(d => d.Role).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Usuarios_Roles");
            #endregion
        }
    }
}
