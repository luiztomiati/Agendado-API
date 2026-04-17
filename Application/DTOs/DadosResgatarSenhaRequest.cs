using System.ComponentModel.DataAnnotations;

namespace Agendado.Application.Dto
{
    public class DadosResgatarSenhaRequest
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;
    }
}
