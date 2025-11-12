using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using LoftComputacion.Domain; // Para usar la clase Cliente

namespace LoftComputacion.BlazorApp.Services
{
    public class ClientesApiService
    {
        private readonly HttpClient _httpClient;

        // Recibirá el HttpClient que ya tiene configurado el AuthorizedHandler
        public ClientesApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Cliente>> GetClientesAsync()
        {
            // Llama al endpoint de la API
            var response = await _httpClient.GetFromJsonAsync<List<Cliente>>("api/clientes");
            return response ?? new List<Cliente>();
        }
    }
}