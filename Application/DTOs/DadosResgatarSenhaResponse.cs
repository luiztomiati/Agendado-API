namespace Agendado.Application.Dto
{
    public class DadosResgatarSenhaResponse
    {
        public bool EmailEnviado { get; set; } = false;
        public string? Token {  get; set; } = string.Empty;
    }
}
