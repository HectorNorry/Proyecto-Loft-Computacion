using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace LoftComputacion.Application
{
    public class AIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AIService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        /// <summary>
        /// MÉTODO 1 (IDEAL): Genera un resumen para el cliente basado en el informe del técnico.
        /// </summary>
        public async Task<string?> GenerarResumenDesdeTecnicoAsync(string resumenTecnico)
        {
            // Un prompt específico para "traducir"
            var prompt = $"""
                Eres un asistente de un taller de reparación de computadoras llamado LOFT COMPUTACIÓN.
                Tu trabajo es "traducir" el siguiente resumen técnico de un reparador a un mensaje amigable y fácil de entender para un cliente no experto.
                
                NO inventes reparaciones que no estén en el resumen. Basa tu respuesta ÚNICAMENTE en el resumen técnico.
                Sé breve, positivo y profesional. Termina con la firma exacta sin modificarla: 'Saludos, El equipo de LOFT COMPUTACIÓN'.
                
                Resumen técnico del reparador:
                "{resumenTecnico}"
                """;

            return await CallGeminiApiAsync(prompt);
        }

        /// <summary>
        /// MÉTODO 2 (FALLBACK): Genera un mensaje genérico basado en la falla reportada por el cliente.
        /// </summary>
        public async Task<string?> GenerarResumenDesdeFallaAsync(string fallaCliente)
        {
            // Un prompt de "plan B" si el técnico no dejó resumen
            var prompt = $"""
                Eres un asistente de un taller de reparación de computadoras llamado LOFT COMPUTACIÓN.
                Tu trabajo es escribir un mensaje amigable y profesional para un cliente.
                La falla que reportó el cliente fue: "{fallaCliente}".
                El equipo ya fue reparado, pero no tenemos un resumen técnico detallado.
                
                Escribe un mensaje genérico informando que el equipo está listo, funcionando correctamente y 
                que la reparación (basada en la falla que reportó) ha sido completada.
                Termina con la firma exacta sin modificarla: 'Saludos, El equipo de LOFT COMPUTACIÓN'.
                """;

            return await CallGeminiApiAsync(prompt);
        }

        /// <summary>
        /// Método privado que maneja la llamada a la API de Gemini.
        /// </summary>
        private async Task<string?> CallGeminiApiAsync(string prompt)
        {
            var apiKey = _configuration["ApiKeyGemini"];
            if (string.IsNullOrEmpty(apiKey)) return null;

            var client = _httpClientFactory.CreateClient();
            var apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

            var payload = new { contents = new[] { new { parts = new[] { new { text = prompt } } } } };
            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                // (Opcional: puedes loguear el error aquí)
                return null;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(jsonResponse);
                return doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
            }
            catch
            {
                // (Opcional: puedes loguear el error de parseo aquí)
                return null;
            }
        }
    }
}