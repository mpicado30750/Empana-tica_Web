using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;

namespace TotalHRInsight.Controllers
{
    public class PedidosController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public PedidosController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.Pedidos.Include(p => p.Estado).Include(p => p.Sucursal).Include(p => p.UsuarioCreacion);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Estado)
                .Include(p => p.Sucursal)
                .Include(p => p.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdPedido == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "EstadoSolicitud");
            ViewData["IdSucursal"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal");
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPedido,FechaPedido,FechaEntrega,UsuarioCreacionId,IdSucursal,IdEstado,MontoTotal")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "EstadoSolicitud", pedido.IdEstado);
            ViewData["IdSucursal"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", pedido.IdSucursal);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", pedido.UsuarioCreacionId);
            return View(pedido);
        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "EstadoSolicitud", pedido.IdEstado);
            ViewData["IdSucursal"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", pedido.IdSucursal);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", pedido.UsuarioCreacionId);
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPedido,FechaPedido,FechaEntrega,UsuarioCreacionId,IdSucursal,IdEstado,MontoTotal")] Pedido pedido)
        {
            if (id != pedido.IdPedido)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.IdPedido))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "EstadoSolicitud", pedido.IdEstado);
            ViewData["IdSucursal"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", pedido.IdSucursal);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", pedido.UsuarioCreacionId);
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Estado)
                .Include(p => p.Sucursal)
                .Include(p => p.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdPedido == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.IdPedido == id);
        }
    }
}
