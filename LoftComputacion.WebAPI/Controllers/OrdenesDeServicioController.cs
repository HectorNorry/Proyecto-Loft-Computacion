using Microsoft.EntityFrameworkCore;
using LoftComputacion.Infrastructure;
using Microsoft.AspNetCore.Mvc;


namespace LoftComputacion.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenesDeServicioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdenesDeServicioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ordenesdeservicio
        [HttpGet]
        public async Task<IActionResult> GetOrdenesDeServicio()
        {
            var ordenes = await _context.OrdenesDeServicio
                                        .Include(o => o.Cliente) // Incluye los datos del Cliente relacionado
                                        .Include(o => o.Equipo)  // Incluye los datos del Equipo relacionado
                                        .Include(o => o.Estado)   // Incluye los datos del Estado relacionado
                                        .ToListAsync();

            return Ok(ordenes);
        }

        // GET: api/ordenesdeservicio/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdenDeServicio(int id)
        {
            var ordenDeServicio = await _context.OrdenesDeServicio
                                                .Include(o => o.Cliente)
                                                .Include(o => o.Equipo)
                                                .Include(o => o.Estado)
                                                .FirstOrDefaultAsync(o => o.Id == id);

            if (ordenDeServicio == null)
            {
                return NotFound(); // Devuelve un error 404 si no se encuentra la orden
            }

            return Ok(ordenDeServicio);
        }
    }
}