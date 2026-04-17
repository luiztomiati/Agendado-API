using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;

namespace Agendado.Model
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public async Task EnviarEmailAsync(List<string> emailsTo, string subject, string body, List<string> attachments)
        {
            var mensagem = await PreparaMensagemAsync(emailsTo, subject, body, attachments);
            await EnviarEmailSmtpAsync(mensagem);
        }
        private async Task<MailMessage> PreparaMensagemAsync(List<string> emailsTo, string subject, string body, List<string> attachments)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(Email);

            foreach(var email in emailsTo)
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
                using (SmtpClient smtpClient = new SmtpClient(SmtpServer, Port))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(Email, Password);
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
