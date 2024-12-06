using System.Net.Mail;
using System.Net;

namespace AccesoDatos.Service
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com"; // Servidor SMTP de Gmail
        private readonly string _smtpUser = "appconade@gmail.com"; // Tu correo de Gmail
        private readonly string _smtpPass = "qzii ywjg ymyq msbu"; // Tu contraseña de Gmail
        private readonly string _fromEmail = "appconade@gmail.com"; // El mismo correo como remitente

        public async Task EnviarCorreoAsync(string toEmail, string subject, string body)
        {
            var client = new SmtpClient(_smtpServer)
            {
                Port = 587, // Puerto estándar para SMTP con TLS
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true, // Habilitar SSL/TLS
            };

            var mailMessage = new MailMessage(_fromEmail, toEmail, subject, body)
            {
                IsBodyHtml = true, // Permitir contenido HTML en el cuerpo del correo
            };

            await client.SendMailAsync(mailMessage);
        }
    }
}
