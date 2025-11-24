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
    public class CarritoService : ICarritoService
    {
        private readonly ICarritoRepository _carritoRepo;
        private readonly IProductoRepository _productoRepo;
        private readonly Ilogger _logger;

        public CarritoService(
            ICarritoRepository carritoRepo,
            IProductoRepository productoRepo,
            Ilogger logger)
        {
            _carritoRepo = carritoRepo;
            _productoRepo = productoRepo;
            _logger = logger;
        }

        public async Task<CarritoDto> ObtenerCarritoUsuarioAsync(int usuarioId)
        {
            try
            {
                var carrito = await _carritoRepo.GetCarritoActivoByUsuarioIdAsync(usuarioId);

                if (carrito == null)
                {
                    // Crear carrito nuevo si no existe
                    carrito = await _carritoRepo.CrearCarritoAsync(usuarioId);
                }

                return MapToDto(carrito);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al obtener carrito: {ex.Message}");
                throw;
            }
        }

        public async Task<CarritoDto> AgregarProductoAsync(int usuarioId, AgregarAlCarritoDto dto)
        {
            try
            {
                // Obtener o crear carrito
                var carrito = await _carritoRepo.GetCarritoActivoByUsuarioIdAsync(usuarioId);
                if (carrito == null)
                {
                    carrito = await _carritoRepo.CrearCarritoAsync(usuarioId);
                }

                // Verificar producto
                var producto = await _productoRepo.GetByIdAsync(dto.ProductoId);
                if (producto == null)
                {
                    _logger.Error($"Producto {dto.ProductoId} no encontrado");
                    throw new Exception("Producto no encontrado");
                }

                if (producto.Stock < dto.Cantidad)
                {
                    _logger.Error($"Stock insuficiente para producto {dto.ProductoId}");
                    throw new Exception("Stock insuficiente");
                }

                // Verificar si el producto ya está en el carrito
                var itemExistente = carrito.Items.FirstOrDefault(i => i.ProductoId == dto.ProductoId);

                if (itemExistente != null)
                {
                    // Actualizar cantidad
                    itemExistente.Cantidad += dto.Cantidad;

                    if (itemExistente.Cantidad > producto.Stock)
                    {
                        throw new Exception("Stock insuficiente");
                    }

                    await _carritoRepo.ActualizarItemAsync(itemExistente);
                }
                else
                {
                    // Agregar nuevo item
                    var nuevoItem = new CarritoItem
                    {
                        CarritoId = carrito.Id,
                        ProductoId = dto.ProductoId,
                        Cantidad = dto.Cantidad,
                        PrecioUnitario = producto.Precio
                    };

                    await _carritoRepo.AgregarItemAsync(nuevoItem);
                }

                _logger.Info($"Producto agregado al carrito. Usuario: {usuarioId}, Producto: {dto.ProductoId}");

                // Recargar carrito actualizado
                carrito = await _carritoRepo.GetCarritoActivoByUsuarioIdAsync(usuarioId);
                return MapToDto(carrito!);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al agregar producto al carrito: {ex.Message}");
                throw;
            }
        }

        public async Task<CarritoDto> ActualizarCantidadAsync(int usuarioId, int itemId, ActualizarCarritoItemDto dto)
        {
            try
            {
                var item = await _carritoRepo.GetCarritoItemByIdAsync(itemId);

                if (item == null || item.Carrito.UsuarioId != usuarioId)
                {
                    throw new Exception("Item no encontrado o no pertenece al usuario");
                }

                // Verificar stock
                var producto = await _productoRepo.GetByIdAsync(item.ProductoId);
                if (producto == null || producto.Stock < dto.Cantidad)
                {
                    throw new Exception("Stock insuficiente");
                }

                item.Cantidad = dto.Cantidad;
                await _carritoRepo.ActualizarItemAsync(item);

                _logger.Info($"Cantidad actualizada en carrito. Usuario: {usuarioId}, Item: {itemId}");

                // Recargar carrito
                var carrito = await _carritoRepo.GetCarritoActivoByUsuarioIdAsync(usuarioId);
                return MapToDto(carrito!);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al actualizar cantidad: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> EliminarItemAsync(int usuarioId, int itemId)
        {
            try
            {
                var item = await _carritoRepo.GetCarritoItemByIdAsync(itemId);

                if (item == null || item.Carrito.UsuarioId != usuarioId)
                {
                    return false;
                }

                var resultado = await _carritoRepo.EliminarItemAsync(itemId);

                if (resultado)
                {
                    _logger.Info($"Item eliminado del carrito. Usuario: {usuarioId}, Item: {itemId}");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al eliminar item: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> VaciarCarritoAsync(int usuarioId)
        {
            try
            {
                var carrito = await _carritoRepo.GetCarritoActivoByUsuarioIdAsync(usuarioId);

                if (carrito == null)
                    return false;

                var resultado = await _carritoRepo.VaciarCarritoAsync(carrito.Id);

                if (resultado)
                {
                    _logger.Info($"Carrito vaciado. Usuario: {usuarioId}");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al vaciar carrito: {ex.Message}");
                throw;
            }
        }

        // Mapper
        private static CarritoDto MapToDto(Carrito carrito)
        {
            return new CarritoDto
            {
                Id = carrito.Id,
                Items = carrito.Items.Select(i => new CarritoItemDto
                {
                    Id = i.Id,
                    ProductoId = i.ProductoId,
                    NombreProducto = i.Producto.Nombre,
                    PrecioUnitario = i.PrecioUnitario,
                    Cantidad = i.Cantidad
                }).ToList()
            };
        }
    }
}
