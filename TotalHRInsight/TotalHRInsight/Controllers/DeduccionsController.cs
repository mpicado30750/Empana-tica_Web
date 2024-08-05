using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using TotalHRInsight.Models.Deduccion;

namespace TotalHRInsight.Controllers
{
    public class DeduccionsController : Controller
    {
        private readonly TotalHRInsightDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeduccionsController(TotalHRInsightDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Deduccions
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.Deduccions
                .Include(d => d.TipoDeduccion)
                .Include(d => d.UsuarioAsignacion)
                .Include(d => d.UsuarioCreacion);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: Deduccions/Details/5
        public async Task<IActionResult> Details(int? IdDeduccion)
        {
            if (IdDeduccion == null)
            {
                return NotFound();
            }

            var deduccion = await _context.Deduccions
                .Include(d => d.TipoDeduccion)
                .Include(d => d.UsuarioAsignacion)
                .Include(d => d.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdDeduccion == IdDeduccion);
            if (deduccion == null)
            {
                return NotFound();
            }

            return View(deduccion);
        }

        // GET: Deduccions/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
           ViewData["TipoDeduccionId"] = new SelectList(_context.TipoDeduccions, "IdTipoDeduccion", "NombreDeduccion");
            ViewData["UsuarioAsignacionId"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto"
            );
            var user = await _userManager.GetUserAsync(User);
            ViewData["CurrentUserId"] = user.Id;
            ViewData["CurrentUserName"] = $"{user.Nombre} {user.PrimerApellido}";

            return View();
        }

        // POST: Deduccions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( CreacionDeduccion deduccion)
        {
            if (ModelState.IsValid)
            {
                Deduccion datos = new Deduccion
                {
                    FechaDeduccion = deduccion.FechaDeduccion,
                    NombreDeduccion = deduccion.NombreDeduccion,
                    MontoDeduccion = deduccion.MontoDeduccion,
                    UsuarioCreacionId = deduccion.CurrentUserId,
                    UsuarioAsignacionId = deduccion.UsuarioAsignacionId,
                    TipoDeduccionId = deduccion.TipoDeduccionId,
                };
                _context.Add(datos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoDeduccionId"] = new SelectList(_context.TipoDeduccions, "IdTipoDeduccion", "NombreDeduccion", deduccion.TipoDeduccionId);
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", deduccion.UsuarioAsignacionId);
            ViewData["CurrentUserId"] = deduccion.UsuarioAsignacionId;
            ViewData["CurrentUserName"] = deduccion.CurrentUserName;
            return View(deduccion);
        }

        // GET: Deduccions/Edit/5
        public async Task<IActionResult> Edit(int? IdDeduccion)
        {
            if (IdDeduccion == null)
            {
                return NotFound();
            }

            var deduccion = await _context.Deduccions.FindAsync(IdDeduccion);
            if (deduccion == null)
            {
                return NotFound();
            }
            ViewData["TipoDeduccionId"] = new SelectList(_context.TipoDeduccions, "IdTipoDeduccion", "NombreDeduccion", deduccion.TipoDeduccionId);
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", deduccion.UsuarioAsignacionId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", deduccion.UsuarioCreacionId);
            return View(deduccion);
        }

        // POST: Deduccions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int IdDeduccion, [Bind("IdDeduccion,FechaDeduccion,NombreDeduccion,Descripcion,MontoDeduccion,UsuarioCreacionId,UsuarioAsignacionId,SalarioId,TipoDeduccionId")] Deduccion deduccion)
        {
            if (IdDeduccion != deduccion.IdDeduccion)
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
            ViewData["TipoDeduccionId"] = new SelectList(_context.TipoDeduccions, "IdTipoDeduccion", "NombreDeduccion", deduccion.TipoDeduccionId);
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", deduccion.UsuarioAsignacionId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", deduccion.UsuarioCreacionId);
            return View(deduccion);
        }

        // GET: Deduccions/Delete/5
        public async Task<IActionResult> Delete(int? IdDeduccion)
        {
            if (IdDeduccion == null)
            {
                return NotFound();
            }

            var deduccion = await _context.Deduccions
                .Include(d => d.TipoDeduccion)
                .Include(d => d.UsuarioAsignacion)
                .Include(d => d.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdDeduccion == IdDeduccion);
            if (deduccion == null)
            {
                return NotFound();
            }

            return View(deduccion);
        }

        // POST: Deduccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdDeduccion)
        {
            var deduccion = await _context.Deduccions.FindAsync(IdDeduccion);
            if (deduccion != null)
            {
                _context.Deduccions.Remove(deduccion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeduccionExists(int IdDeduccion)
        {
            return _context.Deduccions.Any(e => e.IdDeduccion == IdDeduccion);
        }
    }
}
