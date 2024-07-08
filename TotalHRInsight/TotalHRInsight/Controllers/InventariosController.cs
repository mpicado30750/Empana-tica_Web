using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;

namespace TotalHRInsight.Controllers
{
    public class InventariosController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public InventariosController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: Inventarios
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.Inventario
                .Include(i => i.Producto)
                .Include(i => i.Sucursal)
                .Include(i => i.UsuarioCreacion)
                .Include(i => i.UsuarioModificacion);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: Inventarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventario
                .Include(i => i.Producto)
                .Include(i => i.Sucursal)
                .Include(i => i.UsuarioCreacion)
                .Include(i => i.UsuarioModificacion)
                .FirstOrDefaultAsync(m => m.IdInventario == id);
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
            ViewData["UsuarioCreacionid"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto"
            );
            ViewData["UsuarioModificacionid"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto"
            );
            return View();
        }

        // POST: Inventarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdInventario,UsuarioCreacionid,UsuarioModificacionid,FechaCreacion,FechaModificacion,CantidadDisponible,SucursalId,ProductoId")] Inventario inventario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "IdProducto", "Descripcion", inventario.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", inventario.SucursalId);
            ViewData["UsuarioCreacionid"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto",
                inventario.UsuarioCreacionid
            );
            ViewData["UsuarioModificacionid"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto",
                inventario.UsuarioModificacionid
            );
            return View(inventario);
        }

        // GET: Inventarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventario.FindAsync(id);
            if (inventario == null)
            {
                return NotFound();
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "IdProducto", "NombreProducto", inventario.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", inventario.SucursalId);
            ViewData["UsuarioCreacionid"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto",
                inventario.UsuarioCreacionid
            );
            ViewData["UsuarioModificacionid"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto",
                inventario.UsuarioModificacionid
            );
            return View(inventario);
        }

        // POST: Inventarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdInventario,UsuarioCreacionid,UsuarioModificacionid,FechaCreacion,FechaModificacion,CantidadDisponible,SucursalId,ProductoId")] Inventario inventario)
        {
            if (id != inventario.IdInventario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventarioExists(inventario.IdInventario))
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
            ViewData["ProductoId"] = new SelectList(_context.Productos, "IdProducto", "Descripcion", inventario.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", inventario.SucursalId);
            ViewData["UsuarioCreacionid"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto",
                inventario.UsuarioCreacionid
            );
            ViewData["UsuarioModificacionid"] = new SelectList(
                _context.Set<ApplicationUser>().Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.PrimerApellido }),
                "Id",
                "NombreCompleto",
                inventario.UsuarioModificacionid
            );
            return View(inventario);
        }

        // GET: Inventarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventario
                .Include(i => i.Producto)
                .Include(i => i.Sucursal)
                .Include(i => i.UsuarioCreacion)
                .Include(i => i.UsuarioModificacion)
                .FirstOrDefaultAsync(m => m.IdInventario == id);
            if (inventario == null)
            {
                return NotFound();
            }

            return View(inventario);
        }

        // POST: Inventarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventario = await _context.Inventario.FindAsync(id);
            if (inventario != null)
            {
                _context.Inventario.Remove(inventario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventarioExists(int id)
        {
            return _context.Inventario.Any(e => e.IdInventario == id);
        }
        // GET: Inventarios/ExportToExcel
        public async Task<IActionResult> ExportToExcel()
        {
            var inventarios = await _context.Inventario
                .Include(i => i.Producto)
                .Include(i => i.Sucursal)
                .Include(i => i.UsuarioCreacion)
                .Include(i => i.UsuarioModificacion)
                .ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Inventarios");

                // Agregar imágenes y ajustar tamaño
                var imagePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Empana-tica_Logo.png");
                var imagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pyme.png");
                var picture1 = worksheet.AddPicture(imagePath1).MoveTo(worksheet.Cell("A1")).Scale(0.15);
                var picture2 = worksheet.AddPicture(imagePath2).MoveTo(worksheet.Cell("G1")).Scale(0.1);

                // Ajustar celdas para las imágenes
                worksheet.Row(1).Height = 60;
                worksheet.Column(1).Width = 12;
                worksheet.Column(7).Width = 12;

                // Título
                var titleCell = worksheet.Cell("A3");
                titleCell.Value = "Informe de Inventarios";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4"); // Color de fondo azul de Excel
                titleCell.Style.Font.FontColor = XLColor.White;

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

                // Establecer estilo de tabla para los datos
                var tableRange = worksheet.Range("A5:H" + rowIdx);
                var table = tableRange.CreateTable();

                // Establecer estilo de tabla (opcional)
                table.Theme = XLTableTheme.TableStyleMedium2;

                // Ajustar el ancho de las columnas después de agregar los datos
                worksheet.Columns().AdjustToContents();

                // Guardar el archivo Excel en un MemoryStream y devolver como FileResult
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Inventarios.xlsx");
                }
            }
        }
    }
}
