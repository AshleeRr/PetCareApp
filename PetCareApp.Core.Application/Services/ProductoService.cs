using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepo;
        private readonly Ilogger _logger;

        public ProductoService(IProductoRepository productoRepo, Ilogger logger)
        {
            _productoRepo = productoRepo;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductoDto>> ObtenerTodosAsync()
        {
            try
            {
                var productos = await _productoRepo.GetAllAsync();
                return productos.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al obtener productos: {ex.Message}");
                throw;
            }
        }

        public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var producto = await _productoRepo.GetByIdAsync(id);
                return producto != null ? MapToDto(producto) : null;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al obtener producto {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ProductoDto>> ObtenerPorTipoAsync(int tipoProductoId)
        {
            try
            {
                var productos = await _productoRepo.GetByTipoAsync(tipoProductoId);
                return productos.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al obtener productos por tipo: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ProductoDto>> BuscarPorNombreAsync(string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    return await ObtenerTodosAsync();

                var productos = await _productoRepo.BuscarPorNombreAsync(nombre);
                return productos.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al buscar productos: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ProductoCatalogoDto>> ObtenerCatalogoAsync()
        {
            try
            {
                var productos = await _productoRepo.GetDisponiblesAsync();
                return productos.Select(p => new ProductoCatalogoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    //ImagenUrl = p.ImagenUrl,
                    TipoProducto = p.TipoProducto.Tipo,
                    DisponibleParaVenta = p.Stock > 0
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al obtener catálogo: {ex.Message}");
                throw;
            }
        }

        public async Task<ProductoDto> CrearProductoAsync(CrearProductoDto dto)
        {
            try
            {
                var producto = new Producto
                {
                    Nombre = dto.Nombre,
                    Stock = dto.Stock,
                    Precio = dto.Precio,
                    TipoProductoId = dto.TipoProductoId,
                    //ImagenUrl = dto.ImagenUrl
                };

                var resultado = await _productoRepo.AddAsync(producto);
                _logger.Info($"Producto creado: {resultado.Nombre}");

                return MapToDto(resultado);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al crear producto: {ex.Message}");
                throw;
            }
        }

        public async Task<ProductoDto?> ActualizarProductoAsync(int id, ActualizarProductoDto dto)
        {
            try
            {
                var productoExistente = await _productoRepo.GetByIdAsync(id);
                if (productoExistente == null)
                {
                    _logger.Error($"Producto {id} no encontrado");
                    return null;
                }

                var producto = new Producto
                {
                    Nombre = dto.Nombre,
                    Stock = dto.Stock,
                    Precio = dto.Precio,
                    TipoProductoId = dto.TipoProductoId,
                    //ImagenUrl = dto.ImagenUrl
                };

                var resultado = await _productoRepo.UpdateAsync(id, producto);
                if (resultado == null)
                    return null;

                _logger.Info($"Producto actualizado: {resultado.Nombre}");
                return MapToDto(resultado);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al actualizar producto: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> EliminarProductoAsync(int id)
        {
            try
            {
                var resultado = await _productoRepo.DeleteAsync(id);
                if (resultado)
                {
                    _logger.Info($"Producto eliminado: {id}");
                }
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al eliminar producto: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ActualizarStockAsync(int id, int cantidad)
        {
            try
            {
                var producto = await _productoRepo.GetByIdAsync(id);
                if (producto == null)
                    return false;

                producto.Stock += cantidad; // cantidad puede ser negativa para restar

                if (producto.Stock < 0)
                {
                    _logger.Error($"Stock insuficiente para producto {id}");
                    return false;
                }

                var actualizado = new Producto
                {
                    Nombre = producto.Nombre,
                    Stock = producto.Stock,
                    Precio = producto.Precio,
                    TipoProductoId = producto.TipoProductoId,
                    //ImagenUrl = producto.ImagenUrl
                };

                var resultado = await _productoRepo.UpdateAsync(id, actualizado);
                return resultado != null;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al actualizar stock: {ex.Message}");
                throw;
            }
        }

        // Mapper
        private static ProductoDto MapToDto(Producto producto)
        {
            return new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Stock = producto.Stock,
                Precio = producto.Precio,
                TipoProductoId = producto.TipoProductoId,
                TipoProducto = producto.TipoProducto.Tipo,
                //ImagenUrl = producto.ImagenUrl
            };
        }
    }
}
