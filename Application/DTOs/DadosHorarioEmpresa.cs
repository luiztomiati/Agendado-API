namespace Agendado.Application.DTOs
{
    public class DadosHorarioEmpresa
    {
        public DayOfWeek DiaSemana { get; set; }

        public TimeOnly HoraInicio { get; set; }

        public TimeOnly HoraFim { get; set; }
    }
}
