using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var totalHRInsightDbContext = _context.Productos.Include(p => p.Medidas);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Medidas)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            ViewData["MedidasId"] = new SelectList(_context.Medidas, "IdMedida", "NombreMedida");
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

			ViewData["MedidasId"] = new SelectList(_context.Medidas, "IdMedida", "NombreMedida", producto.MedidasId);
			return View(producto);

		}

		// GET: Productos/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["MedidasId"] = new SelectList(_context.Medidas, "IdMedida", "NombreMedida", producto.MedidasId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,NombreProducto,Descripcion,FechaVencimiento,PrecioUnitario,MedidasId")] Producto producto)
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
            ViewData["MedidasId"] = new SelectList(_context.Medidas, "IdMedida", "NombreMedida", producto.MedidasId);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Medidas)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			var producto = await _context.Productos.FindAsync(id);
			if (producto != null)
			{
				_context.Productos.Remove(producto);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.IdProducto == id);
        }
    }
}
