using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using TotalHRInsight.DTO.Productos;

namespace TotalHRInsightAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly TotalHRInsightDbContext _context;

        public ProductosController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        [HttpGet("vencimiento/{sucursalId}")]
        public async Task<ActionResult<List<ProductoVencimientoDTO>>> GetProductosAPuntoDeVencer(int sucursalId)
        {
            var inventarios = await _context.Inventario
                .Include(i => i.Producto)
                .Include(i => i.Sucursal)
                .Where(i => i.SucursalId == sucursalId)
                .ToListAsync(); // Realiza la consulta a la base de datos y convierte a lista

            var productosVencimiento = inventarios
                .Where(i => (i.Producto.FechaVencimiento - DateTime.Now).Days <= 10)
                .Select(i => new ProductoVencimientoDTO
                {
                    IdProducto = i.Producto.IdProducto,
                    NombreProducto = i.Producto.NombreProducto,
                    FechaVencimiento = i.Producto.FechaVencimiento,
                    DiasParaVencer = (i.Producto.FechaVencimiento - DateTime.Now).Days,
                    CantidadDisponible = i.CantidadDisponible,
                    NombreSucursal = i.Sucursal.NombreSucursal
                })
                .ToList(); // Convierte la lista filtrada y proyectada a lista

            if (productosVencimiento == null || !productosVencimiento.Any())
            {
                return StatusCode(500, new { message = "Error actualizando inventario."});
            }

            return Ok(productosVencimiento);
        }



    }
}
