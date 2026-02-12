namespace LoftComputacion.Domain
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }

        // --- NUEVOS CAMPOS PARA RECUPERACIÓN ---
        public string? TokenRecuperacion { get; set; } // El código secreto
        public DateTime? TokenRecuperacionExpiracion { get; set; } // Cuándo vence
    }
}