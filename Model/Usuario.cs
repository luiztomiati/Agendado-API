using Agendado.Dto;

namespace Agendado.Model
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

        public Usuario(DadosUsuarioRequest dados)
        {
            Nome = dados.Nome;
            DDD = dados.DDD;
            Telefone = dados.Telefone;
            Email = dados.Email;
        }
       
        public Usuario() { }
    }
}
