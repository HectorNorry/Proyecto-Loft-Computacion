using System.Text;
using System.Text.Json;
using LoftComputacion.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LoftComputacion.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AIController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // Endpoint para generar el resumen
        [HttpPost("generar-resumen")]
        public async Task<IActionResult> GenerarResumen([FromBody] GenerarResumenDto dto)
        {
            var textoTecnico = dto.TextoTecnico;
            var apiKey = _configuration["ApiKeyGemini"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return StatusCode(500, "La clave de API no está configurada.");
            }

            var client = _httpClientFactory.CreateClient();

            // --- CAMBIO DEFINITIVO AQUÍ ---
            // Usamos la API 'v1beta' y el nombre exacto del modelo 'gemini-2.5-flash' que obtuvimos de la lista.
            var apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";
            // --- FIN DEL CAMBIO ---

            var prompt = $"Eres un asistente de un taller de reparación de computadoras llamado LOFT COMPUTACIÓN. Convierte el siguiente diagnóstico técnico en un mensaje breve, amigable y profesional para un cliente no experto. El mensaje debe ser positivo y claro. Diagnóstico: '{textoTecnico}'";

            var payload = new { contents = new[] { new { parts = new[] { new { text = prompt } } } } };
            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, $"Error de la API de Google: {jsonResponse}");
            }

            try
            {
                using var doc = JsonDocument.Parse(jsonResponse);
                var generatedText = doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
                return Ok(generatedText);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar la respuesta de la IA: {ex.Message}");
            }
        }

        
        
    }
}