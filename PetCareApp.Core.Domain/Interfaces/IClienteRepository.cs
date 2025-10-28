using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface IClienteRepository
    { 
        List<Dueño> ObtenerTodos();
        Dueño ObtenerPorId(int id);
        Dueño ObtenerPorCedula(string cedula);
        void Agregar(Dueño cliente); //  por que no hereda de la generica
        void Editar(Dueño cliente);
        void Eliminar(int id);
        List<Dueño> FiltrarPorNombre(string nombre);
    }
}
// lo mismo aqui, deberia ser task? 
