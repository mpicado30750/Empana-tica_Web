using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using TotalHRInsight.Models.Sucursal;

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

        // GET: Sucursal/Details/5
        public async Task<IActionResult> Details(int? IdSucursal)
        {
            if (IdSucursal == null)
            {
                return NotFound();
            }

            var sucursal = await _context.Sucursales
                .FirstOrDefaultAsync(m => m.IdSucursal == IdSucursal);
            if (sucursal == null)
            {
                return NotFound();
            }

            var model = new DetailsSucursal
            {
                IdSucursal = sucursal.IdSucursal,
                NombreSucursal = sucursal.NombreSucursal,
                UbicacionSucursal = sucursal.UbicacionSucursal,
                Latitud = sucursal.Latitud.ToString(CultureInfo.InvariantCulture),
                Longitud = sucursal.Longitud.ToString(CultureInfo.InvariantCulture)
            };

            return View(model);
        }

        // GET: Sucursals/Create
        public IActionResult Create()
		{
			return View();
		}

		// POST: Sucursals/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CrearSucursal sucursal)
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
					Sucursal datos = new Sucursal
					{
						NombreSucursal = sucursal.NombreSucursal,
						UbicacionSucursal = sucursal.UbicacionSucursal,
						Latitud = double.Parse(sucursal.Latitud, CultureInfo.InvariantCulture),
						Longitud = double.Parse(sucursal.Longitud, CultureInfo.InvariantCulture)
					};
					_context.Add(datos);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
			}
			return View(sucursal);
		}

        public async Task<IActionResult> Edit(int? IdSucursal)
        {
            if (IdSucursal == null)
            {
                return NotFound();
            }

            var sucursal = await _context.Sucursales.FindAsync(IdSucursal);
            if (sucursal == null)
            {
                return NotFound();
            }

            var model = new DetailsSucursal
            {
                IdSucursal = sucursal.IdSucursal,
                NombreSucursal = sucursal.NombreSucursal,
                UbicacionSucursal = sucursal.UbicacionSucursal,
                Latitud = sucursal.Latitud.ToString(CultureInfo.InvariantCulture),
                Longitud = sucursal.Longitud.ToString(CultureInfo.InvariantCulture)
            };

            return View(model);
        }

        // POST: Sucursal/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int IdSucursal,DetailsSucursal model)
        {
            if (IdSucursal != model.IdSucursal)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var sucursal = await _context.Sucursales.FindAsync(IdSucursal);
                    if (sucursal == null)
                    {
                        return NotFound();
                    }

                    sucursal.NombreSucursal = model.NombreSucursal;
                    sucursal.UbicacionSucursal = model.UbicacionSucursal;
                    sucursal.Latitud = double.Parse(model.Latitud, CultureInfo.InvariantCulture);
                    sucursal.Longitud = double.Parse(model.Longitud, CultureInfo.InvariantCulture);

                    _context.Update(sucursal);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SucursalExists(model.IdSucursal))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(model);
        }


        // GET: Sucursals/Delete/5
        public async Task<IActionResult> Delete(int? IdSucursal)
		{
			if (IdSucursal == null)
			{
				return NotFound();
			}

			var sucursal = await _context.Sucursales
				.FirstOrDefaultAsync(m => m.IdSucursal == IdSucursal);
			if (sucursal == null)
			{
				return NotFound();
			}

			return View(sucursal);
		}

		// POST: Sucursals/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int IdSucursal)
		{
			var sucursal = await _context.Sucursales.FindAsync(IdSucursal);
			if (sucursal != null)
			{
				_context.Sucursales.Remove(sucursal);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}

		private bool SucursalExists(int IdSucursal)
		{
			return _context.Sucursales.Any(e => e.IdSucursal == IdSucursal);
		}
	}
}
