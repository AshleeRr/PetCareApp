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
    public class TipoProductoConfiguration : IEntityTypeConfiguration<TipoProducto>
    {
        public void Configure(EntityTypeBuilder<TipoProducto> builder)
        {
            #region base configuration
            builder.ToTable("TipoProducto");
            builder.HasKey(e => e.Id).HasName("PK__TipoProd__3214EC07A5FB8501");
            #endregion

            #region properties configuration
            builder.Property(e => e.Tipo).HasMaxLength(50);
            #endregion

            #region relations configuration
            #endregion
        }
    }
}
