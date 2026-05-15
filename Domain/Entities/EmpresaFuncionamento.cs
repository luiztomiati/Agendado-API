using Agendado.Domain.Model;

namespace Agendado.Domain.Entities
{
    public class EmpresaFuncionamento : BaseEntity
    {
        public Guid EmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!;

        public DayOfWeek DiaSemana { get; set; }

        public TimeOnly HoraInicio { get; set; }

        public TimeOnly HoraFim { get; set; }
    }
}
