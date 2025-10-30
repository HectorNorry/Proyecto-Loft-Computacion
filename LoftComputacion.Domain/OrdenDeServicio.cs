using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftComputacion.Domain
{
    public class OrdenDeServicio
    {
        // Inicializamos las colecciones a una lista vacía para evitar nulos.
        public OrdenDeServicio()
        {
            Fotos = new List<Foto>();
            Historial = new List<HistorialOrden>();
        }

        public int Id { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string FallaDeclaradaPorCliente { get; set; } = string.Empty; // Inicializamos a string vacío
        public DateTime? FechaNotificacionRetiro { get; set; }

        // --- Campos de Precios ---
        public decimal? PrecioPresupuestado { get; set; }
        public decimal? PrecioFinal { get; set; }
        public DateTime? FechaPago { get; set; }

        // --- Relaciones con otras tablas ---
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!; // El '!' le dice al compilador que confíe en que EF lo asignará.

        public int EstadoId { get; set; }
        public Estado Estado { get; set; } = null!;

        public int? MetodoDePagoId { get; set; }
        public MetodoDePago? MetodoDePago { get; set; }

        public int EquipoId { get; set; }
        public Equipo Equipo { get; set; } = null!;

        public string? ResumenTecnico { get; set; }

        // Colecciones de entidades relacionadas
        public ICollection<Foto> Fotos { get; set; }
        public ICollection<HistorialOrden> Historial { get; set; }
    }
}