using BCrypt.Net;
using LoftComputacion.Application;
using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using LoftComputacion.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
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
        private readonly EmailService _emailService;

        public AuthController(ApplicationDbContext context, IConfiguration configuration, EmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
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

            // ==========================================
            // NUEVO: FILTRO DE USUARIOS DADOS DE BAJA
            // ==========================================
            if (!usuario.Activo)
            {
                return Unauthorized("Tu cuenta ha sido desactivada. Por favor, contacta al administrador.");
            }

            bool esPasswordValida = false;
            try
            {
                esPasswordValida = BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.PasswordHash);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error verificando password: {ex.Message}");
            }

            if (!esPasswordValida)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            var token = GenerarJwtToken(usuario);

            return Ok(new
            {
                token = token,
                id = usuario.Id,
                nombreCompleto = usuario.NombreCompleto,
                rol = usuario.Rol
            });
        }

        // --- SOLICITAR RECUPERACIÓN ---
        [HttpPost("solicitar-recuperacion")]
        [AllowAnonymous]
        public async Task<IActionResult> SolicitarRecuperacion([FromBody] RecuperarPasswordDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);

            // Evitamos que los inactivos recuperen password
            if (usuario == null || !usuario.Activo)
                return Ok(new { mensaje = "Si el correo existe y la cuenta está activa, se enviaron las instrucciones." });

            var token = Guid.NewGuid().ToString();
            usuario.TokenRecuperacion = token;
            usuario.TokenRecuperacionExpiracion = DateTime.Now.AddHours(1);

            await _context.SaveChangesAsync();

            var baseUrl = Request.Headers["Origin"].ToString();
            if (string.IsNullOrEmpty(baseUrl)) baseUrl = _configuration["UrlBase"] ?? "https://localhost:7123";

            var link = $"{baseUrl}/restablecer-password/{token}";

            var mensaje = $@"
                <h3>Recuperación de Contraseña</h3>
                <p>Hola {usuario.NombreCompleto},</p>
                <p>Solicitaste restablecer tu contraseña. Hacé clic en el siguiente enlace:</p>
                <p><a href='{link}'>RESTABLECER CONTRASEÑA</a></p>
                <p>Este enlace vence en 1 hora.</p>";

            await _emailService.EnviarEmailAsync(usuario.Email, "Restablecer Contraseña - Loft", mensaje);

            return Ok(new { mensaje = "Si el correo existe y la cuenta está activa, se enviaron las instrucciones." });
        }

        // --- RESTABLECER PASSWORD ---
        [HttpPost("restablecer-password")]
        [AllowAnonymous]
        public async Task<IActionResult> RestablecerPassword([FromBody] RestablecerPasswordDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.TokenRecuperacion == dto.Token);

            if (usuario == null || usuario.TokenRecuperacionExpiracion < DateTime.Now)
            {
                return BadRequest("El enlace es inválido o ha expirado.");
            }

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NuevaPassword);

            usuario.TokenRecuperacion = null;
            usuario.TokenRecuperacionExpiracion = null;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Contraseña actualizada correctamente." });
        }

        private string GenerarJwtToken(Usuario usuario)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtKeyAlt = _configuration["Jwt__Key"];
            var chosenKey = !string.IsNullOrEmpty(jwtKeyAlt) ? jwtKeyAlt : jwtKey;

            var jwtIssuer = _configuration["Jwt:Issuer"] ?? _configuration["Jwt__Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"] ?? _configuration["Jwt__Audience"];

            if (string.IsNullOrEmpty(chosenKey)) throw new Exception("Falta Jwt:Key en configuración");

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