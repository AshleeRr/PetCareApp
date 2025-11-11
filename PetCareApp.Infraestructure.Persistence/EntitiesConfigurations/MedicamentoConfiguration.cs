using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class MedicamentoConfiguration : IEntityTypeConfiguration<Medicamento>
    {
        public void Configure(EntityTypeBuilder<Medicamento> builder)
        {
            #region base configuration
                builder.ToTable("Medicamentos");
                builder.HasKey(e => e.Id);
            #endregion

            #region properties configuration
            builder.Property(e => e.Nombre).HasMaxLength(100);
            builder.Property(e => e.EspecificadoPara).HasMaxLength(255);
            builder.Property(e => e.Presentacion).HasMaxLength(100);
            builder.Property(e => e.Uso).HasMaxLength(100);
            #endregion

            #region relations configuration
            #endregion




        }
    }
}
