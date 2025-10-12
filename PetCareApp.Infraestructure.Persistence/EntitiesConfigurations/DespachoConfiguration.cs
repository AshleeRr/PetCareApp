using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class DespachoConfiguration : IEntityTypeConfiguration<Despacho>
    {
        public void Configure(EntityTypeBuilder<Despacho> builder)
        {
            #region base configurations
                builder.ToTable("Despacho");
                builder.HasKey(e => e.Id).HasName("PK__Despacho__3214EC07C612112A");
            #endregion

            #region properties configurations
                builder.Property(e => e.Fecha)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
            #endregion

            #region relationship configurations
                builder.HasOne(d => d.Personal).WithMany(p => p.Despachos)
               .HasForeignKey(d => d.PersonalId)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_Despacho_Personal");

                builder.HasOne(d => d.Producto).WithMany(p => p.Despachos)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Despacho_Productos");
            #endregion
        }
    }
}
