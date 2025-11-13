using BCrypt.Net;
using LoftComputacion.Application.DTOs;
using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoftComputacion.WebAPI.Controllers
{
    // La nueva ruta para el login es /api/auth
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // ---------------------------------------------------------
        // POST: api/auth/login (El endpoint que Blazor debe usar)
        // ---------------------------------------------------------
        [HttpPost("login")] // ⬅️ Usa la sub-ruta 'login'
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // 1. Buscar usuario por NombreUsuario o Email
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>
                u.NombreCompleto == loginDto.NombreUsuario ||
                u.Email == loginDto.NombreUsuario
            );

            // 2. Verificación de existencia
            if (usuario == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            // 3. Verificación de Contraseña (BCrypt)
            bool esPasswordValida = BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.PasswordHash);

            if (!esPasswordValida)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            // 4. Generación y retorno del token
            var token = GenerarJwtToken(usuario);

            return Ok(new
            {
                token = token,
                id = usuario.Id,
                nombreCompleto = usuario.NombreCompleto,
                rol = usuario.Rol
            });
        }

        // ---------------------------------------------------------
        // Generación del Token (MOVIDO DE UsuariosController)
        // ---------------------------------------------------------
        private string GenerarJwtToken(Usuario usuario)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new InvalidOperationException("Configuración de JWT incompleta en appsettings.json");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, usuario.NombreCompleto),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}