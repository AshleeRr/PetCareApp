using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly PetCareContext _context;

        public ClienteRepository(PetCareContext context)
        {
            _context = context;
        }
        
        public void Agregar(Dueño cliente)
        {
            _context.Dueños.Add(cliente);
            _context.SaveChanges();
        }

        public void Editar(Dueño cliente)
        {
            _context.Dueños.Update(cliente);
            _context.SaveChanges();
        }

        public void Eliminar(int id)
        {
            var cliente = _context.Dueños.Find(id);
            if (cliente != null)
            {
                _context.Dueños.Remove(cliente);
                _context.SaveChanges();
            }
        }

        public List<Dueño> FiltrarPorNombre(string nombre)
        {
            return _context.Dueños
            .Where(d => d.Nombre.Contains(nombre))
            .ToList();
        }

        public Dueño ObtenerPorCedula(string cedula)
        {
            return _context.Dueños.FirstOrDefault(d => d.Cedula == cedula);
        }

        public Dueño ObtenerPorId(int id)
        {
            return _context.Dueños.FirstOrDefault(d => d.Id == id);
        }

        public List<Dueño> ObtenerTodos()
        {
            return _context.Dueños.ToList();
        }
    }
}
