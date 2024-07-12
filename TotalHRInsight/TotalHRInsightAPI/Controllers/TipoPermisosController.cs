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
    public class TipoPermisosController : ControllerBase
    {
        private readonly TotalHRInsightDbContext _context;

        public TipoPermisosController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: api/TipoPermisos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoPermiso>>> GetTipoPermisos()
        {
            return await _context.TipoPermisos.ToListAsync();
        }

    }
}
