// LoftComputacion.Application/UsuarioService.cs
// ¡Nota! Tu proyecto WebAPI debe referenciar a este proyecto Application.

using LoftComputacion.Application;
using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using Microsoft.EntityFrameworkCore; // Necesario para Async y EF Core
using LoftComputacion.Shared.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Clase de resultado para el registro (similar a la que definimos antes)
public class UsuarioResult
{
    public bool Succeeded { get; set; }
    public int? UserId { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}

public class UsuarioService // Sin interfaz, implementamos la clase concreta
{
    // Asumimos que esta es la forma en que accedes a la base de datos
    private readonly ApplicationDbContext _context;
    // Asumimos que tienes un servicio para el hashing de contraseñas
    private readonly ISecurityService _securityService;

    // Constructor para la inyección de las dependencias (el contexto de la DB y la seguridad)
    public UsuarioService(ApplicationDbContext context, ISecurityService securityService)
    {
        _context = context;
        _securityService = securityService;
    }

    // -----------------------------------------------------
    // C - Crear (Registrar)
    // -----------------------------------------------------
    public async Task<UsuarioResult> CreateUsuarioAsync(CreateUsuarioDto dto)
    {
        // 1. Validación de unicidad de email
        if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
        {
            return new UsuarioResult { Succeeded = false, Errors = new[] { "El email ya está registrado." } };
        }

        // 2. Hash de la Contraseña
        string passwordHash = _securityService.HashPassword(dto.Password);

        // 3. Crear Entidad
        var usuario = new Usuario
        {
            NombreCompleto = dto.NombreCompleto,
            Email = dto.Email,
            PasswordHash = passwordHash, // Guardar el hash
            Rol = dto.Rol,
            FechaCreacion = DateTime.UtcNow
        };

        // 4. Guardar en DB
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return new UsuarioResult { Succeeded = true, UserId = usuario.Id };
    }

    // -----------------------------------------------------
    // R - Leer (Listar todos)
    // -----------------------------------------------------
    public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
    {
        return await _context.Usuarios
            .IgnoreQueryFilters()
            .AsNoTracking()
            .ToListAsync();
    }

    // -----------------------------------------------------
    // U - Actualizar
    // -----------------------------------------------------
    public async Task<bool> UpdateUsuarioAsync(int id, UpdateUsuarioDto dto)
    {
        // 1. Buscar el usuario usando el 'id' que viene de la URL.
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id); // <-- Usamos el 'id' de la URL

        if (usuario == null) return false;

        // Los errores de propiedades faltantes (EstaActivo, NewPassword)
        // ahora deberían desaparecer al compilar.

        // Mapear solo los campos editables
        usuario.NombreCompleto = dto.NombreCompleto;
        usuario.Email = dto.Email;
        usuario.Rol = dto.Rol;

        // Si se incluye una nueva contraseña en el DTO (opcional), hashearla
        if (!string.IsNullOrEmpty(dto.NewPassword)) // Ahora existe en el DTO
        {
            usuario.PasswordHash = _securityService.HashPassword(dto.NewPassword);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    // -----------------------------------------------------
    // D - Desactivar/Activar (Eliminación Lógica)
    // -----------------------------------------------------
    
}