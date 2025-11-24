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
    public class VentaRepository : IVentaRepository
    {
        private readonly PetCareContext _context;

        public VentaRepository(PetCareContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Venta>> GetAllAsync()
        {
            return await _context.Ventas
                .Include(v => v.Personal)
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();
        }

        public async Task<Venta?> GetByIdAsync(int id)
        {
            return await _context.Ventas
                .Include(v => v.Personal)
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Receta)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Cita)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Venta>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Ventas
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .Where(v => v.UsuarioId == usuarioId)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<Venta>> GetByFechaRangoAsync(DateTime desde, DateTime hasta)
        {
            return await _context.Ventas
                .Include(v => v.Personal)
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .Where(v => v.Fecha >= desde && v.Fecha <= hasta)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();
        }

        public async Task<Venta> AddAsync(Venta venta)
        {
            var entry = await _context.Ventas.AddAsync(venta);
            await _context.SaveChangesAsync();

            // Recargar con includes
            return (await GetByIdAsync(entry.Entity.Id))!;
        }

        public async Task<Venta?> UpdateAsync(Venta venta)
        {
            _context.Ventas.Update(venta);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(venta.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null) return false;

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ MÉTODOS NUEVOS
        public async Task<decimal> ObtenerIngresosMesActualAsync()
        {
            var inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var finMes = inicioMes.AddMonths(1).AddDays(-1);

            var ingresos = await _context.Ventas
                .Where(v => v.Fecha >= inicioMes && v.Fecha <= finMes)
                .SumAsync(v => v.Total);

            return ingresos;
        }

    }
}
