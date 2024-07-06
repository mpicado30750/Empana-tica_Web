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
	public class TipoPermisoController : Controller
	{
		private readonly TotalHRInsightDbContext _context;

		public TipoPermisoController(TotalHRInsightDbContext context)
		{
			_context = context;
		}

		// GET: TipoPermiso
		public async Task<IActionResult> Index()
		{
			return View(await _context.TipoPermisos.ToListAsync());
		}

		// GET: TipoPermiso/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var tipoPermiso = await _context.TipoPermisos
				.FirstOrDefaultAsync(m => m.IdTipoPermiso == id);
			if (tipoPermiso == null)
			{
				return NotFound();
			}

			return View(tipoPermiso);
		}

		// GET: TipoPermiso/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: TipoPermiso/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("IdTipoPermiso,NombrePermiso")] TipoPermiso tipoPermiso)
		{
			if (ModelState.IsValid)
			{
				string nombrePermisoNormalizado = tipoPermiso.NombrePermiso.Trim().ToLower().Replace(" ", "");

				bool tipoPermisoExiste = _context.TipoPermisos
					.Any(tp => tp.NombrePermiso.Trim().ToLower().Replace(" ", "") == nombrePermisoNormalizado);

				if (tipoPermisoExiste)
				{
					ModelState.AddModelError("NombrePermiso", "Ya existe un tipo de permiso con este nombre.");
				}
				else
				{
					_context.Add(tipoPermiso);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
			}
			return View(tipoPermiso);
		}

		// GET: TipoPermiso/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var tipoPermiso = await _context.TipoPermisos.FindAsync(id);
			if (tipoPermiso == null)
			{
				return NotFound();
			}
			return View(tipoPermiso);
		}

		// POST: TipoPermiso/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("IdTipoPermiso,NombrePermiso")] TipoPermiso tipoPermiso)
		{
			if (id != tipoPermiso.IdTipoPermiso)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				string nombrePermisoNormalizado = tipoPermiso.NombrePermiso.Trim().ToLower().Replace(" ", "");

				bool tipoPermisoExiste = _context.TipoPermisos
					.Any(tp => tp.NombrePermiso.Trim().ToLower().Replace(" ", "") == nombrePermisoNormalizado && tp.IdTipoPermiso != tipoPermiso.IdTipoPermiso);

				if (tipoPermisoExiste)
				{
					ModelState.AddModelError("NombrePermiso", "Ya existe otro tipo de permiso con este nombre.");
				}
				else
				{
					try
					{
						_context.Update(tipoPermiso);
						await _context.SaveChangesAsync();
					}
					catch (DbUpdateConcurrencyException)
					{
						if (!TipoPermisoExists(tipoPermiso.IdTipoPermiso))
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
			return View(tipoPermiso);
		}

		// GET: TipoPermiso/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var tipoPermiso = await _context.TipoPermisos
				.FirstOrDefaultAsync(m => m.IdTipoPermiso == id);
			if (tipoPermiso == null)
			{
				return NotFound();
			}

			return View(tipoPermiso);
		}

		// POST: TipoPermiso/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var tipoPermiso = await _context.TipoPermisos.FindAsync(id);
			if (tipoPermiso != null)
			{
				_context.TipoPermisos.Remove(tipoPermiso);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}

		private bool TipoPermisoExists(int id)
		{
			return _context.TipoPermisos.Any(e => e.IdTipoPermiso == id);
		}
	}
}
