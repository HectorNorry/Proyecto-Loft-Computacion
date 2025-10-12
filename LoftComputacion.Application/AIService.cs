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

        public async Task<string?> GenerarResumenAsync(string textoTecnico)
        {
            var apiKey = _configuration["ApiKeyGemini"];
            if (string.IsNullOrEmpty(apiKey)) return null;

            var client = _httpClientFactory.CreateClient();
            var apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

            var prompt = $"Eres un asistente de un taller de reparación de computadoras llamado LOFT COMPUTACIÓN. Convierte el siguiente diagnóstico técnico en un mensaje breve, amigable y profesional para un cliente no experto. El mensaje debe ser positivo y claro. Al final del mensaje, incluye siempre la siguiente firma sin modificarla: 'Saludos, El equipo de LOFT COMPUTACIÓN'. Diagnóstico: '{textoTecnico}'";

            var payload = new { contents = new[] { new { parts = new[] { new { text = prompt } } } } };
            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(jsonResponse);
                return doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
            }
            catch
            {
                return null;
            }
        }
    }
}