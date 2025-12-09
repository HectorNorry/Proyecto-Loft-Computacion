using LoftComputacion.Shared.DTOs;
using LoftComputacion.Domain;       // Asegúrate que aquí esté tu clase 'Foto'
using LoftComputacion.Infrastructure;
using LoftComputacion.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Necesario para FindAsync

namespace LoftComputacion.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenesDeServicioController : ControllerBase
    {
        // 1. Declaración de variables (SOLO UNA VEZ)
        private readonly ApplicationDbContext _context;
        private readonly OrdenDeServicioService _ordenService;
        private readonly MercadoPagoService _mp;
        private readonly BlobService _blobService;

        // 2. Constructor (Inyección de dependencias)
        public OrdenesDeServicioController(
            OrdenDeServicioService ordenService,
            ApplicationDbContext context, // <--- Aquí recibimos la DB
            MercadoPagoService mp,
            BlobService blobService)
        {
            _ordenService = ordenService;
            _context = context; // <--- Aquí la guardamos. Si esto falta, explota.
            _mp = mp;
            _blobService = blobService;
        }

        /* ===================================================
           MÉTODOS GET
           =================================================== */

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

        [HttpGet("todas-simples")]
        public async Task<IActionResult> GetTodasSimples()
        {
            var ordenes = await _ordenService.GetAllOrdenesAsync(null);
            if (ordenes == null) return Ok(new List<object>());

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

        /* ===================================================
           MÉTODOS POST / PUT
           =================================================== */

        [HttpPost]
        public async Task<IActionResult> CrearOrden([FromBody] CreateOrdenDto dto)
        {
            var id = await _ordenService.CrearOrdenAsync(dto);
            return Ok(new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarOrden(int id, [FromBody] UpdateOrdenDto dto)
        {
            var orden = new OrdenDeServicio
            {
                EstadoId = dto.EstadoId,
                PrecioPresupuestado = dto.PrecioPresupuestado,
                PrecioFinal = dto.PrecioFinal,
                ResumenTecnico = dto.ResumenTecnico

            };

            // CAMBIO AQUÍ: Pasamos también dto.UsuarioAutorizador
            var ok = await _ordenService.UpdateOrdenAsync(id, orden, dto.UsuarioId, dto.UsuarioAutorizador);

            if (!ok) return NotFound("La orden no existe.");

            return Ok();
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

        /* ===================================================
           MÉTODOS FOTOS (SUBIR Y ELIMINAR)
           =================================================== */

        [HttpPost("{id}/fotos")]
        public async Task<IActionResult> SubirFoto(int id, IFormFile archivo)
        {
            if (archivo == null) return BadRequest("El archivo es obligatorio.");
            var foto = await _ordenService.SubirFotoAsync(archivo, id);

            return Ok(new { id = foto.Id, url = foto.RutaArchivo });
        }

        // --- ELIMINAR FOTO ---
        [HttpDelete("fotos/{fotoId}")]
        public async Task<IActionResult> EliminarFoto(int fotoId)
        {
            // IMPORTANTE: Verifica que <Foto> sea el nombre exacto de tu clase en Domain
            // Si tu clase se llama 'FotoOrden', cambia <Foto> por <FotoOrden>
            var foto = await _context.Set<Foto>().FindAsync(fotoId);

            if (foto == null)
            {
                return NotFound("La foto no existe o ya fue eliminada.");
            }

            // Opcional: Borrar archivo físico
            // await _blobService.BorrarArchivo(foto.RutaArchivo);

            _context.Remove(foto);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //METRICAS 
        [HttpGet("metricas-operativas")]
        public async Task<ActionResult<MetricasDto>> GetMetricas()
        {
            return Ok(await _ordenService.GetMetricasOperativasAsync());
        }
    }
}