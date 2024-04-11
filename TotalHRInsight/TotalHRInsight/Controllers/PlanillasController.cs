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
    public class PlanillasController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public PlanillasController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: Planillas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Planillas.ToListAsync());
        }

        // GET: Planillas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planilla = await _context.Planillas
                .FirstOrDefaultAsync(m => m.IdPlanilla == id);
            if (planilla == null)
            {
                return NotFound();
            }

            return View(planilla);
        }

        // GET: Planillas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Planillas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPlanilla,FechaInicio,FechaFin,MontoTotal,UsuarioCrecionId,IdAsistencia,IdPermiso")] Planilla planilla)
        {
            if (ModelState.IsValid)
            {
                _context.Add(planilla);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(planilla);
        }

        // GET: Planillas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planilla = await _context.Planillas.FindAsync(id);
            if (planilla == null)
            {
                return NotFound();
            }
            return View(planilla);
        }

        // POST: Planillas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPlanilla,FechaInicio,FechaFin,MontoTotal,UsuarioCrecionId,IdAsistencia,IdPermiso")] Planilla planilla)
        {
            if (id != planilla.IdPlanilla)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(planilla);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanillaExists(planilla.IdPlanilla))
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
            return View(planilla);
        }

        // GET: Planillas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planilla = await _context.Planillas
                .FirstOrDefaultAsync(m => m.IdPlanilla == id);
            if (planilla == null)
            {
                return NotFound();
            }

            return View(planilla);
        }

        // POST: Planillas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var planilla = await _context.Planillas.FindAsync(id);
            if (planilla != null)
            {
                _context.Planillas.Remove(planilla);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanillaExists(int id)
        {
            return _context.Planillas.Any(e => e.IdPlanilla == id);
        }
    }
}
