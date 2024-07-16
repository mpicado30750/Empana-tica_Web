using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;

namespace TotalHRInsight.Controllers
{
    public class ProductosController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public ProductosController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
			var totalHRInsightDbContext = _context.Productos.Include(p => p.Categorias).Include(p => p.Medidas).Include(p => p.Proveedor);
			return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? IdProducto)
        {
            if (IdProducto == null)
            {
                return NotFound();
            }

			var producto = await _context.Productos
				.Include(p => p.Categorias)
				.Include(p => p.Medidas)
			.Include(p => p.Proveedor)
				.FirstOrDefaultAsync(m => m.IdProducto == IdProducto);
			if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
			ViewData["CategoriaId"] = new SelectList(_context.Categoria, "IdCategoria", "Descripcion");
			ViewData["MedidasId"] = new SelectList(_context.Medidas, "IdMedida", "NombreMedida");
			ViewData["ProveedorId"] = new SelectList(_context.Proveedor, "IdProveedor", "NombreProveedor");
			return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Producto producto)
        {
			if (ModelState.IsValid)
			{
				string nombreProductoNormalizado = producto.NombreProducto.Trim().ToLower().Replace(" ", "");

				bool productoExiste = _context.Productos
					.Any(p => p.NombreProducto.Trim().ToLower().Replace(" ", "") == nombreProductoNormalizado);

				if (productoExiste)
				{
					ModelState.AddModelError("NombreProducto", "El producto ya existe en la base de datos.");
				}
				else
				{
					_context.Add(producto);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
			}

			ViewData["CategoriaId"] = new SelectList(_context.Categoria, "IdCategoria", "Descripcion", producto.CategoriaId);
			ViewData["MedidasId"] = new SelectList(_context.Medidas, "IdMedida", "NombreMedida", producto.MedidasId);
			ViewData["ProveedorId"] = new SelectList(_context.Proveedor, "IdProveedor", "NombreProveedor", producto.ProveedorId);
			return View(producto);

		}

		// GET: Productos/Edit/5
		public async Task<IActionResult> Edit(int? IdProducto)
        {
            if (IdProducto == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(IdProducto);
            if (producto == null)
            {
                return NotFound();
            }
			ViewData["CategoriaId"] = new SelectList(_context.Categoria, "IdCategoria", "Descripcion", producto.CategoriaId);
			ViewData["MedidasId"] = new SelectList(_context.Medidas, "IdMedida", "NombreMedida", producto.MedidasId);
			ViewData["ProveedorId"] = new SelectList(_context.Proveedor, "IdProveedor", "NombreProveedor", producto.ProveedorId);
			return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("IdProducto,NombreProducto,FechaVencimiento,PrecioUnitario,MedidasId,CategoriaId,ProveedorId")] Producto producto)
		{
			if (id != producto.IdProducto)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(producto);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProductoExists(producto.IdProducto))
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
			ViewData["CategoriaId"] = new SelectList(_context.Categoria, "IdCategoria", "Descripcion", producto.CategoriaId);
			ViewData["MedidasId"] = new SelectList(_context.Medidas, "IdMedida", "NombreMedida", producto.MedidasId);
			ViewData["ProveedorId"] = new SelectList(_context.Proveedor, "IdProveedor", "NombreProveedor", producto.ProveedorId);
			return View(producto);
		}

		// GET: Productos/Delete/5
		public async Task<IActionResult> Delete(int? IdProducto)
        {
            if (IdProducto == null)
            {
                return NotFound();
            }

			var producto = await _context.Productos
				.Include(p => p.Categorias)
				.Include(p => p.Medidas)
				.Include(p => p.Proveedor)
				.FirstOrDefaultAsync(m => m.IdProducto == IdProducto);
			if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdProducto)
        {
			var producto = await _context.Productos.FindAsync(IdProducto);
			if (producto != null)
			{
				_context.Productos.Remove(producto);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}

        private bool ProductoExists(int IdProducto)
        {
            return _context.Productos.Any(e => e.IdProducto == IdProducto);
        }

        //Exportar a Excel 
        public async Task<IActionResult> ExportToExcel()
        {
            var productos = await _context.Productos.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Productos");

                // Cabeceras
                worksheet.Cell(1, 1).Value = "IdProducto";
                worksheet.Cell(1, 2).Value = "NombreProducto";
                worksheet.Cell(1, 2).Value = "NombreCategoria";
                worksheet.Cell(1, 4).Value = "Unidad";
                worksheet.Cell(1, 5).Value = "FechaVencimiento";
                worksheet.Cell(1, 6).Value = "PrecioUnitario";

                // Datos
                for (int i = 0; i < productos.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = productos[i].IdProducto;
                    worksheet.Cell(i + 2, 2).Value = productos[i].NombreProducto;
                    worksheet.Cell(i + 2, 2).Value = productos[i].CategoriaId;
                    worksheet.Cell(i + 2, 5).Value = productos[i].FechaVencimiento.ToString("dd-MM-yyyy");
                    worksheet.Cell(i + 2, 6).Value = productos[i].PrecioUnitario;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Productos.xlsx");
                }
            }
        }





    }
}
