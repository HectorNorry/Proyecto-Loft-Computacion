namespace LoftComputacion.Shared.DTOs
{
    public class RecuperarPasswordDto
    {
        public string Email { get; set; } = string.Empty;
    }

    public class RestablecerPasswordDto
    {
        public string Token { get; set; } = string.Empty;
        public string NuevaPassword { get; set; } = string.Empty;
    }
}