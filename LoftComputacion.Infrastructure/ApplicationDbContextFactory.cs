// LoftComputacion.Infrastructure/ApplicationDbContextFactory.cs

using LoftComputacion.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // 🚨 1. CADENA DE CONEXIÓN DE EMERGENCIA 🚨
        // Usa la cadena de conexión de tu Development.json o la que usas localmente.
        const string EMERGENCY_CONNECTION_STRING = "Server=tcp:sqlloftcomputacion.database.windows.net,1433;Database=db-loftcomputacion;... [AÑADE EL RESTO DE TU CADENA DE CONEXIÓN, USUARIO Y CONTRASEÑA]";

        // 2. Intentar cargar la configuración real (lo que siempre falla)
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true) // ❗ HACEMOS OPTIONAL=TRUE
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        // 3. Obtener la cadena: Si no la encuentra, usa la de emergencia
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? EMERGENCY_CONNECTION_STRING;

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("La cadena de conexión no está configurada y la de emergencia no fue provista.");
        }

        // 4. Configurar DbContextOptions:
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseSqlServer(connectionString);

        return new ApplicationDbContext(builder.Options);
    }
}