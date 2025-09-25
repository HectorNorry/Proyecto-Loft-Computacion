using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftComputacion.Domain
{
    public enum TipoDeEquipo
    {
        Notebook,
        PC_Escritorio,
        Impresora
    }

    public class Equipo
    {
        public int Id { get; set; }
        public TipoDeEquipo Tipo { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Componentes { get; set; } // Para describir RAM, CPU, etc.
        public string? NumeroDeSerie { get; set; }
    }
}