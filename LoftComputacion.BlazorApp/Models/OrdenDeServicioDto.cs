namespace LoftComputacion.BlazorApp.Models
{
    public class OrdenDeServicioDto
    {
        public int Id { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public string EquipoNombre { get; set; } = string.Empty;
        public string EstadoNombre { get; set; } = string.Empty;
        public DateTime FechaIngreso { get; set; }
        public string FallaDeclaradaPorCliente { get; set; } = string.Empty;
        public decimal? PrecioPresupuestado { get; set; }
    }
}
