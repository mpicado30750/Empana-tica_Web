﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;

namespace TotalHRInsight.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class GastosController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public GastosController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: Gastos
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.Gastos.Include(g => g.CierreCaja).Include(g => g.TipoGasto);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: Gastos/Details/5
        public async Task<IActionResult> Details(int? IdGasto)
        {
            if (IdGasto == null)
            {
                return NotFound();
            }

            var gasto = await _context.Gastos
                .Include(g => g.CierreCaja)
                .Include(g => g.TipoGasto)
                .FirstOrDefaultAsync(m => m.IdGasto == IdGasto);
            if (gasto == null)
            {
                return NotFound();
            }

            return View(gasto);
        }

        // GET: Gastos/Create
        // GET: Gastos/Create
        public IActionResult Create()
        {
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal");
            ViewData["TipoGastoId"] = new SelectList(_context.TipoGastos, "IdTipoGasto", "NombreGasto");
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


        // POST: Gastos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGasto,Fecha,TipoGastoId,MontoGasto,CierreId")] Gasto gasto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gasto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CierreId"] = new SelectList(_context.CierreCajas, "IdCierraCaja", "Fecha", gasto.CierreId);
            ViewData["TipoGastoId"] = new SelectList(_context.TipoGastos, "IdTipoGasto", "NombreGasto", gasto.TipoGastoId);
            return View(gasto);
        }



        // GET: Gastos/Edit/5
        public async Task<IActionResult> Edit(int? IdGasto)
        {
            if (IdGasto == null)
            {
                return NotFound();
            }

            var gasto = await _context.Gastos.FindAsync(IdGasto);
            if (gasto == null)
            {
                return NotFound();
            }
            ViewData["CierreId"] = new SelectList(_context.CierreCajas, "IdCierraCaja", "Fecha", gasto.CierreId);
            ViewData["TipoGastoId"] = new SelectList(_context.TipoGastos, "IdTipoGasto", "NombreGasto", gasto.TipoGastoId);
            return View(gasto);
        }

        // POST: Gastos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int IdGasto, [Bind("IdGasto,Fecha,TipoGastoId,MontoGasto,CierreId")] Gasto gasto)
        {
            if (IdGasto != gasto.IdGasto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gasto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GastoExists(gasto.IdGasto))
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
            ViewData["CierreId"] = new SelectList(_context.CierreCajas, "IdCierraCaja", "Fecha", gasto.CierreId);
            ViewData["TipoGastoId"] = new SelectList(_context.TipoGastos, "IdTipoGasto", "NombreGasto", gasto.TipoGastoId);
            return View(gasto);
        }

        // GET: Gastos/Delete/5
        public async Task<IActionResult> Delete(int? IdGasto)
        {
            if (IdGasto == null)
            {
                return NotFound();
            }

            var gasto = await _context.Gastos
                .Include(g => g.CierreCaja)
                .Include(g => g.TipoGasto)
                .FirstOrDefaultAsync(m => m.IdGasto == IdGasto);
            if (gasto == null)
            {
                return NotFound();
            }

            return View(gasto);
        }

        // POST: Gastos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdGasto)
        {
            var gasto = await _context.Gastos.FindAsync(IdGasto);
            if (gasto != null)
            {
                _context.Gastos.Remove(gasto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GastoExists(int id)
        {
            return _context.Gastos.Any(e => e.IdGasto == id);
        }
    }
}
