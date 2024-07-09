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
	public class MedidasController : Controller
	{
		private readonly TotalHRInsightDbContext _context;

		public MedidasController(TotalHRInsightDbContext context)
		{
			_context = context;
		}

		// GET: Medidas
		public async Task<IActionResult> Index()
		{
			return View(await _context.Medidas.ToListAsync());
		}

		// GET: Medidas/Details/5
		public async Task<IActionResult> Details(int? IdMedida)
		{
			if (IdMedida == null)
			{
				return NotFound();
			}

			var medida = await _context.Medidas
				.FirstOrDefaultAsync(m => m.IdMedida == IdMedida);
			if (medida == null)
			{
				return NotFound();
			}

			return View(medida);
		}

		// GET: Medidas/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Medidas/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("IdMedida,NombreMedida")] Medida medida)
		{
			if (ModelState.IsValid)
			{
				// Normalizar el nombre de la medida para comparar
				string nombreMedidaNormalizado = medida.NombreMedida.Trim().ToLower().Replace(" ", "");

				// Verificar si ya existe una medida con el mismo nombre normalizado
				bool medidaExiste = _context.Medidas
					.Any(m => m.NombreMedida.Trim().ToLower().Replace(" ", "") == nombreMedidaNormalizado);

				if (medidaExiste)
				{
					ModelState.AddModelError("NombreMedida", "La medida ya existe en la base de datos.");
				}
				else
				{
					_context.Add(medida);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
			}
			return View(medida);
		}

		// GET: Medidas/Edit/5
		public async Task<IActionResult> Edit(int? IdMedida)
		{
			if (IdMedida == null)
			{
				return NotFound();
			}

			var medida = await _context.Medidas.FindAsync(IdMedida);
			if (medida == null)
			{
				return NotFound();
			}
			return View(medida);
		}

		// POST: Medidas/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int IdMedida, [Bind("IdMedida,NombreMedida")] Medida medida)
		{
			if (IdMedida != medida.IdMedida)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(medida);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!MedidaExists(medida.IdMedida))
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
			return View(medida);
		}

		// GET: Medidas/Delete/5
		public async Task<IActionResult> Delete(int? IdMedida)
		{
			if (IdMedida == null)
			{
				return NotFound();
			}

			var medida = await _context.Medidas
				.FirstOrDefaultAsync(m => m.IdMedida == IdMedida);
			if (medida == null)
			{
				return NotFound();
			}

			return View(medida);
		}

		// POST: Medidas/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int IdMedida)
		{
			var medida = await _context.Medidas.FindAsync(IdMedida);
			if (medida != null)
			{
				_context.Medidas.Remove(medida);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}

		private bool MedidaExists(int IdMedida)
		{
			return _context.Medidas.Any(e => e.IdMedida == IdMedida);
		}
	}
}
