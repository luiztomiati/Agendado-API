using Agendado.Model;
using System.ComponentModel.DataAnnotations;

namespace Agendado.Dto
{
    public class DadosServico
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "Descrição é obrigatório")]
        public string Descricao { get; set; } = string.Empty;
        [Required(ErrorMessage = "Valor é obrigatório")]
        public double Valor { get; set; }
        [Required(ErrorMessage = "Tempo é obrigatório")]
        public TimeSpan TempoDuracao { get; set; }
    }
}
