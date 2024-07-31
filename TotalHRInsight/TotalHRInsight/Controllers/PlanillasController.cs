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
        public async Task<ApplicationUser?> GetUserAsync()
        {
            return await _userManager.GetUserAsync(User);
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
        // POST: Planillas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearPlanilla planilla)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                // Obtener el empleado y su sucursal
                var empleado = await _context.ApplicationUsers
                    .Include(i => i.Sucursal)
                    .FirstOrDefaultAsync(f => f.Id == planilla.UsuarioAsignacionId);

                if (empleado == null)
                {
                    ModelState.AddModelError(string.Empty, "No se encontró al empleado especificado.");
                    return View(planilla);
                }

                // Obtener asistencias del empleado en el rango de fechas especificado
                var asistencia = _context.Asistencias
                    .Include(i => i.UsuarioCreacion)
                    .Where(w => w.UsuarioCreacionId == planilla.UsuarioAsignacionId &&
                                w.FechaEntrada >= planilla.FechaInicio &&
                                w.FechaSalida <= planilla.FechaFin)
                    .ToList();

                if (asistencia == null || asistencia.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "No se encontraron asistencias para el usuario y las fechas especificadas.");
                    return View(planilla);
                }

                // Calcular horas de trabajo y horas extras
                var (horasTrabajo, horasExtra) = CalcularHorasTabajo(asistencia);

                // Obtener permisos del empleado en el rango de fechas especificado
                var permisos = _context.Permisos
                    .Include(i => i.UsuarioAsignacion)
                    .Include(i => i.TipoPermisos)
                    .Where(w => w.UsuarioAsignacionId == planilla.UsuarioAsignacionId &&
                                w.FechaInicio >= planilla.FechaInicio &&
                                w.FechaFin <= planilla.FechaFin)
                    .ToList();

                // Calcular permisos
                int permisosTotal = CalcularPermisos(permisos);

                horasTrabajo += 12 * permisosTotal;

                // Obtener deducciones del empleado en el rango de fechas especificado
                var deduccion = _context.Deduccions
                    .Include(i => i.TipoDeduccion)
                    .Include(i => i.UsuarioAsignacion)
                    .Where(w => w.FechaDeduccion >= planilla.FechaInicio &&
                                w.FechaDeduccion <= planilla.FechaFin &&
                                w.UsuarioAsignacionId == planilla.UsuarioAsignacionId)
                    .ToList();

                if (deduccion == null || deduccion.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "No se encontraron datos especificados.");
                    return View(planilla);
                }

                // Calcular total de deducciones
                double totalDeduccion = CantidadDeduccion(deduccion);

                // Calcular salario bruto, horas extras y salario neto
                double salarioBruto = horasTrabajo * empleado.Salario; // Suponiendo que empleado.SalarioBase es el salario por hora
                double salarioExtra = horasExtra * (empleado.Salario * 1.5); // Suponiendo que empleado.SalarioExtra es el salario por hora extra
                double salarioNeto = (salarioBruto + salarioExtra) - totalDeduccion;

                // Crear un nuevo registro de planilla
                var nuevaPlanilla = new Planilla
                {
                    FechaInicio = planilla.FechaInicio,
                    FechaFin = planilla.FechaFin,
                    Descripcion = planilla.Comentarios,
                    MontoTotal = salarioNeto,
                    UsuarioCreacionId = user.Id,
                    UsuarioAsignacionId = planilla.UsuarioAsignacionId
                };

                // Guardar la planilla en la base de datos
                _context.Add(nuevaPlanilla);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UsuarioAsignacionId"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto"
            );
            ViewData["CurrentUserId"] = user.Id;
            ViewData["CurrentUserName"] = $"{user.Nombre} {user.PrimerApellido}";
            return View(planilla);
        }


        private int CalcularPermisos(List<Permiso> permisos)
        {
            int totalVacaciones = 0;
            int totalLicMaternidad = 0;
            int totalLicPaternidad = 0;
            int totalGoce = 0;

            foreach (var dato in permisos)
            {
                string permisoNormalizado = NormalizarNombrePermiso(dato.TipoPermisos.NombrePermiso);

                switch (permisoNormalizado)
                {
                    case "vacaciones":
                        totalVacaciones++;
                        break;
                    case "licenciadematernidad":
                        totalLicMaternidad++;
                        break;
                    case "licenciadepaternidad":
                        totalLicPaternidad++;
                        break;
                    case "gocedesueldo":
                        totalGoce++;
                        break;
                    default:
                        // Manejar casos desconocidos si es necesario
                        break;
                }
            }

            int permisosTotal = totalGoce + totalLicMaternidad + totalLicPaternidad + totalVacaciones;

            return (permisosTotal);
        }

        private static string NormalizarNombrePermiso(string nombrePermiso)
        {
            return nombrePermiso.ToLower()
                                .Replace(" ", "")
                                .Replace("é", "e")  // Reemplazar acentos, si es necesario
                                .Replace("í", "i")
                                .Replace("ó", "o")
                                .Replace("á", "a")
                                .Replace("ú", "u");
        }

        private static (double horasTrabajo, double horasExtra) CalcularHorasTabajo(List<Asistencia> asistenciaList)
        {
            double horasTrabajoTotal = 0;
            double horasExtraTotal = 0;
            const double horasNormales = 12;

            foreach (var datos in asistenciaList)
            {
                TimeSpan diferencia = datos.FechaSalida - datos.FechaEntrada;
                double horasTrabajadas = diferencia.TotalHours;
                double horasExtra = 0;

                if (horasTrabajadas > horasNormales)
                {
                    horasExtra = horasTrabajadas - horasNormales;
                    horasTrabajoTotal += horasNormales;
                }
                else
                {
                    horasTrabajoTotal += horasTrabajadas;
                }

                horasExtraTotal += horasExtra;
            }

            return (horasTrabajoTotal, horasExtraTotal);
        }

        private double CantidadDeduccion (List<Deduccion> deduccion)
        {
            double total = 0;

            foreach (var dato in deduccion)
            {
                total += dato.MontoDeduccion;
            }

            return total;
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


