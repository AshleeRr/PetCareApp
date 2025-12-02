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
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepo;
        private readonly ICarritoRepository _carritoRepo;
        private readonly IProductoRepository _productoRepo;
        private readonly Ilogger _logger;

        public VentaService(
            IVentaRepository ventaRepo,
            ICarritoRepository carritoRepo,
            IProductoRepository productoRepo,
            Ilogger logger)
        {
            _ventaRepo = ventaRepo;
            _carritoRepo = carritoRepo;
            _productoRepo = productoRepo;
            _logger = logger;
        }

        public async Task<IEnumerable<VentaDto>> ObtenerTodasAsync()
        {
            try
            {
                var ventas = await _ventaRepo.GetAllAsync();
                return ventas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al obtener ventas: {ex.Message}");
                throw;
            }
        }



        public async Task<VentaDto?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var venta = await _ventaRepo.GetByIdAsync(id);
                return venta != null ? MapToDto(venta) : null;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al obtener venta: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<VentaDto>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            try
            {
                var ventas = await _ventaRepo.GetByUsuarioIdAsync(usuarioId);
                return ventas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al obtener ventas por usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<VentaDto>> ObtenerPorRangoFechaAsync(DateTime desde, DateTime hasta)
        {
            try
            {
                var ventas = await _ventaRepo.GetByFechaRangoAsync(desde, hasta);
                return ventas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al obtener ventas por fecha: {ex.Message}");
                throw;
            }
        }

        public async Task<VentaDto> CrearVentaDesdeCarritoAsync(int usuarioId, CrearVentaDesdeCarritoDto dto)
        {
            try
            {
                // Obtener carrito activo
                var carrito = await _carritoRepo.GetCarritoActivoByUsuarioIdAsync(usuarioId);

                if (carrito == null || !carrito.Items.Any())
                {
                    throw new Exception("Carrito vacío");
                }

                // Verificar stock de todos los productos
                foreach (var item in carrito.Items)
                {
                    var producto = await _productoRepo.GetByIdAsync(item.ProductoId);
                    if (producto == null || producto.Stock < item.Cantidad)
                    {
                        throw new Exception($"Stock insuficiente para: {item.Producto.Nombre}");
                    }
                }

                // Crear venta
                var venta = new Venta
                {
                    Fecha = DateTime.Now,
                    UsuarioId = usuarioId,
                    MetodoPago = dto.MetodoPago,
                    Estado = "Completada",
                    Total = 0 // Se calculará con los detalles
                };

                var detalles = new List<VentaDetalle>();
                decimal total = 0;

                foreach (var item in carrito.Items)
                {
                    var subtotal = item.PrecioUnitario * item.Cantidad;
                    total += subtotal;

                    detalles.Add(new VentaDetalle
                    {
                        ProductoId = item.ProductoId,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.PrecioUnitario,
                        Subtotal = subtotal
                    });

                    // Descontar stock
                    await _productoRepo.UpdateAsync(item.ProductoId, new Producto
                    {
                        Nombre = item.Producto.Nombre,
                        Stock = item.Producto.Stock - item.Cantidad,
                        Precio = item.Producto.Precio,
                        TipoProductoId = item.Producto.TipoProductoId,
                        //ImagenUrl = item.Producto.ImagenUrl
                    });
                }

                venta.Total = total;
                venta.Detalles = detalles;

                var ventaCreada = await _ventaRepo.AddAsync(venta);

                // Desactivar carrito
                await _carritoRepo.DesactivarCarritoAsync(carrito.Id);

                _logger.Info($"Venta creada desde carrito. Usuario: {usuarioId}, Total: {total}");

                return MapToDto(ventaCreada);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al crear venta desde carrito: {ex.Message}");
                throw;
            }
        }

        public async Task<VentaDto> CrearVentaMostradorAsync(CrearVentaMostradorDto dto)
        {
            try
            {
                // Verificar stock
                foreach (var item in dto.Items)
                {
                    if (item.ProductoId.HasValue)
                    {
                        var producto = await _productoRepo.GetByIdAsync(item.ProductoId.Value);
                        if (producto == null || producto.Stock < item.Cantidad)
                        {
                            throw new Exception($"Stock insuficiente para producto ID: {item.ProductoId}");
                        }
                    }
                }

                var venta = new Venta
                {
                    Fecha = DateTime.Now,
                    PersonalId = dto.PersonalId,
                    ClienteId = dto.ClienteId,
                    MetodoPago = dto.MetodoPago,
                    Estado = "Completada",
                    Total = 0
                };

                var detalles = new List<VentaDetalle>();
                decimal total = 0;

                foreach (var item in dto.Items)
                {
                    var subtotal = item.PrecioUnitario * item.Cantidad;
                    total += subtotal;

                    detalles.Add(new VentaDetalle
                    {
                        ProductoId = item.ProductoId,
                        CitaId = item.CitaId,
                        RecetaId = item.RecetaId,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.PrecioUnitario,
                        Subtotal = subtotal
                    });

                    // Descontar stock si es producto
                    if (item.ProductoId.HasValue)
                    {
                        var producto = await _productoRepo.GetByIdAsync(item.ProductoId.Value);
                        if (producto != null)
                        {
                            await _productoRepo.UpdateAsync(item.ProductoId.Value, new Producto
                            {
                                Nombre = producto.Nombre,
                                Stock = producto.Stock - item.Cantidad,
                                Precio = producto.Precio,
                                TipoProductoId = producto.TipoProductoId,
                                //ImagenUrl = producto.ImagenUrl
                            });
                        }
                    }
                }

                venta.Total = total;
                venta.Detalles = detalles;

                var ventaCreada = await _ventaRepo.AddAsync(venta);

                _logger.Info($"Venta de mostrador creada. Personal: {dto.PersonalId}, Total: {total}");

                return MapToDto(ventaCreada);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al crear venta de mostrador: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CancelarVentaAsync(int ventaId)
        {
            try
            {
                var venta = await _ventaRepo.GetByIdAsync(ventaId);

                if (venta == null || venta.Estado == "Cancelada")
                    return false;

                // Devolver stock
                foreach (var detalle in venta.Detalles)
                {
                    if (detalle.ProductoId.HasValue)
                    {
                        var producto = await _productoRepo.GetByIdAsync(detalle.ProductoId.Value);
                        if (producto != null)
                        {
                            await _productoRepo.UpdateAsync(detalle.ProductoId.Value, new Producto
                            {
                                Nombre = producto.Nombre,
                                Stock = producto.Stock + detalle.Cantidad,
                                Precio = producto.Precio,
                                TipoProductoId = producto.TipoProductoId,
                                //ImagenUrl = producto.ImagenUrl
                            });
                        }
                    }
                }

                venta.Estado = "Cancelada";
                await _ventaRepo.UpdateAsync(venta);

                _logger.Info($"Venta cancelada: {ventaId}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al cancelar venta: {ex.Message}");
                throw;
            }
        }

        // Mapper
        private static VentaDto MapToDto(Venta venta)
        {
            return new VentaDto
            {
                Id = venta.Id,
                Fecha = venta.Fecha,
                Total = venta.Total,
                NombrePersonal = venta.Personal?.Nombre ?? "Online",
                NombreCliente = venta.Cliente?.Nombre ?? venta.Usuario?.UserName ?? "N/A",
                MetodoPago = venta.MetodoPago,
                Estado = venta.Estado,
                Detalles = venta.Detalles.Select(d => new VentaDetalleDto
                {
                    Id = d.Id,
                    NombreProducto = d.Producto?.Nombre,
                    Concepto = d.ProductoId.HasValue ? "Producto" :
                              d.CitaId.HasValue ? "Cita" :
                              d.RecetaId.HasValue ? "Receta" : "Otro",
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList()
            };
        }
    }
}
