using LoftComputacion.Domain;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Preference;
using Microsoft.Extensions.Configuration;
using System; // <-- Asegúrate de tener este
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LoftComputacion.Application
{
    public class MercadoPagoService
    {
        private readonly IConfiguration _configuration;
        private readonly string _accessToken;
        private readonly string _ngrokPublicUrl; // <-- NUEVO CAMPO

        public MercadoPagoService(IConfiguration configuration)
        {
            _configuration = configuration;

            _accessToken = _configuration["MercadoPago:AccessToken"];
            _ngrokPublicUrl = _configuration["NgrokPublicUrl"]; // <-- LEEMOS LA URL DE NGROK}


            if (string.IsNullOrEmpty(_accessToken))
            {
                throw new InvalidOperationException("El 'MercadoPago:AccessToken' no se encontró en appsettings.json.");
            }
            if (string.IsNullOrEmpty(_ngrokPublicUrl))
            {
                // En un entorno de producción, esto debería lanzar una excepción,
                // pero en desarrollo, usamos localhost:52004 como URL de notificación temporal.
                _ngrokPublicUrl = "https://loftcomputacion-api-webapp-ceeyjhrb9fvbj.brazilsouth-01.azurewebsites.net4";
            }
            else
            {
                _ngrokPublicUrl = _ngrokPublicUrl;
            }

            if (string.IsNullOrEmpty(_accessToken))
            {
                // Si el token es nulo, sí lanzamos excepción (es necesario para el SDK)
                throw new InvalidOperationException("El 'MercadoPago:AccessToken' no está configurado.");
            }
        }

        /// <summary>
        /// Crea una "Preferencia de Pago" en Mercado Pago (CORREGIDO)
        /// </summary>
        public async Task<string> CrearPreferenciaDePagoAsync(OrdenDeServicio orden)
        {
            // Asignamos el token justo antes de usarlo
            MercadoPagoConfig.AccessToken = _accessToken;

            var itemRequest = new PreferenceItemRequest
            {
                Id = orden.Id.ToString(),
                Title = $"Reparación de {orden.Equipo?.Tipo.ToString() ?? "Equipo"} (Orden N° {orden.Id})",
                Description = orden.FallaDeclaradaPorCliente,
                Quantity = 1,
                UnitPrice = orden.PrecioFinal ?? 0,
                CurrencyId = "ARS",
            };

            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest> { itemRequest },

                // Nos aseguramos de que MP sepa a qué orden referirse
                ExternalReference = orden.Id.ToString(),

                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://www.google.com", // Redirigimos a Google por ahora
                    Failure = "https://www.google.com",
                    Pending = "https://www.google.com"
                },
                AutoReturn = "approved",

                // --- ¡CAMBIO 1: EL BLOQUE 'Payer' SE ELIMINA! ---
                // (Ya no va aquí, para evitar el conflicto de identidad)

                // --- ¡CAMBIO 2: FORZAMOS LA URL DEL WEBHOOK! ---
                NotificationUrl = $"{_ngrokPublicUrl}/api/mercadopago/notificacion"
            };
                
            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);

            return preference.SandboxInitPoint;
        }

        /// <summary>
        /// Busca un pago específico en la API de Mercado Pago
        /// </summary>
        public async Task<Payment> ObtenerPagoAsync(long pagoId)
        {
            // Asignamos el token justo antes de usarlo
            MercadoPagoConfig.AccessToken = _accessToken;

            var client = new PaymentClient();
            Payment pago = await client.GetAsync(pagoId);
            return pago;
        }
    }
}