using System;

namespace LoftComputacion.Shared.DTOs;

    public class TopClienteDto
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal TotalGenerado { get; set; }
        public int CantidadOrdenes { get; set; }
    }

