using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
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

        // GET: api/Pedidoes/5
        [HttpGet("GetPedido/{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                succes = true,
                pedidos = pedido
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

        // POST: api/Pedidoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPedido", new { id = pedido.IdPedido }, pedido);
        }


    }
}
