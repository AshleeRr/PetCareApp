using AutoMapper;
using Moq;
using PetCareApp.Core.Application.Dtos.UsersDtos;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Application.Services;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using Xunit;

namespace PetCareApp.Testing.ApplicationTest
{
    public class UnitTestUser
    {
        private readonly Mock<IUsuarioRepositorio> _usuarioRepoMock;
        private readonly Mock<Ilogger> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _service;

        public UnitTestUser()
        {
            _usuarioRepoMock = new Mock<IUsuarioRepositorio>();
            _loggerMock = new Mock<Ilogger>();
            _mapperMock = new Mock<IMapper>();

            _service = new UserService(
                _usuarioRepoMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
            );
        }


        [Fact]
        public async Task GetUserByEmailAsync_ShouldThrowArgumentException_WhenEmailIsNullOrEmpty()
        {
            // Act, Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.GetUserByEmailAsync("")
            );


            _loggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldThrowInvalidOperationException_WhenUserNotFound()
        {
            // Arrange
            _usuarioRepoMock
                .Setup(r => r.GetByEmailAsync("test@test.com"))
                .ReturnsAsync((Usuario?)null);

            // Act ,Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.GetUserByEmailAsync("test@test.com")
            );

            _loggerMock.Verify(l => l.Error(It.Is<string>(msg => msg.Contains("Usuario no encontrado")), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldThrowInvalidOperationException_WhenUserHasNoRole()
        {
            // Arrange
            var fakeUser = new Usuario
            {
                Id = 1,
                Email = "test@test.com",
                Role = null,
                UserName = "TestUser",
                PasswordHashed = "HashSeguro123"
            };

            _usuarioRepoMock
                .Setup(r => r.GetByEmailAsync("test@test.com"))
                .ReturnsAsync(fakeUser);

            // Act ,Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.GetUserByEmailAsync("test@test.com")
            );

            _loggerMock.Verify(l => l.Error(It.Is<string>(msg => msg.Contains("no tiene un rol asignado")), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnUserDto_WhenUserExistsAndHasRole()
        {
            // Arrange
            var fakeUser = new Usuario
            {
                Id = 1,
                Email = "test@test.com",
                UserName = "TestUser",
                PasswordHashed = "HashSeguro123",
                Role = new Role { Id = 1, Rol = "Admin" }
            };

            var fakeDto = new UserDto
            {
                Id = 1,
                Email = "test@test.com",
                UserName = "TestUser",
                Role = "Admin"
            };

            _usuarioRepoMock
                .Setup(r => r.GetByEmailAsync("test@test.com"))
                .ReturnsAsync(fakeUser);

            _mapperMock
                .Setup(m => m.Map<UserDto>(fakeUser))
                .Returns(fakeDto);

            // Act
            var result = await _service.GetUserByEmailAsync("test@test.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test@test.com", result.Email);
            Assert.Equal("Admin", result.Role);
        }
    }
}