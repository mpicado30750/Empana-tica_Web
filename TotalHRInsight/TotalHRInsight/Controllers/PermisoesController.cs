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
    public class PermisoesController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public PermisoesController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: Permisoes
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.Permisos.Include(p => p.Incidencia).Include(p => p.UsuarioAsignacion).Include(p => p.UsuarioCreacion);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: Permisoes/Details/5
        public async Task<IActionResult> Details(int? IdPermisos)
        {
            if (IdPermisos == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos
                .Include(p => p.Incidencia)
                .Include(p => p.UsuarioAsignacion)
                .Include(p => p.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdPermisos == IdPermisos);
            if (permiso == null)
            {
                return NotFound();
            }

            return View(permiso);
        }

        // GET: Permisoes/Create
        public IActionResult Create()
        {
            ViewData["IdIncidencia"] = new SelectList(_context.Incidencias, "IdIncidencia", "NombreIncidencia");
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: Permisoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Permiso permiso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(permiso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdIncidencia"] = new SelectList(_context.Incidencias, "IdIncidencia", "NombreIncidencia", permiso.IdIncidencia);
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", permiso.UsuarioAsignacionId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", permiso.UsuarioCreacionId);
            return View(permiso);
        }

        // GET: Permisoes/Edit/5
        public async Task<IActionResult> Edit(int? IdPermisos)
        {
            if (IdPermisos == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos.FindAsync(IdPermisos);
            if (permiso == null)
            {
                return NotFound();
            }
            ViewData["IdIncidencia"] = new SelectList(_context.Incidencias, "IdIncidencia", "NombreIncidencia", permiso.IdIncidencia);
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", permiso.UsuarioAsignacionId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", permiso.UsuarioCreacionId);
            return View(permiso);
        }

        // POST: Permisoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int IdPermisos, Permiso permiso)
        {
            if (IdPermisos != permiso.IdPermisos)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permiso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermisoExists(permiso.IdPermisos))
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
            ViewData["IdIncidencia"] = new SelectList(_context.Incidencias, "IdIncidencia", "NombreIncidencia", permiso.IdIncidencia);
            ViewData["UsuarioAsignacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", permiso.UsuarioAsignacionId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", permiso.UsuarioCreacionId);
            return View(permiso);
        }

        // GET: Permisoes/Delete/5
        public async Task<IActionResult> Delete(int? IdPermisos)
        {
            if (IdPermisos == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos
                .Include(p => p.Incidencia)
                .Include(p => p.UsuarioAsignacion)
                .Include(p => p.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdPermisos == IdPermisos);
            if (permiso == null)
            {
                return NotFound();
            }

            return View(permiso);
        }

        // POST: Permisoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdPermisos)
        {
            var permiso = await _context.Permisos.FindAsync(IdPermisos);
            if (permiso != null)
            { 
                _context.Permisos.Remove(permiso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermisoExists(int IdPermisos)
        {
            return _context.Permisos.Any(e => e.IdPermisos == IdPermisos);
        }
    }
}
