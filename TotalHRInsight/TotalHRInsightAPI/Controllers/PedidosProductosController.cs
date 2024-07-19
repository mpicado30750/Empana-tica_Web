using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;

namespace TotalHRInsightAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosProductosController : ControllerBase
    {
        private readonly TotalHRInsightDbContext _context;

        public PedidosProductosController(TotalHRInsightDbContext context)
        {
            _context = context;
        }


        // GET: api/PedidosProductos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidosProductos>> GetPedidosProductos(int id)
        {
            var pedidosProductos = await _context.PedidosProductos.FindAsync(id);

            if (pedidosProductos == null)
            {
                return NotFound();
            }

            return pedidosProductos;
        }

        // POST: api/PedidosProductos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PedidosProductos>> PostPedidosProductos(PedidosProductos pedidosProductos)
        {
            _context.PedidosProductos.Add(pedidosProductos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPedidosProductos", new { id = pedidosProductos.PedidosProductosID }, pedidosProductos);
        }

        // DELETE: api/PedidosProductos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedidosProductos(int id)
        {
            var pedidosProductos = await _context.PedidosProductos.FindAsync(id);
            if (pedidosProductos == null)
            {
                return NotFound();
            }

            _context.PedidosProductos.Remove(pedidosProductos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PedidosProductosExists(int id)
        {
            return _context.PedidosProductos.Any(e => e.PedidosProductosID == id);
        }
    }
}
