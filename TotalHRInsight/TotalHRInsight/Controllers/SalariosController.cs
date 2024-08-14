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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salario = await _context.Salarios
                .Include(s => s.UsuarioAsignacion)
                .Include(s => s.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdSalario == id);
            if (salario == null)
            {
                return NotFound();
            }

            return View(salario);
        }

        // GET: Salarios/Create
        public IActionResult Create()
        {
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
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
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", salario.UsuarioAsignacionId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", salario.UsuarioCreacionId);
            return View(salario);
        }

        // GET: Salarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salario = await _context.Salarios.FindAsync(id);
            if (salario == null)
            {
                return NotFound();
            }

            // SelectLists actualizados para mostrar el NombreCompleto
            ViewData["UsuarioAsignacionId"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido + " " + u.SegundoApellido }),
                "Id",
                "NombreCompleto",
                salario.UsuarioAsignacionId
            );
            ViewData["UsuarioCreacionId"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido + " " + u.SegundoApellido }),
                "Id",
                "NombreCompleto",
                salario.UsuarioCreacionId
            );

            return View(salario);
        }

        // POST: Salarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSalario,SalarioBruto,SalarioExtra,SalarioNeto,UsuarioCreacionId,UsuarioAsignacionId")] Salario salario)
        {
            if (id != salario.IdSalario)
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

            // SelectLists actualizados para mostrar el NombreCompleto
            ViewData["UsuarioAsignacionId"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido + " " + u.SegundoApellido }),
                "Id",
                "NombreCompleto",
                salario.UsuarioAsignacionId
            );
            ViewData["UsuarioCreacionId"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido + " " + u.SegundoApellido }),
                "Id",
                "NombreCompleto",
                salario.UsuarioCreacionId
            );

            return View(salario);
        }


        // GET: Salarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salario = await _context.Salarios
                .Include(s => s.UsuarioAsignacion)
                .Include(s => s.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdSalario == id);
            if (salario == null)
            {
                return NotFound();
            }

            return View(salario);
        }

        // POST: Salarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salario = await _context.Salarios.FindAsync(id);
            if (salario != null)
            {
                _context.Salarios.Remove(salario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalarioExists(int id)
        {
            return _context.Salarios.Any(e => e.IdSalario == id);
        }
    }
}
