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
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal");
            ViewData["TipoIngresoId"] = new SelectList(_context.TipoIngresos, "IdTipoIngreso", "NombreIngreso");
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetCierresBySucursal(int sucursalId)
        {
            var cierres = await _context.CierreCajas
                .Where(c => c.SucursalId == sucursalId)
                .Select(c => new
                {
                    c.IdCierraCaja,
                    c.Fecha
                })
                .ToListAsync();

            return Json(cierres);
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
       

    }
}

