using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace LoftComputacion.Application
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _direccionLocal; // <-- NUEVO: Guardamos la dirección

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            // --- NUEVO: Leemos la dirección desde appsettings.json ---
            _direccionLocal = _configuration["DireccionLocal"] ?? "Dirección no configurada";
        }

        // --- ¡FIRMA MODIFICADA! ---
        // (Quitamos 'decimal precioFinal')
        public async Task EnviarEmailNotificacion(string emailCliente, string nombreCliente, string resumenIA, int ordenId)
        {
            var apiKey = _configuration["SendGridApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return;
            }

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("piscudelag@gmail.com", "LOFT Computación");
            var to = new EmailAddress(emailCliente, nombreCliente);

            var subject = $"¡Tu equipo está listo para retirar! (Orden N° {ordenId})";

            // --- CORRECCIÓN ---
            // 1. Quitamos el "Hola {nombreCliente}" manual porque la IA ya suele saludar.
            // 2. Quitamos el "Saludos, El equipo..." manual porque la IA ya se despide.
            // 3. Dejamos solo el cuerpo de la IA y le pegamos la dirección al final.

            var plainTextContent = $"{resumenIA}\n\n" +
                                   $"--------------------------------\n" +
                                   $"{_direccionLocal}";

            var htmlContent = $"<div style='font-family: Arial, sans-serif;'>" +
                              // Renderizamos lo que dijo la IA
                              $"<p>{resumenIA.Replace("\n", "<br>")}</p>" +
                              // Agregamos una línea separadora y la dirección (que la IA no sabe)
                              $"<hr style='margin-top: 20px; margin-bottom: 20px; border: 0; border-top: 1px solid #eee;' />" +
                              $"<p style='color: #777; font-size: 14px;'><strong>📍 {_direccionLocal}</strong></p>" +
                              $"</div>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }
    }
}