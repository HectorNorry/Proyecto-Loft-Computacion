// LoftComputacion.BlazorApp/Services/UsuarioApiService.cs
using System.Net.Http.Json;
using LoftComputacion.BlazorApp.DTOs; // Usamos tu carpeta DTOs

public class UsuarioApiService
{
    private readonly HttpClient _httpClient;

    public UsuarioApiService(HttpClient httpClient)
    {
        // Este HttpClient ya tiene el token inyectado por AuthorizedHandler
        _httpClient = httpClient;
    }

    // --- C - CREAR ---
    public async Task<bool> CrearUsuarioAsync(UsuarioCreacionDTO usuario)
    {
        var response = await _httpClient.PostAsJsonAsync("api/usuarios", usuario);
        return response.IsSuccessStatusCode;
    }

    // --- R - LEER (Todos) ---
    public async Task<List<UsuarioDTO>?> GetUsuariosAsync()
    {
        var usuarios = await _httpClient.GetFromJsonAsync<List<UsuarioDTO>>("api/usuarios");
        return usuarios;
    }

    // --- U - ACTUALIZAR ---
    public async Task<bool> ActualizarUsuarioAsync(int id, UsuarioDTO usuario)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/usuarios/{id}", usuario);
        return response.IsSuccessStatusCode;
    }

    // --- D - ELIMINAR (Desactivar) ---
    // Usaremos Delete para simplicidad, aunque en la vida real se recomienda PATCH para desactivar.
    public async Task<bool> EliminarUsuarioAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/usuarios/{id}");
        return response.IsSuccessStatusCode;
    }
}