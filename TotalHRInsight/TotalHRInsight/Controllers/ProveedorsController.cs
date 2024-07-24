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
    public class ProveedorsController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public ProveedorsController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: Proveedors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proveedor.ToListAsync());
        }

        // GET: Proveedors/Details/5
        public async Task<IActionResult> Details(int? IdProveedor)
        {
            if (IdProveedor == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedor
                .FirstOrDefaultAsync(m => m.IdProveedor == IdProveedor);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // GET: Proveedors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proveedors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        // GET: Proveedors/Edit/5
        public async Task<IActionResult> Edit(int? IdProveedor)
        {
            if (IdProveedor == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedor.FindAsync(IdProveedor);
            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int IdProveedor, [Bind("IdProveedor,NombreProveedor,Descripcion,Email,Telefono,MetodoPago")] Proveedor proveedor)
        {
            if (IdProveedor != proveedor.IdProveedor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorExists(proveedor.IdProveedor))
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
            return View(proveedor);
        }

        // GET: Proveedors/Delete/5
        public async Task<IActionResult> Delete(int? IdProveedor)
        {
            if (IdProveedor == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedor
                .FirstOrDefaultAsync(m => m.IdProveedor == IdProveedor);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // POST: Proveedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdProveedor)
        {
            var proveedor = await _context.Proveedor.FindAsync(IdProveedor);
            if (proveedor != null)
            {
                _context.Proveedor.Remove(proveedor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedorExists(int IdProveedor)
        {
            return _context.Proveedor.Any(e => e.IdProveedor == IdProveedor);
        }
        // GET: Proveedors/ExportToExcel
        public async Task<IActionResult> ExportToExcel()
        {
            var proveedores = await _context.Proveedor
                .ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Proveedores");
                worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;

                // Agregar imágenes y ajustar tamaño (si es necesario)
                // Ajustar celdas para las imágenes (si es necesario)

                // Título
                var titleCell = worksheet.Cell("A3");
                titleCell.Value = "Listado de Proveedores";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4"); // Color de fondo azul de Excel
                titleCell.Style.Font.FontColor = XLColor.White;

                // Cabeceras de la tabla
                var headerRow = worksheet.Row(5);
                headerRow.Cell(1).Value = "IdProveedor";
                headerRow.Cell(2).Value = "NombreProveedor";
                headerRow.Cell(3).Value = "Descripcion";
                headerRow.Cell(4).Value = "Email";
                headerRow.Cell(5).Value = "Telefono";
                headerRow.Cell(6).Value = "MetodoPago";
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Font.FontSize = 12;
                headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRow.Style.Font.FontColor = XLColor.White;

                // Datos
                int rowIdx = 6;
                foreach (var proveedor in proveedores)
                {
                    var dataRow = worksheet.Row(rowIdx);
                    dataRow.Cell(1).Value = proveedor.IdProveedor;
                    dataRow.Cell(2).Value = proveedor.NombreProveedor;
                    dataRow.Cell(3).Value = proveedor.Descripcion;
                    dataRow.Cell(4).Value = proveedor.Email;
                    dataRow.Cell(5).Value = proveedor.Telefono;
                    dataRow.Cell(6).Value = proveedor.MetodoPago ?? string.Empty;
                    rowIdx++;
                }

                // Establecer estilo de tabla para los datos
                var tableRange = worksheet.Range("A5:F" + rowIdx);
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
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Proveedores.xlsx");
                }
            }
        }
    }
}
