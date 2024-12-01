using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using TotalHRInsight.Models;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;

namespace TotalHRInsight.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class IngresosController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public IngresosController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: Ingresos
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.Ingresos.Include(i => i.CierreCaja).Include(i => i.TipoIngreso);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: Ingresos/Details/5
        public async Task<IActionResult> Details(int? IdIngreso)
        {
            if (IdIngreso == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos
                .Include(i => i.CierreCaja)
                .Include(i => i.TipoIngreso)
                .FirstOrDefaultAsync(m => m.IdIngreso == IdIngreso);
            if (ingreso == null)
            {
                return NotFound();
            }

            return View(ingreso);
        }

        // GET: Ingresos/Create
        public IActionResult Create()
        {
            ViewData["CierreId"] = new SelectList(_context.CierreCajas, "IdCierraCaja", "UsuarioCreacionId");
            ViewData["TipoIngresoId"] = new SelectList(_context.TipoIngresos, "IdTipoIngreso", "NombreIngreso");
            return View();
        }

        // POST: Ingresos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdIngreso,Fecha,TipoIngresoId,MontoIngreso,CierreId")] Ingreso ingreso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingreso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CierreId"] = new SelectList(_context.CierreCajas, "IdCierraCaja", "UsuarioCreacionId", ingreso.CierreId);
            ViewData["TipoIngresoId"] = new SelectList(_context.TipoIngresos, "IdTipoIngreso", "NombreIngreso", ingreso.TipoIngresoId);
            return View(ingreso);
        }

        // GET: Ingresos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos.FindAsync(id);
            if (ingreso == null)
            {
                return NotFound();
            }
            ViewData["CierreId"] = new SelectList(_context.CierreCajas, "IdCierraCaja", "UsuarioCreacionId", ingreso.CierreId);
            ViewData["TipoIngresoId"] = new SelectList(_context.TipoIngresos, "IdTipoIngreso", "NombreIngreso", ingreso.TipoIngresoId);
            return View(ingreso);
        }

        // POST: Ingresos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int IdIngreso, [Bind("IdIngreso,Fecha,TipoIngresoId,MontoIngreso,CierreId")] Ingreso ingreso)
        {
            if (IdIngreso != ingreso.IdIngreso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingreso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngresoExists(ingreso.IdIngreso))
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
            ViewData["CierreId"] = new SelectList(_context.CierreCajas, "IdCierraCaja", "UsuarioCreacionId", ingreso.CierreId);
            ViewData["TipoIngresoId"] = new SelectList(_context.TipoIngresos, "IdTipoIngreso", "NombreIngreso", ingreso.TipoIngresoId);
            return View(ingreso);
        }

        // GET: Ingresos/Delete/5
        public async Task<IActionResult> Delete(int? IdIngreso)
        {
            if (IdIngreso == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos
                .Include(i => i.CierreCaja)
                .Include(i => i.TipoIngreso)
                .FirstOrDefaultAsync(m => m.IdIngreso == IdIngreso);
            if (ingreso == null)
            {
                return NotFound();
            }

            return View(ingreso);
        }

        // POST: Ingresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdIngreso)
        {
            var ingreso = await _context.Ingresos.FindAsync(IdIngreso);
            if (ingreso != null)
            {
                _context.Ingresos.Remove(ingreso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngresoExists(int IdIngreso)
        {
            return _context.Ingresos.Any(e => e.IdIngreso == IdIngreso);
        }
        public async Task<IActionResult> ResumenFinanciero()
        {
            var totalIngresos = _context.Ingresos.Sum(i => i.MontoIngreso);
            var totalGastos = _context.Gastos.Sum(g => g.MontoGasto);
            var balance = totalIngresos - totalGastos;

            var viewModel = new ResumenFinancieroViewModel
            {
                TotalIngresos = totalIngresos,
                TotalGastos = totalGastos,
                Balance = balance
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerDatosFiltrados(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Asegurarse que endDate incluya todo el día
                endDate = endDate.AddHours(23).AddMinutes(59).AddSeconds(59);

                var totalIngresos = await _context.Ingresos
                    .Where(i => i.Fecha >= startDate && i.Fecha <= endDate)
                    .SumAsync(i => i.MontoIngreso);

                var totalGastos = await _context.Gastos
                    .Where(g => g.Fecha >= startDate && g.Fecha <= endDate)
                    .SumAsync(g => g.MontoGasto);

                var balance = totalIngresos - totalGastos;

                return Json(new
                {
                    totalIngresos,
                    totalGastos,
                    balance,
                    success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error al obtener los datos filtrados"
                });
            }
        }

        public IActionResult ExportarResumenFinanciero()
        {
            // Obtener los datos necesarios para los gráficos
            var totalIngresos = _context.Ingresos.Sum(i => i.MontoIngreso);
            var totalGastos = _context.Gastos.Sum(g => g.MontoGasto);
            var balance = totalIngresos - totalGastos;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Resumen Financiero");

                // Crear encabezados
                worksheet.Cells[1, 1].Value = "Concepto";
                worksheet.Cells[1, 2].Value = "Monto (₡)";

                // Estilo de los encabezados
                worksheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;
                worksheet.Cells[1, 1, 1, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Llenar los datos
                worksheet.Cells[2, 1].Value = "Ingresos";
                worksheet.Cells[2, 2].Value = totalIngresos;

                worksheet.Cells[3, 1].Value = "Gastos";
                worksheet.Cells[3, 2].Value = totalGastos;

                worksheet.Cells[4, 1].Value = "Balance";
                worksheet.Cells[4, 2].Value = balance;

                // Formato de moneda
                worksheet.Column(2).Style.Numberformat.Format = "₡ #,##0.00";

                // Ajustar el ancho de las columnas
                worksheet.Cells.AutoFitColumns();

                // Crear un gráfico de barras
                var barChart = worksheet.Drawings.AddChart("BarChart", eChartType.BarClustered);
                barChart.SetPosition(5, 0, 0, 0);
                barChart.SetSize(500, 300);
                var barSeries = barChart.Series.Add(worksheet.Cells["B2:B4"], worksheet.Cells["A2:A4"]);
                barSeries.Header = "Monto en ₡";
                barChart.Title.Text = "Resumen Financiero (Barras)";

                // Crear un gráfico de pastel
                var pieChart = worksheet.Drawings.AddChart("PieChart", eChartType.Pie);
                pieChart.SetPosition(5, 0, 10, 0);  // Cambia la posición para que haya más espacio entre los gráficos
                pieChart.SetSize(500, 300);
                var pieSeries = pieChart.Series.Add(worksheet.Cells["B2:B4"], worksheet.Cells["A2:A4"]);
                pieSeries.Header = "Monto en ₡";
                pieChart.Title.Text = "Resumen Financiero (Pastel)";

                // Guardar el archivo en un MemoryStream
                using (var stream = new MemoryStream())
                {
                    package.SaveAs(stream);
                    var content = stream.ToArray();

                    // Agregar la fecha al nombre del archivo
                    string fileName = $"ResumenFinanciero_{DateTime.Now:ddMMyyyy}.xlsx";

                    // Devolver el archivo como un archivo descargable
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        public async Task<IActionResult> ExportCierreCajaToExcel(int idCierreCaja)
        {
            var cierreCaja = await _context.CierreCajas
                .Include(c => c.Sucursal)
                .Include(c => c.UsuarioCreacion)
                .FirstOrDefaultAsync(c => c.IdCierraCaja == idCierreCaja);

            if (cierreCaja == null)
            {
                return NotFound();
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add($"CierreCaja_{idCierreCaja}");
                worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;

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
                var titleCell = worksheet.Cell("A2");
                titleCell.Value = $"Detalles del Cierre de Caja #{idCierreCaja}";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4"); // Color de fondo azul de Excel
                titleCell.Style.Font.FontColor = XLColor.White;
                worksheet.Range("A2:G2").Merge();

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

                // Ajustar el ancho de las columnas después de agregar los datos
                worksheet.Columns().AdjustToContents();

                // Guardar el archivo Excel en un MemoryStream y devolver como FileResult
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"CierreCaja_{idCierreCaja}.xlsx");
                }
            }
        }

    }
}