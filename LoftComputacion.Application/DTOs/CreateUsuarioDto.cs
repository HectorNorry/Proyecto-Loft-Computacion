// LoftComputacion.Application/DTOs/CreateUsuarioDto.cs

namespace LoftComputacion.Application.DTOs
{
    public class CreateUsuarioDto
    {
        // Campos que el cliente envía al crear el usuario
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}