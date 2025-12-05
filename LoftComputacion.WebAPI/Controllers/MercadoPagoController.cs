using LoftComputacion.Application;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using MercadoPago.Resource.Payment;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
// --- ¡NUEVOS 'USINGS' NECESARIOS! ---
using System.Security.Cryptography;
using System.Globalization;

namespace LoftComputacion.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MercadoPagoController : ControllerBase
    {
        private readonly MercadoPagoService _mpService;
        private readonly OrdenDeServicioService _ordenService;

        public MercadoPagoController(MercadoPagoService mpService, OrdenDeServicioService ordenService)
        {
            _mpService = mpService;
            _ordenService = ordenService;
        }

        [HttpPost("notificacion")]
        public async Task<IActionResult> RecibirNotificacion()
        {
            try
            {
                // 1. Leemos el aviso de Mercado Pago
                string rawBody;
                using (var reader = new StreamReader(Request.Body))
                {
                    rawBody = await reader.ReadToEndAsync();
                }

                // Console.WriteLine($"🔔 AVISO MP: {rawBody}"); // Descomentar para depurar

                // 2. Buscamos el ID del pago dentro del JSON
                // La documentación dice que viene en 'data.id' o a veces en 'id' directo
                var json = JObject.Parse(rawBody);
                string? paymentId = json["data"]?["id"]?.ToString() ?? json["id"]?.ToString();
                string? topic = json["type"]?.ToString() ?? json["topic"]?.ToString();

                // 3. Validamos si es un pago
                if (topic == "payment" && !string.IsNullOrEmpty(paymentId))
                {
                    // 4. Preguntamos a Mercado Pago: "¿Este pago es real y está aprobado?"
                    // (Esto es seguridad: no confiamos en el aviso, verificamos con la API oficial)
                    var pago = await _mpService.ObtenerPagoAsync(long.Parse(paymentId));

                    if (pago != null && pago.Status == "approved")
                    {
                        // 5. Buscamos a qué orden pertenece
                        if (int.TryParse(pago.ExternalReference, out int ordenId))
                        {
                            string nota = $"Pago Aprobado (MP #{pago.Id}). Total: {pago.TransactionAmount}";
                            await _ordenService.ActualizarEstadoPorPagoAsync(ordenId, nota);
                            Console.WriteLine($"✅ ORDEN #{ordenId} PAGADA EXITOSAMENTE.");
                        }
                    }
                }

                // SIEMPRE responder 200 OK, sino Mercado Pago reintenta por horas
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error Webhook: {ex.Message}");
                return Ok(); // Respondemos OK igual para no trabar la cola de MP
            }
        }
    }
}