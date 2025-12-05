using LoftComputacion.Application;    // Aquí vive UsuarioService
using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using LoftComputacion.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoftComputacion.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        // CORRECCIÓN: Usamos UsuarioService (Singular)
        private readonly UsuarioService _usuarioService;
        private readonly ApplicationDbContext _context;

        // Constructor corregido
        public UsuariosController(UsuarioService usuarioService, ApplicationDbContext context)
        {
            _usuarioService = usuarioService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarios = await _usuarioService.GetAllUsuariosAsync();

            // Proyección segura para asegurar que los nombres viajan bien
            var resultado = usuarios.Select(u => new
            {
                id = u.Id,
                nombreCompleto = u.NombreCompleto,
                email = u.Email,
                rol = u.Rol,
                estaActivo = true // Hardcodeado en true porque ya borramos la columna y todos son activos por defecto
            });

            return Ok(resultado);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear(CreateUsuarioDto dto)
        {
            var result = await _usuarioService.CreateUsuarioAsync(dto); // Tu método se llama CreateUsuarioAsync
            if (!result.Succeeded) return BadRequest(result.Errors);
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Editar(int id, UpdateUsuarioDto dto)
        {
            var result = await _usuarioService.UpdateUsuarioAsync(id, dto); // Tu método se llama UpdateUsuarioAsync
            if (!result) return NotFound();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("El usuario no existe.");

            if (usuario.Email == "admin@admin.com")
                return BadRequest("No se puede eliminar al Super Admin.");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}