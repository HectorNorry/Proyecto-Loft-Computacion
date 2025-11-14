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
        const string EMERGENCY_CONNECTION_STRING =
            "Server=tcp:sqlloftcomputacion.database.windows.net,1433;Database=db-loftcomputacion;User ID=TU_USUARIO;Password=TU_PASSWORD;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? EMERGENCY_CONNECTION_STRING;

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("La cadena de conexión no está configurada y la de emergencia no fue provista.");
        }

        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // ✅ Aquí añadimos la política de reintentos
        builder.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        });

        return new ApplicationDbContext(builder.Options);
    }
}