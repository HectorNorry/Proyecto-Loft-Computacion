using Blazored.LocalStorage;
using LoftComputacion.BlazorApp.Services.Auth;
using LoftComputacion.Shared.DTOs;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace LoftComputacion.BlazorApp.Services.Ordenes
{
    public class OrdenesApiService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public OrdenesApiService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        public async Task<FotoDto> SubirFotoAsync(int idOrden, IBrowserFile archivo)
        {
            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(archivo.OpenReadStream(maxAllowedSize: 15 * 1024 * 1024));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(archivo.ContentType);

            content.Add(fileContent, "archivo", archivo.Name);

            var request = new HttpRequestMessage(HttpMethod.Post, $"api/OrdenesDeServicio/{idOrden}/fotos")
            {
                Content = content
            };

            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim('"'));
            }

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }

            return await response.Content.ReadFromJsonAsync<FotoDto>()
                   ?? throw new Exception("La API no devolvió los datos de la foto.");
        }

        public async Task BorrarFotoAsync(int idFoto)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/OrdenesDeServicio/fotos/{idFoto}");

            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim('"'));
            }

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task CrearOrdenAsync(CreateOrdenDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/OrdenesDeServicio", dto);
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task UpdateOrdenAsync(int id, UpdateOrdenDto dto)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/OrdenesDeServicio/{id}")
            {
                Content = JsonContent.Create(dto)
            };

            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim('"'));
            }

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error actualizando: {await response.Content.ReadAsStringAsync()}");
            }
        }

        // --- ESTE ES EL MÉTODO QUE AHORA SOPORTA TODOS LOS FILTROS ---
        public async Task<List<OrdenSimpleDto>> GetUltimasOrdenesAsync(string? filtro, DateTime? fechaDesde = null, DateTime? fechaHasta = null, string? estado = null, string? tipo = null)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(filtro))
                queryParams.Add($"filtro={Uri.EscapeDataString(filtro)}");

            if (fechaDesde.HasValue)
                queryParams.Add($"fechaDesde={fechaDesde.Value:yyyy-MM-dd}");

            if (fechaHasta.HasValue)
                queryParams.Add($"fechaHasta={fechaHasta.Value:yyyy-MM-dd}");

            if (!string.IsNullOrWhiteSpace(estado))
                queryParams.Add($"estado={Uri.EscapeDataString(estado)}");

            if (!string.IsNullOrWhiteSpace(tipo)) // AGREGAMOS ESTO
                queryParams.Add($"tipo={Uri.EscapeDataString(tipo)}");

            string url = "api/OrdenesDeServicio/lista";
            if (queryParams.Any())
            {
                url += "?" + string.Join("&", queryParams);
            }

            var response = await _http.GetAsync(url);
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());

            return await response.Content.ReadFromJsonAsync<List<OrdenSimpleDto>>() ?? new();
        }

        public async Task<OrdenDetalleDto?> GetDetalleAsync(int id)
        {
            var response = await _http.GetAsync($"api/OrdenesDeServicio/detalle/{id}");
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
            return await response.Content.ReadFromJsonAsync<OrdenDetalleDto>();
        }

        public async Task EliminarOrdenAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/OrdenesDeServicio/{id}");
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var response = await _http.GetAsync("api/OrdenesDeServicio/dashboard");
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
            return await response.Content.ReadFromJsonAsync<DashboardDto>() ?? throw new Exception("Dashboard vacío");
        }

        public async Task<List<dynamic>> GetTodasSimplesAsync()
        {
            var response = await _http.GetAsync("api/OrdenesDeServicio/todas-simples");
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
            return await response.Content.ReadFromJsonAsync<List<dynamic>>() ?? new();
        }

        public async Task<string?> GenerarLinkDePagoAsync(int id)
        {
            var response = await _http.PostAsync($"api/OrdenesDeServicio/{id}/crear-pago", null);
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("urlDePago").GetString();
        }

        public async Task<MetricasDto> GetMetricasAsync()
        {
            var response = await _http.GetAsync("api/OrdenesDeServicio/metricas-operativas");
            if (!response.IsSuccessStatusCode) return new MetricasDto();
            return await response.Content.ReadFromJsonAsync<MetricasDto>() ?? new MetricasDto();
        }
    }
}