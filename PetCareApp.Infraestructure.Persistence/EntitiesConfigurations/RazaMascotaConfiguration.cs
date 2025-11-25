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
    public class RazaMascotaConfiguration : IEntityTypeConfiguration<RazaMascota>
    {
        public void Configure(EntityTypeBuilder<RazaMascota> builder)
        {
            #region base configuration
            builder.ToTable("RazaMascota");
            builder.HasKey(e => e.Id);
            #endregion

            #region properties configuration
            builder.Property(e => e.Raza)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");
            #endregion

            #region relations configuration
            builder.HasMany(r => r.Mascota)
                .WithOne(m => m.Raza)
                .HasForeignKey(m => m.RazaId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
