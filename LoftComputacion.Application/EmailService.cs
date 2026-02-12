using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace LoftComputacion.Application
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _direccionLocal;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _direccionLocal = _configuration["DireccionLocal"] ?? "Dirección no configurada";
        }

        // --- MÉTODO EXISTENTE (PARA ÓRDENES) ---
        // Lo dejamos intacto para que no se rompa nada del sistema actual
        public async Task EnviarEmailNotificacion(string emailCliente, string nombreCliente, string resumenIA, int ordenId)
        {
            var apiKey = _configuration["SendGridApiKey"];
            if (string.IsNullOrEmpty(apiKey)) return;

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("piscudelag@gmail.com", "LOFT Computación");
            var to = new EmailAddress(emailCliente, nombreCliente);

            var subject = $"¡Tu equipo está listo para retirar! (Orden N° {ordenId})";

            var plainTextContent = $"{resumenIA}\n\n--------------------------------\n{_direccionLocal}";

            var htmlContent = $"<div style='font-family: Arial, sans-serif;'>" +
                              $"<p>{resumenIA.Replace("\n", "<br>")}</p>" +
                              $"<hr style='margin-top: 20px; margin-bottom: 20px; border: 0; border-top: 1px solid #eee;' />" +
                              $"<p style='color: #777; font-size: 14px;'><strong>📍 {_direccionLocal}</strong></p>" +
                              $"</div>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }

        // --- NUEVO MÉTODO GENÉRICO (PARA RECUPERAR CLAVE Y OTROS) ---
        // Este es el que va a usar el AuthController
        public async Task EnviarEmailAsync(string emailDestino, string asunto, string mensajeHtml)
        {
            var apiKey = _configuration["SendGridApiKey"];
            if (string.IsNullOrEmpty(apiKey)) return;

            var client = new SendGridClient(apiKey);

            // Usamos el mismo remitente para todo
            var from = new EmailAddress("piscudelag@gmail.com", "LOFT Computación");
            var to = new EmailAddress(emailDestino);

            // Contenido en texto plano por si el cliente de correo no soporta HTML
            var plainTextContent = "Por favor, habilita el contenido HTML para ver este mensaje.";

            var msg = MailHelper.CreateSingleEmail(from, to, asunto, plainTextContent, mensajeHtml);
            await client.SendEmailAsync(msg);
        }
    }
}