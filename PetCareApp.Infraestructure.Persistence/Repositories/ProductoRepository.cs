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
    public class ProductoRepository : IProductoRepository
    {
        private readonly PetCareContext _context;

        public ProductoRepository(PetCareContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _context.Productos
                .Include(p => p.TipoProducto)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.TipoProducto)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Producto>> GetByTipoAsync(int tipoProductoId)
        {
            return await _context.Productos
                .Include(p => p.TipoProducto)
                .Where(p => p.TipoProductoId == tipoProductoId)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> BuscarPorNombreAsync(string nombre)
        {
            return await _context.Productos
                .Include(p => p.TipoProducto)
                .Where(p => p.Nombre.Contains(nombre))
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetDisponiblesAsync()
        {
            return await _context.Productos
                .Include(p => p.TipoProducto)
                .Where(p => p.Stock > 0)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<Producto> AddAsync(Producto producto)
        {
            var entry = await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();

            // Cargar el TipoProducto
            await _context.Entry(entry.Entity)
                .Reference(p => p.TipoProducto)
                .LoadAsync();

            return entry.Entity;
        }

        public async Task<Producto?> UpdateAsync(int id, Producto producto)
        {
            var existente = await _context.Productos.FindAsync(id);
            if (existente == null)
                return null;

            existente.Nombre = producto.Nombre;
            existente.Stock = producto.Stock;
            existente.Precio = producto.Precio;
            existente.TipoProductoId = producto.TipoProductoId;
            existente.ImagenUrl = producto.ImagenUrl;

            await _context.SaveChangesAsync();

            await _context.Entry(existente)
                .Reference(p => p.TipoProducto)
                .LoadAsync();

            return existente;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
