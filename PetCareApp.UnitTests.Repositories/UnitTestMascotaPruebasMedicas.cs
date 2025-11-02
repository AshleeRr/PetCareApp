using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Infraestructure.Persistence.Context;
using PetCareApp.Infraestructure.Persistence.Repositories;

namespace PetCareApp.UnitTests.Repositories
{
    public class UnitTestMascotaPruebasMedicas      
    {
        private PetCareContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<PetCareContext>()
                .UseInMemoryDatabase(databaseName: $"BdTest.{Guid.NewGuid().ToString()}")
                .Options;
            return new PetCareContext(options);
        }

        [Fact]
        public async Task GetPruebasOfMascotaById_ShouldReturnPruebas_WhenMascotaHasPruebas()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var prueba1 = new PruebasMedica { Id = 1, NombrePrueba = "Hemograma" };
            var prueba2 = new PruebasMedica { Id = 2, NombrePrueba = "Rayos X" };

            context.PruebasMedicas.AddRange(prueba1, prueba2);
            context.MascotaPruebasMedicas.AddRange(
                new MascotaPruebasMedica { MascotaId = 1, PruebaMedicaId = 1, Fecha = DateTime.Now.AddDays(-1), PruebaMedica = prueba1 },
                new MascotaPruebasMedica { MascotaId = 1, PruebaMedicaId = 2, Fecha = DateTime.Now, PruebaMedica = prueba2 }
            );
            await context.SaveChangesAsync();

            var repo = new TratamientoRepository(context);

            // Act
            var result = await repo.GetPruebasOfMascotaById(1);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Rayos X", result.First().PruebaMedica.NombrePrueba); 
        }

        [Fact]
        public async Task GetPruebasOfMascotaById_ShouldReturnEmpty_WhenMascotaHasNoPruebas()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new TratamientoRepository(context);

            // Act
            var result = await repo.GetPruebasOfMascotaById(2);

            // Assert
            Assert.Empty(result);
        }
    }
}
