using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using TotalHRInsight.Models;
using TotalHRInsight.Models.Asistencia;

namespace TotalHRInsight.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AsistenciasController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public AsistenciasController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: Asistencias
        public async Task<IActionResult> Index()
        {
			
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> FiltroAsistencia(string nombre, DateTime? fechaIngreso, DateTime? fechaSalida)
        {
            try
            {
                var asistencias = await _context.Asistencias
                    .Include(a => a.UsuarioCreacion)
                    .Where(a =>
                        (string.IsNullOrWhiteSpace(nombre) || a.UsuarioCreacion.Nombre.Contains(nombre)) &&
                        (!fechaIngreso.HasValue || a.FechaEntrada.Date >= fechaIngreso.Value.Date) &&
                        (!fechaSalida.HasValue || a.FechaSalida.Date <= fechaSalida.Value.Date))
                    .OrderByDescending(a => a.FechaEntrada) 
                    .Select(a => new AsistenciaModel
                    {
                        Id = a.IdAsistencia,
                        FechaEntrada = a.FechaEntrada,
                        FechaSalida = a.FechaSalida,
                        LatitudEntrada = ExtractLatitude(a.UbicacionEntrada),
                        LongitudEntrada = ExtractLongitude(a.UbicacionEntrada),
                        LatitudSalida = ExtractLatitude(a.UbicacionSalida),
                        LongitudSalida = ExtractLongitude(a.UbicacionSalida),
                        UsuarioCreacionId = a.UsuarioCreacionId,
                        UsuarioCreacion = a.UsuarioCreacion.UserName,
                        Nombre = a.UsuarioCreacion.Nombre,
                        PrimerApellido = a.UsuarioCreacion.PrimerApellido,
                        SegundoApellido = a.UsuarioCreacion.SegundoApellido
                    })
                    .ToListAsync();

                return Json(asistencias);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public static double ExtractLatitude(string input)
        {
            string pattern = @"lat:\s*([\d\.,-]+)";
            Regex regex = new Regex(pattern);

            Match match = regex.Match(input);

            if (match.Success)
            {
                string latitudeStr = match.Groups[1].Value;
                latitudeStr = RemoveTrailingComma(latitudeStr);
                latitudeStr = latitudeStr.Replace(',', '.');
                return double.Parse(latitudeStr,CultureInfo.InvariantCulture);
            }
            else
            {
                throw new FormatException("No se encontró la latitud en la cadena de entrada.");
            }
        }

        private static string RemoveTrailingComma(string input)
        {
            if (input.EndsWith(","))
            {
                return input.Substring(0, input.Length - 1);
            }
            return input;
        }
        // Función para extraer la longitud de una cadena
        public static double ExtractLongitude(string input)
        {
            string pattern = @"lng:\s*([\d\.,-]+)";
            Regex regex = new Regex(pattern);

            Match match = regex.Match(input);

            if (match.Success)
            {
                string longitudeStr = match.Groups[1].Value;
                longitudeStr = longitudeStr.Replace(',', '.');
                return double.Parse(longitudeStr,CultureInfo.InvariantCulture);
            }
            else
            {
                throw new FormatException("No se encontró la longitud en la cadena de entrada.");
            }
        }

        // GET: Asistencias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistencia = await _context.Asistencias
                .Include(a => a.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdAsistencia == id);
            if (asistencia == null)
            {
                return NotFound();
            }

            return View(asistencia);
        }

		// GET: Asistencias/Create
		public async Task<IActionResult> Create()
        {
            ViewData["UsuarioCreacionid"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto"
            );
            var sucursales = await _context.Sucursales.ToListAsync();
            ViewData["IdSucursal"] = new SelectList(sucursales, "IdSucursal", "NombreSucursal");

            return View();
        }

        // POST: Asistencias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearAsistencia asistencia)
        {
            if (ModelState.IsValid)
            {
                var consultaUbicacion = await _context.Sucursales
                    .FirstOrDefaultAsync(f => f.IdSucursal == asistencia.IdSucursal);
                Asistencia datos = new Asistencia
                {
                    FechaEntrada = asistencia.FechaEntrada,
                    FechaSalida = asistencia.FechaSalida,
                    UbicacionEntrada = $"LatLng(lat: {consultaUbicacion.Latitud}, lng:{consultaUbicacion.Longitud})",
                    UbicacionSalida = $"LatLng(lat: {consultaUbicacion.Latitud}, lng:{consultaUbicacion.Longitud})",
                    UsuarioCreacionId = asistencia.UsuarioCreacionId
                };
                _context.Add(datos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto",
                asistencia.UsuarioCreacionId
            );
            return View(asistencia);
        }

        // GET: Asistencias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistencia = await _context.Asistencias.FindAsync(id);
            if (asistencia == null)
            {
                return NotFound();
            }
            var usuarioCreacion = await _context.Set<ApplicationUser>()
    .Where(u => u.Id == asistencia.UsuarioCreacionId)
    .Select(u => new
    {
        u.Id,
        NombreCompleto = u.Nombre + " " + u.PrimerApellido
    })
    .FirstOrDefaultAsync();

            ViewData["UsuarioCreacionId"] = usuarioCreacion?.Id;
            ViewData["NombreCompletoUsuarioCreacion"] = usuarioCreacion?.NombreCompleto;



            return View(asistencia);
        }

        // POST: Asistencias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAsistencia,FechaEntrada,FechaSalida,UbicacionEntrada,UbicacionSalida,UsuarioCreacionId")] Asistencia asistencia)
        {
            if (id != asistencia.IdAsistencia)
            {
                return NotFound();
            }
            //ModelState.Remove("UsuarioCreacionId");
            //Asistencia datos = new Asistencia
            //{
            //    IdAsistencia = asistencia.IdAsistencia,
            //    FechaEntrada = asistencia.FechaEntrada,
            //    FechaSalida = asistencia.FechaSalida,
            //    UbicacionEntrada = asistencia.UbicacionEntrada,
            //    UbicacionSalida = asistencia.UbicacionSalida
            //};
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asistencia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsistenciaExists(asistencia.IdAsistencia))
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
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto",
                asistencia.UsuarioCreacionId
            );
            return View(asistencia);
        }

        // GET: Asistencias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistencia = await _context.Asistencias
                .Include(a => a.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdAsistencia == id);
            if (asistencia == null)
            {
                return NotFound();
            }

            return View(asistencia);
        }

        // POST: Asistencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdAsistencia)
        {
            var asistencia = await _context.Asistencias.FindAsync(IdAsistencia);
            if (asistencia != null)
            {
                _context.Asistencias.Remove(asistencia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AsistenciaExists(int id)
        {
            return _context.Asistencias.Any(e => e.IdAsistencia == id);
        }

        public async Task<IActionResult> ExportToExcel(string nombre, DateTime? fechaIngreso, DateTime? fechaSalida)
        {
            try
            {
                var asistencias = await _context.Asistencias
                    .Include(a => a.UsuarioCreacion)
                    .Where(a =>
                        (string.IsNullOrWhiteSpace(nombre) || a.UsuarioCreacion.Nombre.Contains(nombre)) &&
                        (!fechaIngreso.HasValue || a.FechaEntrada.Date >= fechaIngreso.Value.Date) &&
                        (!fechaSalida.HasValue || a.FechaSalida.Date <= fechaSalida.Value.Date))
                    .OrderByDescending(a => a.FechaEntrada)
                    .Select(a => new AsistenciaModel
                    {
                        Id = a.IdAsistencia,
                        FechaEntrada = a.FechaEntrada,
                        FechaSalida = a.FechaSalida,
                        LatitudEntrada = ExtractLatitude(a.UbicacionEntrada),
                        LongitudEntrada = ExtractLongitude(a.UbicacionEntrada),
                        LatitudSalida = ExtractLatitude(a.UbicacionSalida),
                        LongitudSalida = ExtractLongitude(a.UbicacionSalida),
                        UsuarioCreacionId = a.UsuarioCreacionId,
                        UsuarioCreacion = a.UsuarioCreacion.UserName,
                        Nombre = a.UsuarioCreacion.Nombre,
                        PrimerApellido = a.UsuarioCreacion.PrimerApellido,
                        SegundoApellido = a.UsuarioCreacion.SegundoApellido
                    })
                    .ToListAsync();

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Reporte de Asistencias");

                    // Configuración de página
                    worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                    worksheet.PageSetup.FitToPages(1, 1);

                    // Agregar logos
                    var imagePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Empana-tica_Logo.png");
                    var imagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pyme.png");

                    if (System.IO.File.Exists(imagePath1))
                    {
                        worksheet.AddPicture(imagePath1).MoveTo(worksheet.Cell("A1")).Scale(0.15);
                    }
                    if (System.IO.File.Exists(imagePath2))
                    {
                        worksheet.AddPicture(imagePath2).MoveTo(worksheet.Cell("G1")).Scale(0.1);
                    }

                    // Configurar altura y ancho inicial
                    worksheet.Row(1).Height = 60;
                    worksheet.Column(1).Width = 15;
                    worksheet.Column(7).Width = 15;

                    // Título del reporte con formato
                    var titleRange = worksheet.Range("A3:G3").Merge();
                    titleRange.Value = "Reporte de Control de Asistencias";
                    titleRange.Style
                        .Font.SetBold(true)
                        .Font.SetFontSize(16)
                        .Fill.SetBackgroundColor(XLColor.FromHtml("#4472C4"))
                        .Font.SetFontColor(XLColor.White)
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    // Información del filtrado
                    var filterRow = 4;
                    if (!string.IsNullOrEmpty(nombre) || fechaIngreso.HasValue || fechaSalida.HasValue)
                    {
                        worksheet.Cell($"A{filterRow}").Value = "Filtros aplicados:";
                        worksheet.Cell($"A{filterRow}").Style.Font.Bold = true;

                        var filterText = "";
                        if (!string.IsNullOrEmpty(nombre)) filterText += $"Nombre: {nombre}, ";
                        if (fechaIngreso.HasValue) filterText += $"Desde: {fechaIngreso.Value:dd/MM/yyyy}, ";
                        if (fechaSalida.HasValue) filterText += $"Hasta: {fechaSalida.Value:dd/MM/yyyy}";

                        worksheet.Cell($"B{filterRow}").Value = filterText.TrimEnd(',', ' ');
                        filterRow++;
                    }

                    // Cabeceras de la tabla
                    var headers = new[] {
                "Fecha de Entrada", "Hora de Entrada",
                "Fecha de Salida", "Hora de Salida",
                "Nombre Completo", "Usuario",
                "Ubicación Entrada", "Ubicación Salida"
            };

                    var headerRow = worksheet.Range($"A{filterRow + 1}:H{filterRow + 1}");
                    for (int i = 0; i < headers.Length; i++)
                    {
                        headerRow.Cell(1, i + 1).Value = headers[i];
                    }

                    headerRow.Style
                        .Font.SetBold(true)
                        .Font.SetFontSize(12)
                        .Fill.SetBackgroundColor(XLColor.FromHtml("#D9E1F2"))
                        .Font.SetFontColor(XLColor.Black)
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    // Datos
                    int rowIdx = filterRow + 2;
                    foreach (var asistencia in asistencias)
                    {
                        var row = worksheet.Row(rowIdx);

                        // Fecha y hora separadas para mejor lectura
                        row.Cell(1).Value = asistencia.FechaEntrada.ToString("dd/MM/yyyy");
                        row.Cell(2).Value = asistencia.FechaEntrada.ToString("HH:mm:ss");
                        row.Cell(3).Value = asistencia.FechaSalida.ToString("dd/MM/yyyy") ?? "-";
                        row.Cell(4).Value = asistencia.FechaSalida.ToString("HH:mm:ss") ?? "-";

                        // Nombre completo formateado
                        row.Cell(5).Value = $"{asistencia.Nombre} {asistencia.PrimerApellido} {asistencia.SegundoApellido}".Trim();
                        row.Cell(6).Value = asistencia.UsuarioCreacion;

                        // Ubicaciones formateadas
                        row.Cell(7).Value = $"{asistencia.LatitudEntrada}, {asistencia.LongitudEntrada}";
                        row.Cell(8).Value = asistencia.LatitudSalida.HasValue ?
                            $"{asistencia.LatitudSalida}, {asistencia.LongitudSalida}" : "-";

                        rowIdx++;
                    }

                    // Estilo de la tabla
                    var tableRange = worksheet.Range($"A{filterRow + 1}:H{rowIdx - 1}");
                    tableRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    tableRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                    tableRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    // Alternar colores de filas para mejor lectura
                    for (int i = filterRow + 2; i < rowIdx; i++)
                    {
                        if (i % 2 == 0)
                        {
                            worksheet.Range($"A{i}:H{i}").Style
                                .Fill.SetBackgroundColor(XLColor.FromHtml("#F2F2F2"));
                        }
                    }

                    // Ajustar el ancho de las columnas
                    worksheet.Columns().AdjustToContents();

                    // Generar el archivo
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        string fileName = $"Control_Asistencias_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            fileName
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al generar el archivo Excel: {ex.Message}");
            }
        }


    }
}
