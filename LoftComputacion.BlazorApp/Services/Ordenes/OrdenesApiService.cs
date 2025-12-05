using Blazored.LocalStorage;
using LoftComputacion.BlazorApp.Services.Auth;
using LoftComputacion.Shared.DTOs;
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
        private readonly CustomAuthStateProvider _authProvider;

        public OrdenesApiService(
            HttpClient http,
            ILocalStorageService localStorage,
            CustomAuthStateProvider authProvider)
        {
            _http = http;
            _localStorage = localStorage;
            _authProvider = authProvider;
        }

        // ============================
        // CREAR ORDEN
        // ============================
        public async Task CrearOrdenAsync(CreateOrdenDto dto)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            var request = new HttpRequestMessage(HttpMethod.Post, "api/OrdenesDeServicio");
            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(dto);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear orden: {body}");
            }
        }

        // ============================
        // LISTA
        // ============================
        public async Task<List<OrdenSimpleDto>> GetUltimasOrdenesAsync(string? filtro)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            string url = string.IsNullOrWhiteSpace(filtro)
                ? "api/OrdenesDeServicio/lista"
                : $"api/OrdenesDeServicio/lista?filtro={Uri.EscapeDataString(filtro)}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al cargar órdenes: {body}");
            }

            return await response.Content.ReadFromJsonAsync<List<OrdenSimpleDto>>() ?? new();
        }

        // ============================
        // DETALLE
        // ============================
        public async Task<OrdenDetalleDto?> GetDetalleAsync(int id)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/OrdenesDeServicio/detalle/{id}");
            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Error al obtener detalle");

            return await response.Content.ReadFromJsonAsync<OrdenDetalleDto>();
        }

        // ============================
        // ELIMINAR
        // ============================
        public async Task EliminarOrdenAsync(int id)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/OrdenesDeServicio/{id}");
            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al eliminar orden: {body}");
            }
        }
        // ============================
        // DASHBOARD
        // ============================
        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            var request = new HttpRequestMessage(HttpMethod.Get, "api/OrdenesDeServicio/dashboard");

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener dashboard: {body}");
            }

            var dto = await response.Content.ReadFromJsonAsync<DashboardDto>();

            if (dto == null)
                throw new Exception("La API devolvió un dashboard vacío.");

            return dto;
        }

        public async Task<HttpResponseMessage> SendAuthorizedRequestAsync(HttpRequestMessage request)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await _http.SendAsync(request);
        }

        public async Task<int> GetUserIdAsync()
        {
            var auth = await _authProvider.GetAuthenticationStateAsync();
            var user = auth.User;

            var idClaim = user.FindFirst("sub")?.Value
                         ?? user.FindFirst("id")?.Value;

            if (int.TryParse(idClaim, out int userId))
                return userId;

            return 0;
        }

        // ============================
        // OBTENER TODAS LAS ÓRDENES (para reportes / ganancias)
        // ============================
        public async Task<List<OrdenDetalleDto>> GetTodasLasOrdenesAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "api/OrdenesDeServicio/todas"   // 🔴 Necesitamos crear este endpoint en la API
            );

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener órdenes: {body}");
            }

            return await response.Content.ReadFromJsonAsync<List<OrdenDetalleDto>>() ?? new();
        }

        public async Task<List<dynamic>> GetTodasSimplesAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            var request = new HttpRequestMessage(HttpMethod.Get, "api/OrdenesDeServicio/todas-simples");

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());

            return await response.Content.ReadFromJsonAsync<List<dynamic>>() ?? new();
        }

        // ============================
        // GENERAR LINK DE PAGO MP
        // ============================
        public async Task<string?> GenerarLinkDePagoAsync(int id)
        {
            // Usamos el endpoint que ya existe en el controlador (HttpPost("{id}/crear-pago"))
            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"api/OrdenesDeServicio/{id}/crear-pago");

            var response = await SendAuthorizedRequestAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al generar link de pago: {body}");
            }

            // La respuesta del backend es un objeto anónimo { urlDePago: '...' }
            var jsonBody = await response.Content.ReadAsStringAsync();

            try
            {
                // NOTA: Usaremos System.Text.Json para manejar la respuesta simple
                using var document = JsonDocument.Parse(jsonBody);
                var url = document.RootElement.GetProperty("urlDePago").GetString();
                return url;
            }
            catch
            {
                throw new Exception("Error al procesar la URL de pago recibida.");
            }
        }



        // Nota: Tu método SendAuthorizedRequestAsync ya se encarga de adjuntar el token, ¡es genial!
        // La definición de SendAuthorizedRequestAsync es:
        // public async Task<HttpResponseMessage> SendAuthorizedRequestAsync(HttpRequestMessage request) { ... }

        //METRICAS

        public async Task<MetricasDto?> GetMetricasAsync()
        {
            // Usamos SendAuthorizedRequestAsync para asegurar que lleve el token si el controller lo pide
            var request = new HttpRequestMessage(HttpMethod.Get, "api/OrdenesDeServicio/metricas-operativas");
            var response = await SendAuthorizedRequestAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<MetricasDto>();
            }
            return new MetricasDto(); // Retornamos vacío si falla para no romper la UI
        }


    }
}
