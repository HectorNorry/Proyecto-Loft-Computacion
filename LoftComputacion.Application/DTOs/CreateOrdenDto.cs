using LoftComputacion.Domain;

namespace LoftComputacion.Application.DTOs
{
    public class CreateOrdenDto
    {
        public int ClienteId { get; set; }

        // Datos del equipo
        public TipoDeEquipo TipoEquipo { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string NumeroSerie { get; set; } = string.Empty;
        public string DescripcionCompleta { get; set; } = string.Empty;

        // Datos de la orden
        public string FallaDeclaradaPorCliente { get; set; } = string.Empty;
        public decimal? PrecioPresupuestado { get; set; }
        public int? MetodoDePagoId { get; set; }
    }
}
