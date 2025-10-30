using LoftComputacion.Application;
using LoftComputacion.WebAPI.DTOs;
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
    }
}