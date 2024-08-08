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

        // GET: api/Permisoes/5
        //// GET: api/Permisoes/{usuarioCreacionId}
        [HttpGet("{usuarioCreacionId}")]
        public async Task<ActionResult<IEnumerable<PermisoGetIdUsuarioDTO>>> GetPermisos(string usuarioCreacionId)
        {
            var permisos = await _context.Permisos
                                        .Where(p => p.UsuarioCreacionId == usuarioCreacionId)
                                        .Include(p => p.TipoPermisos)
                                        .Include(p => p.Estado)
                                        .Select(p => new PermisoGetIdUsuarioDTO
                                        {
                                            IdPermisos = p.IdPermisos,
                                            FechaInicio = p.FechaInicio,
                                            FechaFin = p.FechaFin,
                                            TipoPermiso = p.TipoPermisos.NombrePermiso,
                                            Estado = p.Estado.EstadoSolicitud,
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


        // PUT: api/Permisoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostPermiso(PermisoPostUsuarioDTO permisoDto)
        {
            try
            {
                var estadoPendiente = await _context.Estados.FirstOrDefaultAsync(e => e.EstadoSolicitud == "En proceso");
                if (estadoPendiente == null)
                {
                    return NotFound("No se encontró el estado 'En proceso'");
                }

                var permiso = new Permiso
                {
                    FechaInicio = permisoDto.FechaInicio,
                    FechaFin = permisoDto.FechaFin,
                    Comentario = permisoDto.Comentario,
                    CantidadDias = permisoDto.CantidadDias,
                    IdTipoPermiso = permisoDto.IdTipoPermiso,
                    IdEstado = estadoPendiente.IdEstado,
                    MotivoAdmin = " ",
                    UsuarioCreacionId = permisoDto.UsuarioCreacionId,
                    UsuarioAsignacionId = permisoDto.UsuarioAsignacionId
                };

                _context.Permisos.Add(permiso);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Se creó correctamente" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { success = false, message = ex.InnerException?.Message ?? ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        private bool PermisoExists(int id)
        {
            return _context.Permisos.Any(e => e.IdPermisos == id);
        }
    }
}
