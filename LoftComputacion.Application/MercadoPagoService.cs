using LoftComputacion.Domain;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Preference;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LoftComputacion.Application
{
    public class MercadoPagoService
    {
        private readonly string _accessToken;

        // ⚠️ IMPORTANTE: Aquí pegarás la URL que te dé Visual Studio al arrancar
        // Ejemplo: "https://tu-tunnel-id.use.devtunnels.ms"
        private const string BaseUrlTunnel = "https://25bkxsbn-7081.brs.devtunnels.ms";

        public MercadoPagoService(IConfiguration configuration)
        {
            _accessToken = configuration["MercadoPago:AccessToken"];
        }

        public async Task<string> CrearPreferenciaDePagoAsync(OrdenDeServicio orden)
        {
            MercadoPagoConfig.AccessToken = _accessToken;

            // 1. Configuración de URL de Notificación
            // Le decimos a MP: "Cuando paguen, avisa a MI túnel de Visual Studio"
            string webhookUrl = $"{BaseUrlTunnel}/api/mercadopago/notificacion";

            var request = new PreferenceRequest
            {
                // Referencia para saber qué orden es cuando vuelva el aviso
                ExternalReference = orden.Id.ToString(),

                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Id = orden.Id.ToString(),
                        Title = $"Servicio Técnico - Orden #{orden.Id}",
                        Quantity = 1,
                        CurrencyId = "ARS",
                        UnitPrice = orden.PrecioFinal ?? 1 // Evitamos error si es nulo
                    }
                },

                // Aquí está la magia
                NotificationUrl = webhookUrl,

                AutoReturn = "approved",
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://www.google.com", // O tu localhost si prefieres
                    Failure = "https://www.google.com",
                    Pending = "https://www.google.com"
                }
            };

            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);

            return preference.InitPoint; // Usamos InitPoint (Prod) o SandboxInitPoint según corresponda
        }

        public async Task<Payment> ObtenerPagoAsync(long id)
        {
            MercadoPagoConfig.AccessToken = _accessToken;
            var client = new PaymentClient();
            return await client.GetAsync(id);
        }
    }
}