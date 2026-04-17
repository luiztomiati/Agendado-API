using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Agendado.Application.Dto
{
    public class DadosLogin
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "A senha deve conter ao menos 8 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "A senha deve conter ao menos uma letra maiúscula, uma minúscula, um número e um caractere especial.")]
        public string Password { get; set; } = string.Empty;
    }
}
