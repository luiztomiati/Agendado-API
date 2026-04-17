using Agendado.Infraestructure.Settings;
using Agendado.Interface.Service;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Agendado.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailOptions)
        {
            _emailSettings = emailOptions.Value;
        }

        public async Task EnviarEmailAsync(List<string> emailsTo, string subject, string body, List<string> attachments)
        {
            var mensagem = await PreparaMensagemAsync(emailsTo, subject, body, attachments);
            await EnviarEmailSmtpAsync(mensagem);
        }
        private async Task<MailMessage> PreparaMensagemAsync(List<string> emailsTo, string subject, string body, List<string> attachments)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(_emailSettings.Email);

            foreach (var email in emailsTo)
            {
                mail.To.Add(email);
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            return mail;
        }
        private async Task EnviarEmailSmtpAsync(MailMessage mensagem)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
                    smtpClient.Send(mensagem);
                    smtpClient.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao enviar e-mail." + ex.Message);
            }
        }
    }
}
