
namespace Agendado.Dto
{
    public class DadosUsuarioResponse
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DDD { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;

        public DadosUsuarioResponse(string nome, string email, string ddd, string Telefone)
        {
            this.Nome = nome;
            this.Email = email;
            this.DDD = ddd;
            this.Telefone = Telefone;
        }
    }
}
