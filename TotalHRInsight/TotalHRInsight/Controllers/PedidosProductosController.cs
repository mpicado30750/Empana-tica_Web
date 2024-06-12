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
    public class PedidosProductosController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public PedidosProductosController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: PedidosProductos
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.PedidosProductos.Include(p => p.Pedido).Include(p => p.Producto);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: PedidosProductos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidosProductos = await _context.PedidosProductos
                .Include(p => p.Pedido)
                .Include(p => p.Producto)
                .FirstOrDefaultAsync(m => m.PedidosProductosID == id);
            if (pedidosProductos == null)
            {
                return NotFound();
            }

            return View(pedidosProductos);
        }

        // GET: PedidosProductos/Create
        public IActionResult Create()
        {
            ViewData["PedidoID"] = new SelectList(_context.Pedidos, "IdPedido", "EstadoPedido");
            ViewData["ProductosID"] = new SelectList(_context.Productos, "IdProducto", "Descripcion");
            return View();
        }

        // POST: PedidosProductos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PedidosProductosID,ProductosID,PedidoID,Cantidad,Medida")] PedidosProductos pedidosProductos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedidosProductos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PedidoID"] = new SelectList(_context.Pedidos, "IdPedido", "EstadoPedido", pedidosProductos.PedidoID);
            ViewData["ProductosID"] = new SelectList(_context.Productos, "IdProducto", "Descripcion", pedidosProductos.ProductosID);
            return View(pedidosProductos);
        }

        // GET: PedidosProductos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidosProductos = await _context.PedidosProductos.FindAsync(id);
            if (pedidosProductos == null)
            {
                return NotFound();
            }
            ViewData["PedidoID"] = new SelectList(_context.Pedidos, "IdPedido", "EstadoPedido", pedidosProductos.PedidoID);
            ViewData["ProductosID"] = new SelectList(_context.Productos, "IdProducto", "Descripcion", pedidosProductos.ProductosID);
            return View(pedidosProductos);
        }

        // POST: PedidosProductos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PedidosProductosID,ProductosID,PedidoID,Cantidad,Medida")] PedidosProductos pedidosProductos)
        {
            if (id != pedidosProductos.PedidosProductosID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedidosProductos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidosProductosExists(pedidosProductos.PedidosProductosID))
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
            ViewData["PedidoID"] = new SelectList(_context.Pedidos, "IdPedido", "EstadoPedido", pedidosProductos.PedidoID);
            ViewData["ProductosID"] = new SelectList(_context.Productos, "IdProducto", "Descripcion", pedidosProductos.ProductosID);
            return View(pedidosProductos);
        }

        // GET: PedidosProductos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidosProductos = await _context.PedidosProductos
                .Include(p => p.Pedido)
                .Include(p => p.Producto)
                .FirstOrDefaultAsync(m => m.PedidosProductosID == id);
            if (pedidosProductos == null)
            {
                return NotFound();
            }

            return View(pedidosProductos);
        }

        // POST: PedidosProductos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedidosProductos = await _context.PedidosProductos.FindAsync(id);
            if (pedidosProductos != null)
            {
                _context.PedidosProductos.Remove(pedidosProductos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidosProductosExists(int id)
        {
            return _context.PedidosProductos.Any(e => e.PedidosProductosID == id);
        }
    }
}
