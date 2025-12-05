using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using LoftComputacion.Shared.DTOs;

namespace LoftComputacion.BlazorApp.Services.Clientes
{
    public class ClientesApiService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public ClientesApiService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        // ============================================================
        // Helper para agregar el token
        // ============================================================
        private async Task AddTokenAsync(HttpRequestMessage request)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // ============================================================
        // LISTA
        // ============================================================
        public async Task<List<ClienteDto>> GetClientesAsync(string? filtro = null)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                string.IsNullOrWhiteSpace(filtro)
                    ? "api/clientes"
                    : $"api/clientes?filtro={filtro}"
            );

            await AddTokenAsync(request);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Error al obtener clientes.");

            return await response.Content.ReadFromJsonAsync<List<ClienteDto>>() ?? new();
        }

        // ============================================================
        // OBTENER UNO
        // ============================================================
        public async Task<ClienteDto?> GetClienteAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/clientes/{id}");
            await AddTokenAsync(request);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ClienteDto>();
        }

        // ============================================================
        // CREAR
        // ============================================================
        public async Task CrearClienteAsync(ClienteCrearDto dto)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/clientes")
            {
                Content = JsonContent.Create(dto)
            };

            await AddTokenAsync(request);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception("No se pudo crear el cliente.");
        }

        // ============================================================
        // EDITAR
        // ============================================================
        public async Task EditarClienteAsync(int id, ClienteEditarDto dto)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/clientes/{id}")
            {
                Content = JsonContent.Create(dto)
            };

            await AddTokenAsync(request);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception("No se pudo editar el cliente.");
        }

        // ============================================================
        // ELIMINAR
        // ============================================================
        public async Task EliminarClienteAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/clientes/{id}");

            await AddTokenAsync(request);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception("No se pudo eliminar el cliente.");
        }
    }
}
