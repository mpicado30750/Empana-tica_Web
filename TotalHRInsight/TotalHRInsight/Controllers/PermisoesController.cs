using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace TotalHRInsight.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PermisoesController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public PermisoesController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: Permisoes
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.Permisos
                .Include(p => p.TipoPermisos)
                .Include(p => p.UsuarioAsignacion)
                .Include(p => p.UsuarioCreacion)
                .Include(p => p.Estado);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: Permisoes/Details/5
        public async Task<IActionResult> Details(int? IdPermisos)
        {
            if (IdPermisos == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos
                .Include(p => p.TipoPermisos)
                .Include(p => p.UsuarioAsignacion)
                .Include(p => p.UsuarioCreacion)
                .Include(p => p.Estado)
                .FirstOrDefaultAsync(m => m.IdPermisos == IdPermisos);
            if (permiso == null)
            {
                return NotFound();
            }

            return View(permiso);
        }

        // GET: Permisoes/Create
        public IActionResult Create()
        {
            ViewData["IdTipoPermiso"] = new SelectList(_context.TipoPermisos, "IdTipoPermiso", "NombrePermiso");
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "EstadoSolicitud");
            ViewData["UsuarioAsignacionId"] = new SelectList(
               _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
               "Id",
               "NombreCompleto"
           );
            ViewData["UsuarioCreacionid"] = new SelectList(
               _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
               "Id",
               "NombreCompleto"
           );
            return View();
        }

        // POST: Permisoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Permiso permiso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(permiso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTipoPermiso"] = new SelectList(_context.TipoPermisos, "IdTipoPermiso", "NombrePermiso", permiso.IdTipoPermiso);
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "EstadoSolicitud");
            ViewData["UsuarioAsignacionId"] = new SelectList(
            _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
            "Id",
            "NombreCompleto"
        );
            ViewData["UsuarioCreacionid"] = new SelectList(
               _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
               "Id",
               "NombreCompleto"
           );
            return View(permiso);
        }

        // GET: Permisoes/Edit/5
        public async Task<IActionResult> Edit(int? IdPermisos)
        {
            if (IdPermisos == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos.FindAsync(IdPermisos);
            if (permiso == null)
            {
                return NotFound();
            }
            ViewData["IdTipoPermiso"] = new SelectList(_context.TipoPermisos, "IdTipoPermiso", "NombrePermiso", permiso.IdTipoPermiso);
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "EstadoSolicitud");
            ViewData["UsuarioAsignacionId"] = new SelectList(
             _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
             "Id",
             "NombreCompleto"
         );
            ViewData["UsuarioCreacionid"] = new SelectList(
               _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
               "Id",
               "NombreCompleto"
           );
            return View(permiso);
        }

        // POST: Permisoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int IdPermisos, Permiso permiso)
        {
            if (IdPermisos != permiso.IdPermisos)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permiso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermisoExists(permiso.IdPermisos))
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
            ViewData["IdTipoPermiso"] = new SelectList(_context.TipoPermisos, "IdTipoPermiso", "NombrePermiso", permiso.IdTipoPermiso);
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "EstadoSolicitud");
            ViewData["UsuarioAsignacionId"] = new SelectList(
              _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
              "Id",
              "NombreCompleto"
          );
            ViewData["UsuarioCreacionid"] = new SelectList(
               _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
               "Id",
               "NombreCompleto"
           );
            return View(permiso);
        }

        // GET: Permisoes/Delete/5
        public async Task<IActionResult> Delete(int? IdPermisos)
        {
            if (IdPermisos == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos
                .Include(p => p.TipoPermisos)
                .Include(p => p.UsuarioAsignacion)
                .Include(p => p.UsuarioCreacion)
                .Include(p => p.Estado)
                .FirstOrDefaultAsync(m => m.IdPermisos == IdPermisos);
            if (permiso == null)
            {
                return NotFound();
            }

            return View(permiso);
        }

        // POST: Permisoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdPermisos)
        {
            var permiso = await _context.Permisos.FindAsync(IdPermisos);
            if (permiso != null)
            { 
                _context.Permisos.Remove(permiso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermisoExists(int IdPermisos)
        {
            return _context.Permisos.Any(e => e.IdPermisos == IdPermisos);
        }

        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                Console.WriteLine("Inicio del método ExportToExcel");

                var permisos = await _context.Permisos
                    .Include(p => p.TipoPermisos)
                    .Include(p => p.UsuarioAsignacion)
                    .Include(p => p.UsuarioCreacion)
                    .Include(p => p.Estado)
                    .ToListAsync();
                Console.WriteLine($"Cantidad de permisos obtenidos: {permisos.Count}");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Permisos");
                    worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                    Console.WriteLine("Orientación de página establecida a paisaje");

                    //// Agregar imágenes y ajustar tamaño
                    //var imagePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Empana-tica_Logo.png");
                    //var imagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pyme.png");

                    //Console.WriteLine($"Ruta de imagen 1: {imagePath1}");
                    //Console.WriteLine($"Ruta de imagen 2: {imagePath2}");

                    //if (System.IO.File.Exists(imagePath1))
                    //{
                    //    var picture1 = worksheet.AddPicture(imagePath1).MoveTo(worksheet.Cell("A1")).Scale(0.15);
                    //    Console.WriteLine("Imagen 1 agregada al Excel");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Imagen 1 no encontrada");
                    //}

                    //if (System.IO.File.Exists(imagePath2))
                    //{
                    //    var picture2 = worksheet.AddPicture(imagePath2).MoveTo(worksheet.Cell("G1")).Scale(0.1);
                    //    Console.WriteLine("Imagen 2 agregada al Excel");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Imagen 2 no encontrada");
                    //}

                    // Ajustar celdas para las imágenes
                    worksheet.Row(1).Height = 60;
                    worksheet.Column(1).Width = 12;
                    worksheet.Column(7).Width = 12;
                    Console.WriteLine("Celdas ajustadas para las imágenes");

                    // Título
                    var titleCell = worksheet.Cell("A3");
                    titleCell.Value = "Informe de Permisos";
                    titleCell.Style.Font.Bold = true;
                    titleCell.Style.Font.FontSize = 16;
                    titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    titleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
                    titleCell.Style.Font.FontColor = XLColor.White;
                    Console.WriteLine("Título del informe configurado");

                    // Cabeceras de la tabla
                    var headerRow = worksheet.Row(5);
                    headerRow.Cell(1).Value = "IdPermiso";
                    headerRow.Cell(2).Value = "Tipo de Permiso";
                    headerRow.Cell(3).Value = "Usuario Asignado";
                    headerRow.Cell(4).Value = "Usuario Creación";
                    headerRow.Cell(5).Value = "Fecha Creación";
                    headerRow.Cell(6).Value = "Fecha Modificación";
                    headerRow.Cell(7).Value = "Estado";
                    headerRow.Style.Font.Bold = true;
                    headerRow.Style.Font.FontSize = 12;
                    headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerRow.Style.Font.FontColor = XLColor.White;
                    Console.WriteLine("Cabeceras de la tabla configuradas");

                    // Datos
                    int rowIdx = 6;
                    foreach (var permiso in permisos)
                    {
                        var dataRow = worksheet.Row(rowIdx);
                        dataRow.Cell(1).Value = permiso.IdPermisos;
                        dataRow.Cell(2).Value = permiso.TipoPermisos.NombrePermiso;
                        dataRow.Cell(3).Value = permiso.UsuarioAsignacion.Nombre + " " + permiso.UsuarioAsignacion.PrimerApellido;
                        dataRow.Cell(4).Value = permiso.UsuarioCreacion.Nombre + " " + permiso.UsuarioCreacion.PrimerApellido;
                        dataRow.Cell(5).Value = permiso.FechaInicio.ToString("dd-MM-yyyy");
                        dataRow.Cell(6).Value = permiso.FechaFin.ToString("dd-MM-yyyy") ?? string.Empty;
                        dataRow.Cell(7).Value = permiso.Estado.EstadoSolicitud;
                        rowIdx++;
                    }
                    Console.WriteLine("Datos de permisos agregados al Excel");

                    // Establecer estilo de tabla para los datos
                    var tableRange = worksheet.Range("A5:G" + rowIdx);
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
                        string fileName = $"Permisos_{DateTime.Now:ddMMyyyy}.xlsx";
                        Console.WriteLine("Archivo Excel guardado en memoria");

                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, "Ocurrió un error al generar el archivo Excel.");
            }
        }



    }
}
