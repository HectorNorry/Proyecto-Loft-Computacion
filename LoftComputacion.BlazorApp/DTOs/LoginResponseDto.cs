using System.Text.Json.Serialization; 

namespace LoftComputacion.BlazorApp.DTOs
{
    public class LoginResponseDto
    {
        // Le dice al serializador: "Busca 'token' (minúscula) en el JSON
        // y ponlo en esta propiedad 'Token' (mayúscula)"
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombreCompleto")]
        public string NombreCompleto { get; set; } = string.Empty;

        // --- ¡ESTA ES LA LÍNEA CLAVE! ---
        [JsonPropertyName("rol")]
        public string Rol { get; set; } = string.Empty;
    }
}