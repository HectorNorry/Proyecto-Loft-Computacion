using LoftComputacion.Application;
using LoftComputacion.Domain;
using LoftComputacion.WebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoftComputacion.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenesDeServicioController : ControllerBase
    {
        private readonly OrdenDeServicioService _ordenDeServicioService;

        public OrdenesDeServicioController(OrdenDeServicioService ordenDeServicioService)
        {
            _ordenDeServicioService = ordenDeServicioService;
        }

        [HttpGet]
        // Indicamos que 'filtro' viene de la URL (query string) y es opcional
        public async Task<IActionResult> GetOrdenesDeServicio([FromQuery] string? filtro = null)
        {
            var ordenes = await _ordenDeServicioService.GetAllOrdenesAsync(filtro); // Pasamos el filtro al servicio
            return Ok(ordenes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdenDeServicio(int id)
        {
            var orden = await _ordenDeServicioService.GetOrdenByIdAsync(id);
            if (orden == null)
            {
                return NotFound();
            }
            return Ok(orden);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrdenDeServicio([FromBody] CreateOrdenDto ordenDto)
        {
            var nuevaOrden = new OrdenDeServicio
            {
                ClienteId = ordenDto.ClienteId,
                EquipoId = ordenDto.EquipoId,
                FallaDeclaradaPorCliente = ordenDto.FallaDeclaradaPorCliente
            };

            var ordenCreada = await _ordenDeServicioService.CreateOrdenAsync(nuevaOrden);
            return CreatedAtAction(nameof(GetOrdenDeServicio), new { id = ordenCreada.Id }, ordenCreada);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrdenDeServicio(int id, [FromBody] UpdateOrdenDto ordenDto)
        {
            var ordenActualizada = new OrdenDeServicio
            {
                EstadoId = ordenDto.EstadoId,
                PrecioPresupuestado = ordenDto.PrecioPresupuestado,
                PrecioFinal = ordenDto.PrecioFinal
            };

            // Pasamos el UsuarioId al servicio
            var resultado = await _ordenDeServicioService.UpdateOrdenAsync(id, ordenActualizada, ordenDto.UsuarioId);
            if (!resultado)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrdenDeServicio(int id)
        {
            var resultado = await _ordenDeServicioService.DeleteOrdenAsync(id);
            if (!resultado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}