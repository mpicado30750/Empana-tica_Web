using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;

namespace TotalHRInsight.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CierreCajasController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public CierreCajasController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: CierreCajas
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.CierreCajas.Include(c => c.Sucursal).Include(c => c.UsuarioCreacion);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: CierreCajas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cierreCaja = await _context.CierreCajas
                .Include(c => c.Sucursal)
                .Include(c => c.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdCierraCaja == id);
            if (cierreCaja == null)
            {
                return NotFound();
            }

            return View(cierreCaja);
        }

        // GET: CierreCajas/Create
        public IActionResult Create()
        {
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal");
            ViewData["UsuarioCreacionid"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto"
            );
            return View();
        }

        // POST: CierreCajas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCierraCaja,Fecha,SucursalId,MontoTotal,UsuarioCreacionId")] CierreCaja cierreCaja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cierreCaja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", cierreCaja.SucursalId);
            ViewData["UsuarioCreacionid"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto"
            );
            return View(cierreCaja);
        }

        // GET: CierreCajas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cierreCaja = await _context.CierreCajas.FindAsync(id);
            if (cierreCaja == null)
            {
                return NotFound();
            }
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", cierreCaja.SucursalId);
            ViewData["UsuarioCreacionid"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto"
            );
            return View(cierreCaja);
        }

        // POST: CierreCajas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCierraCaja,Fecha,SucursalId,MontoTotal,UsuarioCreacionId")] CierreCaja cierreCaja)
        {
            if (id != cierreCaja.IdCierraCaja)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cierreCaja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CierreCajaExists(cierreCaja.IdCierraCaja))
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
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", cierreCaja.SucursalId);
            ViewData["UsuarioCreacionid"] = new SelectList(
               _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
               "Id",
               "NombreCompleto"
           );
            return View(cierreCaja);
        }

        // GET: CierreCajas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cierreCaja = await _context.CierreCajas
                .Include(c => c.Sucursal)
                .Include(c => c.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdCierraCaja == id);
            if (cierreCaja == null)
            {
                return NotFound();
            }

            return View(cierreCaja);
        }

        // POST: CierreCajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cierreCaja = await _context.CierreCajas.FindAsync(id);
            if (cierreCaja != null)
            {
                _context.CierreCajas.Remove(cierreCaja);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CierreCajaExists(int id)
        {
            return _context.CierreCajas.Any(e => e.IdCierraCaja == id);
        }

        public async Task<IActionResult> ExportCierreCajaToExcel(int idCierreCaja)
        {
            try
            {
                Console.WriteLine($"Inicio del método ExportCierreCajaToExcel con idCierreCaja: {idCierreCaja}");

                var cierreCaja = await _context.CierreCajas
                    .Include(c => c.Sucursal)
                    .Include(c => c.UsuarioCreacion)
                    .FirstOrDefaultAsync(c => c.IdCierraCaja == idCierreCaja);

                if (cierreCaja == null)
                {
                    Console.WriteLine("Cierre de Caja no encontrado.");
                    return NotFound();
                }

                Console.WriteLine("Cierre de Caja encontrado.");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add($"CierreCaja_{idCierreCaja}");
                    worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                    Console.WriteLine("Orientación de página establecida a paisaje");

                    // Agregar las imágenes y ajustar tamaño
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
                    var titleCell = worksheet.Cell("A2");
                    titleCell.Value = $"Detalles del Cierre de Caja #{idCierreCaja}";
                    titleCell.Style.Font.Bold = true;
                    titleCell.Style.Font.FontSize = 16;
                    titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    titleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
                    titleCell.Style.Font.FontColor = XLColor.White;
                    worksheet.Range("A2:G2").Merge();
                    Console.WriteLine("Título del informe configurado");

                    // Información del cierre de caja
                    worksheet.Cell("A3").Value = "Fecha Cierre:";
                    worksheet.Cell("B3").Value = cierreCaja.Fecha.ToString("yyyy-MM-dd");
                    worksheet.Cell("A4").Value = "Sucursal:";
                    worksheet.Cell("B4").Value = cierreCaja.Sucursal.NombreSucursal;
                    worksheet.Cell("A5").Value = "Usuario Creación:";
                    worksheet.Cell("B5").Value = cierreCaja.UsuarioCreacion.Nombre + " " + cierreCaja.UsuarioCreacion.PrimerApellido;
                    worksheet.Cell("A6").Value = "Monto Total:";
                    worksheet.Cell("B6").Value = cierreCaja.MontoTotal;
                    worksheet.Cell("B6").Style.NumberFormat.Format = "₡ #,##0.00";
                    Console.WriteLine("Información del cierre de caja agregada al Excel");

                    // Ajustar el ancho de las columnas después de agregar los datos
                    worksheet.Columns().AdjustToContents();
                    Console.WriteLine("Columnas ajustadas al contenido");

                    // Guardar el archivo Excel en un MemoryStream y devolver como FileResult
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        Console.WriteLine("Archivo Excel guardado en memoria");

                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"CierreCaja_{DateTime.Now:ddMMyyyy}.xlsx");
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
