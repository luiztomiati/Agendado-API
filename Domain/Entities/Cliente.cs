using System.ComponentModel.DataAnnotations.Schema;

namespace Agendado.Domain.Model
{
    public class Cliente : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string DDD { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [ForeignKey("EmpresaId")]
        public Guid EmpresaId { get; set; }
        public required Empresa empresa { get; set; }
    }
}
