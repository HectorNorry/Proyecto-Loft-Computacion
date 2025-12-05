using System.Net.Http.Json;
using System.Text.Json;
using LoftComputacion.Shared.DTOs;

namespace LoftComputacion.BlazorApp.Services.Auth // O el namespace que corresponda
{
    public class UsuariosApiService
    {
        private readonly HttpClient _http;

        // Configuración para leer JSON sin importar mayúsculas/minúsculas
        private readonly JsonSerializerOptions _options;

        public UsuariosApiService(HttpClient http)
        {
            _http = http;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<UsuarioDto>?> GetUsuarios()
        {
            // Usamos las opciones para asegurar que lea los datos aunque el nombre varíe un poco
            return await _http.GetFromJsonAsync<List<UsuarioDto>>("api/usuarios", _options);
        }

        public Task<UsuarioDto?> GetUsuario(int id)
            => _http.GetFromJsonAsync<UsuarioDto>($"api/usuarios/{id}", _options);

        public Task<HttpResponseMessage> Crear(CreateUsuarioDto dto)
        {
            // CORRECCIÓN: La ruta en el controller es "api/usuarios/crear"
            return _http.PostAsJsonAsync("api/usuarios/crear", dto);
        }

        public Task<HttpResponseMessage> Editar(int id, UpdateUsuarioDto dto)
        {
            // CORRECCIÓN: La ruta en el controller es "api/usuarios/editar/{id}"
            return _http.PutAsJsonAsync($"api/usuarios/editar/{id}", dto);
        }

        public Task<HttpResponseMessage> Eliminar(int id)
        {
            // Esta ruta es correcta (api/usuarios/{id}) para el DELETE
            return _http.DeleteAsync($"api/usuarios/{id}");
        }

        // Método de compatibilidad por si alguna vista vieja lo llama (redirige a eliminar o hace nada)
        public async Task<HttpResponseMessage> CambiarEstado(int id)
        {
            // Como eliminamos la lógica de "Estado", asumimos que esto es para bloquear/eliminar
            // O simplemente retornamos OK si queremos ignorarlo.
            // Por ahora, lo redirigimos a Eliminar para mantener consistencia con la UI nueva.
            return await Eliminar(id);
        }
    }
}