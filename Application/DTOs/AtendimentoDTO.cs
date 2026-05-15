namespace Agendado.Application.DTOs
{
    public class AtendimentoDTO
    {
        public Guid Id { get; set; }

        public DayOfWeek DiaSemana { get; set; }

        public TimeOnly HoraInicio { get; set; }

        public TimeOnly HoraFim { get; set; }
    }
}
