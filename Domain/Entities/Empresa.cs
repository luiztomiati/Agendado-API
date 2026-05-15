using Agendado.Domain.Entities;

namespace Agendado.Domain.Model
{
    public class Empresa: BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string Cidade {  get; set; } = string.Empty;
        public string Uf {  get; set; } = string.Empty;
        public string Logradouro {  get; set; } = string.Empty;
        public string Bairro {  get; set; } = string.Empty;
        public List<EmpresaFuncionamento> Horarios { get; set; } = new();
        public Empresa(string nome, string numero, string cep, string cidade, string uf, string logradouro, string bairro)
        {
            Nome = nome;
            Numero = numero;
            Cep = cep;
            Cidade = cidade;
            Uf = uf;
            Logradouro = logradouro;
            Bairro = bairro;
            Horarios = new();
        }
    }
}
