using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class LogInventarioConfiguration : IEntityTypeConfiguration<LogInventario>
    {
        public void Configure(EntityTypeBuilder<LogInventario> builder)
        {
            #region base configurations
                builder.ToTable("LogInventario");
                builder.HasKey(e => e.Id).HasName("PK__LogInven__3214EC07D374162A");
            #endregion

            #region properties configurations
            builder.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            builder.Property(e => e.Observaciones).HasMaxLength(255);
            builder.Property(e => e.TipoMovimiento).HasMaxLength(50);
            #endregion

            #region relationship configurations
            builder.HasOne(d => d.Personal).WithMany(p => p.LogInventarios)
                .HasForeignKey(d => d.PersonalId)
                .HasConstraintName("FK_LogInventario_Personal");

            builder.HasOne(d => d.Producto).WithMany(p => p.LogInventarios)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogInventario_Producto");
            #endregion
        }
    }
}
