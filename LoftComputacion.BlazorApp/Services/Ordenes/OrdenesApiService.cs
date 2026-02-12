using Blazored.LocalStorage;
using LoftComputacion.BlazorApp.Services.Auth;
using LoftComputacion.Shared.DTOs;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http;
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

        // ============================
        // SUBIR FOTO (CORREGIDO Y RETORNA DATO)
        // ============================
        public async Task<FotoDto> SubirFotoAsync(int idOrden, IBrowserFile archivo)
        {
            using var content = new MultipartFormDataContent();

            // Límite 15MB
            var fileContent = new StreamContent(archivo.OpenReadStream(maxAllowedSize: 15 * 1024 * 1024));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(archivo.ContentType);

            content.Add(fileContent, "archivo", archivo.Name);

            var request = new HttpRequestMessage(HttpMethod.Post, $"api/OrdenesDeServicio/{idOrden}/fotos");
            request.Content = content;

            // --- INYECCIÓN MANUAL DE TOKEN ---
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                token = token.Trim('"');
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            // ---------------------------------

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            // Leemos la respuesta para devolver la URL y el ID y actualizar la vista
            // Asumimos que la API devuelve un objeto con { url: "...", fotoId: 123 }
            return await response.Content.ReadFromJsonAsync<FotoDto>()
                   ?? throw new Exception("La API no devolvió los datos de la foto.");
        }

        // ============================
        // BORRAR FOTO (NUEVO)
        // ============================
        public async Task BorrarFotoAsync(int idFoto)
        {
            // Usamos la ruta que tenías en tu Razor: api/OrdenesDeServicio/fotos/{id}
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/OrdenesDeServicio/fotos/{idFoto}");

            // --- INYECCIÓN MANUAL DE TOKEN ---
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                token = token.Trim('"');
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            // ---------------------------------

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        // ... EL RESTO DE MÉTODOS IGUAL QUE ANTES ...

        public async Task CrearOrdenAsync(CreateOrdenDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/OrdenesDeServicio", dto);
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<List<OrdenSimpleDto>> GetUltimasOrdenesAsync(string? filtro)
        {
            string url = string.IsNullOrWhiteSpace(filtro) ? "api/OrdenesDeServicio/lista" : $"api/OrdenesDeServicio/lista?filtro={Uri.EscapeDataString(filtro)}";
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