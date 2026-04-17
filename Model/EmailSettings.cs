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
    }
}
