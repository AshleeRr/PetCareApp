
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;

namespace PetCareApp.Core.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repo;

        public ClienteService(IClienteRepository repo)
        {
            _repo = repo;
        }

        private static ClienteDto MapToDto(Dueño c) => // cambiado a due;o porque estaba dando error porque tenia cliente, no hay entidad cliente.
            new ClienteDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Direccion = c.Direccion,
                Cedula = c.Cedula,
              //  Email = c.Email
            };

        public async Task<List<ClienteDto>> ObtenerClientesAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(MapToDto).ToList();
        }

        public async Task<ClienteDto?> ObtenerPorIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : MapToDto(entity);
        }

        public async Task<ClienteDto?> ObtenerPorCedulaAsync(string cedula)
        {
            var entity = await _repo.GetByCedulaAsync(cedula);
            return entity == null ? null : MapToDto(entity);
        }

        public async Task<ClienteDto> CrearClienteAsync(CrearClienteDto dto)
        {
            // validaciones básicas
            if (string.IsNullOrWhiteSpace(dto.Cedula)) throw new System.ArgumentException("Cédula requerida");

            var exist = await _repo.GetByCedulaAsync(dto.Cedula);
            if (exist != null) throw new System.InvalidOperationException("Ya existe un cliente con esa cédula");

            var cliente = new Dueño()
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Direccion = dto.Direccion,
                Cedula = dto.Cedula,
               // Email = dto.Email
            };

            

            var created = await _repo.AddAsync(cliente);
            return MapToDto(created);
        }

        public async Task<bool> EditarClienteAsync(int id, ActualizarClienteDto dto)
        {
            var cliente = await _repo.GetByIdAsync(id);
            if (cliente == null) return false;

            cliente.Nombre = dto.Nombre ?? cliente.Nombre;
            cliente.Apellido = dto.Apellido ?? cliente.Apellido;
            cliente.Direccion = dto.Direccion ?? cliente.Direccion;
           // cliente.Email = dto.Email ?? cliente.Email;

            await _repo.UpdateAsync(cliente);
            return true;
        }

        public async Task<bool> EliminarClienteAsync(int id)
        {
            var cliente = await _repo.GetByIdAsync(id);
            if (cliente == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }

        public async Task<List<ClienteDto>> FiltrarPorNombreAsync(string nombre, string cedula)
        {
            var list = await _repo.FilterAsync(nombre, cedula);
            return list.Select(MapToDto).ToList();
        }
    }
}
