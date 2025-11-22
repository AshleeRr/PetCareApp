using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Infraestructure.Persistence.Context;
using PetCareApp.Infraestructure.Persistence.Repositories;

namespace PetCareApp.UnitTests.Repositories
{
    public class UnitTestMedicamentos
    {/*
        private PetCareContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<PetCareContext>()
                .UseInMemoryDatabase(databaseName: $"BdTest.{Guid.NewGuid().ToString()}")
                .Options;
            return new PetCareContext(options);
        }
        [Fact]
        public async Task GetMedicamentoByName_ShouldReturnMedicamento_WhenExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Medicamentos.Add(new Medicamento { Id = 1, Presentacion = "Paracetamol 500mg" });
            await context.SaveChangesAsync();

            var repo = new MedicamentosRepository(context);

            // Act
            var result = await repo.GetMedicamentoByName("Paracetamol 500mg");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Paracetamol 500mg", result.Presentacion);
        }

        [Fact]
        public async Task GetMedicamentoByName_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new MedicamentosRepository(context);

            // Act
            var result = await repo.GetMedicamentoByName("Ibuprofeno 400mg");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetMedicamentoByName_ShouldIgnoreCaseAndSpaces()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Medicamentos.Add(new Medicamento { Id = 1, Presentacion = "Paracetamol 500mg", Uso = "Dolor" });
            await context.SaveChangesAsync();

            var repo = new MedicamentosRepository(context);

            // Act
            var result = await repo.GetMedicamentoByName("  PARACETAMOL 500MG ");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Paracetamol 500mg", result.Presentacion);
        }
    }
        */
    }
}
