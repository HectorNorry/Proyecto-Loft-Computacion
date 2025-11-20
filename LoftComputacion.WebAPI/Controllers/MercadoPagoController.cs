using LoftComputacion.Application;
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
        private readonly MercadoPagoService _mercadoPagoService;
        private readonly OrdenDeServicioService _ordenDeServicioService;
        private readonly IConfiguration _configuration;

        public MercadoPagoController(
            MercadoPagoService mercadoPagoService,
            OrdenDeServicioService ordenDeServicioService,
            IConfiguration configuration)
        {
            _mercadoPagoService = mercadoPagoService;
            _ordenDeServicioService = ordenDeServicioService;
            _configuration = configuration;
        }

        [HttpPost("notificacion")]
        public async Task<IActionResult> RecibirNotificacion()
        {
            string signatureHeader = Request.Headers["x-signature"];
            string rawBody;
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                rawBody = await reader.ReadToEndAsync();
            }

            try
            {
                string secret = _configuration["MercadoPago:WebhookSecret"];
                if (string.IsNullOrEmpty(secret))
                {
                    return StatusCode(500, "Error: WebhookSecret no está configurado en el servidor.");
                }

                // --- ¡AQUÍ ESTÁ LA NUEVA LÓGICA DE VALIDACIÓN MANUAL! ---
                if (!IsValidSignature(rawBody, signatureHeader, secret))
                {
                    // ¡ALERTA! La firma no es válida.
                    return Unauthorized("Firma de Webhook inválida.");
                }
                // --- FIN DE LA NUEVA LÓGICA ---

                // Si llegamos aquí, la firma es VÁLIDA.
                JObject body = JObject.Parse(rawBody);
                var topic = body["type"]?.ToString();
                var id = body["data"]?["id"]?.ToString();

                if (id == "123456")
                {
                    return Ok("Notificación de prueba (firmada) recibida. OK.");
                }

                if (topic == "payment")
                {
                    long pagoId = long.Parse(id);
                    Payment pago = await _mercadoPagoService.ObtenerPagoAsync(pagoId);

                    if (pago != null && pago.Status == "approved")
                    {
                        int ordenId = 0;
                        if (!int.TryParse(pago.ExternalReference, out ordenId))
                        {
                            ordenId = int.Parse(pago.AdditionalInfo.Items[0].Id);
                        }

                        string nota = $"Pago confirmado vía Mercado Pago (ID: {pago.Id}). Monto: {pago.TransactionAmount:C}";
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error procesando Webhook: {ex.Message}");
            }
        }


        /// <summary>
        /// Este es el nuevo método ayudante que valida la firma de MP
        /// basado en la documentación oficial.
        /// </summary>
        private bool IsValidSignature(string body, string signatureHeader, string secret)
        {
            try
            {
                // 1. Extraer 'ts' (timestamp) y 'v1' (la firma) del header
                string timestamp = "";
                string signature = "";

                var parts = signatureHeader.Split(',');
                foreach (var part in parts)
                {
                    var kv = part.Trim().Split('=');
                    if (kv.Length == 2)
                    {
                        if (kv[0] == "ts") timestamp = kv[1];
                        if (kv[0] == "v1") signature = kv[1];
                    }
                }

                if (string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(signature))
                {
                    return false; // Header inválido
                }

                // 2. Crear el "mensaje firmado"
                // El formato oficial es: 'timestamp.body'
                string signedMessage = $"{timestamp}.{body}";

                // 3. Calcular nuestra propia firma (HMACSHA256)
                byte[] keyBytes = Encoding.UTF8.GetBytes(secret);
                byte[] messageBytes = Encoding.UTF8.GetBytes(signedMessage);

                using (var hmac = new HMACSHA256(keyBytes))
                {
                    byte[] hashBytes = hmac.ComputeHash(messageBytes);

                    // 4. Convertir nuestro hash a un string Hexadecimal
                    string localSignature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                    // 5. Comparar nuestra firma con la que mandó Mercado Pago
                    return localSignature.Equals(signature);
                }
            }
            catch
            {
                // Si algo falla (ej: parseo de header), la firma no es válida
                return false;
            }
        }
    }
}