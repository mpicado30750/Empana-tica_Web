using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;

namespace TotalHRInsight.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CierreCajasController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public CierreCajasController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: CierreCajas
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.CierreCajas.Include(c => c.Sucursal).Include(c => c.UsuarioCreacion);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: CierreCajas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cierreCaja = await _context.CierreCajas
                .Include(c => c.Sucursal)
                .Include(c => c.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdCierraCaja == id);
            if (cierreCaja == null)
            {
                return NotFound();
            }

            return View(cierreCaja);
        }

        // GET: CierreCajas/Create
        public IActionResult Create()
        {
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal");
            ViewData["UsuarioCreacionid"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto"
            );
            return View();
        }

        // POST: CierreCajas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCierraCaja,Fecha,SucursalId,MontoTotal,UsuarioCreacionId")] CierreCaja cierreCaja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cierreCaja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", cierreCaja.SucursalId);
            ViewData["UsuarioCreacionid"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto"
            );
            return View(cierreCaja);
        }

        // GET: CierreCajas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cierreCaja = await _context.CierreCajas.FindAsync(id);
            if (cierreCaja == null)
            {
                return NotFound();
            }
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", cierreCaja.SucursalId);
            ViewData["UsuarioCreacionid"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto"
            );
            return View(cierreCaja);
        }

        // POST: CierreCajas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCierraCaja,Fecha,SucursalId,MontoTotal,UsuarioCreacionId")] CierreCaja cierreCaja)
        {
            if (id != cierreCaja.IdCierraCaja)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cierreCaja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CierreCajaExists(cierreCaja.IdCierraCaja))
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
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", cierreCaja.SucursalId);
            ViewData["UsuarioCreacionid"] = new SelectList(
               _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
               "Id",
               "NombreCompleto"
           );
            return View(cierreCaja);
        }

        // GET: CierreCajas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cierreCaja = await _context.CierreCajas
                .Include(c => c.Sucursal)
                .Include(c => c.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdCierraCaja == id);
            if (cierreCaja == null)
            {
                return NotFound();
            }

            return View(cierreCaja);
        }

        // POST: CierreCajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cierreCaja = await _context.CierreCajas.FindAsync(id);
            if (cierreCaja != null)
            {
                _context.CierreCajas.Remove(cierreCaja);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CierreCajaExists(int id)
        {
            return _context.CierreCajas.Any(e => e.IdCierraCaja == id);
        }


    }
}
