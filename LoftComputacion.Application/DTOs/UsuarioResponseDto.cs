// LoftComputacion.Application/DTOs/UsuarioResponseDto.cs

namespace LoftComputacion.Application.DTOs
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public bool EstaActivo { get; set; } // Añadido
        public DateTime FechaCreacion { get; set; } // Añadido
    }
}