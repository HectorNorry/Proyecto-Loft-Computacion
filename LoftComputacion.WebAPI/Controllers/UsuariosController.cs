using LoftComputacion.Application;
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
        private readonly UsuarioService _usuarioService;
        private readonly ApplicationDbContext _context;

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
                activo = u.Activo // <--- AHORA LEE EL ESTADO REAL DE LA DB
            });

            return Ok(resultado);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear(CreateUsuarioDto dto)
        {
            var result = await _usuarioService.CreateUsuarioAsync(dto);
            if (!result.Succeeded) return BadRequest(result.Errors);
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Editar(int id, UpdateUsuarioDto dto)
        {
            var result = await _usuarioService.UpdateUsuarioAsync(id, dto);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("El usuario no existe.");

            // Protección vital: No dejar a la app sin el admin principal
            if (usuario.Email == "admin@admin.com")
                return BadRequest("No se puede dar de baja al Super Admin.");

            // <--- LA MAGIA DEL SOFT DELETE --->
            var exito = await _usuarioService.ToggleUsuarioStatusAsync(id);

            if (!exito)
                return BadRequest("Hubo un error al cambiar el estado del usuario.");

            return Ok();
        }
    }
}