using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using LoftComputacion.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoftComputacion.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetClientes([FromQuery] string? filtro = null)
        {
            var query = _context.Clientes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = filtro.ToLower();

                query = query.Where(c =>
                    c.NombreCompleto.ToLower().Contains(filtro) ||
                    c.Telefono.ToLower().Contains(filtro) ||
                    (c.Email != null && c.Email.ToLower().Contains(filtro)) ||
                    (c.DNI != null && c.DNI.ToLower().Contains(filtro))
                );
            }

            var clientes = await query
                .OrderBy(c => c.NombreCompleto)   // Orden default
                .ToListAsync();

            return Ok(clientes);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound(); // Devuelve 404 si no lo encuentra
            }

            return Ok(cliente);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] ClienteEditarDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();

            // actualizar SOLO los campos editables
            cliente.NombreCompleto = dto.NombreCompleto;
            cliente.Telefono = dto.Telefono;
            cliente.Email = dto.Email;
            cliente.DNI = dto.DNI;

            await _context.SaveChangesAsync();

            return NoContent();
        }



        // POST: api/clientes
        [HttpPost]
        public async Task<IActionResult> CreateCliente([FromBody] CreateClienteDto clienteDto)
        {
            var nuevoCliente = new Cliente
            {
                NombreCompleto = clienteDto.NombreCompleto,
                Telefono = clienteDto.Telefono,
                Email = clienteDto.Email,
                DNI = clienteDto.DNI
            };

            await _context.Clientes.AddAsync(nuevoCliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = nuevoCliente.Id }, nuevoCliente);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}