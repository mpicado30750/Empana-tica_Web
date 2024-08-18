using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClosedXML.Excel;
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
			
			var asistencias = await _context.Asistencias
                .Include(a => a.UsuarioCreacion)
                .ToListAsync();

            var viewModel = asistencias.Select(a => new AsistenciaModel
            {

                Id = a.IdAsistencia,
                FechaEntrada = a.FechaEntrada,
                FechaSalida = a.FechaSalida,
                LatitudEntrada = ExtractLatitude(a.UbicacionEntrada),
                LongitudEntrada = ExtractLongitude(a.UbicacionEntrada), // Manejo de valor null
                LatitudSalida = ExtractLatitude(a.UbicacionSalida),
                LongitudSalida = ExtractLongitude(a.UbicacionSalida), // Manejo de valor null
                UsuarioCreacionId = a.UsuarioCreacionId,
                UsuarioCreacion = a.UsuarioCreacion.UserName,
                Nombre = a.UsuarioCreacion.Nombre,
                PrimerApellido = a.UsuarioCreacion.PrimerApellido,
                SegundoApellido = a.UsuarioCreacion.SegundoApellido
			}).ToList();

            return View(viewModel);
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
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                 "Id",
                 "NombreCompleto",
                 asistencia.UsuarioCreacionId
             );
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

        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                Console.WriteLine("Inicio del método ExportToExcel");

                var asistencias = await _context.Asistencias
                    .Include(a => a.UsuarioCreacion)
                    .ToListAsync();
                Console.WriteLine($"Cantidad de asistencias obtenidas: {asistencias.Count}");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Asistencias");

                    worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                    Console.WriteLine("Orientación de página establecida a paisaje");

                    // Verificar si las imágenes existen antes de agregarlas
                    var imagePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Empana-tica_Logo.png");
                    var imagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pyme.png");

                    Console.WriteLine($"Ruta de imagen 1: {imagePath1}");
                    Console.WriteLine($"Ruta de imagen 2: {imagePath2}");

                    if (System.IO.File.Exists(imagePath1))
                    {
                        var picture1 = worksheet.AddPicture(imagePath1).MoveTo(worksheet.Cell("A1")).Scale(0.15);
                        Console.WriteLine("Imagen 1 agregada al Excel");
                    }
                    else
                    {
                        Console.WriteLine("Imagen 1 no encontrada");
                    }

                    if (System.IO.File.Exists(imagePath2))
                    {
                        var picture2 = worksheet.AddPicture(imagePath2).MoveTo(worksheet.Cell("G1")).Scale(0.1);
                        Console.WriteLine("Imagen 2 agregada al Excel");
                    }
                    else
                    {
                        Console.WriteLine("Imagen 2 no encontrada");
                    }

                    // Ajustar las celdas para las imágenes
                    worksheet.Row(1).Height = 60;
                    worksheet.Column(1).Width = 12;
                    worksheet.Column(7).Width = 12;
                    Console.WriteLine("Celdas ajustadas para las imágenes");

                    // Título
                    var titleCell = worksheet.Cell("A3");
                    titleCell.Value = "Informe de Asistencias";
                    titleCell.Style.Font.Bold = true;
                    titleCell.Style.Font.FontSize = 16;
                    titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    titleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
                    titleCell.Style.Font.FontColor = XLColor.White;
                    Console.WriteLine("Título del informe configurado");

                    // Cabeceras de la tabla
                    var headerRow = worksheet.Row(5);
                    headerRow.Cell(1).Value = "Fecha de Entrada";
                    headerRow.Cell(2).Value = "Fecha de Salida";
                    headerRow.Cell(3).Value = "Nombre";
                    headerRow.Cell(4).Value = "Primer Apellido";
                    headerRow.Cell(5).Value = "Segundo Apellido";
                    headerRow.Style.Font.Bold = true;
                    headerRow.Style.Font.FontSize = 12;
                    headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerRow.Style.Font.FontColor = XLColor.White;
                    Console.WriteLine("Cabeceras de la tabla configuradas");

                    // Datos
                    int rowIdx = 6;
                    foreach (var asistencia in asistencias)
                    {
                        var dataRow = worksheet.Row(rowIdx);
                        dataRow.Cell(1).Value = asistencia.FechaEntrada.ToString("yyyy-MM-dd HH:mm:ss");
                        dataRow.Cell(2).Value = asistencia.FechaSalida.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
                        dataRow.Cell(3).Value = asistencia.UsuarioCreacion.Nombre;
                        dataRow.Cell(4).Value = asistencia.UsuarioCreacion.PrimerApellido;
                        dataRow.Cell(5).Value = asistencia.UsuarioCreacion.SegundoApellido;
                        rowIdx++;
                    }
                    Console.WriteLine("Datos de asistencias agregados al Excel");

                    // Establecer el estilo de tabla para los datos
                    var tableRange = worksheet.Range("A5:E" + rowIdx);
                    var table = tableRange.CreateTable();
                    table.Theme = XLTableTheme.TableStyleMedium2;
                    Console.WriteLine("Estilo de tabla establecido");

                    // Ajustar el ancho de las columnas después de agregar los datos
                    worksheet.Columns().AdjustToContents();
                    Console.WriteLine("Columnas ajustadas al contenido");

                    // Guardar el archivo Excel en un MemoryStream y devolver como FileResult
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        Console.WriteLine("Archivo Excel guardado en memoria");

                        // Agregar la fecha al nombre del archivo
                        string fileName = $"Asistencias_{DateTime.Now:ddMMyyyy}.xlsx";

                        // Devolver el archivo como un archivo descargable
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                // Capturar la excepción y mostrar un mensaje de error
                Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
                return Content($"Ocurrió un error al generar el archivo Excel: {ex.Message}\n{ex.StackTrace}");
            }
        }


    }
}
