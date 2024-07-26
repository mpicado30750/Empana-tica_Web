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
    public class TipoDeduccionsController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public TipoDeduccionsController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: TipoDeduccions
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoDeduccions.ToListAsync());
        }

        // GET: TipoDeduccions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDeduccion = await _context.TipoDeduccions
                .FirstOrDefaultAsync(m => m.IdTipoDeduccion == id);
            if (tipoDeduccion == null)
            {
                return NotFound();
            }

            return View(tipoDeduccion);
        }

        // GET: TipoDeduccions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoDeduccions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoDeduccion,NombreDeduccion")] TipoDeduccion tipoDeduccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoDeduccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoDeduccion);
        }

        // GET: TipoDeduccions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDeduccion = await _context.TipoDeduccions.FindAsync(id);
            if (tipoDeduccion == null)
            {
                return NotFound();
            }
            return View(tipoDeduccion);
        }

        // POST: TipoDeduccions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoDeduccion,NombreDeduccion")] TipoDeduccion tipoDeduccion)
        {
            if (id != tipoDeduccion.IdTipoDeduccion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoDeduccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoDeduccionExists(tipoDeduccion.IdTipoDeduccion))
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
            return View(tipoDeduccion);
        }

        // GET: TipoDeduccions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDeduccion = await _context.TipoDeduccions
                .FirstOrDefaultAsync(m => m.IdTipoDeduccion == id);
            if (tipoDeduccion == null)
            {
                return NotFound();
            }

            return View(tipoDeduccion);
        }

        // POST: TipoDeduccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoDeduccion = await _context.TipoDeduccions.FindAsync(id);
            if (tipoDeduccion != null)
            {
                _context.TipoDeduccions.Remove(tipoDeduccion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoDeduccionExists(int id)
        {
            return _context.TipoDeduccions.Any(e => e.IdTipoDeduccion == id);
        }
    }
}
