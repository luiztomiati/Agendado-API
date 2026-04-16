using Agendado.Dto;

namespace Agendado.Model
{
    public class Servicos : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao {  get; set; } = string.Empty;
        public double Valor {  get; set; }
        public TimeSpan TempoDuracao {  get; set; }
        public Guid EmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!;

        public Servicos(DadosServico dados)
        {
            Nome = dados.Nome;
            Descricao = dados.Descricao;
            Valor = dados.Valor;
            TempoDuracao = dados.TempoDuracao;
        }
        public Servicos() { }

    }
}
