using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftComputacion.Domain
{
    public class Foto
    {
        public int Id { get; set; }
        public string RutaArchivo { get; set; } = string.Empty;

        // Relación con la orden de servicio
        public int OrdenDeServicioId { get; set; }
        public OrdenDeServicio OrdenDeServicio { get; set; } = null!;
    }
}
