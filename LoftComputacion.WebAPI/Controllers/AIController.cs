using LoftComputacion.Application;
using LoftComputacion.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LoftComputacion.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly AIService _aiService;

        public AIController(AIService aiService)
        {
            _aiService = aiService;
        }

        // ENDPOINT 1: Generar Resumen (Este ya lo tenías)
        [HttpPost("generar-resumen")]
        public async Task<IActionResult> GenerarResumen([FromBody] GenerarResumenDto dto)
        {
            var resumen = await _aiService.GenerarResumenDesdeTecnicoAsync(dto.TextoTecnico);

            if (string.IsNullOrEmpty(resumen))
            {
                return StatusCode(500, "Ocurrió un error al generar el resumen.");
            }

            return Ok(resumen);
        }

        // ENDPOINT 2: Chatbot Técnico (ESTE ES EL QUE FALTABA)
        [HttpPost("consultar")]
        public async Task<IActionResult> ConsultarExperto([FromBody] ConsultaChatDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("La consulta no puede estar vacía.");

            // Llamamos a tu servicio de IA
            var respuesta = await _aiService.ConsultarExpertoTecnicoAsync(request.Prompt);

            if (respuesta == null)
                return StatusCode(500, "La IA no pudo procesar la solicitud.");

            // Devolvemos un objeto anónimo con la propiedad 'resultado' que espera el cliente
            return Ok(new { resultado = respuesta });
        }
    }

    // Clase auxiliar para recibir el dato del chat (puedes dejarla aquí mismo)
    public class ConsultaChatDto
    {
        public string Prompt { get; set; }
    }
}