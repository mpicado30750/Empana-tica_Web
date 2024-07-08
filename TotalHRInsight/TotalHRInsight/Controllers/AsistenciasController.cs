using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using TotalHRInsight.Models;

namespace TotalHRInsight.Controllers
{
    public class AsistenciasController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public AsistenciasController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: Asistencias
        public async Task<IActionResult> Index()
        {
			
			var asistencias = await _context.Asistencias
                .Include(a => a.UsuarioCreacion)
                .ToListAsync();

            var viewModel = asistencias.Select(a => new AsistenciaModel
            {
                Id = a.IdAsistencia,
                FechaEntrada = a.FechaEntrada,
                FechaSalida = a.FechaSalida,
                LatitudEntrada = ConvertirLatitud(a.UbicacionEntrada),
                LongitudEntrada = ConvertirLongitud(a.UbicacionEntrada) ?? 0.0, // Manejo de valor null
                LatitudSalida = ConvertirLatitud(a.UbicacionSalida),
                LongitudSalida = ConvertirLongitud(a.UbicacionSalida) ?? 0.0, // Manejo de valor null
                UsuarioCreacionId = a.UsuarioCreacionId,
                UsuarioCreacion = a.UsuarioCreacion.UserName,
                Nombre = a.UsuarioCreacion.Nombre,
                PrimerApellido = a.UsuarioCreacion.PrimerApellido,
                SegundoApellido = a.UsuarioCreacion.SegundoApellido
			}).ToList();

            return View(viewModel);
        }
 
       
        private static double ConvertirLatitud(string input)
        {
            int startIndex = input.IndexOf("lat:") + 4;
            int endIndex = input.IndexOf(",", startIndex);

            if (startIndex < 0 || endIndex < 0)
            {
                throw new ArgumentException("Formato de cadena no válido para latitud.");
            }

            string latStr = input.Substring(startIndex, endIndex - startIndex);
            return double.Parse(latStr, CultureInfo.InvariantCulture);
        }

        private static double? ConvertirLongitud(string input)
        {
            int startIndex = input.IndexOf("lng:") + 4;
            int endIndex = input.IndexOf(")", startIndex);

            if (startIndex < 0 || endIndex < 0)
            {
                return null; // Devuelve null si no se puede encontrar la longitud
            }

            string lngStr = input.Substring(startIndex, endIndex - startIndex);
            return double.Parse(lngStr, CultureInfo.InvariantCulture);
        }



        // GET: Asistencias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistencia = await _context.Asistencias
                .Include(a => a.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdAsistencia == id);
            if (asistencia == null)
            {
                return NotFound();
            }

            return View(asistencia);
        }

		// GET: Asistencias/Create
		public IActionResult Create()
        {
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: Asistencias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAsistencia,FechaEntrada,FechaSalida,UbicacionEntrada,UbicacionSalida,UsuarioCreacionId")] Asistencia asistencia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asistencia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", asistencia.UsuarioCreacionId);
            return View(asistencia);
        }

        // GET: Asistencias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistencia = await _context.Asistencias.FindAsync(id);
            if (asistencia == null)
            {
                return NotFound();
            }
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", asistencia.UsuarioCreacionId);
            return View(asistencia);
        }

        // POST: Asistencias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAsistencia,FechaEntrada,FechaSalida,UbicacionEntrada,UbicacionSalida,UsuarioCreacionId")] Asistencia asistencia)
        {
            if (id != asistencia.IdAsistencia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asistencia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsistenciaExists(asistencia.IdAsistencia))
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
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", asistencia.UsuarioCreacionId);
            return View(asistencia);
        }

        // GET: Asistencias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistencia = await _context.Asistencias
                .Include(a => a.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdAsistencia == id);
            if (asistencia == null)
            {
                return NotFound();
            }

            return View(asistencia);
        }

        // POST: Asistencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asistencia = await _context.Asistencias.FindAsync(id);
            if (asistencia != null)
            {
                _context.Asistencias.Remove(asistencia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AsistenciaExists(int id)
        {
            return _context.Asistencias.Any(e => e.IdAsistencia == id);
        }
    }
}
