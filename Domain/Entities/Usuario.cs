using Agendado.Application.Dto;
using Agendado.Service;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Agendado.Domain.Model
{
    public class Usuario : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string DDD { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; } = string.Empty;
        public Guid EmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!;
        public List<Atendimento> Atendimentos { get; set; } = [];
        public string IdentityUserId { get; set; }

        public Usuario(string nome, string dDD, string telefone, string email, Guid empresaId, string identityUserId)
        {
            Nome = nome ?? throw new ArgumentNullException(nameof(nome));
            DDD = dDD ?? throw new ArgumentNullException(nameof(dDD));
            Telefone = telefone ?? throw new ArgumentNullException(nameof(telefone));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            EmpresaId = empresaId;
            IdentityUserId = identityUserId ?? throw new ArgumentNullException(nameof(identityUserId));
        }

        public Usuario() { }
    }
}
