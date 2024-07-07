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
    public class InventariosController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public InventariosController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: Inventarios
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.Inventario.Include(i => i.Producto).Include(i => i.Sucursal).Include(i => i.UsuarioCreacion).Include(i => i.UsuarioModificacion);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: Inventarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventario
                .Include(i => i.Producto)
                .Include(i => i.Sucursal)
                .Include(i => i.UsuarioCreacion)
                .Include(i => i.UsuarioModificacion)
                .FirstOrDefaultAsync(m => m.IdInventario == id);
            if (inventario == null)
            {
                return NotFound();
            }

            return View(inventario);
        }

        // GET: Inventarios/Create
        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.Productos, "IdProducto", "NombreProducto");
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal");
            ViewData["UsuarioCreacionid"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre");
            ViewData["UsuarioModificacionid"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre");
            return View();
        }

        // POST: Inventarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdInventario,UsuarioCreacionid,UsuarioModificacionid,FechaCreacion,FechaModificacion,CantidadDisponible,SucursalId,ProductoId")] Inventario inventario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "IdProducto", "Descripcion", inventario.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", inventario.SucursalId);
            ViewData["UsuarioCreacionid"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", inventario.UsuarioCreacionid);
            ViewData["UsuarioModificacionid"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", inventario.UsuarioModificacionid);
            return View(inventario);
        }

        // GET: Inventarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventario.FindAsync(id);
            if (inventario == null)
            {
                return NotFound();
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "IdProducto", "NombreProducto", inventario.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", inventario.SucursalId);
            ViewData["UsuarioCreacionid"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre", inventario.UsuarioCreacionid);
            ViewData["UsuarioModificacionid"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre", inventario.UsuarioModificacionid);
            return View(inventario);
        }

        // POST: Inventarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdInventario,UsuarioCreacionid,UsuarioModificacionid,FechaCreacion,FechaModificacion,CantidadDisponible,SucursalId,ProductoId")] Inventario inventario)
        {
            if (id != inventario.IdInventario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventarioExists(inventario.IdInventario))
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
            ViewData["ProductoId"] = new SelectList(_context.Productos, "IdProducto", "Descripcion", inventario.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", inventario.SucursalId);
            ViewData["UsuarioCreacionid"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", inventario.UsuarioCreacionid);
            ViewData["UsuarioModificacionid"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", inventario.UsuarioModificacionid);
            return View(inventario);
        }

        // GET: Inventarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventario
                .Include(i => i.Producto)
                .Include(i => i.Sucursal)
                .Include(i => i.UsuarioCreacion)
                .Include(i => i.UsuarioModificacion)
                .FirstOrDefaultAsync(m => m.IdInventario == id);
            if (inventario == null)
            {
                return NotFound();
            }

            return View(inventario);
        }

        // POST: Inventarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventario = await _context.Inventario.FindAsync(id);
            if (inventario != null)
            {
                _context.Inventario.Remove(inventario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventarioExists(int id)
        {
            return _context.Inventario.Any(e => e.IdInventario == id);
        }
    }
}
