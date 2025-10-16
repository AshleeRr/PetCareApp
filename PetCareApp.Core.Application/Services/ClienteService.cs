using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }
         
        public void CrearCliente(CrearClienteDto dto)
        {
            var nuevo = new Dueño
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Direccion = dto.Direccion,
                Cedula = dto.Cedula
            };
            _repository.Agregar(nuevo);
        }
        public void EditarCliente(int id, CrearClienteDto dto)
        {
            var existente = _repository.ObtenerPorId(id);
            if (existente == null) return;

            existente.Nombre = dto.Nombre;
            existente.Apellido = dto.Apellido;
            existente.Direccion = dto.Direccion;
            existente.Cedula = dto.Cedula;

            _repository.Editar(existente);
        }
        public void EliminarCliente(int id)
        {
            _repository.Eliminar(id);
        }
        public List<ClienteDto> FiltrarPorNombre(string nombre)
        {
            return _repository.FiltrarPorNombre(nombre)
            .Select(c => new ClienteDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Direccion = c.Direccion,
                Cedula = c.Cedula
            }).ToList();
        }

        public ClienteDto ObtenerPorCedula(string cedula)
        {
            var c = _repository.ObtenerPorCedula(cedula);
            if (c == null) return null;


            return new ClienteDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Direccion = c.Direccion,
                Cedula = c.Cedula
            };
        }
        public ClienteDto ObtenerPorId(int id)
        {
            var c = _repository.ObtenerPorId(id);
            if (c == null) return null;


            return new ClienteDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Direccion = c.Direccion,
                Cedula = c.Cedula
            };
        }
        public List<ClienteDto> ObtenerClientes()
        {
            return _repository.ObtenerTodos()
            .Select(c => new ClienteDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Direccion = c.Direccion,
                Cedula = c.Cedula
            }).ToList();
        }
    }
}
