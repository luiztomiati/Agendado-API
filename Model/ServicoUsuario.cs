namespace Agendado.Model
{
    public class ServicoUsuario : BaseEntity
    {
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = new Usuario();

        public Guid ServicoId { get; set; }
        public Servicos Servicos { get; set; } = new Servicos();

    }
}
