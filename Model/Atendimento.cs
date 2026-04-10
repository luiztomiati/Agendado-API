namespace Agendado.Model
{
    public class Atendimento : BaseEntity
    {
        public DayOfWeek DiaSemana { get; set; }
        public DateOnly HoraInicio { get; set; }
        public DateOnly HoraFim { get; set; }
        public Guid Usuario { get; set; }
        public Usuario Usuarios { get; set; } = null!;
    }
}
