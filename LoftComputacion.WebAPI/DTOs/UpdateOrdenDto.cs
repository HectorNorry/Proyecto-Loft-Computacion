namespace LoftComputacion.WebAPI.DTOs
{
    public class UpdateOrdenDto
    {
        public int EstadoId { get; set; }
        public decimal? PrecioPresupuestado { get; set; }
        public decimal? PrecioFinal { get; set; }
        public int UsuarioId { get; set; }

        public string? ResumenTecnico { get; set; }
    }
}