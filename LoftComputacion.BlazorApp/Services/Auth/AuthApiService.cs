using System.Net.Http.Json;
using LoftComputacion.BlazorApp.DTOs;


namespace LoftComputacion.BlazorApp.Services.Auth
{
    public class AuthApiService
    {
        private readonly HttpClient _http;

        public AuthApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var body = new LoginRequest
            {
                NombreUsuario = email,
                Password = password
            };

            var response = await _http.PostAsJsonAsync("api/Auth/login", body);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error login API: {response.StatusCode} - {error}");
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result?.Token;
        }
    }
}
