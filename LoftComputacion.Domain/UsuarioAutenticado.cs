namespace LoftComputacion.Domain
{
    // Esta clase representa al usuario que ha iniciado sesión
    public class UsuarioAutenticado
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}