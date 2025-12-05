using System;
using System.Collections.Generic;

namespace LoftComputacion.Shared.DTOs;

    public class DashboardDto
    {
        public int OrdenesHoy { get; set; }
        public int EnTaller { get; set; }
        public int Finalizadas { get; set; }
        public decimal Ingresos { get; set; }

        // Estado -> Cantidad
        public Dictionary<string, int> Estados { get; set; } = new();

        // Estado -> Color (hex)
        public Dictionary<string, string> EstadoColores { get; set; } = new();

        // Últimas órdenes para el bloque "Órdenes recientes"
        public List<OrdenSimpleDto> UltimasOrdenes { get; set; } = new();

        // Bloque de alertas del taller
        public DashboardAlertas Alertas { get; set; } = new();

        // Top clientes del mes
        public List<TopClienteDto> TopClientes { get; set; } = new();
    }

