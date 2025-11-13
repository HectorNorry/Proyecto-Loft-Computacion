using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using LoftComputacion.BlazorApp.Models;
using LoftComputacion.BlazorApp.DTOs;

namespace LoftComputacion.BlazorApp.Services
{
    public class OrdenesApiService
    {
        private readonly HttpClient _httpClient;

        public OrdenesApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<OrdenListaDto>?> GetOrdenesAsync()
        {
            // 🚨 El tipo de retorno ahora es OrdenListaDto 🚨
            return await _httpClient.GetFromJsonAsync<IEnumerable<OrdenListaDto>>("api/ordenesdeservicio");
        }
    }
}
