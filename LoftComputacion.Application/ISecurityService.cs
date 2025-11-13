using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftComputacion.Application
{
    // LoftComputacion.Application/ISecurityService.cs

public interface ISecurityService
    {
        // Método para hashear una contraseña (usado al crear usuario)
        string HashPassword(string password);

        // Método para verificar el hash (usado al iniciar sesión, si aplica)
        bool VerifyPasswordHash(string password, string storedHash);
    }
}
