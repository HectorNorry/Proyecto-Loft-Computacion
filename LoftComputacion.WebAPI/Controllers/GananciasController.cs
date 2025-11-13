using LoftComputacion.Application; // Necesario para el servicio
using LoftComputacion.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LoftComputacion.Domain;
using System; // Necesario para DateTime
using System.IO;
using System.Linq; // Necesario para Sum()
using System.Text;
using System.Threading.Tasks; // Necesario para async/await

namespace LoftComputacion.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GananciasController : ControllerBase
    {
        private readonly GananciasService _gananciasService;

        public GananciasController(GananciasService gananciasService)
        {
            _gananciasService = gananciasService;
        }

        // GET: api/ganancias?fechaDesde=YYYY-MM-DD&fechaHasta=YYYY-MM-DD
        [HttpGet]
        public async Task<IActionResult> GetGanancias([FromQuery] DateTime fechaDesde, [FromQuery] DateTime fechaHasta)
        {
            var ordenesPagadas = await _gananciasService.GetGananciasPorFechaAsync(fechaDesde, fechaHasta);

            // Calculamos el total de ganancias (sumando PrecioFinal donde no sea null)
            var totalGanancias = ordenesPagadas.Sum(o => o.PrecioFinal ?? 0);

            // Devolvemos un objeto que contiene tanto la lista de órdenes como el total
            var resultado = new
            {
                Ordenes = ordenesPagadas,
                Total = totalGanancias
            };

            return Ok(resultado);
        }

        [HttpGet("reporte")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrdenDeServicio>>> GetReporteSimple([FromQuery] DateTime inicio, [FromQuery] DateTime fin)
        {
            // Llama al método que ya tienes en tu GananciasService.cs:
            var ordenes = await _gananciasService.GetGananciasPorFechaAsync(inicio, fin);

            // Devolvemos la lista de órdenes con toda su información relacionada
            return Ok(ordenes);
        }

        [HttpGet("exportar")]
        public async Task<IActionResult> ExportarGanancias([FromQuery] DateTime fechaDesde, [FromQuery] DateTime fechaHasta)
        {
            // 1. Obtenemos los bytes del archivo Excel desde el servicio
            var fileBytes = await _gananciasService.GetGananciasExcelAsync(fechaDesde, fechaHasta);

            // 2. Definimos el nombre del archivo
            var fileName = $"Ganancias_Loft_{fechaDesde:yyyyMMdd}_a_{fechaHasta:yyyyMMdd}.xlsx";

            // 3. Devolvemos el archivo con el tipo MIME correcto para Excel
            return File(fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}