using LoftComputacion.Application; // Para usar AIService
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LoftComputacion.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IaController : ControllerBase
    {
        private readonly AIService _aiService;

        // Inyectamos tu servicio existente
        public IaController(AIService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("consultar")]
        public async Task<IActionResult> Consultar([FromBody] JsonElement body)
        {
            try
            {
                // Obtenemos el texto que mandó el chat
                string prompt = string.Empty;

                // Manejo seguro del JSON
                if (body.ValueKind == JsonValueKind.Object && body.TryGetProperty("prompt", out var prop))
                {
                    prompt = prop.GetString() ?? "";
                }

                if (string.IsNullOrWhiteSpace(prompt))
                    return BadRequest("La consulta está vacía.");

                // Llamamos a tu servicio existente con el nuevo método
                var respuesta = await _aiService.ConsultarExpertoTecnicoAsync(prompt);

                if (string.IsNullOrEmpty(respuesta))
                    return StatusCode(500, "La IA no devolvió respuesta (posible error de API Key o conexión).");

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}