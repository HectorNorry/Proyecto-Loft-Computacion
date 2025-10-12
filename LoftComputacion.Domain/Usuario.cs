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
        public string PasswordHash { get; set; } = string.Empty; // Guardaremos la contraseña encriptada, no el texto plano
        public string Rol { get; set; } = string.Empty; // Ej: "Administrador", "Técnico"
    }
}