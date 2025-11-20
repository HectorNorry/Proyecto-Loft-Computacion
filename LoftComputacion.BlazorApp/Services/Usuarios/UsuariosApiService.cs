using System.Net.Http.Json;
using LoftComputacion.Application.DTOs;

public class UsuariosApiService
{
    private readonly HttpClient _http;

    public UsuariosApiService(HttpClient http)
    {
        _http = http;
    }

    public Task<List<UsuarioDto>?> GetUsuarios()
        => _http.GetFromJsonAsync<List<UsuarioDto>>("api/usuarios");

    public Task<UsuarioDto?> GetUsuario(int id)
        => _http.GetFromJsonAsync<UsuarioDto>($"api/usuarios/{id}");

    public Task<HttpResponseMessage> Crear(CreateUsuarioDto dto)
        => _http.PostAsJsonAsync("api/usuarios", dto);

    public Task<HttpResponseMessage> Editar(int id, UpdateUsuarioDto dto)
        => _http.PutAsJsonAsync($"api/usuarios/{id}", dto);

    public Task<HttpResponseMessage> Eliminar(int id)
        => _http.DeleteAsync($"api/usuarios/{id}");

    public Task<HttpResponseMessage> CambiarEstado(int id)
        => _http.PutAsync($"api/usuarios/{id}/estado", null);
}
