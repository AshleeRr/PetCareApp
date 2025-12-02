using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using PetCareApp.Core.Application.Dtos.RecetasDtos;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace ApplicationTest
{
    public class UnitTestReceta
    {

        private readonly Mock<IRecetaRepository> _recetaRepoMock = new();
        private readonly Mock<ICitaRepository> _citaRepoMock = new();
        private readonly Mock<IMedicamentoRepository> _medRepoMock = new();
        private readonly IMapper _mapper;

        public UnitTestReceta()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateRecetaDto, Receta>();
                cfg.CreateMap<Receta, RecetaDto>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task CreateRecetaAsync_ShouldCreateReceta_WhenCitaExists()
        {
            // ARRANGE
            var dto = new CreateRecetaDto
            {
                CitaId = 5,
                Observaciones = "Todo normal"
            };

            _citaRepoMock.Setup(r => r.GetByIdAsync(5))
                .ReturnsAsync(new Cita { Id = 5 });

            _recetaRepoMock.Setup(r => r.AddAsync(It.IsAny<Receta>()))
                .ReturnsAsync((Receta r) =>
                {
                    r.Id = 10; 
                    return r;
                });

            var service = new RecetaService(
                _recetaRepoMock.Object,
                _citaRepoMock.Object,
                _medRepoMock.Object,
                _mapper
            );

            // ACT
            var result = await service.CreateRecetaAsync(dto);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(10, result.Id);
            Assert.Equal("Todo normal", result.Observaciones);

            _recetaRepoMock.Verify(r => r.AddAsync(It.IsAny<Receta>()), Times.Once);
        }

        [Fact]
        public async Task AddMedicamentoToRecetaAsync_ShouldAddMedicamento_WhenDataIsValid()
        {
            // ARRANGE
            var dto = new AddMedicamentoToRecetaDto
            {
                RecetaId = 1,
                MedicamentoId = 3,
                Dosis = "20 mg",
                Duracion = "5 días",
                Observaciones = "Con comida"
            };

            _recetaRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Receta { Id = 1 });

            _medRepoMock.Setup(r => r.GetByIdAsync(3))
                .ReturnsAsync(new Medicamento
                {
                     Id = 3,
                     Nombre = "Amoxicilina",
                     Uso = "Infecciones bacterianas",
                     Presentacion = "Tabletas",
                     EspecificadoPara = "Perros"
                });

            var service = new RecetaService(
                _recetaRepoMock.Object,
                _citaRepoMock.Object,
                _medRepoMock.Object,
                _mapper
            );

            // ACT
            var result = await service.AddMedicamentoToRecetaAsync(dto);

            // ASSERT
            Assert.True(result);

            _recetaRepoMock.Verify(r => r.AddMedicamentoToRecetaAsync(
                It.Is<RecetaMedicamento>(rm =>
                    rm.RecetaId == 1 &&
                    rm.MedicamentoId == 3 &&
                    rm.DosisIndicada == "20 mg" &&
                    rm.DuracionTratamiento == "5 días" &&
                    rm.Observaciones == "Con comida"
                )
            ), Times.Once);
        }


        [Fact]
        public async Task GetRecetasByCitaAsync_ShouldReturnMappedDtos()
        {
            // ARRANGE
            var recetas = new List<Receta>
    {
        new Receta { Id = 1, CitaId = 5, Observaciones = "OK" },
        new Receta { Id = 2, CitaId = 5, Observaciones = "Seguimiento" }
    };

            _recetaRepoMock.Setup(r => r.GetByCitaIdAsync(5))
                .ReturnsAsync(recetas);

            var service = new RecetaService(
                _recetaRepoMock.Object,
                _citaRepoMock.Object,
                _medRepoMock.Object,
                _mapper
            );

            // ACT
            var result = await service.GetRecetasByCitaAsync(5);

            // ASSERT
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.Id == 1);
            Assert.Contains(result, x => x.Id == 2);
        }

    }
}