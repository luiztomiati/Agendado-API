using Agendado.Domain.Model;

namespace Agendado.Application.Dto
{
    public class DadosUsuarioResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DDD { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        

        public DadosUsuarioResponse(Guid id,string nome, string email, string ddd, string telefone)
        {
            Id = id;
            Nome = nome;
            Email = email;
            DDD = ddd;
            Telefone = telefone;
        }
    }
}
