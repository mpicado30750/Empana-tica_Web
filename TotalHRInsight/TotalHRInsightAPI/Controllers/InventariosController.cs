using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using TotalHRInsight.DTO.Inventario;

namespace TotalHRInsightAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventariosController : ControllerBase
    {
        private readonly TotalHRInsightDbContext _context;

        public InventariosController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: api/Inventarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventario>>> GetInventario()
        {
            return await _context.Inventario.ToListAsync();
        }

        // GET: api/Inventarios/5
        [HttpGet("GetInventarioSucursal/{id}")]
        public async Task<ActionResult<IEnumerable<GetInventarioSucursalDTO>>> GetInventarioSucursal(int id)
        {
            var inventarioList = await _context.Inventario
        .Where(w => w.SucursalId == id)
        .Include(i => i.Producto)
        .Include(i => i.Sucursal)
        .ToListAsync();

            if (inventarioList == null || !inventarioList.Any())
            {
                return NotFound();
            }

            var inventarioDTOList = inventarioList.Select(s => new GetInventarioSucursalDTO
            {
                IdInventario = s.IdInventario,
                FechaVencimientoProducto = s.Producto.FechaVencimiento,
                CantidadDisponible = s.CantidadDisponible,
                PrecioProducto = s.Producto.PrecioUnitario,
                NombreProducto = s.Producto.NombreProducto,
                DiasParaCaducar = (s.Producto.FechaVencimiento - DateTime.Now).Days
            }).ToList();

            return Ok(inventarioDTOList);
        }

        // GET: api/Inventarios/5
        [HttpGet("GetProductoInventario/{id}")]
        public async Task<ActionResult<GetInventarioSucursalDTO>> GetProductoInventario(int id)
        {
            var inventario = await _context.Inventario
                .Include(i => i.Producto)
                .Include(i => i.Sucursal)
                .FirstOrDefaultAsync(m => m.IdInventario == id);

            if (inventario == null)
            {
                return NotFound();
            }

            var inventarioDTO = new GetInventarioSucursalDTO
            {
                IdInventario = inventario.IdInventario,
                FechaVencimientoProducto = inventario.Producto.FechaVencimiento,
                CantidadDisponible = inventario.CantidadDisponible,
                PrecioProducto = inventario.Producto.PrecioUnitario,
                NombreProducto = inventario.Producto.NombreProducto,
                DiasParaCaducar = (inventario.Producto.FechaVencimiento - DateTime.Now).Days
            };

            return Ok(inventarioDTO);
        }


        // POST: api/Inventarios
        [HttpPost]
        public async Task<ActionResult<DevolverProductosDTO>> PostInventario(DevolverProductosDTO inventario)
        {
            if (inventario == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //Creacion de pedidos para la devolucion
                var estadoPendiente = await _context.Estados.FirstOrDefaultAsync(e => e.EstadoSolicitud == "En Progreso");
                var sucursalID = await _context.Sucursales.FirstOrDefaultAsync(e => e.NombreSucursal == "Centro de Produccion");

                var nuevoPedido = new Pedido
                {
                    FechaEntrega = inventario.Fecha,
                    FechaPedido = DateTime.Now,
                    UsuarioCreacionId = inventario.idUsuario,
                    IdSucursal = sucursalID.IdSucursal,
                    IdEstado = estadoPendiente.IdEstado,
                    MontoTotal = 0
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
                        mensajeError = $"El producto con ID {producto.IdInventario} no existe.";
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
                        mensajeError = $"El producto con ID {inventarioExistente.IdInventario} no existe.";
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
                return Ok(new { message = "Inventario actualizado correctamente." });
            }
            catch (Exception ex)
            {
                // Manejar excepción
                return StatusCode(500, new { message = "Error actualizando inventario.", error = ex.Message });
            }
        }

        // DELETE: api/Inventarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventario(int id)
        {
            var inventario = await _context.Inventario.FindAsync(id);
            if (inventario == null)
            {
                return NotFound();
            }

            _context.Inventario.Remove(inventario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Inventarios/ActualizarCantidadDisponible
        [HttpPut("ActualizarCantidadDisponible")]
        public async Task<IActionResult> ActualizarCantidadDisponible([FromBody] ActualizarCantidadDTO actualizarCantidadDTO)
        {
            // Busca el inventario por ID
            var inventario = await _context.Inventario.FindAsync(actualizarCantidadDTO.Id);

            // Verifica si el inventario existe
            if (inventario == null)
            {
                return NotFound(new { success = false, message = "Inventario no encontrado" });
            }

            // Actualiza la cantidad disponible
            inventario.CantidadDisponible = actualizarCantidadDTO.NuevaCantidad;
            inventario.FechaModificacion = DateTime.Now;
            inventario.UsuarioModificacionid = actualizarCantidadDTO.UsuarioModificacionid;
            // Marca el inventario como modificado
            _context.Entry(inventario).State = EntityState.Modified;

            // Guarda los cambios en la base de datos
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventarioExists(actualizarCantidadDTO.Id))
                {
                    return NotFound(new { success = false, message = "Inventario no encontrado" });
                }
                else
                {
                    return StatusCode(500, new { success = false, message = "Error de concurrencia al actualizar el inventario" });
                }
            }

            return Ok(new { success = true, message = "Cantidad disponible actualizada exitosamente" });
        }

        // Método auxiliar para verificar si un inventario existe
        private bool InventarioExists(int id)
        {
            return _context.Inventario.Any(e => e.IdInventario == id);
        }
    }
}
