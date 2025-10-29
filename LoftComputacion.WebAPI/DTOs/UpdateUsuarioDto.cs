namespace LoftComputacion.WebAPI.DTOs
{
    public class UpdateUsuarioDto
    {
        // Nota: El Id no se incluye aquí, viene de la URL.
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string? Password { get; set; } // La contraseña es opcional al actualizar
    }
}