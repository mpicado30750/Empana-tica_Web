using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using TotalHRInsight.DTO;

namespace TotalHRInsightAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisoesController : ControllerBase
    {
        private readonly TotalHRInsightDbContext _context;

        public PermisoesController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: api/Permisoes/{usuarioCreacionId}
        [HttpGet("{usuarioCreacionId}")]
        public async Task<ActionResult<IEnumerable<PermisoGetIdUsuarioDTO>>> GetPermisos(string usuarioCreacionId)
        {
            var permisos = await _context.Permisos
                                        .Where(p => p.UsuarioCreacionId == usuarioCreacionId)
                                        .Select(p => new PermisoGetIdUsuarioDTO
                                        {
                                            IdPermisos = p.IdPermisos,
                                            FechaInicio = p.FechaInicio,
                                            FechaFin = p.FechaFin,
                                            Estado = p.Estado,
                                            IdIncidencia = p.IdIncidencia,
                                            Comentario = p.Comentario,
                                            UsuarioCreacionId = p.UsuarioCreacionId
                                        })
                                        .ToListAsync();

            if (permisos == null || !permisos.Any())
            {
                return NotFound();
            }

            return Ok(permisos);
        }

        // POST: api/Permisoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Permiso>> PostPermiso(PermisoPostUsuarioDTO permisoDto)
        {
            // Mapea los datos del DTO al modelo Permiso
            var permiso = new Permiso
            {
                FechaInicio = permisoDto.FechaInicio,
                FechaFin = permisoDto.FechaFin,
                Comentario = permisoDto.Comentario,
                CantidadDias = permisoDto.CantidadDias,
                IdIncidencia = permisoDto.IdIncidencia,
                Estado = permisoDto.Estado,
                UsuarioCreacionId = permisoDto.UsuarioCreacionId,
                UsuarioAsignacionId = permisoDto.UsuarioAsignacionId
            };

            _context.Permisos.Add(permiso);
            await _context.SaveChangesAsync();

            return Ok(permiso);
        }


        private bool PermisoExists(int id)
        {
            return _context.Permisos.Any(e => e.IdPermisos == id);
        }
    }
}
