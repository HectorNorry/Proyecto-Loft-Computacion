namespace LoftComputacion.Application.DTOs
{
    public class ClienteEditarDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? DNI { get; set; }
    }
}
