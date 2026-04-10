using System.ComponentModel.DataAnnotations;

namespace Agendado.Dto
{
    public class DadosEditUsuario
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "DDD é obrigatório")]
        public string DDD { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefone é obrigatório")]
        public string Telefone { get; set; } = string.Empty;

        public DadosEditUsuario(string  nome, string email, string ddd, string Telefone)
        {
            this.Nome = nome;
            this.Email = email;
            this.DDD = ddd;
            this.Telefone = Telefone;
        }
    }
}
