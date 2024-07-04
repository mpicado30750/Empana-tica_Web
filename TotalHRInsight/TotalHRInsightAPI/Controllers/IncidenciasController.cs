using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;

namespace TotalHRInsightAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidenciasController : ControllerBase
    {
        private readonly TotalHRInsightDbContext _context;

        public IncidenciasController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: api/Incidencias
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Incidencia>>> GetIncidencias()
        //{
        //    return await _context.Incidencias.ToListAsync();
        //}
    }
}
