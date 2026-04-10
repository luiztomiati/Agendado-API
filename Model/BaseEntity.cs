namespace Agendado.Model
{
    public abstract class  BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DtInclusao { get; set; } = DateTime.UtcNow;
        public DateTime? DtAlteracao { get; set; }
    }
}
