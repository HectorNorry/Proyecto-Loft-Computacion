using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftComputacion.Domain
{
    public class OrdenDeServicio
    {
        public int Id { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string FallaDeclaradaPorCliente { get; set; } // Descripción breve del cliente
        public DateTime? FechaNotificacionRetiro { get; set; } // Para la regla de los 3 meses

        // --- Campos de Precios ---
        public decimal? PrecioPresupuestado { get; set; }
        public decimal? PrecioFinal { get; set; }

        public DateTime? FechaPago { get; set; }

        // --- Relaciones con otras tablas ---
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public int EstadoId { get; set; }
        public Estado Estado { get; set; }

        public int? MetodoDePagoId { get; set; }
        public MetodoDePago? MetodoDePago { get; set; }

        public int EquipoId { get; set; } // Nueva relación con el equipo
        public Equipo Equipo { get; set; }

        // Colecciones de entidades relacionadas
        public ICollection<Foto> Fotos { get; set; } // Una orden puede tener muchas fotos
        public ICollection<HistorialOrden> Historial { get; set; } // Una orden tiene un historial de cambios
    }
}
