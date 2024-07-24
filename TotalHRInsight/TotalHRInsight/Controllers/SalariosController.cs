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
    public class SalariosController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public SalariosController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: Salarios
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.Salarios.Include(s => s.UsuarioAsignacion).Include(s => s.UsuarioCreacion);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: Salarios/Details/5
        public async Task<IActionResult> Details(int? IdSalario)
        {
            if (IdSalario == null)
            {
                return NotFound();
            }

            var salario = await _context.Salarios
                .Include(s => s.UsuarioAsignacion)
                .Include(s => s.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdSalario == IdSalario);
            if (salario == null)
            {
                return NotFound();
            }

            return View(salario);
        }

        // GET: Salarios/Create
        public IActionResult Create()
        {
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre");
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre");
            return View();
        }

        // POST: Salarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSalario,SalarioBruto,SalarioExtra,SalarioNeto,UsuarioCreacionId,UsuarioAsignacionId")] Salario salario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre", salario.UsuarioAsignacionId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre", salario.UsuarioCreacionId);
            return View(salario);
        }

        // GET: Salarios/Edit/5
        public async Task<IActionResult> Edit(int? IdSalario)
        {
            if (IdSalario == null)
            {
                return NotFound();
            }

            var salario = await _context.Salarios.FindAsync(IdSalario);
            if (salario == null)
            {
                return NotFound();
            }
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre", salario.UsuarioAsignacionId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre", salario.UsuarioCreacionId);
            return View(salario);
        }

        // POST: Salarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int IdSalario, [Bind("IdSalario,SalarioBruto,SalarioExtra,SalarioNeto,UsuarioCreacionId,UsuarioAsignacionId")] Salario salario)
        {
            if (IdSalario != salario.IdSalario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalarioExists(salario.IdSalario))
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
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre", salario.UsuarioAsignacionId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre", salario.UsuarioCreacionId);
            return View(salario);
        }

        // GET: Salarios/Delete/5
        public async Task<IActionResult> Delete(int? IdSalario)
        {
            if (IdSalario == null)
            {
                return NotFound();
            }

            var salario = await _context.Salarios
                .Include(s => s.UsuarioAsignacion)
                .Include(s => s.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdSalario == IdSalario);
            if (salario == null)
            {
                return NotFound();
            }

            return View(salario);
        }

        // POST: Salarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdSalario)
        {
            var salario = await _context.Salarios.FindAsync(IdSalario);
            if (salario != null)
            {
                _context.Salarios.Remove(salario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalarioExists(int IdSalario)
        {
            return _context.Salarios.Any(e => e.IdSalario == IdSalario);
        }
    }
}
