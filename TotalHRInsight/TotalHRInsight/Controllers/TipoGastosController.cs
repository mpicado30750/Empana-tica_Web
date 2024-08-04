﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;

namespace TotalHRInsight.Controllers
{
    public class TipoGastosController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public TipoGastosController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: TipoGastos
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoGastos.ToListAsync());
        }

        // GET: TipoGastos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoGasto = await _context.TipoGastos
                .FirstOrDefaultAsync(m => m.IdTipoGasto == id);
            if (tipoGasto == null)
            {
                return NotFound();
            }

            return View(tipoGasto);
        }

        // GET: TipoGastos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoGastos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoGasto,NombreGasto")] TipoGasto tipoGasto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoGasto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoGasto);
        }

        // GET: TipoGastos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoGasto = await _context.TipoGastos.FindAsync(id);
            if (tipoGasto == null)
            {
                return NotFound();
            }
            return View(tipoGasto);
        }

        // POST: TipoGastos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoGasto,NombreGasto")] TipoGasto tipoGasto)
        {
            if (id != tipoGasto.IdTipoGasto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoGasto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoGastoExists(tipoGasto.IdTipoGasto))
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
            return View(tipoGasto);
        }

        // GET: TipoGastos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoGasto = await _context.TipoGastos
                .FirstOrDefaultAsync(m => m.IdTipoGasto == id);
            if (tipoGasto == null)
            {
                return NotFound();
            }

            return View(tipoGasto);
        }

        // POST: TipoGastos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoGasto = await _context.TipoGastos.FindAsync(id);
            if (tipoGasto != null)
            {
                _context.TipoGastos.Remove(tipoGasto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoGastoExists(int id)
        {
            return _context.TipoGastos.Any(e => e.IdTipoGasto == id);
        }
    }
}