using LoftComputacion.Domain;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Preference;
using Microsoft.Extensions.Configuration;

public class MercadoPagoService
{
    private readonly string _accessToken;
    private readonly string _apiUrl;

    public MercadoPagoService(IConfiguration configuration)
    {
        _accessToken = configuration["MercadoPago:AccessToken"];
        _apiUrl = configuration["ApiUrl"];
    }

    public async Task<string> CrearPreferenciaDePagoAsync(OrdenDeServicio orden)
    {
        MercadoPagoConfig.AccessToken = _accessToken;

        string webhookUrl = $"{_apiUrl}/api/mercadopago/notificacion";

        var request = new PreferenceRequest
        {
            ExternalReference = orden.Id.ToString(),

            Items = new List<PreferenceItemRequest>
            {
                new PreferenceItemRequest
                {
                    Id         = orden.Id.ToString(),
                    Title      = $"Reparación - Orden #{orden.Id}",
                    Quantity   = 1,
                    CurrencyId = "ARS",
                    UnitPrice  = orden.PrecioFinal ?? 100
                }
            },

            NotificationUrl = webhookUrl,

            AutoReturn = "approved",
            BackUrls = new PreferenceBackUrlsRequest
            {
                Success = "https://loftcomputacion-api-webapp-ceeycjhrb9evfvbj.brazilsouth-01.azurewebsites.net/pago-exitoso",
                Failure = "https://loftcomputacion-api-webapp-ceeycjhrb9evfvbj.brazilsouth-01.azurewebsites.net/pago-fallido",
                Pending = "https://loftcomputacion-api-webapp-ceeycjhrb9evfvbj.brazilsouth-01.azurewebsites.net/pago-pendiente"
            }
        };

        var client = new PreferenceClient();
        Preference preference = await client.CreateAsync(request);

        // 🔴 cambio importante
        return preference.InitPoint;   // antes: SandboxInitPoint
    }

    // 👇 Este lo dejás tal cual lo tenías
    public async Task<Payment> ObtenerPagoAsync(long id)
    {
        MercadoPagoConfig.AccessToken = _accessToken;
        var client = new PaymentClient();
        return await client.GetAsync(id);
    }
}