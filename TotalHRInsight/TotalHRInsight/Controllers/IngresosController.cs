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
    public class IngresosController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public IngresosController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: Ingresos
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.Ingresos.Include(i => i.CierreCaja).Include(i => i.TipoIngreso);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: Ingresos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos
                .Include(i => i.CierreCaja)
                .Include(i => i.TipoIngreso)
                .FirstOrDefaultAsync(m => m.IdIngreso == id);
            if (ingreso == null)
            {
                return NotFound();
            }

            return View(ingreso);
        }

        // GET: Ingresos/Create
        public IActionResult Create()
        {
            ViewData["CierreId"] = new SelectList(_context.CierreCajas, "IdCierraCaja", "UsuarioCreacionId");
            ViewData["TipoIngresoId"] = new SelectList(_context.TipoIngresos, "IdTipoIngreso", "NombreIngreso");
            return View();
        }

        // POST: Ingresos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdIngreso,Fecha,TipoIngresoId,MontoIngreso,CierreId")] Ingreso ingreso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingreso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CierreId"] = new SelectList(_context.CierreCajas, "IdCierraCaja", "UsuarioCreacionId", ingreso.CierreId);
            ViewData["TipoIngresoId"] = new SelectList(_context.TipoIngresos, "IdTipoIngreso", "NombreIngreso", ingreso.TipoIngresoId);
            return View(ingreso);
        }

        // GET: Ingresos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos.FindAsync(id);
            if (ingreso == null)
            {
                return NotFound();
            }
            ViewData["CierreId"] = new SelectList(_context.CierreCajas, "IdCierraCaja", "UsuarioCreacionId", ingreso.CierreId);
            ViewData["TipoIngresoId"] = new SelectList(_context.TipoIngresos, "IdTipoIngreso", "NombreIngreso", ingreso.TipoIngresoId);
            return View(ingreso);
        }

        // POST: Ingresos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdIngreso,Fecha,TipoIngresoId,MontoIngreso,CierreId")] Ingreso ingreso)
        {
            if (id != ingreso.IdIngreso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingreso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngresoExists(ingreso.IdIngreso))
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
            ViewData["CierreId"] = new SelectList(_context.CierreCajas, "IdCierraCaja", "UsuarioCreacionId", ingreso.CierreId);
            ViewData["TipoIngresoId"] = new SelectList(_context.TipoIngresos, "IdTipoIngreso", "NombreIngreso", ingreso.TipoIngresoId);
            return View(ingreso);
        }

        // GET: Ingresos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos
                .Include(i => i.CierreCaja)
                .Include(i => i.TipoIngreso)
                .FirstOrDefaultAsync(m => m.IdIngreso == id);
            if (ingreso == null)
            {
                return NotFound();
            }

            return View(ingreso);
        }

        // POST: Ingresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingreso = await _context.Ingresos.FindAsync(id);
            if (ingreso != null)
            {
                _context.Ingresos.Remove(ingreso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngresoExists(int id)
        {
            return _context.Ingresos.Any(e => e.IdIngreso == id);
        }
    }
}
