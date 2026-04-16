using System.ComponentModel.DataAnnotations;

namespace Agendado.Dto
{
    public class DadosResetarPasswordTokenRequest
    {
        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "A senha deve conter ao menos 8 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "A senha deve conter ao menos uma letra maiúscula, uma minúscula, um número e um caractere especial.")]
        public string NovoPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        public string Token {  get; set; } = string.Empty;
    }
}
