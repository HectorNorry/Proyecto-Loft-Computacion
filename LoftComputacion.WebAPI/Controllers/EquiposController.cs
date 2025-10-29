using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoftComputacion.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EquiposController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EquiposController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/equipos
        [HttpGet]
        public async Task<IActionResult> GetEquipos()
        {
            return Ok(await _context.Equipos.ToListAsync());
        }

        // POST: api/equipos
        [HttpPost]
        public async Task<IActionResult> CreateEquipo([FromBody] Equipo equipo)
        {
            await _context.Equipos.AddAsync(equipo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEquipos), new { id = equipo.Id }, equipo);
        }
    }
}