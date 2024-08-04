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
    public class TipoIngresosController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public TipoIngresosController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: TipoIngresos
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoIngresos.ToListAsync());
        }

        // GET: TipoIngresos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoIngreso = await _context.TipoIngresos
                .FirstOrDefaultAsync(m => m.IdTipoIngreso == id);
            if (tipoIngreso == null)
            {
                return NotFound();
            }

            return View(tipoIngreso);
        }

        // GET: TipoIngresos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoIngresos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoIngreso,NombreIngreso")] TipoIngreso tipoIngreso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoIngreso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoIngreso);
        }

        // GET: TipoIngresos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoIngreso = await _context.TipoIngresos.FindAsync(id);
            if (tipoIngreso == null)
            {
                return NotFound();
            }
            return View(tipoIngreso);
        }

        // POST: TipoIngresos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoIngreso,NombreIngreso")] TipoIngreso tipoIngreso)
        {
            if (id != tipoIngreso.IdTipoIngreso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoIngreso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoIngresoExists(tipoIngreso.IdTipoIngreso))
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
            return View(tipoIngreso);
        }

        // GET: TipoIngresos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoIngreso = await _context.TipoIngresos
                .FirstOrDefaultAsync(m => m.IdTipoIngreso == id);
            if (tipoIngreso == null)
            {
                return NotFound();
            }

            return View(tipoIngreso);
        }

        // POST: TipoIngresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoIngreso = await _context.TipoIngresos.FindAsync(id);
            if (tipoIngreso != null)
            {
                _context.TipoIngresos.Remove(tipoIngreso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoIngresoExists(int id)
        {
            return _context.TipoIngresos.Any(e => e.IdTipoIngreso == id);
        }
    }
}
