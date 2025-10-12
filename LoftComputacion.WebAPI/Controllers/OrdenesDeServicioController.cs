using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using LoftComputacion.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
        // POST: api/ordenesdeservicio
        [HttpPost]
        public async Task<IActionResult> CreateOrdenDeServicio([FromBody] CreateOrdenDto ordenDto)
        {
            // Aquí iría la lógica para encontrar el estado inicial "Recibido", pero por ahora lo simulamos.
            // TODO: Obtener el primer estado de la base de datos.
            const int estadoInicialId = 1;

            var nuevaOrden = new OrdenDeServicio
            {
                ClienteId = ordenDto.ClienteId,
                EquipoId = ordenDto.EquipoId,
                FallaDeclaradaPorCliente = ordenDto.FallaDeclaradaPorCliente,
                FechaIngreso = DateTime.UtcNow,
                EstadoId = estadoInicialId // Asignamos el estado inicial
            };

            await _context.OrdenesDeServicio.AddAsync(nuevaOrden);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrdenDeServicio), new { id = nuevaOrden.Id }, nuevaOrden);
        }
        // PUT: api/ordenesdeservicio/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrdenDeServicio(int id, [FromBody] UpdateOrdenDto ordenDto)
        {
            // Primero, buscamos la orden de servicio existente en la base de datos
            var ordenExistente = await _context.OrdenesDeServicio.FindAsync(id);

            if (ordenExistente == null)
            {
                return NotFound("No se encontró la orden de servicio.");
            }

            // Actualizamos los campos de la orden existente con los datos del DTO
            ordenExistente.EstadoId = ordenDto.EstadoId;
            ordenExistente.PrecioPresupuestado = ordenDto.PrecioPresupuestado;
            ordenExistente.PrecioFinal = ordenDto.PrecioFinal;

            // Aquí irá la lógica de auditoría para guardar en la tabla HistorialOrdenes
            // TODO: Registrar el cambio de estado en el historial con el usuario correspondiente.

            await _context.SaveChangesAsync();

            return NoContent(); // Código 204: Éxito, sin contenido que devolver.
        }
        // DELETE: api/ordenesdeservicio/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrdenDeServicio(int id)
        {
            // Buscamos la orden de servicio por su ID
            var ordenExistente = await _context.OrdenesDeServicio.FindAsync(id);

            if (ordenExistente == null)
            {
                return NotFound("No se encontró la orden de servicio para eliminar.");
            }

            // Le decimos a Entity Framework que elimine esta entidad
            _context.OrdenesDeServicio.Remove(ordenExistente);
            await _context.SaveChangesAsync(); // Aplicamos los cambios en la base de datos

            return NoContent(); // Código 204: Éxito, sin contenido que devolver.
        }
    }
}