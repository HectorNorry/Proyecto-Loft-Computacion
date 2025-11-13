// LoftComputacion.Application/DTOs/OrdenListaDto.cs

namespace LoftComputacion.BlazorApp.DTOs
{
    public class OrdenListaDto
    {
        public int Id { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string FallaDeclaradaPorCliente { get; set; } = string.Empty;

        // Datos aplanados de las tablas relacionadas
        public string? NombreCliente { get; set; }
        public string? NombreEstado { get; set; }
        public string? ModeloEquipo { get; set; }

        public decimal? PrecioFinal { get; set; }
    }
}