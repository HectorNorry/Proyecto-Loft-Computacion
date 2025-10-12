namespace LoftComputacion.WebAPI.DTOs
{
    public class CreateUsuarioDto
    {
        public string NombreCompleto { get; set; }
        public string Password { get; set; } // Recibimos la contraseña en texto plano
        public string Rol { get; set; }
    }
}