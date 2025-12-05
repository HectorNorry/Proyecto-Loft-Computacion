namespace LoftComputacion.Shared.DTOs;

    public class UpdateOrdenDto
    {
        public int EstadoId { get; set; }
        public decimal? PrecioPresupuestado { get; set; }
        public decimal? PrecioFinal { get; set; }
        public int UsuarioId { get; set; }

        public string? ResumenTecnico { get; set; }

        public string? UsuarioAutorizador { get; set; } // El que puso la contraseña en el popup
}
