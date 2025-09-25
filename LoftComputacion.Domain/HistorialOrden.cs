using LoftComputacion.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

// En HistorialOrden.cs
namespace LoftComputacion.Domain
{
    public class HistorialOrden
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string DescripcionDelCambio { get; set; } = string.Empty;

        // Relaciones
        public int OrdenDeServicioId { get; set; }
        public OrdenDeServicio OrdenDeServicio { get; set; } = null!;

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}