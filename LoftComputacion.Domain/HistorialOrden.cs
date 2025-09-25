using LoftComputacion.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LoftComputacion.Domain
{
    public class HistorialOrden
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string DescripcionDelCambio { get; set; } // Ej: "Estado cambiado de 'Recibido' a 'Esperando Aprobación'."

        // Relaciones
        public int OrdenDeServicioId { get; set; }
        public OrdenDeServicio OrdenDeServicio { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}