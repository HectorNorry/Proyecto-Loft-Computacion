using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using LoftComputacion.Domain;

namespace LoftComputacion.WinForms
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        // ¡OJO AQUÍ! Asegúrate de que el puerto (52004) sea el mismo que usa tu API al ejecutarse.
        private const string _apiUrl = "https://localhost:52004/api";

        public ApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Cliente>> GetClientesAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/clientes");
            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<List<Cliente>>(json_response);
                return clientes ?? new List<Cliente>();
            }
            return new List<Cliente>();
        }

        public async Task<List<OrdenDeServicio>> GetOrdenesDeServicioAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/ordenesdeservicio");
            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var ordenes = JsonConvert.DeserializeObject<List<OrdenDeServicio>>(json_response);
                return ordenes ?? new List<OrdenDeServicio>();
            }
            return new List<OrdenDeServicio>();
        }

        // Aquí iremos agregando más métodos: GetOrdenesAsync, CreateClienteAsync, etc.
    }
}