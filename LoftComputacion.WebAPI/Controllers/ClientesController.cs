using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoftComputacion.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // El constructor recibe el DbContext para poder hablar con la base de datos.
        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Este método responderá a las peticiones GET a la URL: api/clientes
        [HttpGet]
        public async Task<IActionResult> GetClientes()
        {
            // Busca en la tabla Clientes, los convierte a una lista y los devuelve.
            var clientes = await _context.Clientes.ToListAsync();
            return Ok(clientes);
        }

        // POST: api/clientes
        [HttpPost]
        public async Task<IActionResult> CreateCliente([FromBody] Cliente cliente)
        {
            if (cliente == null)
            {
                return BadRequest("El cliente no puede ser nulo.");
            }

            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();

            // Devolvemos una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetClientes), new { id = cliente.Id }, cliente);
        }
    }
}