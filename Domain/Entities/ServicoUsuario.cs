using System.ComponentModel.DataAnnotations.Schema;

namespace Agendado.Domain.Model
{
    public class ServicoUsuario : BaseEntity
    {
        public Guid UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; } = null!;

        public Guid ServicoId { get; set; }
        [ForeignKey("ServicoId")]
        public Servicos Servico { get; set; } = null!;

        public ServicoUsuario(Guid usuarioId, Guid servicoId)
        {
            UsuarioId = usuarioId;
            ServicoId = servicoId;
        }
        public ServicoUsuario() { }
    }
}
