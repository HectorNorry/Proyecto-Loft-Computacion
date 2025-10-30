using Microsoft.Extensions.Configuration; // Para leer la clave
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks; // Para Task

namespace LoftComputacion.Application
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarEmailNotificacion(string emailCliente, string nombreCliente, string resumenIA, decimal precioFinal, int ordenId)
        {
            // 1. Obtenemos la clave de API... (esto queda igual)
            var apiKey = _configuration["SendGridApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return;
            }

            var client = new SendGridClient(apiKey);

            // 2. Definimos el remitente... (esto queda igual)
            var from = new EmailAddress("piscudelag@gmail.com", "LOFT Computación");

            // 3. Definimos el destinatario... (esto queda igual)
            var to = new EmailAddress(emailCliente, nombreCliente);

            // 4. Creamos el contenido del email
            // --- ¡CORRECCIÓN 2: Usar 'ordenId' en el asunto! ---
            var subject = $"¡Tu equipo está listo para retirar! (Orden N° {ordenId})";
            var plainTextContent = $"Hola {nombreCliente},\n\n{resumenIA}\n\nEl precio final es: ${precioFinal:F2}.\n\n¡Te esperamos!";

            var htmlContent = $"<p>Hola {nombreCliente},</p><p>{resumenIA.Replace("\n", "<br>")}</p><p>El precio final es: <strong>${precioFinal:F2}</strong></p><p>¡Te esperamos!</p>";

            // 5. Creamos el mensaje y lo enviamos... (esto queda igual)
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }
    }
}