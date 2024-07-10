using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using TotalHRInsight.Models.Inventario;

namespace TotalHRInsight.Controllers
{
    public class InventariosController : Controller
    {
        private readonly TotalHRInsightDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public InventariosController(TotalHRInsightDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Inventarios
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.Inventario.Include(i => i.Producto).Include(i => i.Sucursal).Include(i => i.UsuarioCreacion).Include(i => i.UsuarioModificacion);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: Inventarios/Details/5
        public async Task<IActionResult> Details(int? IdInventario)
        {
            if (IdInventario == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventario
                .Include(i => i.Producto)
                .Include(i => i.Sucursal)
                .Include(i => i.UsuarioCreacion)
                .Include(i => i.UsuarioModificacion)
                .FirstOrDefaultAsync(m => m.IdInventario == IdInventario);
            if (inventario == null)
            {
                return NotFound();
            }

            return View(inventario);
        }

        // GET: Inventarios/Create
        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.Productos, "IdProducto", "Descripcion");
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal");
            return View();
        }

        // POST: Inventarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearInventario datos)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                var inventario = new Inventario
                {
                    UsuarioModificacionid = user.Id,
                    UsuarioCreacionid = user.Id,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now,
                    CantidadDisponible = datos.CantidadDisponible,
                    SucursalId = datos.SucursalId,
                    ProductoId = datos.ProductoId
                };

                _context.Add(inventario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "IdProducto", "Descripcion", datos.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", datos.SucursalId);
            return View(datos);
        }

        // GET: Inventarios/Edit/5
        public async Task<IActionResult> Edit(int? IdInventario)
        {
            if (IdInventario == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventario.FindAsync(IdInventario);
            if (inventario == null)
            {
                return NotFound();
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "IdProducto", "Descripcion", inventario.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", inventario.SucursalId);
            ViewData["UsuarioCreacionid"] = new SelectList(_context.Set<ApplicationUser>(),"Id","Nombre",inventario.UsuarioCreacionid);
            ViewData["UsuarioModificacionid"] = new SelectList(_context.Set<ApplicationUser>(),"Id", "Nombre", inventario.UsuarioModificacionid);
            return View(inventario);
        }

        // POST: Inventarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int IdInventario, Inventario inventario)
        {
            if (IdInventario != inventario.IdInventario)
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
            ViewData["UsuarioCreacionid"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre", inventario.UsuarioCreacionid);
            ViewData["UsuarioModificacionid"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "NombreCompleto", inventario.UsuarioModificacionid);
            return View(inventario);
        }

        // GET: Inventarios/Delete/5
        public async Task<IActionResult> Delete(int? IdInventario)
        {
            if (IdInventario == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventario
                .Include(i => i.Producto)
                .Include(i => i.Sucursal)
                .Include(i => i.UsuarioCreacion)
                .Include(i => i.UsuarioModificacion)
                .FirstOrDefaultAsync(m => m.IdInventario == IdInventario);
            if (inventario == null)
            {
                return NotFound();
            }

            return View(inventario);
        }

        // POST: Inventarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdInventario)
        {
            var inventario = await _context.Inventario.FindAsync(IdInventario);
            if (inventario != null)
            {
                _context.Inventario.Remove(inventario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventarioExists(int IdInventario)
        {
            return _context.Inventario.Any(e => e.IdInventario == IdInventario);
        }
    }
}