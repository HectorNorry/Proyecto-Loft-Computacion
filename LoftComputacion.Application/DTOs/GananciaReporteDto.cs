// LoftComputacion.Application/DTOs/GananciaReporteDto.cs
namespace LoftComputacion.Application.DTOs
{
    public class GananciaReporteDto
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal IngresosTotales { get; set; }
        public int OrdenesPagadas { get; set; }
        public decimal GananciaNeta => IngresosTotales; // Simplificado, sin costos
    }
}