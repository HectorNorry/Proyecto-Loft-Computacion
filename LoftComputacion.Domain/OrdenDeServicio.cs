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
        public string DescripcionProblema { get; set; }
        public string? RutaFoto { get; set; } // La foto es opcional

        // Campos de pago (pueden ser nulos hasta que se entrega)
        public decimal? Precio { get; set; }
        public DateTime? FechaPago { get; set; }

        // --- Relaciones con otras tablas ---

        // Relación con Cliente
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } // Propiedad de navegación

        // Relación con Estado
        public int EstadoId { get; set; }
        public Estado Estado { get; set; } // Propiedad de navegación

        // Relación con MetodoDePago (es opcional hasta el pago)
        public int? MetodoDePagoId { get; set; }
        public MetodoDePago? MetodoDePago { get; set; } // Propiedad de navegación
    }
}
