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
    public class AsistenciasController : ControllerBase
    {
        private readonly TotalHRInsightDbContext _context;

        public AsistenciasController(TotalHRInsightDbContext context)
        {
            _context = context;
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
            if (id != asistencia.idAsistencia)
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

        // POST: api/Asistencias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AsistenciaDTO>> PostAsistencia(AsistenciaDTO asistenciaDTO)
        {
			var asistencia = new Asistencia
			{
				FechaEntrada = asistenciaDTO.FechaEntrada,
				FechaSalida = asistenciaDTO.FechaSalida,
				Longitud = asistenciaDTO.Longitud,
				Latitud = asistenciaDTO.Latitud,
				Ubicacion = asistenciaDTO.Ubicacion,
				UsuarioCreacionId = asistenciaDTO.UsuarioCreacionId
			};
			_context.Asistencias.Add(asistencia);
            await _context.SaveChangesAsync();

			return Ok(new { success = true, message = "User logged out successfully." });
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
            return _context.Asistencias.Any(e => e.idAsistencia == id);
        }
    }
}
