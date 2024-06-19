using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using TotalHRInsight.DTO;

namespace TotalHRInsightAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciasController : ControllerBase
    {
        private readonly TotalHRInsightDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AsistenciasController(TotalHRInsightDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Asistencias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Asistencia>>> GetAsistencias()
        {
            return await _context.Asistencias.ToListAsync();
        }

        // GET: api/Asistencias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Asistencia>> GetAsistencia(int id)
        {
            var asistencia = await _context.Asistencias.FindAsync(id);

            if (asistencia == null)
            {
                return NotFound();
            }

            return asistencia;
        }

        // PUT: api/Asistencias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsistencia(int id, Asistencia asistencia)
        {
            if (id != asistencia.IdAsistencia)
            {
                return BadRequest();
            }

            _context.Entry(asistencia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AsistenciaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/AsistenciasInicio/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("AsistenciasFinal")]
        public async Task<IActionResult> PutAsistenciasFinal(AsistenciaSalidaDTO asistenciaDTO)
        {
            try
            {
                var asistencia = await _context.Asistencias.FindAsync(asistenciaDTO.IdAsistencia);
                if (asistencia == null)
                {
                    return NotFound(new { success = false, message = "Asistencia no encontrada." });
                }

                asistencia.FechaSalida = asistenciaDTO.FechaSalida;
                asistencia.UbicacionSalida = asistenciaDTO.UbicacionSalida;

                _context.Entry(asistencia).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Fecha de salida actualizada con éxito." });
            }
            catch (Exception ex)
            {
                var errorMessage = new
                {
                    success = false,
                    message = "Ocurrió un error al actualizar la fecha de salida.",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                };
                Console.WriteLine($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(500, errorMessage);
            }
        }

        // POST: api/AsistenciasInicio
        [HttpPost("AsistenciasInicio")]
        public async Task<ActionResult<AsistenciaEntradaDTO>> PostAsistenciasInicio(AsistenciaEntradaDTO asistenciaDTO)
        {
            try
            {
                // Verificar si el usuario existe
                var usuario = await _userManager.FindByIdAsync(asistenciaDTO.UsuarioCreacionId);
                if (usuario == null)
                {
                    return BadRequest(new { success = false, message = "El usuario especificado no existe." });
                }

                var asistencia = new Asistencia
                {
                    FechaEntrada = asistenciaDTO.FechaEntrada,
                    UbicacionEntrada = asistenciaDTO.UbicacionEntrada,
                    UsuarioCreacionId = asistenciaDTO.UsuarioCreacionId
                };
                _context.Asistencias.Add(asistencia);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Asistencia creada con éxito.", id = asistencia.  IdAsistencia });
            }
            catch (Exception ex)
            {
                var errorMessage = new
                {
                    success = false,
                    message = "Ocurrió un error al crear la asistencia.",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                };
                Console.WriteLine($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(500, errorMessage);
            }
        }



        // DELETE: api/Asistencias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsistencia(int id)
        {
            var asistencia = await _context.Asistencias.FindAsync(id);
            if (asistencia == null)
            {
                return NotFound();
            }

            _context.Asistencias.Remove(asistencia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AsistenciaExists(int id)
        {
            return _context.Asistencias.Any(e => e.IdAsistencia == id);
        }
    }
}
