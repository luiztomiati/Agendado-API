using System.Net.Mail;

namespace Agendado.Interface.Service
{
    public interface IEmailService
    {
        Task EnviarEmailAsync(List<string> emailsTo, string subject, string body, List<string> attachments);
        
    }
}
