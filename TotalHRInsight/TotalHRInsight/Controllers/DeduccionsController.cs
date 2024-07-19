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
    public class DeduccionsController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public DeduccionsController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: Deduccions
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.Deduccions.Include(d => d.Salario).Include(d => d.UsuarioAsignacion).Include(d => d.UsuarioCreacion);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: Deduccions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deduccion = await _context.Deduccions
                .Include(d => d.Salario)
                .Include(d => d.UsuarioAsignacion)
                .Include(d => d.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdDeduccion == id);
            if (deduccion == null)
            {
                return NotFound();
            }

            return View(deduccion);
        }

        // GET: Deduccions/Create
        public IActionResult Create()
        {
            ViewData["SalarioId"] = new SelectList(_context.Salarios, "IdSalario", "UsuarioAsignacionId");
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: Deduccions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDeduccion,FechaDeduccion,NombreDeduccion,Descripcion,MontoDeduccion,UsuarioCreacionId,UsuarioAsignacionId,SalarioId")] Deduccion deduccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deduccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalarioId"] = new SelectList(_context.Salarios, "IdSalario", "UsuarioAsignacionId", deduccion.SalarioId);
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", deduccion.UsuarioAsignacionId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", deduccion.UsuarioCreacionId);
            return View(deduccion);
        }

        // GET: Deduccions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deduccion = await _context.Deduccions.FindAsync(id);
            if (deduccion == null)
            {
                return NotFound();
            }
            ViewData["SalarioId"] = new SelectList(_context.Salarios, "IdSalario", "UsuarioAsignacionId", deduccion.SalarioId);
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", deduccion.UsuarioAsignacionId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", deduccion.UsuarioCreacionId);
            return View(deduccion);
        }

        // POST: Deduccions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDeduccion,FechaDeduccion,NombreDeduccion,Descripcion,MontoDeduccion,UsuarioCreacionId,UsuarioAsignacionId,SalarioId")] Deduccion deduccion)
        {
            if (id != deduccion.IdDeduccion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deduccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeduccionExists(deduccion.IdDeduccion))
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
            ViewData["SalarioId"] = new SelectList(_context.Salarios, "IdSalario", "UsuarioAsignacionId", deduccion.SalarioId);
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", deduccion.UsuarioAsignacionId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", deduccion.UsuarioCreacionId);
            return View(deduccion);
        }

        // GET: Deduccions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deduccion = await _context.Deduccions
                .Include(d => d.Salario)
                .Include(d => d.UsuarioAsignacion)
                .Include(d => d.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdDeduccion == id);
            if (deduccion == null)
            {
                return NotFound();
            }

            return View(deduccion);
        }

        // POST: Deduccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deduccion = await _context.Deduccions.FindAsync(id);
            if (deduccion != null)
            {
                _context.Deduccions.Remove(deduccion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeduccionExists(int id)
        {
            return _context.Deduccions.Any(e => e.IdDeduccion == id);
        }
    }
}
