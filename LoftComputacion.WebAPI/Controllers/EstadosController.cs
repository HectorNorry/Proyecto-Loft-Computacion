using LoftComputacion.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoftComputacion.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EstadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/estados
        [HttpGet]
        public async Task<IActionResult> GetEstados()
        {
            return Ok(await _context.Estados.ToListAsync());
        }
    }
}