using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Infraestructure.Persistence.Context;
using PetCareApp.Infraestructure.Persistence.Repositories;

namespace PetCareApp.UnitTests.Repositories
{
    public class UnitTestCita
    {
        private PetCareContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<PetCareContext>()
                .UseInMemoryDatabase(databaseName: $"BdTest.{Guid.NewGuid().ToString()}")
                .Options;
            return new PetCareContext(options);
        }

        [Fact] 
        public async Task GetCitasByDate_ShouldSuccess_WhenDateIsValid()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var input = new DateOnly(2025, 10, 17);

            context.Citas.AddRange(
                new Cita { FechaHora = new DateTime(2025, 10, 17, 9, 0, 0) },
                new Cita { FechaHora = new DateTime(2025, 10, 17, 14, 30, 0) },
                new Cita { FechaHora = new DateTime(2025, 10, 18, 11, 0, 0) }
            );
            await context.SaveChangesAsync();
            var repositoy = new CitaRepository(context);

            // Act
            var result = await repositoy.GetCitasByDate(input);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetCitasOfMascotaById_ShouldCitas_WhenCitasExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Citas.AddRange(
                new Cita { Id = 1, MascotaId = 1, FechaHora = DateTime.Now },
                new Cita { Id = 2, MascotaId = 1, FechaHora = DateTime.Now.AddDays(-1) },
                new Cita { Id = 3, MascotaId = 2, FechaHora = DateTime.Now } // otra mascota
            );
            await context.SaveChangesAsync();

            var repo = new CitaRepository(context);

            // Act
            var result = await repo.GetCitasOfMascotaById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, c => Assert.Equal(1, c.MascotaId));
        }
    }
}
