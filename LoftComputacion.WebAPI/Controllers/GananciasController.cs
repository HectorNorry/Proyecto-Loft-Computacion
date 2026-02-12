using LoftComputacion.Application;
using LoftComputacion.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LoftComputacion.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoftComputacion.WebAPI.Controllers
{
    [Authorize] // Bloqueo total: solo usuarios con token válido entran
    [Route("api/[controller]")]
    [ApiController]
    public class GananciasController : ControllerBase
    {
        private readonly GananciasService _gananciasService;

        public GananciasController(GananciasService gananciasService)
        {
            _gananciasService = gananciasService;
        }

        // --- CONSULTA DASHBOARD ---
        [HttpGet]
        public async Task<IActionResult> GetGanancias([FromQuery] DateTime fechaDesde, [FromQuery] DateTime fechaHasta)
        {
            var ordenesPagadas = await _gananciasService.GetGananciasPorFechaAsync(fechaDesde, fechaHasta);
            var totalGanancias = ordenesPagadas.Sum(o => o.PrecioFinal ?? 0);

            return Ok(new
            {
                Ordenes = ordenesPagadas,
                Total = totalGanancias
            });
        }

        // --- EXPORTAR EXCEL ---
        [HttpGet("exportar")]
        public async Task<IActionResult> ExportarGanancias([FromQuery] DateTime fechaDesde, [FromQuery] DateTime fechaHasta)
        {
            var fileBytes = await _gananciasService.GetGananciasExcelAsync(fechaDesde, fechaHasta);
            var fileName = $"Ganancias_Loft_{fechaDesde:yyyyMMdd}.xlsx";

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        // --- EXPORTAR PDF (Versión Segura) ---
        [HttpGet("exportar-pdf")]
        public async Task<IActionResult> ExportarPdf([FromQuery] DateTime fechaDesde, [FromQuery] DateTime fechaHasta)
        {
            try
            {
                var pdf = await _gananciasService.GetGananciasPdfAsync(fechaDesde, fechaHasta);
                var fileName = $"Ganancias_{fechaDesde:yyyyMMdd}.pdf";

                return File(pdf, "application/pdf", fileName);
            }
            catch (Exception)
            {
                // Ya no mostramos el error técnico al usuario para evitar fugas de información.
                // Simplemente devolvemos un error 500 estándar.
                return StatusCode(500, "Error interno al generar el PDF. Contacte al administrador.");
            }
        }
    }
}