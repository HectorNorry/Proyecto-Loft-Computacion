using System;

namespace LoftComputacion.Application.DTOs
{
    public class HistorialLineaDto
    {
        public DateTime FechaHora { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }
}
