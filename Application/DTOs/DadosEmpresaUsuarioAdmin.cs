using Agendado.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Agendado.Application.Dto
{
    public class DadosEmpresaUsuarioAdmin
    {
        //Empresa
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string NomeEmpresa { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CEP é obrigatório")]
        [StringLength(9, MinimumLength = 8, ErrorMessage = "O CEP deve ter entre 8 e 9 caracteres")]
        public string Cep { get; set; } = string.Empty;


        [Required(ErrorMessage = "O número é obrigatório")]
        public string Numero {  get; set; } = string.Empty;

        // Usuario ADMIN
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string NomeUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "DDD é obrigatório")]
        [StringLength(2, MinimumLength = 2)]
        public string DDD { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [RegularExpression(@"^(9\d{8}|\d{8})$", ErrorMessage = "Telefone inválido")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "A senha deve conter ao menos 8 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "A senha deve conter ao menos uma letra maiúscula, uma minúscula, um número e um caractere especial.")]
        public string Password { get; set; } = string.Empty;
        public List<DadosHorarioEmpresa> Horarios { get; set; } = new();
    }
}
