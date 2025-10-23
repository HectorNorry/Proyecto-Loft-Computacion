using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftComputacion.Domain
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string? Email { get; set; } 
        public string PasswordHash { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}