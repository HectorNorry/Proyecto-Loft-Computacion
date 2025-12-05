// LoftComputacion.Application/DTOs/UsuarioResponseDto.cs

namespace LoftComputacion.Shared.DTOs;

    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; } // Añadido
    }
