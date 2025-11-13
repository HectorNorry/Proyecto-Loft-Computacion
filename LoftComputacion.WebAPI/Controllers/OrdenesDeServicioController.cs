using LoftComputacion.Application;
using LoftComputacion.Domain;
using LoftComputacion.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; 

namespace LoftComputacion.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenesDeServicioController : ControllerBase
    {
        private readonly OrdenDeServicioService _ordenDeServicioService;
        private readonly MercadoPagoService _mercadoPagoService; 

        public OrdenesDeServicioController(
            OrdenDeServicioService ordenDeServicioService,
            MercadoPagoService mercadoPagoService)
        {
            _ordenDeServicioService = ordenDeServicioService;
            _mercadoPagoService = mercadoPagoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdenesDeServicio([FromQuery] string? filtro = null)
        {
            var ordenes = await _ordenDeServicioService.GetAllOrdenesAsync(filtro);
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

        // --- ¡NUESTRO NUEVO ENDPOINT PARA MERCADO PAGO! ---
        [HttpPost("{id}/crear-pago")]
        public async Task<IActionResult> CrearPreferenciaDePago(int id)
        {
            try
            {
                // 1. Buscamos la orden completa para obtener el precio y los datos del cliente
                var orden = await _ordenDeServicioService.GetOrdenByIdAsync(id);
                if (orden == null)
                {
                    return NotFound("No se encontró la orden de servicio.");
                }

                // 2. Validamos que la orden tenga un precio final asignado
                if (orden.PrecioFinal == null || orden.PrecioFinal <= 0)
                {
                    return BadRequest("La orden no tiene un precio final válido para generar el pago.");
                }

                // 3. Llamamos a nuestro servicio para crear el link de pago
                string urlPreferencia = await _mercadoPagoService.CrearPreferenciaDePagoAsync(orden);

                // 4. Devolvemos el link de pago al frontend (WinForms)
                //    Devolvemos un objeto anónimo para que sea un JSON limpio
                return Ok(new { urlDePago = urlPreferencia });
            }
            catch (System.Exception ex)
            {
                // Manejamos cualquier error que ocurra al hablar con Mercado Pago
                return StatusCode(500, $"Error al crear la preferencia de pago: {ex.Message}");
            }
        }
        // --- FIN DEL NUEVO ENDPOINT ---


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
                PrecioFinal = ordenDto.PrecioFinal,
                ResumenTecnico = ordenDto.ResumenTecnico
            };

            var resultado = await _ordenDeServicioService.UpdateOrdenAsync(id, ordenActualizada, ordenDto.UsuarioId);
            if (!resultado)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("{id}/historial")]
        public async Task<IActionResult> GetHistorialDeOrden(int id)
        {
            var historial = await _ordenDeServicioService.GetHistorialByOrdenIdAsync(id);
            if (historial == null)
            {
                return NotFound();
            }
            return Ok(historial);
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