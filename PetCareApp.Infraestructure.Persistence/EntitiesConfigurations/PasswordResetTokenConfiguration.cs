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
    public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
    {
        public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
        {
            builder.ToTable("PasswordResetTokens");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Token)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.Usado)
                .HasDefaultValue(false);

            builder.HasIndex(e => e.Token);

            builder.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
