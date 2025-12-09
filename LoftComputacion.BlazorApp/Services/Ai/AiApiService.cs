using System.Net.Http.Json;

namespace LoftComputacion.BlazorApp.Services.Ai
{
    public class AiApiService
    {
        private readonly HttpClient _httpClient;

        // El Program.cs inyecta aquí el cliente YA CONFIGURADO con el Token
        public AiApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> ConsultarIa(string prompt)
        {
            // Hacemos la petición
            var response = await _httpClient.PostAsJsonAsync("api/ia/consultar", new { prompt = prompt });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Si falla (ej. 401 o 500), devolvemos null o lanzamos error para manejarlo en la vista
                throw new Exception($"Error del servidor: {response.StatusCode}");
            }
        }
    }
}