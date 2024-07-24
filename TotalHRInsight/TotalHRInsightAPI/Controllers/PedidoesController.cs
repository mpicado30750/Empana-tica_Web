using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using TotalHRInsight.DTO.Inventario;
using TotalHRInsight.DTO.Pedidos;

namespace TotalHRInsightAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoesController : ControllerBase
    {
        private readonly TotalHRInsightDbContext _context;

        public PedidoesController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // DELETE: api/Pedidoes/DeletePedido/5
        [HttpDelete("DeletePedido/{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            try
            {
                // Obtener el pedido
                var pedido = await _context.Pedidos.FindAsync(id);

                if (pedido == null)
                {
                    return NotFound(new { success = false, message = "Pedido no encontrado" });
                }

                // Asumiendo que 3 es el ID del estado "cancelado"
                const int EstadoCanceladoId = 3;

                // Actualizar el estado del pedido a "cancelado"
                pedido.IdEstado = EstadoCanceladoId;

                // Guardar cambios
                _context.Entry(pedido).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Pedido cancelado con éxito" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al cancelar el pedido", error = ex.Message });
            }
        }

        [HttpPost("AddProductsToInventory")]
        public async Task<IActionResult> AddProductsToInventory(int idPedido, int idSucursal)
        {
            try
            {
                // Obtener el pedido con sus productos
                var pedido = await _context.Pedidos
                    .Include(p => p.PedidosProductos)
                    .ThenInclude(pp => pp.Producto)
                    .FirstOrDefaultAsync(p => p.IdPedido == idPedido);

                if (pedido == null)
                {
                    return NotFound(new { success = false, message = "Pedido no encontrado" });
                }

                // Obtener el inventario de la sucursal
                var inventarioSucursal = await _context.Inventario
                    .Where(i => i.SucursalId == idSucursal)
                    .ToListAsync();

                // Agregar productos del pedido al inventario
                foreach (var pedidoProducto in pedido.PedidosProductos)
                {
                    var inventarioProducto = inventarioSucursal.FirstOrDefault(i => i.ProductoId == pedidoProducto.ProductosID);

                    if (inventarioProducto != null)
                    {
                        // Si el producto ya existe en el inventario, actualizar la cantidad
                        inventarioProducto.CantidadDisponible += (int)pedidoProducto.Cantidad;
                        inventarioProducto.UsuarioModificacionid = pedido.UsuarioCreacionId; // Asumimos que el usuario que crea el pedido modifica el inventario
                        inventarioProducto.FechaModificacion = DateTime.Now;
                        _context.Entry(inventarioProducto).State = EntityState.Modified;
                    }
                    else
                    {
                        // Si el producto no existe en el inventario, agregar un nuevo registro
                        var nuevoInventario = new Inventario
                        {
                            UsuarioCreacionid = pedido.UsuarioCreacionId,
                            UsuarioModificacionid = pedido.UsuarioCreacionId, // Asumimos que el usuario que crea el pedido también crea el inventario
                            FechaCreacion = DateTime.Now,
                            FechaModificacion = DateTime.Now,
                            CantidadDisponible = (int)pedidoProducto.Cantidad,
                            SucursalId = idSucursal,
                            ProductoId = pedidoProducto.ProductosID,
                            Producto = pedidoProducto.Producto
                        };
                        await _context.Inventario.AddAsync(nuevoInventario);
                    }
                }

                // Guardar cambios en la base de datos
                await _context.SaveChangesAsync();

                // Asumiendo que 3 es el ID del estado "cancelado"
                const int EstadoCanceladoId = 2;

                // Actualizar el estado del pedido a "cancelado"
                pedido.IdEstado = EstadoCanceladoId;

                // Guardar cambios
                _context.Entry(pedido).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Productos agregados al inventario con éxito" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al agregar productos al inventario", error = ex.Message });
            }
        }



        [HttpGet("GetPedido/{id}")]
        public async Task<ActionResult<PedidoViewModel>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(i => i.Estado)
                .Include(p => p.PedidosProductos)
                    .ThenInclude(pp => pp.Producto)
                
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
            {
                return NotFound();
            }

            var pedidoViewModel = new PedidoViewModel
            {
                IdPedido = pedido.IdPedido,
                FechaPedido = pedido.FechaPedido,
                FechaEntrega = pedido.FechaEntrega,
                UsuarioCreacionId = pedido.UsuarioCreacionId,
                IdSucursal = pedido.IdSucursal,
                IdEstado = pedido.Estado.EstadoSolicitud,
                MontoTotal = pedido.MontoTotal,
                PedidosProductos = pedido.PedidosProductos.Select(pp => new PedidoProductoViewModel
                {
                    PedidosProductosID = pp.PedidosProductosID,
                    ProductosID = pp.ProductosID,
                    PedidoID = pp.PedidoID,
                    Cantidad = (int)pp.Cantidad,
                    Producto = new ProductoViewModel
                    {
                        IdProducto = pp.Producto.IdProducto,
                        NombreProducto = pp.Producto.NombreProducto,
                        FechaVencimiento = pp.Producto.FechaVencimiento,
                        PrecioUnitario = (decimal)pp.Producto.PrecioUnitario,
                        MedidasId = pp.Producto.MedidasId,
                        CategoriaId = pp.Producto.CategoriaId,
                        ProveedorId = pp.Producto.ProveedorId
                    }
                }).ToList()
            };

            return Ok(new
            {
                success = true,
                pedido = pedidoViewModel
            });
        }


        // GET: api/Pedidoes/5
        [HttpGet("GetListaPedido/{idSucursal}")]
        public async Task<ActionResult<ListaPedidosDTO>> GetListaPedido(int idSucursal)
        {
            var pedido = await _context.Pedidos
                .Include(i => i.Sucursal)
                .Include(i => i.Estado)
                .Where(s => s.IdSucursal == idSucursal)
                .ToListAsync();

            if (pedido == null)
            {
                return NotFound();
            }
            var listaPedidos = pedido.Select(p => new ListaPedidosDTO
            {
                IdPedido = p.IdPedido,
                FechaPedido = p.FechaPedido,
                FechaEntrega = p.FechaEntrega,
                UsuarioCreacionId = p.UsuarioCreacionId,
                IdEstado = p.IdEstado,
                IdSucursal = p.IdSucursal, 
                sucursal = p.Sucursal.NombreSucursal,
                estado = p.Estado.EstadoSolicitud,
                MontoTotal = p.MontoTotal
            }).ToList();
            return Ok(new
            {
                succes = true,
                pedidos = pedido
            });
        }

        // POST: api/Inventarios
        [HttpPost]
        public async Task<ActionResult<PedidosProductosDTO>> PostPedidos(PedidosProductosDTO inventario)
        {
            if (inventario == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //Creacion de pedidos para la devolucion
                var estadoPendiente = await _context.Estados.FirstOrDefaultAsync(e => e.EstadoSolicitud == "En Progreso");
                
                var nuevoPedido = new Pedido
                {
                    FechaEntrega = inventario.Fecha,
                    FechaPedido = DateTime.Now,
                    UsuarioCreacionId = inventario.idUsuario,
                    IdSucursal = inventario.idSucursal,
                    IdEstado = estadoPendiente.IdEstado,
                    MontoTotal = inventario.total
                };
                _context.Pedidos.Add(nuevoPedido);
                await _context.SaveChangesAsync();

                bool errorEnProductos = false;
                string mensajeError = "";

                foreach (var producto in inventario.listaProductoDevolvers)
                {
                    var productoEncotrado = await _context.Inventario
                        .FirstOrDefaultAsync(e => e.IdInventario == producto.IdInventario);

                    if (productoEncotrado == null)
                    {
                        errorEnProductos = true;
                        mensajeError = $"Producto con ID {producto.IdInventario} no existe.";
                        break;
                    }

                    var detallePedido = new PedidosProductos
                    {
                        ProductosID = productoEncotrado.ProductoId,
                        PedidoID = nuevoPedido.IdPedido,
                        Cantidad = producto.cantidadDisponible,
                    };
                    _context.PedidosProductos.Add(detallePedido);
                }

                if (errorEnProductos)
                {
                    // Eliminar el pedido si hubo un error con los productos
                    _context.Pedidos.Remove(nuevoPedido);
                    await _context.SaveChangesAsync();
                    return BadRequest(mensajeError);
                }

                //Actualiza los datos en el inventario
                foreach (var producto in inventario.listaProductoDevolvers)
                {
                    var inventarioExistente = await _context.Inventario.FindAsync(producto.IdInventario);
                    if (inventarioExistente != null)
                    {
                        inventarioExistente.FechaModificacion = inventario.Fecha;
                        inventarioExistente.CantidadDisponible = inventarioExistente.CantidadDisponible - producto.cantidadDisponible;
                        inventarioExistente.UsuarioModificacionid = inventario.idUsuario;
                    }
                    else
                    {
                        errorEnProductos = true;
                        mensajeError = $"Producto con ID {inventarioExistente.IdInventario} no existe.";
                        break;
                    }
                }

                if (errorEnProductos)
                {
                    // Eliminar el pedido si hubo un error con los productos
                    _context.Pedidos.Remove(nuevoPedido);
                    await _context.SaveChangesAsync();
                    return BadRequest(mensajeError);
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Pedido creado correctamente" });
            }
            catch (Exception ex)
            {
                // Manejar excepción
                return StatusCode(500, new { message = "Error realizando pedido.", error = ex.Message });
            }
        }


    }
}
