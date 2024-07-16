﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using TotalHRInsight.Models.Inventario;

namespace TotalHRInsight.Controllers
{
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
            var totalHRInsightDbContext = _context.Inventario.Include(i => i.Producto).Include(i => i.Sucursal).Include(i => i.UsuarioCreacion).Include(i => i.UsuarioModificacion);
            return View(await totalHRInsightDbContext.ToListAsync());
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