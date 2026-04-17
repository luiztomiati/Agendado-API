namespace Agendado.Domain.Model
{
    public class Empresa: BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string Logradouro {  get; set; } = string.Empty;
        public string Bairro {  get; set; } = string.Empty;
        public int numero { get; set; }
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public List<Usuario> Usuarios { get; set; } = [];
        public List<Servicos> Servicos { get; set; } = [];
    }
}
