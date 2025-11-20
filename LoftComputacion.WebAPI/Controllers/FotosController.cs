using LoftComputacion.Application; // Para BlobService
using LoftComputacion.Domain;      // Para la clase Foto
using LoftComputacion.Infrastructure; // Para ApplicationDbContext
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoftComputacion.WebAPI.Controllers
{
    [Authorize]
    [Route("api")] // Ruta base personalizada
    [ApiController]
    public class FotosController : ControllerBase
    {
        private readonly BlobService _blobService;
        private readonly ApplicationDbContext _context;

        public FotosController(BlobService blobService, ApplicationDbContext context)
        {
            _blobService = blobService;
            _context = context;
        }

        /// <summary>
        /// Sube una nueva foto para una orden de servicio específica.
        /// </summary>
        /// <param name="ordenId">El ID de la orden a la que pertenece la foto.</param>
        /// <param name="file">El archivo de imagen a subir (desde form-data).</param>
        [HttpPost("ordenes/{ordenId}/fotos")]
        public async Task<IActionResult> SubirFoto(int ordenId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No se ha seleccionado ningún archivo.");
            }

            // 1. Verificar que la orden exista
            var orden = await _context.OrdenesDeServicio.FindAsync(ordenId);
            if (orden == null)
            {
                return NotFound($"No se encontró la orden con ID {ordenId}.");
            }

            try
            {
                // 2. Generar un nombre de archivo único para evitar colisiones
                var extension = Path.GetExtension(file.FileName); // .jpg, .png
                var fileName = $"orden-{ordenId}-{Guid.NewGuid()}{extension}"; // ej: "orden-5-abc123.jpg"

                // 3. Subir el archivo a Azure Blob Storage
                // Usamos "fotos" como el nombre del contenedor (carpeta)
                string fileUrl;
                using (var stream = file.OpenReadStream())
                {
                    fileUrl = await _blobService.UploadFileAsync(stream, fileName, "fotos");
                }

                // 4. Crear el registro de la foto en nuestra base de datos SQL
                var nuevaFoto = new Foto
                {
                    RutaArchivo = fileUrl, // Guardamos la URL de Azure
                    OrdenDeServicioId = ordenId
                };

                _context.Fotos.Add(nuevaFoto);
                await _context.SaveChangesAsync();

                // 5. Devolvemos el objeto "Foto" recién creado
                return CreatedAtAction(nameof(GetFoto), new { fotoId = nuevaFoto.Id }, nuevaFoto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al subir la foto: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene los detalles de una foto específica.
        /// </summary>
        [HttpGet("fotos/{fotoId}")]
        public async Task<IActionResult> GetFoto(int fotoId)
        {
            var foto = await _context.Fotos.FindAsync(fotoId);
            if (foto == null)
            {
                return NotFound();
            }
            return Ok(foto);
        }

        // GET: api/ordenes/5/fotos
        [HttpGet("ordenes/{ordenId}/fotos")]
        public async Task<IActionResult> GetFotosDeLaOrden(int ordenId)
        {
            var fotos = await _context.Fotos
                                .Where(f => f.OrdenDeServicioId == ordenId)
                                .ToListAsync();

            return Ok(fotos);
        }

        [HttpDelete("fotos")]
        public async Task<IActionResult> DeleteFoto([FromQuery] string url)
        {
            if (string.IsNullOrEmpty(url))
                return BadRequest("URL inválida.");

            // Buscar la foto por URL exacta
            var foto = await _context.Fotos.FirstOrDefaultAsync(f => f.RutaArchivo == url);
            if (foto == null)
                return NotFound("No existe la foto.");

            try
            {
                var fileName = new Uri(foto.RutaArchivo).Segments.Last();

                await _blobService.DeleteFileAsync(fileName, "fotos");

                _context.Fotos.Remove(foto);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al eliminar la foto: {ex.Message}");
            }
        }
    }
}