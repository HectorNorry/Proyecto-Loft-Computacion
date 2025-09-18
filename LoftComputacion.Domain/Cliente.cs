using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftComputacion.Domain
{
    public class Cliente
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Telefono { get; set; }
        public string? Email { get; set; } // El '?' indica que el email puede ser opcional (nulo)
    }
}
