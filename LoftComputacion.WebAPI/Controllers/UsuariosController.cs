using BCrypt.Net;
using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using LoftComputacion.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoftComputacion.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UsuariosController(ApplicationDbContext context, IConfiguration configuration) 
        {
            _context = context;
            _configuration = configuration; // <-- AHORA 'configuration' SÍ EXISTE
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }
        // POST: api/usuarios
        // POST: api/usuarios
        [HttpPost]
        public async Task<IActionResult> CreateUsuario([FromBody] CreateUsuarioDto usuarioDto)
        {
            // 1. Verificamos que la contraseña no esté vacía
            if (string.IsNullOrWhiteSpace(usuarioDto.Password))
            {
                return BadRequest("La contraseña es requerida.");
            }

            // 2. Creamos el hash seguro usando BCrypt
            // Esto genera automáticamente un "salt" (valor aleatorio) y lo incluye en el hash.
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Password);

            var nuevoUsuario = new Usuario
            {
                NombreCompleto = usuarioDto.NombreCompleto,
                Email = usuarioDto.Email,
                PasswordHash = passwordHash, // Guardamos el hash real, no el texto plano
                Rol = usuarioDto.Rol
            };

            await _context.Usuarios.AddAsync(nuevoUsuario);
            await _context.SaveChangesAsync();

            // 3. Creamos una respuesta segura (NUNCA devolvemos el hash)
            var respuestaUsuario = new
            {
                nuevoUsuario.Id,
                nuevoUsuario.NombreCompleto,
                nuevoUsuario.Rol
            };

            // Usamos nameof(GetUsuario) para que la URL de respuesta sea correcta
            return CreatedAtAction(nameof(GetUsuario), new { id = nuevoUsuario.Id }, respuestaUsuario);
        }
        // PUT: api/usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            // No devolvemos el hash de la contraseña
            var respuestaUsuario = new
            {
                usuario.Id,
                usuario.NombreCompleto,
                usuario.Rol
            };

            return Ok(respuestaUsuario);
        }

        // POST: api/usuarios/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>
                u.NombreCompleto == loginDto.NombreUsuario ||
                u.Email == loginDto.NombreUsuario
            );

            if (usuario == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            bool esPasswordValida = BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.PasswordHash);

            if (!esPasswordValida)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            // --- ¡CAMBIO IMPORTANTE! ---
            // En lugar de devolver solo los datos del usuario, generamos y devolvemos un token.
            var token = GenerarJwtToken(usuario);

            return Ok(new { token = token }); // Devolvemos un objeto JSON que contiene el token
        }

        private string GenerarJwtToken(Usuario usuario)
        {
            // 1. Leemos la clave secreta y el emisor desde appsettings.json
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new InvalidOperationException("Configuración de JWT incompleta en appsettings.json");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 2. Creamos los "Claims" (información que guardamos dentro del token)
            // Guardamos el ID del usuario, su nombre y su Rol.
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()), // El "Sujeto" del token
        new Claim(JwtRegisteredClaimNames.Name, usuario.NombreCompleto),
        new Claim(ClaimTypes.Role, usuario.Rol), // El Rol (importante para permisos)
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Un ID único para el token
    };

            // 3. Creamos el token
            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(8), // El token será válido por 8 horas
                signingCredentials: credentials);

            // 4. Lo convertimos a un string y lo devolvemos
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}