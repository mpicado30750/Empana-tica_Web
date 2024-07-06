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
	public class SucursalsController : Controller
	{
		private readonly TotalHRInsightDbContext _context;

		public SucursalsController(TotalHRInsightDbContext context)
		{
			_context = context;
		}

		// GET: Sucursals
		public async Task<IActionResult> Index()
		{
			return View(await _context.Sucursales.ToListAsync());
		}

		// GET: Sucursals/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var sucursal = await _context.Sucursales
				.FirstOrDefaultAsync(m => m.IdSucursal == id);
			if (sucursal == null)
			{
				return NotFound();
			}

			return View(sucursal);
		}

		// GET: Sucursals/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Sucursals/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("IdSucursal,NombreSucursal,UbicacionSucursal")] Sucursal sucursal)
		{
			if (ModelState.IsValid)
			{
				string nombreSucursalNormalizado = sucursal.NombreSucursal.Trim().ToLower().Replace(" ", "");

				bool sucursalExiste = _context.Sucursales
					.Any(s => s.NombreSucursal.Trim().ToLower().Replace(" ", "") == nombreSucursalNormalizado);

				if (sucursalExiste)
				{
					ModelState.AddModelError("NombreSucursal", "Ya existe una sucursal con este nombre.");
				}
				else
				{
					_context.Add(sucursal);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
			}
			return View(sucursal);
		}

		// GET: Sucursals/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var sucursal = await _context.Sucursales.FindAsync(id);
			if (sucursal == null)
			{
				return NotFound();
			}
			return View(sucursal);
		}

		// POST: Sucursals/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("IdSucursal,NombreSucursal,UbicacionSucursal")] Sucursal sucursal)
		{
			if (id != sucursal.IdSucursal)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				string nombreSucursalNormalizado = sucursal.NombreSucursal.Trim().ToLower().Replace(" ", "");

				bool sucursalExiste = _context.Sucursales
					.Any(s => s.NombreSucursal.Trim().ToLower().Replace(" ", "") == nombreSucursalNormalizado && s.IdSucursal != sucursal.IdSucursal);

				if (sucursalExiste)
				{
					ModelState.AddModelError("NombreSucursal", "Ya existe otra sucursal con este nombre.");
				}
				else
				{
					try
					{
						_context.Update(sucursal);
						await _context.SaveChangesAsync();
					}
					catch (DbUpdateConcurrencyException)
					{
						if (!SucursalExists(sucursal.IdSucursal))
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
			}
			return View(sucursal);
		}

		// GET: Sucursals/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var sucursal = await _context.Sucursales
				.FirstOrDefaultAsync(m => m.IdSucursal == id);
			if (sucursal == null)
			{
				return NotFound();
			}

			return View(sucursal);
		}

		// POST: Sucursals/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var sucursal = await _context.Sucursales.FindAsync(id);
			if (sucursal != null)
			{
				_context.Sucursales.Remove(sucursal);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}

		private bool SucursalExists(int id)
		{
			return _context.Sucursales.Any(e => e.IdSucursal == id);
		}
	}
}
