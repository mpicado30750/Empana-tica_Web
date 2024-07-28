using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using TotalHRInsight.Models.Planilla;

namespace TotalHRInsight.Controllers
{
    public class PlanillasController : Controller
    {
        private readonly TotalHRInsightDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlanillasController(TotalHRInsightDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Planillas
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.Planillas.Include(p => p.UsuarioAsignacion).Include(p => p.UsuarioCreacion);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: Planillas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planilla = await _context.Planillas
                .Include(p => p.UsuarioAsignacion)
                .Include(p => p.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdPlanilla == id);
            if (planilla == null)
            {
                return NotFound();
            }

            return View(planilla);
        }

        // GET: Planillas/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
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

        // POST: Planillas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( CrearPlanilla planilla)
        {
            if (ModelState.IsValid)
            {
                var empleado = _context.ApplicationUsers
                    .Include(i => i.Sucursal)
                    .FirstOrDefaultAsync(f => f.Id == planilla.UsuarioAsignacionId);

                if (empleado == null)
                {
                    ModelState.AddModelError(string.Empty, "No se encontro al empleado especificado.");
                    return View(planilla);
                }

                var asistencia = _context.Asistencias
                    .Include(i => i.UsuarioCreacion)
                    .Where(w => w.UsuarioCreacionId == planilla.UsuarioAsignacionId && 
                    (w.FechaEntrada >= planilla.FechaInicio)&&
                    (w.FechaSalida <= planilla.FechaFin)).ToList();

                if (asistencia == null || asistencia.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "No se encontraron asistencias para el usuario y las fechas especificadas.");
                    return View(planilla);
                }

                var (horasTrabajo, horasExtra) = CalcularHorasTabajo(asistencia);

                var permisos = _context.Permisos
                    .Include(i => i.UsuarioAsignacion)
                    .Include(i => i.TipoPermisos)
                    .Where(w => w.UsuarioAsignacionId == planilla.UsuarioAsignacionId &&
                    (w.FechaInicio >= planilla.FechaInicio) &&
                    (w.FechaFin <= planilla.FechaFin)).ToList();

                var deduccion = _context.Deduccions
                    .Include(i => i.TipoDeduccion)
                    .Include(i => i.UsuarioAsignacion)
                    .Where(w => w.FechaDeduccion >= planilla.FechaInicio &&
                    w.FechaDeduccion <= planilla.FechaFin &&
                    w.UsuarioAsignacionId == planilla.UsuarioAsignacionId);


                //_context.Add(planilla);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioAsignacionId"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto"
            );
            var user = await _userManager.GetUserAsync(User);
            ViewData["CurrentUserId"] = user.Id;
            ViewData["CurrentUserName"] = $"{user.Nombre} {user.PrimerApellido}";
            return View(planilla);
        }

        private static (double horasTrabajo, double horasExtra) CalcularHorasTabajo (List<Asistencia> asistenciaList)
        {
            double horasExtraTotal = 0;
            double horasTrabajoTotal = 0;

            foreach ( var datos in asistenciaList)
            {
                TimeSpan diferencia = datos.FechaSalida - datos.FechaEntrada;
                double horas = diferencia.TotalHours;
                double horasExtra = horas - 8;
                
                if(horasExtra > 0)
                {
                    horasExtraTotal += horasExtra;
                }

                horasTrabajoTotal += horas - horasExtra;
            }

            return (horasTrabajoTotal, horasExtraTotal);
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
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", planilla.UsuarioAsignacionId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", planilla.UsuarioCreacionId);
            return View(planilla);
        }

        // POST: Planillas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPlanilla,FechaInicio,FechaFin,MontoTotal,UsuarioCreacionId,UsuarioAsignacionId")] Planilla planilla)
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
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", planilla.UsuarioAsignacionId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", planilla.UsuarioCreacionId);
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
                .Include(p => p.UsuarioAsignacion)
                .Include(p => p.UsuarioCreacion)
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

        // Método para calcular la planilla
        public async Task<IActionResult> CalcularPlanilla(DateTime fechaInicio, DateTime fechaFin)
        {
            var planilla = from u in _context.ApplicationUsers
                           join a in _context.Asistencias on u.Id equals a.UsuarioCreacionId into asistencias
                           from a in asistencias.DefaultIfEmpty()
                           join d in _context.Deduccions on u.Id equals d.UsuarioAsignacionId into deducciones
                           from d in deducciones.DefaultIfEmpty()
                           group new { u, a, d } by new { u.Id, u.Nombre, u.Salario } into g
                           select new
                           {
                               Id = g.Key.Id,
                               Nombre = g.Key.Nombre,
                               SalarioBase = g.Key.Salario,
                               TotalDeducciones = g.Where(x => x.d != null && x.d.FechaDeduccion >= fechaInicio && x.d.FechaDeduccion <= fechaFin).Sum(x => (double?)x.d.MontoDeduccion) ?? 0,
                               DiasTrabajados = g.Count(x => x.a != null && x.a.FechaEntrada >= fechaInicio && x.a.FechaEntrada <= fechaFin),
                           };

            var result = await planilla.ToListAsync();

            var viewModel = result.Select(p => new
            {
                p.Id,
                p.Nombre,
                p.SalarioBase,
                p.TotalDeducciones,
                p.DiasTrabajados,
                SalarioNeto = (p.SalarioBase / 15.0 * p.DiasTrabajados) - p.TotalDeducciones
            });

            return View(viewModel);
        }

    }

}


