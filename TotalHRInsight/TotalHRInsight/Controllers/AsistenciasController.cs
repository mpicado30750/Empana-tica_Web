using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using TotalHRInsight.Models;

namespace TotalHRInsight.Controllers
{
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
                LatitudEntrada = ConvertirLatitud(a.UbicacionEntrada),
                LongitudEntrada = ConvertirLongitud(a.UbicacionEntrada) ?? 0.0, // Manejo de valor null
                LatitudSalida = ConvertirLatitud(a.UbicacionSalida),
                LongitudSalida = ConvertirLongitud(a.UbicacionSalida) ?? 0.0, // Manejo de valor null
                UsuarioCreacionId = a.UsuarioCreacionId,
                UsuarioCreacion = a.UsuarioCreacion.UserName,
                Nombre = a.UsuarioCreacion.Nombre,
                PrimerApellido = a.UsuarioCreacion.PrimerApellido,
                SegundoApellido = a.UsuarioCreacion.SegundoApellido
			}).ToList();

            return View(viewModel);
        }
 
       
        private static double ConvertirLatitud(string input)
        {
            int startIndex = input.IndexOf("lat:") + 4;
            int endIndex = input.IndexOf(",", startIndex);

            if (startIndex < 0 || endIndex < 0)
            {
                throw new ArgumentException("Formato de cadena no válido para latitud.");
            }

            string latStr = input.Substring(startIndex, endIndex - startIndex);
            return double.Parse(latStr, CultureInfo.InvariantCulture);
        }

        private static double? ConvertirLongitud(string input)
        {
            int startIndex = input.IndexOf("lng:") + 4;
            int endIndex = input.IndexOf(")", startIndex);

            if (startIndex < 0 || endIndex < 0)
            {
                return null; // Devuelve null si no se puede encontrar la longitud
            }

            string lngStr = input.Substring(startIndex, endIndex - startIndex);
            return double.Parse(lngStr, CultureInfo.InvariantCulture);
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
		public IActionResult Create()
        {
            ViewData["UsuarioCreacionid"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto"
            );
            return View();
        }

        // POST: Asistencias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAsistencia,FechaEntrada,FechaSalida,UbicacionEntrada,UbicacionSalida,UsuarioCreacionId")] Asistencia asistencia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asistencia);
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
            var asistencias = await _context.Asistencias
                .Include(a => a.UsuarioCreacion)
                .ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Asistencias");

                // Agregar las imágenes y ajustar tamaño
                var imagePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Empana-tica_Logo.png");
                var imagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pyme.png");
                var picture1 = worksheet.AddPicture(imagePath1).MoveTo(worksheet.Cell("A1")).Scale(0.15);
                var picture2 = worksheet.AddPicture(imagePath2).MoveTo(worksheet.Cell("G1")).Scale(0.1);

                // Ajustar las celdas para las imágenes
                worksheet.Row(1).Height = 60;
                worksheet.Column(1).Width = 12;
                worksheet.Column(7).Width = 12;

                // Título
                var titleCell = worksheet.Cell("A3");
                titleCell.Value = "Informe de Asistencias";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4"); // Color de fondo azul de Excel
                titleCell.Style.Font.FontColor = XLColor.White;

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

                // Establecer el estilo de tabla para los datos
                var tableRange = worksheet.Range("A5:E" + rowIdx);
                var table = tableRange.CreateTable();

                // Establecer estilo de tabla
                table.Theme = XLTableTheme.TableStyleMedium2; // Ejemplo de estilo de tabla

                // Ajustar el ancho de las columnas después de agregar los datos
                worksheet.Columns().AdjustToContents();

                // Guardar el archivo Excel en un MemoryStream y devolver como FileResult
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Asistencias.xlsx");
                }
            }
        }
    }
}
