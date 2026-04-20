using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Agendado.Application.DTOs
{
    public class DadosCliente
    {
        public string Nome { get; set; } = string.Empty;
        public string DDD { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public DadosCliente(string nome, string ddd, string telefone, string email)
        {
            Nome = nome;
            DDD = ddd;
            Telefone = telefone;
            Email = email;
        }
    }
}
