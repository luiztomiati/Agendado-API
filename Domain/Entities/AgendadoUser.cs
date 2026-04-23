using Microsoft.AspNetCore.Identity;

namespace Agendado.Domain.Model
{
    public class AgendadoUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime? TempoExpiracao { get; set; }
        public bool PrimeiroLogin { get; set; } = true;
    }
}
