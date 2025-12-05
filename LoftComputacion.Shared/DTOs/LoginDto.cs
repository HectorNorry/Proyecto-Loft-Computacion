namespace LoftComputacion.Shared.DTOs;

    public class LoginDto
    {
        // Permitimos que inicie sesión con NombreCompleto o Email
        public string NombreUsuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
