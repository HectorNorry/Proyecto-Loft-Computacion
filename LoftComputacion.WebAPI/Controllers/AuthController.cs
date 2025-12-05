using BCrypt.Net;
using LoftComputacion.Shared.DTOs;
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

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

            // -------------------------
            // LOGGING TEMPORAL DE DEBUG
            // -------------------------
            try
            {
                var dbHash = usuario.PasswordHash ?? "<NULL>";
                var dbHashLen = dbHash == "<NULL>" ? 0 : dbHash.Length;
                var dbHashBytes = System.Text.Encoding.ASCII.GetBytes(dbHash);
                var dbHashHex = BitConverter.ToString(dbHashBytes).Replace("-", "");

                Console.WriteLine($"[DEBUG-VERIFY] Email: {usuario.Email}");
                Console.WriteLine($"[DEBUG-VERIFY] Hash desde DB: [{dbHash}]");
                Console.WriteLine($"[DEBUG-VERIFY] Hash LEN: {dbHashLen}");
                Console.WriteLine($"[DEBUG-VERIFY] Hash HEX: {dbHashHex}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG-VERIFY] Error al preparar logging del hash: {ex.Message}");
            }
            // -------------------------

            bool esPasswordValida = false;
            try
            {
                esPasswordValida = BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.PasswordHash);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG-VERIFY] BCrypt.Verify lanzó excepción: {ex.Message}");
            }

            Console.WriteLine($"[DEBUG-VERIFY] Resultado Verify: {esPasswordValida}");

            if (!esPasswordValida)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            // 4. Generación y retorno del token
            string token;
            try
            {
                token = GenerarJwtToken(usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR-JWT] Error generando token: {ex.Message}");
                throw;
            }

            return Ok(new
            {
                token = token,
                id = usuario.Id,
                nombreCompleto = usuario.NombreCompleto,
                rol = usuario.Rol
            });
        }

        private string GenerarJwtToken(Usuario usuario)
        {
            // Comprobación robusta de configuración: soporta Jwt:Key y Jwt__Key
            var jwtKey = _configuration["Jwt:Key"];
            var jwtKeyAlt = _configuration["Jwt__Key"];
            var chosenKey = !string.IsNullOrEmpty(jwtKeyAlt) ? jwtKeyAlt : jwtKey;

            var jwtIssuer = _configuration["Jwt:Issuer"] ?? _configuration["Jwt__Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"] ?? _configuration["Jwt__Audience"];

            // Logging temporal (no imprimir el valor completo de la clave, sólo existencia/longitud)
            Console.WriteLine($"[DEBUG-CFG] Jwt:Key pres: {(!string.IsNullOrEmpty(jwtKey) ? "1" : "0")} | Jwt__Key pres: {(!string.IsNullOrEmpty(jwtKeyAlt) ? "1" : "0")}");
            Console.WriteLine($"[DEBUG-CFG] ChosenKey length: {(string.IsNullOrEmpty(chosenKey) ? 0 : chosenKey.Length)}");
            Console.WriteLine($"[DEBUG-CFG] JwtIssuer present: {(!string.IsNullOrEmpty(jwtIssuer) ? "1" : "0")} | JwtAudience present: {(!string.IsNullOrEmpty(jwtAudience) ? "1" : "0")}");

            if (string.IsNullOrEmpty(chosenKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new InvalidOperationException("Configuración de JWT incompleta en appsettings.json o App Settings");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chosenKey));
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