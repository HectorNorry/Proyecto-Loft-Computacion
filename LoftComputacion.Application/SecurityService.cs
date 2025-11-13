using DocumentFormat.OpenXml.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftComputacion.Application
{
    // LoftComputacion.Application/SecurityService.cs (o donde hayas puesto ISecurityService)

    // Asegúrate de que esta clase implemente la interfaz que creaste:
    public class SecurityService : ISecurityService
    {
        // Implementación simple de ejemplo (¡usa una librería robusta como BCrypt en producción!)
        public string HashPassword(string password)
        {
            // Usar un hasheador de contraseñas real (ej. BCrypt.Net)
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPasswordHash(string password, string storedHash)
        {
            // Verificar el hash
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
