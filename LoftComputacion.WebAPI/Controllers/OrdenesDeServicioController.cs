using DocumentFormat.OpenXml.InkML;
using LoftComputacion.Application;
using LoftComputacion.Application.DTOs;
using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace LoftComputacion.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenesDeServicioController : ControllerBase
    {
        private readonly OrdenDeServicioService _service;
        private readonly ApplicationDbContext _context;
        private readonly OrdenDeServicioService _ordenService;
        private readonly MercadoPagoService _mp;
        private readonly BlobService _blobService;

        public OrdenesDeServicioController(
            OrdenDeServicioService ordenService,
            MercadoPagoService mp,
            BlobService blobService)
        {
            _ordenService = ordenService;
            _mp = mp;
            _blobService = blobService;
            _ordenService = ordenService;
        }

        [HttpGet("lista")]
        public async Task<ActionResult<IEnumerable<OrdenSimpleDto>>> GetLista([FromQuery] string? filtro)
        {
            var lista = await _ordenService.GetAllOrdenesAsync(filtro);

            return Ok(lista.Select(o => new OrdenSimpleDto
            {
                Id = o.Id,
                Fecha = o.FechaIngreso,
                ClienteNombre = o.NombreCliente,
                Estado = o.NombreEstado,
                Total = o.PrecioFinal
            }));
        }

        [HttpGet("detalle/{id}")]
        public async Task<ActionResult<OrdenDetalleDto>> GetDetalle(int id)
        {
            var detalle = await _ordenService.GetOrdenDetalleAsync(id);
            if (detalle == null) return NotFound();

            return Ok(detalle);
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardDto>> Dashboard()
        {
            return Ok(await _ordenService.GetDashboardDataAsync());
        }

        [HttpPost("{id}/crear-pago")]
        public async Task<IActionResult> CrearPago(int id)
        {
            var orden = await _ordenService.GetOrdenByIdAsync(id);
            if (orden == null) return NotFound();

            if (orden.PrecioFinal == null || orden.PrecioFinal <= 0)
                return BadRequest("La orden no tiene precio final.");

            var url = await _mp.CrearPreferenciaDePagoAsync(orden);
            return Ok(new { urlDePago = url });
        }

        [HttpPost]
        public async Task<IActionResult> CrearOrden([FromBody] CreateOrdenDto dto)
        {
            var id = await _ordenService.CrearOrdenAsync(dto);
            return Ok(new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarOrden(int id, [FromBody] UpdateOrdenDto dto)
        {
            Console.WriteLine($"📌 LLEGO UsuarioId = {dto.UsuarioId}");

            var orden = new OrdenDeServicio
            {
                EstadoId = dto.EstadoId,
                PrecioPresupuestado = dto.PrecioPresupuestado,
                PrecioFinal = dto.PrecioFinal,
                ResumenTecnico = dto.ResumenTecnico
            };

            var ok = await _ordenService.UpdateOrdenAsync(id, orden, dto.UsuarioId);

            if (!ok)
                return NotFound("La orden no existe.");

            return Ok();
        }

        [HttpPost("{id}/fotos")]
        public async Task<IActionResult> SubirFoto(int id, IFormFile archivo)
        {
            if (archivo == null)
                return BadRequest("El archivo es obligatorio.");

            var foto = await _ordenService.SubirFotoAsync(archivo, id);

            return Ok(new
            {
                id = foto.Id,
                url = foto.RutaArchivo
            });
        }

        // ==============================================
        // NUEVO ENDPOINT: Obtener TODAS las órdenes 
        // ==============================================
        // ============================================================
        // NUEVO ENDPOINT PARA GANANCIAS — Usa OrdenSimpleDto (válido)
        // ============================================================
        [HttpGet("todas-simples")]
        public async Task<IActionResult> GetTodasSimples()
        {
            var ordenes = await _service.GetAllOrdenesAsync(null);

            if (ordenes == null)
                return Ok(new List<object>());

            var lista = ordenes.Select(o => new
            {
                Id = o.Id,
                FechaIngreso = o.FechaIngreso,
                ClienteNombre = o.NombreCliente,
                Total = o.PrecioFinal ?? 0,
                Estado = o.NombreEstado
            });

            return Ok(lista);
        }








    }
}
