using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using TotalHRInsight.Models.Planilla;

namespace TotalHRInsight.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PlanillasController : Controller
    {
        private readonly TotalHRInsightDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PlanillasController> _logger;

        public PlanillasController(TotalHRInsightDbContext context, UserManager<ApplicationUser> userManager, ILogger<PlanillasController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
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


                var nuevoSalario = new Salario
                {
                    SalarioBruto = salarioBruto,
                    SalarioExtra = salarioExtra,
                    SalarioNeto = salarioNeto,
                    UsuarioCreacionId = user.Id,
                    UsuarioAsignacionId = planilla.UsuarioAsignacionId
                };

                _context.Add(nuevoSalario);
                await _context.SaveChangesAsync();

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
                TimeSpan diferencia = dato.FechaFin - dato.FechaInicio;
                int diasTrabajados = diferencia.Days;
                switch (permisoNormalizado)
                {
                    case "vacaciones":
                        totalVacaciones += diasTrabajados;
                        break;
                    case "licenciadematernidad":
                        totalLicMaternidad += diasTrabajados;
                        break;
                    case "licenciadepaternidad":
                        totalLicPaternidad += diasTrabajados;
                        break;
                    case "gocedesueldo":
                        totalGoce += diasTrabajados;
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

            // SelectLists actualizados para mostrar el NombreCompleto
            ViewData["UsuarioAsignacionId"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido + " " + u.SegundoApellido}),
                "Id",
                "NombreCompleto",
                planilla.UsuarioAsignacionId
            );
            ViewData["UsuarioCreacionId"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido + " " + u.SegundoApellido}),
                "Id",
                "NombreCompleto",
                planilla.UsuarioCreacionId
            );

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

            // SelectLists actualizados para mostrar el NombreCompleto
            ViewData["UsuarioAsignacionId"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto",
                planilla.UsuarioAsignacionId
            );
            ViewData["UsuarioCreacionId"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto",
                planilla.UsuarioCreacionId
            );

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

        //Export
        public async Task<IActionResult> ExportarColillaPago(int id)
        {
            try
            {
                // Fetch the planilla including relevant details
                var planilla = await _context.Planillas
                    .Include(p => p.UsuarioAsignacion)
                    .Include(p => p.UsuarioCreacion)
                    .FirstOrDefaultAsync(p => p.IdPlanilla == id);

                if (planilla == null)
                {
                    return NotFound();
                }

                var usuario = await _context.ApplicationUsers
                    .FirstOrDefaultAsync(u => u.Id == planilla.UsuarioAsignacionId);

                if (usuario == null)
                {
                    return NotFound();
                }

                var salario = await _context.Salarios
                    .FirstOrDefaultAsync(s => s.UsuarioAsignacionId == usuario.Id);

                if (salario == null)
                {
                    return NotFound();
                }

                var permisos = await _context.Permisos
                    .Where(p => p.UsuarioAsignacionId == usuario.Id)
                    .Include(p => p.TipoPermisos)
                    .ToListAsync();

                var deducciones = await _context.Deduccions
                    .Where(d => d.UsuarioAsignacionId == usuario.Id)
                    .Include(d => d.TipoDeduccion)
                    .ToListAsync();

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Colilla de Pago");
                    worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;

                    // Add images (optional)
                    var imagePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Empana-tica_Logo.png");
                    var imagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pyme.png");
                    if (System.IO.File.Exists(imagePath1) && System.IO.File.Exists(imagePath2))
                    {
                        worksheet.AddPicture(imagePath1).MoveTo(worksheet.Cell("A1")).Scale(0.15);
                        worksheet.AddPicture(imagePath2).MoveTo(worksheet.Cell("F1")).Scale(0.1);
                    }

                    worksheet.Row(1).Height = 60;
                    worksheet.Column(1).Width = 12;
                    worksheet.Column(6).Width = 12;

                    // Title
                    var titleCell = worksheet.Cell("A3");
                    titleCell.Value = "Colilla de Pago";
                    titleCell.Style.Font.Bold = true;
                    titleCell.Style.Font.FontSize = 16;
                    titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    titleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
                    titleCell.Style.Font.FontColor = XLColor.White;
                    worksheet.Range("A3:F3").Merge();  // Asegúrate que solo estas celdas tienen el fondo azul

                    // Headers
                    var headerRow = worksheet.Row(5);
                    headerRow.Cell(1).Value = "Nombre";
                    headerRow.Cell(2).Value = "Sueldo Bruto";
                    headerRow.Cell(3).Value = "Deducciones";
                    headerRow.Cell(4).Value = "Permisos";
                    headerRow.Cell(5).Value = "Horas Extra";
                    headerRow.Cell(6).Value = "Sueldo Neto";
                    headerRow.Style.Font.Bold = true;
                    headerRow.Style.Font.FontSize = 12;
                    headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerRow.Style.Font.FontColor = XLColor.White;
                    worksheet.Range("A5:F5").Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");  // Aplicar azul solo a las celdas A5:F5

                    // Format for currency in colones
                    var colonesCurrencyFormat = "₡ #,##0.00";

                    // Content
                    worksheet.Cell(6, 1).Value = $"{usuario.Nombre} {usuario.PrimerApellido} {usuario.SegundoApellido}";
                    worksheet.Cell(6, 2).Value = salario.SalarioBruto;
                    worksheet.Cell(6, 2).Style.NumberFormat.Format = colonesCurrencyFormat;
                    worksheet.Cell(6, 3).Value = "Ver detalles abajo";
                    worksheet.Cell(6, 4).Value = "Ver detalles abajo";
                    worksheet.Cell(6, 5).Value = salario.SalarioExtra;
                    worksheet.Cell(6, 5).Style.NumberFormat.Format = colonesCurrencyFormat;
                    worksheet.Cell(6, 6).Value = salario.SalarioNeto;
                    worksheet.Cell(6, 6).Style.NumberFormat.Format = colonesCurrencyFormat;

                    // Add deduction details
                    var deduccionesStartRow = 10; // Starting row for deductions details
                    worksheet.Cell(deduccionesStartRow, 1).Value = "Tipo de Deducción";
                    worksheet.Cell(deduccionesStartRow, 2).Value = "Descripción";
                    worksheet.Cell(deduccionesStartRow, 3).Value = "Monto";
                    worksheet.Row(deduccionesStartRow).Style.Font.Bold = true;
                    worksheet.Range(deduccionesStartRow, 1, deduccionesStartRow, 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#D9EAD3");

                    int row = deduccionesStartRow + 1;
                    foreach (var deduccion in deducciones)
                    {
                        worksheet.Cell(row, 1).Value = deduccion.TipoDeduccion?.NombreDeduccion;
                        worksheet.Cell(row, 2).Value = deduccion.NombreDeduccion;
                        worksheet.Cell(row, 3).Value = deduccion.MontoDeduccion;
                        worksheet.Cell(row, 3).Style.NumberFormat.Format = colonesCurrencyFormat;
                        row++;
                    }

                    // Add permission details
                    var permisosStartRow = row + 2; // Starting row for permissions details
                    worksheet.Cell(permisosStartRow, 1).Value = "Tipo de Permiso";
                    worksheet.Cell(permisosStartRow, 2).Value = "Cantidad de Días";
                    worksheet.Row(permisosStartRow).Style.Font.Bold = true;
                    worksheet.Range(permisosStartRow, 1, permisosStartRow, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#D9EAD3");

                    row = permisosStartRow + 1;
                    foreach (var permiso in permisos)
                    {
                        worksheet.Cell(row, 1).Value = permiso.TipoPermisos?.NombrePermiso;
                        worksheet.Cell(row, 2).Value = permiso.CantidadDias;
                        row++;
                    }

                    // Add planilla details
                    worksheet.Cell(row + 2, 1).Value = "Comentarios:";
                    worksheet.Cell(row + 2, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#D9EAD3"); // Mantiene el fondo verde en "Comentarios:"
                    worksheet.Cell(row + 2, 2).Value = planilla.Descripcion;
                    // Se eliminó el color de fondo para la celda que contiene la descripción

                    worksheet.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        var fileName = $"ColillaPago_{usuario.Nombre}{usuario.PrimerApellido}_{DateTime.Now:ddMMyyyy}.xlsx";
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar la colilla de pago.");
                return StatusCode(500, "Ocurrió un error al generar la colilla de pago.");
            }
        }
    }
}

