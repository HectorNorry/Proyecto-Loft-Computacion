// LoftComputacion.BlazorApp/DTOs/GenerarResumenDto.cs (Debe coincidir con la estructura del Backend)

namespace LoftComputacion.BlazorApp.DTOs
{
    public class GananciaReporteDto
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public decimal IngresosTotales { get; set; }
        public int OrdenesCompletadas { get; set; }
        public decimal Costos { get; set; }

        // Propiedad calculada:
        public decimal GananciaNeta => IngresosTotales - Costos;
    }
}