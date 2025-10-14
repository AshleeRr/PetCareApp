using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCareApp.Core.Application.Dtos;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IClienteService
    {
        List<ClienteDto> ObtenerClientes();
        ClienteDto ObtenerPorId(int id);
        ClienteDto ObtenerPorCedula(string cedula);
        void CrearCliente(CrearClienteDto dto);
        void EditarCliente(int id, CrearClienteDto dto);
        void EliminarCliente(int id);
        List<ClienteDto> FiltrarPorNombre(string nombre);
    }
}
