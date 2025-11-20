using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class CarritoRepository : ICarritoRepository
    {
        private readonly PetCareContext _context;

        public CarritoRepository(PetCareContext context)
        {
            _context = context;
        }

        public async Task<Carrito?> GetCarritoActivoByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Carritos
                .Include(c => c.Items)
                    .ThenInclude(i => i.Producto)
                        .ThenInclude(p => p.TipoProducto)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Activo);
        }

        public async Task<Carrito> CrearCarritoAsync(int usuarioId)
        {
            var carrito = new Carrito
            {
                UsuarioId = usuarioId,
                FechaCreacion = DateTime.Now,
                Activo = true
            };

            var entry = await _context.Carritos.AddAsync(carrito);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<CarritoItem?> GetCarritoItemByIdAsync(int itemId)
        {
            return await _context.CarritoItems
                .Include(i => i.Producto)
                .Include(i => i.Carrito)
                .FirstOrDefaultAsync(i => i.Id == itemId);
        }

        public async Task<CarritoItem> AgregarItemAsync(CarritoItem item)
        {
            var entry = await _context.CarritoItems.AddAsync(item);
            await _context.SaveChangesAsync();

            await _context.Entry(entry.Entity)
                .Reference(i => i.Producto)
                .LoadAsync();

            return entry.Entity;
        }

        public async Task<CarritoItem?> ActualizarItemAsync(CarritoItem item)
        {
            _context.CarritoItems.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> EliminarItemAsync(int itemId)
        {
            var item = await _context.CarritoItems.FindAsync(itemId);
            if (item == null)
                return false;

            _context.CarritoItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VaciarCarritoAsync(int carritoId)
        {
            var items = await _context.CarritoItems
                .Where(i => i.CarritoId == carritoId)
                .ToListAsync();

            _context.CarritoItems.RemoveRange(items);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DesactivarCarritoAsync(int carritoId)
        {
            var carrito = await _context.Carritos.FindAsync(carritoId);
            if (carrito == null)
                return false;

            carrito.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
