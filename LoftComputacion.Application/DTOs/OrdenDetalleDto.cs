using System;
using System.Collections.Generic;

namespace LoftComputacion.Application.DTOs
{
    public class OrdenDetalleDto
    {
        public int Id { get; set; }
        public DateTime FechaIngreso { get; set; }

        public string ClienteNombre { get; set; } = string.Empty;
        public string EmailCliente { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;

        public decimal? PrecioPresupuestado { get; set; }
        public decimal? PrecioFinal { get; set; }

        public string EquipoModelo { get; set; } = string.Empty;

        public string FallaDeclarada { get; set; } = string.Empty;
        public string? ResumenTecnico { get; set; }

        public List<FotoDto> Fotos { get; set; } = new();

        public List<HistorialLineaDto> Historial { get; set; } = new();

        public string EquipoTipo { get; set; } = string.Empty;
        public string EquipoDescripcion { get; set; } = string.Empty;

    }
}
