using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Infraestructure.Persistence.Context;
using PetCareApp.Infraestructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Testing.PersistenceTest
{
    public class UnitTestReceta
    {
        private PetCareContext GetInMemoryDbContext()
        {
            var options  = new DbContextOptionsBuilder<PetCareContext>()
                .UseInMemoryDatabase(databaseName: $"DbTest_{Guid.NewGuid()}")
                .Options;

            return new PetCareContext(options);
        }


        [Fact]
        public async Task AddMedicamentoToRecetaAsync_ShouldAddRelation_WhenValid()
        {

            //ARRANGE
            var context  = GetInMemoryDbContext();
            var repo = new RecetaRepository(context);

            var relacion = new RecetaMedicamento
            { 
                RecetaId = 1,
                MedicamentoId = 5,
                DosisIndicada = "50 gramos",
                DuracionTratamiento = "3 meses",
                Observaciones = "Tiene cancer"
            };

            //ACT
            await repo.AddMedicamentoToRecetaAsync(relacion);


            //ASSERT
            var resultado = await context.RecetaMedicamentos.FirstOrDefaultAsync();

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.RecetaId);
            Assert.Equal(5, resultado.MedicamentoId);
            Assert.Equal("50 gramos", resultado.DosisIndicada);
            Assert.Equal("3 meses", resultado.DuracionTratamiento);
            Assert.Equal("Tiene cancer", resultado.Observaciones);
        }

        [Fact]
        public async Task GetByCitaIdAsync_ShouldReturnRecetasWithMedicamentos_WhenExist()
        {
            // ARRANGE
            var context = GetInMemoryDbContext();
            var repo = new RecetaRepository(context);

            var med = new Medicamento
            {
                Id = 10,
                Nombre = "Amoxicilina",
                Uso = "Oral",
                Presentacion = "Caja",
                EspecificadoPara = "Infecciones"
            };

            context.Medicamentos.Add(med);

            var receta = new Receta
            {
                Id = 1,
                CitaId = 7,
                Observaciones = "Paciente con infección"
            };
            context.Recetas.Add(receta);

            context.RecetaMedicamentos.Add(new RecetaMedicamento
            {
                RecetaId = 1,
                MedicamentoId = 10,
                DosisIndicada = "100 mg",
                DuracionTratamiento = "7 días",
                Observaciones = "Tomar después de comer"
            });

            await context.SaveChangesAsync();

            // ACT
            var result = await repo.GetByCitaIdAsync(7);

            // ASSERT
            Assert.NotNull(result);
            Assert.Single(result);                        
            Assert.Equal(1, result[0].Id);                 

            Assert.Single(result[0].RecetaMedicamentos);   

            var rm = result[0].RecetaMedicamentos.First();

            Assert.Equal(10, rm.MedicamentoId);
            Assert.NotNull(rm.Medicamento);                
            Assert.Equal("Amoxicilina", rm.Medicamento.Nombre);
        }



    }
}
