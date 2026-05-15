namespace Agendado.Application.DTOs
{
    public class DadosAtendimento
    {
        public DayOfWeek DiaSemana { get; set; }
        public DateOnly HoraInicio { get; set; }
        public DateOnly HoraFim { get; set; }
    }
}
