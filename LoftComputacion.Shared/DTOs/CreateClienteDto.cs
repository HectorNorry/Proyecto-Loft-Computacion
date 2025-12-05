namespace LoftComputacion.Shared.DTOs;

    public class CreateClienteDto
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? DNI { get; set; }
    }
