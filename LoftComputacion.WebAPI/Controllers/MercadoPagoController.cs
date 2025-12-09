using LoftComputacion.Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json.Linq;
using System.IO;

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
                // 1. Leer el cuerpo de la notificación
                string rawBody;
                using (var reader = new StreamReader(Request.Body))
                {
                    rawBody = await reader.ReadToEndAsync();
                }

                // Log para ver en Azure Log Stream qué está llegando (Vital para debug)
                Console.WriteLine($"🔔 [WEBHOOK] Recibido: {rawBody}");

                var json = JObject.Parse(rawBody);

                // 2. Extraer ID. 
                // MercadoPago a veces manda 'data.id' (nuevo estándar) o 'id' (legacy/topics). 
                // Cubrimos ambos casos.
                string paymentId = json["data"]?["id"]?.ToString() ?? json["id"]?.ToString();
                string type = json["type"]?.ToString() ?? json["topic"]?.ToString();

                // 3. Solo procesamos si es un pago
                if ((type == "payment" || type == "payment_intent") && !string.IsNullOrEmpty(paymentId))
                {
                    Console.WriteLine($"🔎 Verificando pago #{paymentId} en API de Mercado Pago...");

                    // 4. CONSULTA DE VERDAD (Esto confirma que no es un hackeo)
                    // Usamos las credenciales configuradas en el servicio (que serán las TEST)
                    var pago = await _mpService.ObtenerPagoAsync(long.Parse(paymentId));

                    // 5. Verificamos estado
                    if (pago != null && pago.Status == "approved")
                    {
                        Console.WriteLine($"✅ Pago #{paymentId} APROBADO por {pago.TransactionAmount} ARS.");

                        // 6. Impactar en la Base de Datos
                        if (int.TryParse(pago.ExternalReference, out int ordenId))
                        {
                            string nota = $"Pago Online Aprobado (MP ID: {pago.Id}). Monto: ${pago.TransactionAmount}";

                            // Aquí llamas a tu lógica para pasar la orden a "Finalizado - Pagado"
                            await _ordenService.ActualizarEstadoPorPagoAsync(ordenId, nota);

                            Console.WriteLine($"🚀 Orden #{ordenId} actualizada correctamente.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ El pago #{paymentId} existe pero su estado es: {pago?.Status}");
                    }
                }

                // SIEMPRE responder 200 OK. Si respondes 400/500, MP te sigue mandando la notificación por horas.
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en Webhook: {ex.Message}");
                return Ok(); // Respondemos OK aunque falle para no bloquear la cola de MP
            }
        }
    }
}