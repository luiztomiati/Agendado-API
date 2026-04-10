namespace Agendado.Model
{
    public class Agenda : BaseEntity
    {
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public Guid EmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!;

        public Guid Servico {  get; set; }
        public Servicos Servicos { get; set; } = null!;

        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }

    }
}
