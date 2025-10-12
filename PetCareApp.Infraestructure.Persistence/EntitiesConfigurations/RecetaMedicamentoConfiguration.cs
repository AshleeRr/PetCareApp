using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.EntitiesConfigurations
{
    public class RecetaMedicamentoConfiguration : IEntityTypeConfiguration<RecetaMedicamento>
    {
        public void Configure(EntityTypeBuilder<RecetaMedicamento> builder)
        {
            #region base configuration
            builder.ToTable("Receta_Medicamento");
            builder.HasKey(e => new { e.RecetaId, e.MedicamentoId }).HasName("PK__Receta_M__23D3A18525547B9E");
            #endregion

            #region properties configuration
            builder.Property(e => e.DosisIndicada).HasMaxLength(100);
            builder.Property(e => e.DuracionTratamiento).HasMaxLength(100);
            builder.Property(e => e.Observaciones).HasMaxLength(255);
            #endregion

            #region relations configuration
            builder.HasOne(d => d.Medicamento).WithMany(p => p.RecetaMedicamentos)
                .HasForeignKey(d => d.MedicamentoId)
                .HasConstraintName("FK_RM_Medicamentos");

            builder.HasOne(d => d.Receta).WithMany(p => p.RecetaMedicamentos)
                .HasForeignKey(d => d.RecetaId)
                .HasConstraintName("FK_RM_Recetas");
            #endregion
        }
    }
}
