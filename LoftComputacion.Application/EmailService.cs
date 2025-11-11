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

            // (Asumimos que el remitente verificado está bien)
            var from = new EmailAddress("piscudelag@gmail.com", "LOFT Computación");
            var to = new EmailAddress(emailCliente, nombreCliente);

            // --- ASUNTO MODIFICADO (para usar el N° de Orden) ---
            var subject = $"¡Tu equipo está listo para retirar! (Orden N° {ordenId})";

            // --- ¡CONTENIDO MODIFICADO! (Sin precio, con dirección) ---
            var plainTextContent = $"Hola {nombreCliente},\n\n{resumenIA}\n\n" +
                                   $"Saludos,\nEl equipo de LOFT COMPUTACIÓN\n\n" +
                                   $"{_direccionLocal}";

            var htmlContent = $"<p>Hola {nombreCliente},</p>" +
                              $"<p>{resumenIA.Replace("\n", "<br>")}</p><br/>" +
                              $"<p>Saludos,<br>El equipo de LOFT COMPUTACIÓN</p>" +
                              $"<hr><p><strong>{_direccionLocal}</strong></p>";
            // --- FIN DE LA MODIFICACIÓN DE CONTENIDO ---

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }
    }
}