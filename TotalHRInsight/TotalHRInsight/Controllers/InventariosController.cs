using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using TotalHRInsight.Models.Inventario;

namespace TotalHRInsight.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class InventariosController : Controller
    {
        private readonly TotalHRInsightDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public InventariosController(TotalHRInsightDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Inventarios
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.Sucursales;
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        public async Task<IActionResult> DetalleSucursal(int? idSucursal)
        {
            var inventario = await _context.Inventario
                .Where(w => w.SucursalId == idSucursal)
                .Include(i => i.Producto)
                .Include(i => i.Sucursal)
                .Include(i => i.UsuarioCreacion)
                .Include(i => i.UsuarioModificacion)
                .ToListAsync();

            if (inventario == null)
            {
                return NotFound();
            }

            var nombreSucursal = await _context.Sucursales
                .FirstOrDefaultAsync(m => m.IdSucursal == idSucursal);
            ViewData["SucursalNombre"] = nombreSucursal.NombreSucursal;

            return View(inventario);
        }

        // GET: Inventarios/Details/5
        public async Task<IActionResult> Details(int? IdInventario)
        {
            if (IdInventario == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventario
                .Include(i => i.Producto)
                .Include(i => i.Sucursal)
                .Include(i => i.UsuarioCreacion)
                .Include(i => i.UsuarioModificacion)
                .FirstOrDefaultAsync(m => m.IdInventario == IdInventario);
            if (inventario == null)
            {
                return NotFound();
            }

            return View(inventario);
        }

        // GET: Inventarios/Create
        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.Productos, "IdProducto", "NombreProducto");
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal");
            return View();
        }

        // POST: Inventarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearInventario datos)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                var inventario = new Inventario
                {
                    UsuarioModificacionid = user.Id,
                    UsuarioCreacionid = user.Id,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now,
                    CantidadDisponible = datos.CantidadDisponible,
                    SucursalId = datos.SucursalId,
                    ProductoId = datos.ProductoId
                };

                _context.Add(inventario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "IdProducto", "NombreProducto", datos.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", datos.SucursalId);
            return View(datos);
        }

        // GET: Inventarios/Edit/5
        public async Task<IActionResult> Edit(int? IdInventario)
        {
            if (IdInventario == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventario.FindAsync(IdInventario);
            if (inventario == null)
            {
                return NotFound();
            }

            var editarInventario = new EditarInventario
            {
                IdInventario = inventario.IdInventario,
                ProductoId = inventario.ProductoId,
                SucursalId = inventario.SucursalId,
                CantidadDisponible = inventario.CantidadDisponible
            };

            ViewData["ProductoId"] = new SelectList(_context.Productos, "IdProducto", "NombreProducto", inventario.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", inventario.SucursalId);

            return View(editarInventario);
        }

        // POST: Inventarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int IdInventario, EditarInventario editarInventario)
        {
            if (IdInventario != editarInventario.IdInventario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var inventario = await _context.Inventario.FindAsync(IdInventario);
                    if (inventario == null)
                    {
                        return NotFound();
                    }

                    var user = await GetCurrentUserAsync();

                    inventario.ProductoId = editarInventario.ProductoId;
                    inventario.SucursalId = editarInventario.SucursalId;
                    inventario.CantidadDisponible = editarInventario.CantidadDisponible;
                    inventario.UsuarioModificacionid = user.Id;
                    inventario.FechaModificacion = DateTime.Now;

                    _context.Update(inventario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventarioExists(editarInventario.IdInventario))
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

            ViewData["ProductoId"] = new SelectList(_context.Productos, "IdProducto", "NombreProducto", editarInventario.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", editarInventario.SucursalId);

            return View(editarInventario);
        }

        // GET: Inventarios/Delete/5
        public async Task<IActionResult> Delete(int? IdInventario)
        {
            if (IdInventario == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventario
                .Include(i => i.Producto)
                .Include(i => i.Sucursal)
                .Include(i => i.UsuarioCreacion)
                .Include(i => i.UsuarioModificacion)
                .FirstOrDefaultAsync(m => m.IdInventario == IdInventario);
            if (inventario == null)
            {
                return NotFound();
            }

            return View(inventario);
        }

        // POST: Inventarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdInventario)
        {
            var inventario = await _context.Inventario.FindAsync(IdInventario);
            if (inventario != null)
            {
                _context.Inventario.Remove(inventario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventarioExists(int IdInventario)
        {
            return _context.Inventario.Any(e => e.IdInventario == IdInventario);
        }

        // GET: Inventarios/ExportToExcel
        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                Console.WriteLine("Inicio del método ExportToExcel");

                var inventarios = await _context.Inventario
                    .Include(i => i.Producto)
                    .Include(i => i.Sucursal)
                    .Include(i => i.UsuarioCreacion)
                    .Include(i => i.UsuarioModificacion)
                    .ToListAsync();
                Console.WriteLine($"Cantidad de inventarios obtenidos: {inventarios.Count}");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Inventarios");
                    worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                    Console.WriteLine("Orientación de página establecida a paisaje");

                    // Agregar imágenes y ajustar tamaño
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

                    //// Ajustar celdas para las imágenes
                    //worksheet.Row(1).Height = 60;
                    //worksheet.Column(1).Width = 12;
                    //worksheet.Column(7).Width = 12;
                    //Console.WriteLine("Celdas ajustadas para las imágenes");

                    // Título
                    var titleCell = worksheet.Cell("A3");
                    titleCell.Value = "Informe de Inventarios";
                    titleCell.Style.Font.Bold = true;
                    titleCell.Style.Font.FontSize = 16;
                    titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    titleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
                    titleCell.Style.Font.FontColor = XLColor.White;
                    Console.WriteLine("Título del informe configurado");

                    // Cabeceras de la tabla
                    var headerRow = worksheet.Row(5);
                    headerRow.Cell(1).Value = "IdInventario";
                    headerRow.Cell(2).Value = "FechaCreacion";
                    headerRow.Cell(3).Value = "FechaModificacion";
                    headerRow.Cell(4).Value = "UsuarioCreacion";
                    headerRow.Cell(5).Value = "UsuarioModificacion";
                    headerRow.Cell(6).Value = "CantidadDisponible";
                    headerRow.Cell(7).Value = "Sucursal";
                    headerRow.Cell(8).Value = "Producto";
                    headerRow.Style.Font.Bold = true;
                    headerRow.Style.Font.FontSize = 12;
                    headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerRow.Style.Font.FontColor = XLColor.White;
                    Console.WriteLine("Cabeceras de la tabla configuradas");

                    // Datos
                    int rowIdx = 6;
                    foreach (var inventario in inventarios)
                    {
                        var dataRow = worksheet.Row(rowIdx);
                        dataRow.Cell(1).Value = inventario.IdInventario;
                        dataRow.Cell(2).Value = inventario.FechaCreacion.ToString("dd-MM-yyyy");
                        dataRow.Cell(3).Value = inventario.FechaModificacion.ToString("dd-MM-yyyy") ?? string.Empty;
                        dataRow.Cell(4).Value = inventario.UsuarioCreacion.Nombre + " " + inventario.UsuarioCreacion.PrimerApellido;
                        dataRow.Cell(5).Value = inventario.UsuarioModificacion.Nombre + " " + inventario.UsuarioModificacion.PrimerApellido;
                        dataRow.Cell(6).Value = inventario.CantidadDisponible;
                        dataRow.Cell(7).Value = inventario.Sucursal.NombreSucursal;
                        dataRow.Cell(8).Value = inventario.Producto.NombreProducto;
                        rowIdx++;
                    }
                    Console.WriteLine("Datos de inventarios agregados al Excel");

                    // Establecer estilo de tabla para los datos
                    var tableRange = worksheet.Range("A5:H" + rowIdx);
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
                        string fileName = $"InventarioGeneral_{DateTime.Now:ddMMyyyy}.xlsx";

                        // Devolver el archivo como un archivo descargable
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
                return Content($"Ocurrió un error al generar el archivo Excel: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public async Task<IActionResult> Export(int IdInventario)
        {
            try
            {
                Console.WriteLine($"Inicio del método Export con IdInventario: {IdInventario}");

                var inventario = await _context.Inventario
                    .Include(i => i.Producto)
                    .Include(i => i.Sucursal)
                    .Include(i => i.UsuarioCreacion)
                    .Include(i => i.UsuarioModificacion)
                    .Where(i => i.SucursalId == IdInventario)
                    .ToListAsync();
                Console.WriteLine($"Cantidad de elementos en el inventario obtenidos: {inventario.Count}");

                if (inventario == null || !inventario.Any())
                {
                    Console.WriteLine("Inventario no encontrado o vacío.");
                    return NotFound();
                }

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add($"Inventario_{IdInventario}");
                    worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                    Console.WriteLine("Orientación de página establecida a paisaje");

                    //// Agregar las imágenes y ajustar tamaño
                    //var imagePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Empana-tica_Logo.png");
                    //var imagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pyme.png");

                    //Console.WriteLine($"Ruta de imagen 1: {imagePath1}");
                    //Console.WriteLine($"Ruta de imagen 2: {imagePath2}");

                    //if (!System.IO.File.Exists(imagePath1) || !System.IO.File.Exists(imagePath2))
                    //{
                    //    Console.WriteLine("Una o más imágenes no se encuentran en la ubicación especificada.");
                    //    throw new FileNotFoundException("Una o más imágenes no se encuentran en la ubicación especificada.");
                    //}

                    //var picture1 = worksheet.AddPicture(imagePath1).MoveTo(worksheet.Cell("A1")).Scale(0.15);
                    //Console.WriteLine("Imagen 1 agregada al Excel");
                    //var picture2 = worksheet.AddPicture(imagePath2).MoveTo(worksheet.Cell("G1")).Scale(0.1);
                    //Console.WriteLine("Imagen 2 agregada al Excel");

                    //// Ajustar las celdas para las imágenes
                    //worksheet.Row(1).Height = 60;
                    //worksheet.Column(1).Width = 12;
                    //worksheet.Column(7).Width = 12;
                    //Console.WriteLine("Celdas ajustadas para las imágenes");

                    // Título
                    var titleCell = worksheet.Cell("A2");
                    titleCell.Value = $"Detalles del Inventario #{IdInventario}";
                    titleCell.Style.Font.Bold = true;
                    titleCell.Style.Font.FontSize = 16;
                    titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    titleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
                    titleCell.Style.Font.FontColor = XLColor.White;
                    worksheet.Range("A2:G2").Merge();
                    Console.WriteLine("Título del informe configurado");

                    // Información del inventario general
                    var firstInventario = inventario.First();
                    worksheet.Cell("A3").Value = "Fecha Creación:";
                    worksheet.Cell("B3").Value = firstInventario.FechaCreacion.ToString("dd-MM-yyyy");
                    worksheet.Cell("A4").Value = "Fecha Modificación:";
                    worksheet.Cell("B4").Value = firstInventario.FechaModificacion.ToString("dd-MM-yyyy") ?? "N/A";
                    worksheet.Cell("A5").Value = "Usuario Creación:";
                    worksheet.Cell("B5").Value = $"{firstInventario.UsuarioCreacion.Nombre} {firstInventario.UsuarioCreacion.PrimerApellido}";
                    worksheet.Cell("A6").Value = "Usuario Modificación:";
                    worksheet.Cell("B6").Value = firstInventario.UsuarioModificacion != null ? $"{firstInventario.UsuarioModificacion.Nombre} {firstInventario.UsuarioModificacion.PrimerApellido}" : "N/A";
                    worksheet.Cell("A7").Value = "Sucursal:";
                    worksheet.Cell("B7").Value = firstInventario.Sucursal.NombreSucursal;
                    Console.WriteLine("Información general del inventario configurada");

                    // Encabezado de la tabla de productos
                    var headers = new[] { "Producto", "Cantidad Disponible" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cell(9, i + 1).Value = headers[i];
                        worksheet.Cell(9, i + 1).Style.Font.Bold = true;
                        worksheet.Cell(9, i + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
                        worksheet.Cell(9, i + 1).Style.Font.FontColor = XLColor.White;
                    }
                    Console.WriteLine("Cabeceras de la tabla configuradas");

                    // Información de los productos en el inventario
                    int row = 10;
                    foreach (var item in inventario)
                    {
                        worksheet.Cell(row, 1).Value = item.Producto.NombreProducto;
                        worksheet.Cell(row, 2).Value = item.CantidadDisponible;
                        row++;
                    }
                    Console.WriteLine("Datos de los productos agregados al Excel");

                    worksheet.Columns().AdjustToContents();
                    Console.WriteLine("Columnas ajustadas al contenido");

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        Console.WriteLine("Archivo Excel guardado en memoria");

                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Inventario_{DateTime.Now:ddMMyyyy}.xlsx");
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error de archivo: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(404, "No se encontraron las imágenes necesarias para generar el archivo Excel.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, "Ocurrió un error al generar el archivo Excel.");
            }
        }

    }
}