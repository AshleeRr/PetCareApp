using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            #region base configuration
            builder.ToTable("Productos");
            builder.HasKey(e => e.Id).HasName("PK__Producto__3214EC07EFE62DF7");
            #endregion

            #region properties configuration
            builder.Property(e => e.Nombre).HasMaxLength(100);
            builder.Property(e => e.Stock).IsRequired();
            builder.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            //builder.Property(e => e.ImagenUrl).HasMaxLength(500);
            #endregion

            #region relations configuration
            builder.HasOne(d => d.TipoProducto).WithMany(p => p.Productos)
                .HasForeignKey(d => d.TipoProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_TipoProductos");
            #endregion

        }
    }
}
