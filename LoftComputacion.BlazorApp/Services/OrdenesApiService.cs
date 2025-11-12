using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using LoftComputacion.BlazorApp.Models;

namespace LoftComputacion.BlazorApp.Services
{
    public class OrdenesApiService
    {
        private readonly HttpClient _httpClient;

        public OrdenesApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OrdenDeServicioDto>> GetOrdenesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<OrdenDeServicioDto>>("api/OrdenesDeServicio");
            return response ?? new List<OrdenDeServicioDto>();
        }
    }
}
