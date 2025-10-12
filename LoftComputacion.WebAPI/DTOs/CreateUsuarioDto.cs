namespace LoftComputacion.WebAPI.DTOs
{
    public class CreateUsuarioDto
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;   // Recibimos la contraseña en texto plano
        public string Rol { get; set; } = string.Empty; 
    }   
}