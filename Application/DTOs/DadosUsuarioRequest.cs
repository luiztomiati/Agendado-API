using System.ComponentModel.DataAnnotations;

namespace Agendado.Application.Dto
{
    public class DadosUsuarioRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [StringLength(2, MinimumLength = 2, ErrorMessage = "DDD inválido")]
        [Required(ErrorMessage = "DDD é obrigatório")]
        public string DDD { get; set; } = string.Empty;

        [RegularExpression(@"^(9\d{8}|\d{8})$", ErrorMessage = "Telefone inválido")]
        [Required(ErrorMessage = "Telefone é obrigatório")]
        public string Telefone { get; set; } = string.Empty;
    }
}
