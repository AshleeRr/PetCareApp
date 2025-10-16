using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;
namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class ProveedorConfiguration : IEntityTypeConfiguration<Proveedor>
    {
        public void Configure(EntityTypeBuilder<Proveedor> builder)
        {
            #region base configuration
            builder.ToTable("Proveedor");
            builder.HasKey(e => e.Id).HasName("PK__Proveedo__3214EC07B7A2E7B5");
            #endregion

            #region properties configuration
            builder.Property(e => e.Contacto).HasMaxLength(20);
            builder.Property(e => e.Direccion).HasMaxLength(150);
            builder.Property(e => e.Nombre).HasMaxLength(100);
            #endregion

            #region relations configuration
            builder.HasMany(d => d.Productos).WithMany(p => p.Proveedores)
                .UsingEntity<Dictionary<string, object>>(
                    "Provee",
                    r => r.HasOne<Producto>().WithMany()
                        .HasForeignKey("ProductoId")
                        .HasConstraintName("FK_Provee_Productos"),
                    l => l.HasOne<Proveedor>().WithMany()
                        .HasForeignKey("ProveedorId")
                        .HasConstraintName("FK_Provee_Proveedor"),
                    j =>
                    {
                        j.HasKey("ProveedorId", "ProductoId").HasName("PK__Provee__4B6560B36B50FA3C");
                        j.ToTable("Provee");
                    });
            #endregion
        }
    }
}
