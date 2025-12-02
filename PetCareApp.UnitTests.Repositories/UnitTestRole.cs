using Infraestructura.Persistencia.Repositorios;
using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Infraestructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Testing.PersistenceTest
{
    public class UnitTestRole
    {
        private PetCareContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<PetCareContext>()
                .UseInMemoryDatabase(databaseName: $"RoleTestDb_{Guid.NewGuid()}")
                .Options;

            return new PetCareContext(options);
        }

        [Fact]
        public async Task GetByNameAsync_ShouldReturnRole_WhenRoleExists()
        {
            //ARRANGE 
            var context = GetInMemoryDbContext();

            context.Roles.Add(new Role { Id = 1, Rol = "Admin" });
            context.Roles.Add(new Role { Id = 2, Rol = "Cliente" });
            await context.SaveChangesAsync();

            var repositorio = new RoleRepositorio(context);

            // ACT 
            var resultado = await repositorio.GetByNameAsync("Admin");

            // ASSERT 
            Assert.NotNull(resultado);            
            Assert.Equal("Admin", resultado.Rol);  
            Assert.Equal(1, resultado.Id);         
        }

        [Fact]
        public async Task GetByNameAsync_ShouldReturnNull_WhenRoleDoesNotExist()
        {
            // ARRANGE 
            var context = GetInMemoryDbContext();

            context.Roles.Add(new Role { Id = 1, Rol = "Cliente" });
            await context.SaveChangesAsync();

            var repositorio = new RoleRepositorio(context);

            // ACT 
            var resultado = await repositorio.GetByNameAsync("Veterinario");

            // ASSERT
            Assert.Null(resultado); 
        }
    }
}
