using BCrypt.Net;
using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using LoftComputacion.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoftComputacion.WebAPI.Controllers
{
    [Route("api/[controller]")] // api/usuarios
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UsuarioService _usuarioService; // Servicio de la capa Application

        // 🚨 Constructor Corregido y Completo 🚨
        public UsuariosController(ApplicationDbContext context, IConfiguration configuration, UsuarioService usuarioService)
        {
            _context = context;
            _configuration = configuration;
            this._usuarioService = usuarioService;
        }

        // ----------------------------------------------------------------------
        // GET: api/usuarios (Listar todos - DEBE USAR EL SERVICIO)
        // ----------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            // Usamos el servicio que tiene IgnoreQueryFilters (más limpio)
            var usuarios = await _usuarioService.GetAllUsuariosAsync();
            return Ok(usuarios);
        }

        // ----------------------------------------------------------------------
        // POST: api/usuarios (Crear Usuario - DELEGA LA LÓGICA AL SERVICIO)
        // 🚨 Esto elimina la lógica duplicada y resuelve el conflicto 405 🚨
        // ----------------------------------------------------------------------
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUsuario([FromBody] CreateUsuarioDto usuarioDto)
        {
            var resultado = await _usuarioService.CreateUsuarioAsync(usuarioDto);

            if (resultado.Succeeded)
            {
                // Devolvemos CreatedAtAction usando el ID devuelto por el servicio
                return CreatedAtAction(nameof(GetUsuario), new { id = resultado.UserId }, null);
            }
            return BadRequest(resultado.Errors);
        }

        // ----------------------------------------------------------------------
        // POST: api/usuarios/login (Login - RUTA CORREGIDA)
        // ----------------------------------------------------------------------
        //[HttpPost("login")] // ⬅️ Usa la ruta más específica para evitar el conflicto 405
        //public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        //{
        //    var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>
        //        u.NombreCompleto == loginDto.NombreUsuario ||
        //        u.Email == loginDto.NombreUsuario
        //    );
        //
        //    if (usuario == null) { return Unauthorized("Usuario o contraseña incorrectos."); }
        //
        //    // ⚠️ La validación de seguridad (BCrypt.Verify) SÍ debe estar aquí ⚠️
        //    bool esPasswordValida = BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.PasswordHash);
        //
        //    if (!esPasswordValida) { return Unauthorized("Usuario o contraseña incorrectos."); }
        //
        //    var token = GenerarJwtToken(usuario);
        //
        //    return Ok(new
        //    {
        //        token = token,
        //        id = usuario.Id,
        //        nombreCompleto = usuario.NombreCompleto,
        //        rol = usuario.Rol
        //    });
        //}

        // ----------------------------------------------------------------------
        // PUT, DELETE, GET/{id} y GenerarJwtToken (Se mantienen limpios)
        // ----------------------------------------------------------------------

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UpdateUsuarioDto updateDto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) { return NotFound(); }

            usuario.NombreCompleto = updateDto.NombreCompleto;
            usuario.Email = updateDto.Email;
            usuario.Rol = updateDto.Rol;

            if (!string.IsNullOrWhiteSpace(updateDto.Password))
            {
                usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateDto.Password);
            }

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!_context.Usuarios.Any(e => e.Id == id)) { return NotFound(); } else { throw; } }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ToggleUsuarioStatus(int id)
        {
            bool resultado = await _usuarioService.DeactivateUsuarioAsync(id);

            if (resultado) { return NoContent(); }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null) { return NotFound(); }

            var respuestaUsuario = new
            {
                usuario.Id,
                usuario.NombreCompleto,
                usuario.Rol
            };

            return Ok(respuestaUsuario);
        }

        //private string GenerarJwtToken(Usuario usuario)
        //{
        //    // 1. Leemos la clave secreta y el emisor desde appsettings.json
        //    var jwtKey = _configuration["Jwt:Key"];
        //    var jwtIssuer = _configuration["Jwt:Issuer"];
        //    var jwtAudience = _configuration["Jwt:Audience"];
        //
        //    if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
        //    {
        //        throw new InvalidOperationException("Configuración de JWT incompleta en appsettings.json");
        //    }
        //
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //
        //    // 2. Creamos los "Claims" (información que guardamos dentro del token)
        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
        //        new Claim(JwtRegisteredClaimNames.Name, usuario.NombreCompleto),
        //        new Claim(ClaimTypes.Role, usuario.Rol),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    };
        //
        //    // 3. Creamos el token
        //    var token = new JwtSecurityToken(
        //        issuer: jwtIssuer,
        //        audience: jwtAudience,
        //        claims: claims,
        //        expires: DateTime.Now.AddHours(8),
        //        signingCredentials: credentials);
        //
        //    // 4. Lo convertimos a un string y lo devolvemos
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}